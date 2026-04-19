# E2E Test Verification Report

**Date:** 2026-03-29
**Scope:** First live run of E2E tests against CashCtrl REST API v1

## Summary

374 E2E test methods across 58 fixtures were implemented. During the first live verification run, we discovered and fixed numerous model, service, and test data issues caused by undocumented API behavior. This document tracks progress, findings, and remaining work.

## Verification Progress

| Group | Scope | Fixtures | Tests | Status |
|-------|-------|----------|-------|--------|
| 1 | Read-only | Report, Organization, History, SalaryField | 5 | **5/5 passed** |
| 2 | Simple CRUD | Currency, CustomField, CustomFieldGroup, Rounding, SequenceNumber, TaxRate, TextTemplate, PersonTitle, Unit, 6x categories | 86 | **86/86 passed** |
| 3 | CRUD + exports | Account, AccountBank, CostCenter, Article, FixedAsset, Person, File | 77 | **77/77 passed** |
| 4 | Import workflows | InventoryImport, PersonImport | 10 | **10/10 passed** — see [2026-04-19 report](2026-04-19-group4-import-e2e-verification.md) |
| 5 | Journal | Journal, JournalImport, JournalImportEntry | ~24 | **not yet run** |
| 6 | Order | OrderCategory, OrderLayout, Order, BookEntry, Document, OrderPayment | ~39 | **not yet run** |
| 7 | Salary | 15 salary fixtures | ~90 | **not yet run** |
| 8 | Meta (highest risk) | Settings, Location, FiscalPeriodTask, FiscalPeriod | ~26 | **not yet run** |

**178 passed, 0 skipped, 0 failed.** ~179 tests remaining across Groups 5-8.

## What Was Fixed

### Safety fixes (before running any tests)

| Fix | File(s) | Risk prevented |
|-----|---------|---------------|
| Removed `EmptyArchive` test | `FileE2eTests` | Permanently deletes ALL archived files in the account |
| Redirected `BookDepreciations`/`BookExchangeDiff` to test period | `FiscalPeriodE2eTests` | Was modifying the live current fiscal period |
| Added orphan scavenging for 2090 test periods | `FiscalPeriodE2eTests` | Orphaned test periods from crashed runs |
| Tracked continued order for cleanup | `OrderE2eTests` | `Continue()` creates a new order but ID was discarded |
| Added `Persist()` after `Prepare()` | `InventoryImportE2eTests`, `PersonImportE2eTests` | Violated 3-step file upload workflow |
| Added missing `RunCleanup()` calls | `FiscalPeriodTaskE2eTests` | Cleanup actions never executed |
| Standardized orphan scavenging | `FiscalPeriodTaskE2eTests` | Manual inline code instead of `ScavengeOrphans()` helper |

### Model deserialization fixes

| Model | Property | Issue | Fix |
|-------|----------|-------|-----|
| `Report` | `Id`, `ParentId` | API returns string, model had `int` | Changed to `string` |
| `SalaryTemplateUpdate` | `Id` | Tree returns `"42"` string | Added `[JsonNumberHandling(AllowReadingFromString)]` |
| `SalaryCertificateTemplateUpdate` | `Id` | Same | Same |
| `SalaryTemplateCreate` | `Insurances` | API returns array, model had `string?` | Changed to `JsonElement?` |
| `SalaryCertificateTemplateCreate` | `Elements` | Same | Same |
| `SalaryLayoutCreate` | `Elements` | Same (proactive) | Same |
| `SalaryStatementCreate` | `Custom`, `Insurances` | Same (proactive) | Same |
| `SalaryStatementUpdateMultiple` | `Custom`, `Insurances` | Same (proactive) | Same |
| `SalaryStatementCalculate` | `Custom` | Same (proactive) | Same |
| `SalaryTypeCreate` | `Allocations` | Same (proactive) | Same |
| `CustomFieldCreate` | `Values` | Same (proactive) | Same |
| `FiscalPeriodCreate` | `StartDate`, `EndDate` | `required` but not in response (API uses `start`/`end`) | Made nullable, added `Start`/`End` to Listed |
| `PersonTitleCreate` | `Title` | API field is `name`, not `title` | Renamed to `Name` with `[JsonPropertyName("name")]` |
| `RoundingCreate` | `Value` | API field is `rounding`, not `value` | Changed `[JsonPropertyName("rounding")]` |
| `FixedAssetCreate` | (missing) | API requires `accountId`, `purchaseCreditId`, `dateAdded` | Added 3 new required properties |

