# CashCtrl API Documentation Discrepancies

**Date:** 2026-03-29
**Source:** Findings from first live E2E test run against CashCtrl REST API v1
**API docs:** https://app.cashctrl.com/static/help/en/api/index.html

## Overview

The CashCtrl API documentation documents request parameters but **does not document response schemas**. This is the root cause of most model bugs found during E2E testing. When building the C# models, property types and names were inferred from the request parameter documentation, but the actual API responses use different field names, types, or structures.

## Systematic issues

### 1. No response schemas documented

The API docs describe request parameters (name, type, required/optional) but never describe the JSON response structure. This means:

- Response-only fields (audit fields, computed fields) are undiscoverable without calling the API
- Field name differences between request and response are invisible (e.g., `title` vs `name`)
- Type differences between request and response are invisible (e.g., `string` request → `array` response)

### 2. Enum values don't match documentation

The docs list parameter values like `ADD`, `SUBTRACT`, but the API response returns different values:

| Documented | Actual response | Enum |
|-----------|----------------|------|
| `ADD` | `ADDITION` | `SalaryTypeKind` |
| `SUBTRACT` | `SUBTRACTION` | `SalaryTypeKind` |
| `ROUND_UP` | `UP` | `RoundingMode` |
| `ROUND_DOWN` | `DOWN` | `RoundingMode` |
| `ROUND_HALF_UP` | `HALF_UP` | `RoundingMode` |
| `ORDER` (create param) | `ORDER_HEADER` (response) | `TextTemplateType` |

**Impact:** Any enum-typed property will fail deserialization if the model uses the documented values.

### 3. Request field names differ from response field names

Some endpoints accept one field name for create/update but return data with a different name:

| Endpoint | Request field | Response field | Entity |
|----------|--------------|---------------|--------|
| `person/title` | `title` | `name` | PersonTitle |
| `fiscalperiod` | `startDate`, `endDate` | `start`, `end` | FiscalPeriod |
| `rounding` | `rounding` (not `value`) | `rounding` | Rounding |

**Impact:** Using `required` on properties that only exist in requests (not responses) breaks deserialization. Using the wrong `[JsonPropertyName]` means the API rejects create/update requests.

### 4. JSON string parameters returned as parsed objects

Several parameters are documented as TEXT (accepting a JSON string), but the API response returns them as parsed JSON arrays/objects:

| Endpoint | Parameter | Create format | Response format |
|----------|-----------|--------------|-----------------|
| `salary/template` | `insurances` | `string` (JSON) | `array` |
| `salary/certificate/template` | `elements` | `string` (JSON) | `array` |
| `salary/layout` | `elements` | `string` (JSON) | `array` |
| `salary/statement` | `insurances`, `custom` | `string` (JSON) | `array`/`object` |
| `salary/type` | `allocations` | `string` (JSON) | `array` |
| `customfield` | `values` | `string` (JSON) | `array` |

**Impact:** `string?` properties fail deserialization when the response contains `[...]` instead of `"..."`. Fixed by using `JsonElement?` which accepts any JSON token.

### 5. Tree endpoints return string IDs

Category tree endpoints (`account/category/tree.json`, etc.) return numeric IDs and work correctly. But non-category trees return string IDs:

| Endpoint | ID format | Example |
|----------|-----------|---------|
| `report/tree.json` | string | `"1"`, possibly non-numeric |
| `salary/template/tree.json` | string-wrapped int | `"42"` |
| `salary/certificate/template/tree.json` | string-wrapped int | `"42"` |
| All `*/category/tree.json` | int | `42` |

**Impact:** Models with `int Id` fail on tree responses that return `"42"`. Fixed with `[JsonNumberHandling(AllowReadingFromString)]` for numeric string IDs, and `string` for potentially non-numeric IDs (Report).

### 6. Undocumented required fields

Some create endpoints require fields not marked as required in the docs:

| Endpoint | Field | Documented as | Actually |
|----------|-------|---------------|----------|
| `inventory/asset/create.json` | `accountId` | not listed | required |
| `inventory/asset/create.json` | `purchaseCreditId` | not listed | required |
| `inventory/asset/create.json` | `dateAdded` | not listed | required |
| `account/costcenter/create.json` | `number` | optional | required |
| `customfield/reorder.json` | `type` | not listed | required |
| `customfield/group/reorder.json` | `type` | not listed | required |

### 7. Non-JSON response endpoints

Some endpoints return plain text instead of JSON, despite being `.json` suffixed:

| Endpoint | Documented return | Actual return |
|----------|------------------|---------------|
| `sequencenumber/get` | not documented | Plain text string (e.g., `RE-2603291`) |
| `currency/exchangerate` | not documented | Plain decimal or empty body |
| `file/get` | not documented | HTTP 302 redirect to cloud storage URL |

### 8. File upload workflow not obvious from docs

The `file/prepare.json` endpoint is documented as accepting a `files` TEXT parameter, but the docs don't clarify:

- `files` is a **form-encoded parameter** containing a **JSON array** of `{name, mimeType}` objects
- The response returns pre-authenticated `writeUrl` values for each file
- Binary content must be uploaded via **HTTP PUT** to these URLs (not to CashCtrl)
- `file/persist.json` must be called after upload to finalize
- The `file/get` endpoint returns a **302 redirect** to a pre-authenticated cloud storage URL, not the file content directly

