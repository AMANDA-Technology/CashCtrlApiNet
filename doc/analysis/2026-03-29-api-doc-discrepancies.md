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

### 18. Order repeats §§3, 4, 13: `items` is array on read, `sequenceNumberId` is write-only

`order/create.json` takes `items` as a JSON string (TEXT) and `sequenceNumberId` as mandatory, but the read/list responses return `items` as a parsed array and omit `sequenceNumberId` entirely (replaced by the server-generated `nr`). Same fixes as Journal (§§12–13): `Items` → `JsonElement?`, `SequenceNumberId` → `int?`.

### 19. OrderCategory uses `sequenceNrId`, every other endpoint uses `sequenceNumberId`

Order's own create/update endpoints take the parameter as `sequenceNumberId`; `order/category/create.json` and `order/category/update.json` take the same concept as `sequenceNrId`. Both are documented — but only visible if you read the two docs side-by-side. Models must use a separate `[JsonPropertyName("sequenceNrId")]` on the category type even though the C# name can still be `SequenceNumberId` (or `SequenceNrId` for clarity).

### 20. OrderCategory create: `name` is a phantom — the real mandatory fields are `nameSingular`, `namePlural`, `accountId`, `status`

The initial model accepted a single `name` parameter, which the server silently discards. The actual create takes four mandatory parameters:

- `nameSingular` (TEXT, MAX:100) — e.g. `"Invoice"`
- `namePlural` (TEXT, MAX:100) — e.g. `"Invoices"`
- `accountId` (NUMBER) — typically debtors (sales) or creditors (purchase)
- `status` (JSON array, at least one entry) — `[{"icon":"BLUE","name":"Draft"}, …]` with icon from `BLUE/GREEN/RED/YELLOW/ORANGE/BLACK/GRAY/BROWN/VIOLET/PINK`

Calling with only `name` fails with `[accountId|nameSingular|namePlural] This field cannot be empty. [] At least 1 status must be defined.` The read response additionally exposes a derived `name` field (not a create parameter) — put it on the response model only.

### 21. `read_status.json` takes a status ID, not a category ID

`order/category/read_status.json` is documented as "Returns a single status by ID", where the `id` parameter is the **status**'s ID — not the category's ID. Passing a category ID by mistake may return a different category's status by accident (if the numeric ranges happen to collide, as they did in our test account). Discover the real status ID by reading the parent category and walking its `status` array. This endpoint also returns an `OrderCategoryStatus` record (id, categoryId, actionId, name, icon, pos, isBook, isAddStock, isRemoveStock, isClosed), not an `OrderCategory`.

### 22. Plural-IDs-as-CSV across batch endpoints

Many batch-capable Order endpoints take a comma-separated list of IDs under `ids` (or `orderIds`), not a singular `id`. The library's older models that used singular `int Id` fields silently serialized to the wrong form param name and the server responded with empty/validation errors:

| Endpoint | Parameter | Old model had |
|----------|-----------|---------------|
| `order/continue.json` | `categoryId` + `ids` (CSV) | `id` (singular) |
| `order/update_status.json` | `ids` (CSV) + `statusId` | `id` (singular) |
| `order/dossier_add.json` / `dossier_remove.json` | `groupId` + `ids` (CSV) | `id` + `dossierId` |
| `order/bookentry/create.json` | `orderIds` (CSV) | `orderId` (singular) |
| `order/payment/create.json` / `download` | `orderIds` (CSV) | `orderId` (singular) |

Use `ImmutableArray<int>` with `[JsonConverter(typeof(IntArrayAsCsvJsonConverter))]` for every `ids`/`orderIds` parameter.

### 23. `order/dossier.json` returns a single dossier object, not a list

The endpoint is named "Read dossier" but despite the plural-looking payload, it returns `SingleResponse<{id, items:[…]}>` — not `ListResponse<OrderListed>`. The items are a slim projection (id, date, type, nr, nameSingular, status, icon, total, open, percentage, pos, lastUpdatedBy, isHighlight), not full `OrderListed` records. Needs a dedicated `OrderDossier`/`OrderDossierItem` model.

### 24. Order recurrence: `startDate` is documented as optional but required when `recurrence` is set

`order/update_recurrence.json` marks `startDate` as optional. In practice, setting `recurrence` to any non-null value without a `startDate` fails with `[startDate] This field cannot be empty.` Always send them together; pass `recurrence=null` alone to clear recurrence.

### 25. `order/bookentry/*`: `Document does not allow book entries` unless order is in an `isBook=true` status, plus split Create/Update shapes

