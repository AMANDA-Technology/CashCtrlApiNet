# Group 4 Import E2E Verification — Final Report

**Date:** 2026-04-19
**Scope:** Live E2E verification for `InventoryImportE2eTests` and `PersonImportE2eTests`
**Issue:** #89
**Status:** Complete — 10/10 tests passing

## Summary

Both import fixtures now run green against the live CashCtrl API. Fixes span four areas: mapping JSON shape, field ID casing, the undocumented `async=true` requirement on Execute, and test data isolation. This report replaces the initial speculative-risks analysis that was written when live credentials were not available.

## Fixes applied

### 1. Mapping JSON shape (`[{from, to}]`)

The `mapping` parameter sent to `/inventory/article/import/mapping.json` and `/person/import/mapping.json` is a JSON array of `{"from":"<CSV column>","to":"<field ID>"}` objects. The old tests sent `[{"column":"Nr","field":"nr"}]` (wrong keys) for Inventory and `{"lastName":"lastName"}` (wrong structure — flat object) for Person. XML doc comments on `InventoryImportMapping.Mapping` / `PersonImportMapping.Mapping` now document the required shape.

### 2. Field IDs use `UPPER_SNAKE_CASE`

Fields available for import mapping are listed by `mapping_combo.json` and use identifiers like `NR`, `NAME_DE`, `FIRST_NAME`, `LAST_NAME` — not `nr`, `name`, `firstName`, `lastName`. This casing is specific to the two `mapping_combo.json` endpoints; all other API fields use `camelCase`. Documented in [`2026-03-29-api-doc-discrepancies.md`](2026-03-29-api-doc-discrepancies.md) §11.

### 3. Undocumented `async=true` required on Execute

`execute.json` documents `id` and optional `indexes`, but the sync path returns a generic `{"success":false,"message":"An unexpected error occurred. Please contact support."}` — a masked server exception. The CashCtrl UI always sends `async=true`, which succeeds and returns a `progressId`. `InventoryImportExecute` / `PersonImportExecute` now expose `Async` and `Indexes` properties; both E2E tests set `Async=true`. Documented in [`2026-03-29-api-doc-discrepancies.md`](2026-03-29-api-doc-discrepancies.md) §9.

### 4. Test data isolation

- Hardcoded `E2E-IMPORT-001` replaced with `GenerateTestId()` per CLAUDE.md convention.
- Orphan scavenging in `InventoryImportE2eTests` extended to match articles whose `Name` **or** `Nr` starts with `E2E-`, since earlier broken-mapping runs left orphans with empty names.
- `PersonImportE2eTests` switched from vCard to CSV source file. The API docs state Mapping is not needed for vCards — testing the Mapping endpoint with a vCard source is meaningless by design.

### 5. Assertion diagnostics

`CashCtrlE2eTestBase.AssertSuccess` / `AssertCreated` now surface the response `Message` and `Errors` in the Shouldly failure message. Failure output changed from "True expected, False got" to "API response: Message='...', Errors=[...]", which accelerated the debugging cycle throughout this verification.

## Non-issues confirmed

- **Preview HTTP verb**: docs say `GET`, UI uses `POST`; our library uses `POST`, which works. Documented in [`2026-03-29-api-doc-discrepancies.md`](2026-03-29-api-doc-discrepancies.md) §10.
- **No hidden required fields on `create.json`** (Inventory or Person) — `fileId` is the only mandatory parameter. Optional `categoryId` exists but is not required.
- **Parameter name is `id`, not `importId`** on Mapping/Preview/Execute (matches existing code).
- **No `source` parameter for Person imports** — risk #5 from the initial analysis was unfounded.

## Investigation trail

Diagnosis was done via a curl reproduction that bypassed the library entirely:

1. Replicated the full flow (prepare/persist/create/mapping/execute) with curl — same generic Execute failure, ruling out a library bug.
2. Created the same target article data directly via `POST /inventory/article/create.json` — succeeded, proving the data was valid.
3. Inspected the CashCtrl UI's own network traffic during an import — observed `async=true` in the Execute POST body.
4. Confirmed the fix with a minimal curl call (`id=..., async=true`) and rolled it into the library.

## Test status

| Fixture | Tests | Status |
|---------|-------|--------|
| `InventoryImportE2eTests` | 5 | all green |
| `PersonImportE2eTests` | 5 | all green |

Unit tests: 631 passing. Integration tests: 453 passing. Build: 0 warnings, 0 errors.