### 9. Undocumented `async=true` required on import Execute

The `inventory/article/import/execute.json` and `person/import/execute.json` endpoints document only `id` (mandatory) and `indexes` (optional). In practice, calling with only those parameters returns HTTP 200 with `{"success":false,"message":"An unexpected error occurred. Please contact support."}` — a masked server-side exception.

The CashCtrl UI always sends `async=true`, which routes to a background-job path that succeeds. The response shape changes too: the async path returns `{"success":true,"attributes":{"progressId":"..."}}` while `insertId` is absent and the imported rows are created asynchronously.

Verified via curl: creating the same article data through `POST /inventory/article/create.json` succeeds, but the import Execute endpoint with just `id` fails with the generic error. Adding `async=true` is the minimal fix.

**Impact:** Execute appears completely broken when called per the docs. Models must include an `Async` parameter, and clients should default to `Async=true`.

### 10. Preview endpoints documented as GET, actually POST

`inventory/article/import/preview.json` and `person/import/preview.json` show `GET` in the docs, but the CashCtrl UI always sends `POST` with `id` in the form body. Both verbs work in practice; matching the UI (`POST`) is the safer choice.

### 11. `mapping_combo.json` field IDs use `UPPER_SNAKE_CASE`

The "List mapping fields" endpoints (`inventory/article/import/mapping_combo.json`, `person/import/mapping_combo.json`) return `{text, value, group}` triples where `value` is an `UPPER_SNAKE_CASE` identifier: `NR`, `NAME_DE`, `NAME_EN`, `FIRST_NAME`, `LAST_NAME`, `ROLE_CUSTOMER`, `CATEGORY_NAME_DE`, etc. These `value` strings are what must appear on the `to` side of each `mapping.json` array entry.

This is the **only** place in the CashCtrl API where `UPPER_SNAKE_CASE` identifiers appear — all other request/response fields use `camelCase`. The docs link to the combo endpoint but never show the response shape, so the casing convention is undiscoverable without a live call.

**Impact:** Inferring field IDs from model property names (e.g., `nr`, `name`, `firstName`) is wrong. Always call `mapping_combo.json` (or inspect the UI payload) to learn the correct `value` strings.

### 12. Journal `items` returned as array (repeat of §4)

`journal/create.json` and `journal/update.json` accept `items` as a JSON string (TEXT) for collective entries, but `journal/read.json` / `journal/list.json` return it as a parsed array (empty `[]` for regular entries). Same pattern as §4. Fix: `JsonElement?` instead of `string?`.

### 13. Journal `sequenceNumberId` is write-only

`journal/create.json` marks `sequenceNumberId` as mandatory, but `journal/read.json` / `journal/list.json` do not return it — the server emits the generated `reference` (e.g. `RE-2604193`) instead. A `required int` on the create model breaks deserialization across the shared hierarchy (§3 pattern). Fix: nullable `int?`, treat as required at the application level.

### 14. Journal Update: echoing `items: []` back creates a broken "collective entry with 0 items"

Per the update docs: *"Omit to create a regular book entry, and set if you want to create a collective book entry."* A Get→Update round-trip on a regular journal entry will echo `items: []` back, which the server interprets as a collective entry with zero items and rejects with **"At least 1 book entry must be created."** Callers must explicitly clear `items` when updating regular entries.

### 15. Journal import `create.json`: `mappings` is effectively mandatory

Docs don't emphasize this, but omitting `mappings` returns the masked `{"success":false,"message":"An unexpected error occurred. Please contact support."}` — no validation error hint, just a 500-style reply. The API also **silently ignores** any non-documented parameters (we had a phantom `name` field the server silently dropped).

### 16. Journal import entry list / exports: `importId` query is mandatory

`journal/import/entry/list.json` (and the `.xlsx` / `.csv` / `.pdf` export variants) require `importId` as a mandatory query parameter. Without it the API returns `{"success":false,"message":"ID missing."}`. The library must expose a dedicated request type (we added `JournalImportEntryListRequest : ListParams` with `required int ImportId`).

### 17. Journal import entry update: undocumented mandatory fields

Docs list only `id`, `amount`, `contraAccountId`, and `dateAdded` as mandatory for `journal/import/entry/update.json`. In practice the live API also rejects an update missing `debitId` / `creditId`:

```
[debitId] This field cannot be empty.
[creditId] This field cannot be empty.
```

Callers need to echo the server-populated debit/credit IDs back on every update. Additionally, `dateAdded` in the read/list response is a CashCtrl datetime string (`2026-01-01 00:00:00.0`), but the update endpoint only accepts `YYYY-MM-DD` — so the string needs to be trimmed before sending.

## Recommendations

1. **Never use `required` on properties that appear in both create requests and read responses**, unless the field name and type are identical in both directions. Use nullable properties and validate at the application level.

2. **All enums must use `[JsonConverter(typeof(JsonStringEnumConverter<T>))]`** with `[JsonStringEnumMemberName("...")]` attributes. Never assume the API returns the same string you send.

3. **Properties documented as TEXT that accept JSON should use `JsonElement?`** to handle both string (create) and parsed (response) formats.

4. **Test every endpoint against the live API** before considering the model complete. The documentation is insufficient for building correct models.