Book entries can only be created against orders whose current status has `isBook=true`. The default "Offer" category's statuses are all `isBook=false`; the "Invoice" category has the `Open`/`Paid`/etc. statuses with `isBook=true`. Callers must either pick a category whose statuses include an `isBook=true` entry, or move the order into such a status (via `order/update_status.json`) before creating book entries.

Also: `bookentry/create.json` accepts `orderIds` (CSV), while `bookentry/update.json` deliberately does **not** — the parent order(s) are immutable on an existing entry. The library's `BookEntryUpdate` should not inherit from `BookEntryCreate`; they are genuinely different request shapes. `bookentry/list.json` additionally requires the order's `id` as a mandatory query parameter.

Finally, `date` on `bookentry/create.json` is documented as optional but the live API rejects with `[date] This field cannot be empty.` when omitted. Always provide a date within an existing fiscal period.

### 26. Document is keyed 1:1 by orderId — the read response has no top-level `id`

`order/document/read.json` returns the document for a given order. The response has `orderId` but no top-level `id` field, because a document is not a first-class entity — there is exactly one document per order, and the order's ID is the document's identity. The library's `Document` read model must therefore stand alone (not inherit from a `DocumentUpdate` that had a `required int Id`) and expose an `OrderId` property mapped to `[JsonPropertyName("orderId")]`.

Additionally, `DocumentUpdate`'s previously modeled `text` field does not appear in the API docs and is silently dropped by the server. The real update parameters include `orgAddress`, `recipientAddress`, `header`, `footer`, `layoutId`, `language`, `customReference`, `fileId`, `isDisplayItemGross`, `isFileReplacement` and bank account IDs.

### 27. Order payment has no "payment id"; Create + Download share the same request shape

`order/payment/create.json` and `order/payment/download` both take the same parameters (`date` + `orderIds` + optional `amount`/`isCombine`/`statusId`/`type`). There is no separate payment entity — a payment is identified by its `(date, orderIds)` tuple, and Download matches an existing payment by those same parameters. The docs even state explicitly: *"Please use the create endpoint first, and then use the same parameters to download the file here."*

Additionally, payment validation requires a fully-provisioned business context (undocumented beyond cryptic `Sender/Recipient: Address must be set` errors):

- Sender address: set on a `Location` entity that the document's `orgLocationId` points to.
- Recipient address: set on the `Person.addresses` JSON array (itself an `addresses: [{type, address, city, zip, ...}]` structure on `person/create.json`).
- For `PAIN`/`SEPA_PAIN`/`WIRE_PDF`: also bank accounts and BICs on both sides.
- `CASH_PDF` is the least-demanding type — still requires addresses but not bank info.

The `OrderPaymentRequest` model matches the API spec exactly; the Create/Download E2E tests in `OrderPaymentE2eTests` are marked `[Ignore]` pending a dedicated fixture that provisions a Location + a Person with addresses.

### 28. Report domain: "report set" vs "report collection" naming drift, and element-scope IDs

The CashCtrl UI / docs use both "report set" and "report collection" to refer to the same entity. The actual API paths live under `/api/v1/report/collection/*` and the parameter name is consistently `collectionId`, not `reportSetId` or `reportId`. Every endpoint that takes a report set as input uses `collectionId`:

- `report/collection/meta.json`, `download.pdf`, `download.csv`, `download.xlsx`, `download_annualreport.pdf` → `collectionId` (not `id`)
- `report/element/create.json`, `update.json` → `collectionId` is the parent scope field (*not* `reportId` or `setId`)
- `report/element/reorder.json` → **requires** `collectionId` (undocumented — see below)

Meanwhile the element-side endpoints use *yet another* ID parameter:

- `report/element/read.json` → `id`
- `report/element/data.json`, `data.html`, `meta.json`, `download.pdf`, `download.csv`, `download.xlsx` → `elementId`

Three different names (`id`, `elementId`, `collectionId`) for what's logically "the element's ID" / "the collection's ID" across sibling endpoints on the same entity.

### 29. `report/element/create.json`: `type` is required despite being documented as optional

Omitting `type` causes the API to reject with `[type] Invalid value.` — the docs list `type` as a plain parameter with no required flag. `type` is an enum (`JOURNAL`, `BALANCE`, `PLS`, `STAGGERED`, `COST_CENTER_PLS`, `COST_CENTER_BALANCE`, `CHART_OF_ACCOUNTS`, `OPEN_DEBTORS`, … ~40 values) modeled as `ReportElementType`.

Related: `accountId` is documented as required but is only required for element types with a primary account dimension. `ChartOfAccounts`, for example, treats the whole chart as its dataset and silently stores `accountId=null` on create.