### Enum fixes

All enums now have `[JsonConverter(typeof(JsonStringEnumConverter<T>))]` and `[JsonStringEnumMemberName]` attributes. Key value corrections:

| Enum | Issue |
|------|-------|
| `SalaryTypeKind` | API returns `ADDITION`/`SUBTRACTION`, not `ADD`/`SUBTRACT` |
| `RoundingMode` | API returns `HALF_UP`, not `ROUND_HALF_UP` |
| `TextTemplateType` | API returns `ORDER_HEADER`/`ORDER_FOOTER`/`ORDER_MAIL`/`SALARY_MAIL`, not `ORDER`/`PERSON` |

### Service-layer fixes

| Service | Method | Issue | Fix |
|---------|--------|-------|-----|
| `SequenceNumberService` | `GetGeneratedNumber` | Returns plain text, was typed as `SingleResponse<SequenceNumber>` | New `PlainTextResponse` type + `GetPlainTextAsync` |
| `CurrencyService` | `GetExchangeRate` | Returns plain decimal, was typed as `SingleResponse<CurrencyExchangeRate>` | Changed to `DecimalResponse` via `GetBalanceAsync` |
| `FileService` | `Prepare` | Used `PostMultipartAsync` but API expects form-encoded `files=<json>` | New `FilePrepareRequest`/`FilePrepareResponse` + `PostAsync` |
| `CashCtrlConnectionHandler` | `GetBinaryApiResult` | `file/get` returns 302 redirect to cloud storage | Added redirect-following for binary downloads |

Also renamed `BalanceResponse` to `DecimalResponse` (with `.Value` instead of `.Balance`) since it's now used for both balances and exchange rates.

### Test data fixes

| Fixture | Issue |
|---------|-------|
| `AccountBankE2eTests` | Invalid IBAN/BIC test data |
| `CostCenterE2eTests` | Missing required `Number` field |
| `FixedAssetE2eTests` | Missing required fields + fiscal period date handling + `GetList` needs `fiscalPeriodId` |
| `CustomFieldE2eTests` | Update failed due to response-only fields in POST; Reorder needed `Type` parameter |
| `CustomFieldGroupE2eTests` | Reorder needed `Type` parameter |
| `SalaryFieldE2eTests` | `Name` assertion on field that uses a different property |
| `OrganizationE2eTests` | Logo endpoint has no `Content-Disposition` header |
| `ReportElementE2eTests` | Used tree node ID instead of report set ID |
| All file upload tests (5 files) | Rewrote to use 3-step workflow (Prepare metadata + PUT binary + Persist) |

## Remaining Work for Groups 4-8

Groups 4-8 have not been run against the live API. Based on the patterns found in Groups 1-3, expect similar issues:

- **Model mismatches**: Properties that exist in create requests but have different names in responses
- **Missing required fields**: Models missing fields the API requires but doesn't document as required
- **Enum value mismatches**: API returns different strings than documented
- **`string?` → `JsonElement?`**: More properties where the API returns arrays instead of JSON strings
- **Test data dependencies**: Fixtures needing valid account IDs, fiscal period dates, etc.

### Recommended run order

Continue running groups sequentially, fixing issues as they surface:

4. **Import workflows** — InventoryImport, PersonImport (file upload now fixed, should work better). A verification attempt was made on 2026-04-19 (issue #89) but could not be completed due to missing live credentials in the sandbox environment. See the [2026-04-19 status report](2026-04-19-group4-import-e2e-verification.md) for a static review of the architect's flagged risk areas.
5. **Journal** — creates real bookings, verify cleanup
6. **Order** — complex dependencies (Person, Account, OrderCategory)
7. **Salary** — most complex, many interdependencies
8. **Meta** — Settings, FiscalPeriod (highest risk to live data)

After each group, verify in CashCtrl UI:
- No entities with "E2E-" prefix left behind
- Account balances unchanged (after Journal)
- Active fiscal period unchanged (after Meta)