### 30. Report meta endpoints return a slim document-header shape — not the entity shape

`report/element/meta.json` and `report/collection/meta.json` don't return the `ReportElement` / `ReportSet` shape — they return a compact "document header" projection used when rendering the report:

| Endpoint | Response fields |
|----------|-----------------|
| `report/element/meta.json` | `title`, `text`, `periodLabel`, `isHideTitle`, `isBeta`, `isPro` |
| `report/collection/meta.json` | `title`, `text`, `periodLabel`, `logoUrl`, `logoHeight` |

Neither payload has an `id` field — the response is keyed by the request's `elementId`/`collectionId`. The library therefore has dedicated `ReportElementMeta` and `ReportCollectionMeta` records distinct from `ReportElement`/`ReportSet`.

### 31. `report/element/data.json` response shape varies by report type

The data endpoint returns JSON whose structure depends entirely on the element's `type`. `ChartOfAccounts` returns a recursive tree of account nodes; `Journal` returns a flat list of entries; `Balance`/`Pls` return grouped sections; etc. There is no common envelope beyond `{"success": true, "data": …}`.

The library therefore exposes `IReportElementService.GetData` as `Task<ApiResult>` (untyped) and carries the raw JSON on `ApiResult.RawResponseContent`. Callers parse into the shape appropriate for their report type. The untyped `GetAsync` overloads on `ICashCtrlConnectionHandler` were updated to always populate `RawResponseContent` (previously only populated on deserialization failure).

### 32. `report/element/reorder.json`: `collectionId` is required but undocumented

Reordering elements without `collectionId` fails with `ID missing.` — the API needs the parent scope to know *which* collection's element order is being changed. The docs list only `ids`, `target`, `before`.

This follows the same pattern as `customfield/reorder.json` / `customfield/group/reorder.json` requiring an undocumented `type` scope field (§6). Generalizes to: **reorder endpoints on child entities require the parent-scope ID.**

### 33. Report element read/export edge cases

- **`negateAmount` is silently discarded on read for certain element types.** `ChartOfAccounts` accepts `negateAmount` on create/update but never reflects it back in `read.json`. The flag is only meaningful for report types with a primary amount dimension. Don't round-trip-verify this field in tests against `ChartOfAccounts` elements.
- **`data.html` serves the body inline with no `Content-Disposition` header.** Unlike the PDF/CSV/Excel downloads, the HTML endpoint's response carries no filename. `BinaryResponse.FileName` will be `null` — callers must synthesize a filename if saving locally.

## Recommendations

1. **Never use `required` on properties that appear in both create requests and read responses**, unless the field name and type are identical in both directions. Use nullable properties and validate at the application level.

2. **All enums must use `[JsonConverter(typeof(JsonStringEnumConverter<T>))]`** with `[JsonStringEnumMemberName("...")]` attributes. Never assume the API returns the same string you send.

3. **Properties documented as TEXT that accept JSON should use `JsonElement?`** to handle both string (create) and parsed (response) formats.

4. **Test every endpoint against the live API** before considering the model complete. The documentation is insufficient for building correct models.

5. **Batch-capable write endpoints take plural-CSV IDs** (`ids`, `orderIds`, etc.), not a singular `id`. Use `ImmutableArray<int>` with `IntArrayAsCsvJsonConverter` for these. If you see a singular `id` field on a request model that targets a batch endpoint, it's almost certainly wrong.

6. **Do not share write-side models between Create and Update when the two genuinely diverge.** The `BookEntryUpdate : BookEntryCreate` pattern broke because Update does not accept `orderIds`; inheritance forced a field that isn't valid for the endpoint. Prefer composition or standalone records over inheritance when the semantics differ.

7. **When a read response has no top-level `id`, the entity is not first-class.** `Document` is identified by `orderId` (one document per order). Model it as a standalone record, not by inheriting from an Update type that has `required int Id`.

8. **Parameter names can differ between sibling endpoints.** `OrderCategory` takes `sequenceNrId` while every other Order endpoint uses `sequenceNumberId`. Never assume a field name carries over — read the endpoint's own docs each time.

9. **Some endpoints need server-side state to be "allowed" before they accept requests.** Book entries require the parent order to be in a status with `isBook=true`. Payments require a fully populated `Location` (sender) and `Person.addresses` (recipient). Discover these constraints by reading the error messages — the docs rarely mention them.

10. **`[Ignore]` a test when the cheapest path to green is environment setup, not a fix.** Document the prerequisites in the XML doc comment (not just a terse reason) so the follow-up issue inherits the checklist.
