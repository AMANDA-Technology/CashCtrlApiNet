# Full API Implementation Design

**Date:** 2026-03-23
**Status:** Approved
**Scope:** 100% CashCtrl API endpoint coverage for CashCtrlApiNet client library

---

## Goal

Implement every CashCtrl REST API endpoint in the CashCtrlApiNet client library -- all models, interfaces, services, connector wiring, and unit tests. This includes the 9 existing domain groups, the Salary module (new 10th group), and binary endpoints (export/upload).

## Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Testing strategy | Unit tests with mocked HTTP first; integration tests later | Fast CI, no credentials needed during development |
| Test libraries | NSubstitute (mocking) + Shouldly (assertions) | User preference |
| Work management | One GitHub issue per domain group | Manageable scope per issue, clear ownership |
| Execution approach | Parallel by domain group (3-4 agents per wave) | 3-4x faster than sequential; quality maintained via golden reference |
| Salary module | Included as 10th group, tackled last | Complete coverage goal |
| Binary endpoints | Included; infrastructure added in Phase 0 | No deferred gaps |
| Binary return type | `Task<ApiResult<BinaryResponse>>` for all export/download endpoints | Consistent API surface; `BinaryResponse : ApiResponse` satisfies `ApiResult<T>` constraint and carries content-type/filename metadata |

---

## Phase 0: Infrastructure, Bug Fixes & Golden Reference (#3)

One GitHub issue. Must merge before any domain group work begins.

### 0.1 Fix Known Bugs

1. `PersonEndpoints.cs` line 43: Inner class named `Order` instead of `Person`
2. `FileEndpoints.cs` line 109: Category class named `ArticleCategory` instead of `FileCategory`
3. `ReportEndpoints.cs`: Uses `report/set/` path but API uses `report/collection/`
4. `ReportConnector.cs` lines 41-43: Uses `new IReportService(...)` instead of `new ReportService(...)`
5. `AccountService.Get()`: Takes `int` instead of `Entry`, doesn't pass ID as query param
6. `AccountConnector.cs` line 45: Comment says `Account = new CostCenterCategoryService(...)` -- wrong
7. Account models: Empty stubs with incorrect inheritance

### 0.2 Binary HTTP Support

- Add `GetBinaryAsync` method to `ICashCtrlConnectionHandler` returning `Task<ApiResult<BinaryResponse>>`
- All export/download service methods use `Task<ApiResult<BinaryResponse>>` as return type
- Add multipart form data support for `file/prepare.json` upload
- Unit tests for both

### 0.3 Unit Test Infrastructure

- Add NSubstitute and Shouldly NuGet packages to test project
- Create `ServiceTestBase<TService>` providing a mocked `ICashCtrlConnectionHandler`
- Establish test pattern:
  - Verify correct endpoint called
  - Verify correct HTTP method (GET/POST)
  - Verify request parameters serialized correctly
  - Verify response deserialized correctly
  - Test edge cases: optional params omitted, collections, null handling

### 0.4 Account Group as Golden Reference

Fully implement all 5 Account services with unit tests:

| Service | Endpoints | Notes |
|---------|-----------|-------|
| AccountService (fix models + service) | 11 (incl. exports) | Fix stub models, fix Get() signature |
| AccountBankService | 9 (incl. exports) | NEW -- missing entirely from codebase; new interface, endpoints, models, service |
| AccountCategoryService | 6 | Standard tree pattern |
| CostCenterService | 11 (incl. exports) | Includes GetBalance, Categorize, UpdateAttachments |
| CostCenterCategoryService | 6 | Standard tree pattern |

**Deliverable:** One PR. After merge, this is the reference pattern for all parallel agents.

---

## Wave 1: Core Business Entities (3 parallel agents)

### Implement Journal Group (#4)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| JournalService | 10 (incl. exports) | UpdateAttachments, UpdateRecurrence; items JSON array for collective entries |
| JournalImportService | 4 | Non-standard: Read, List, Create, Execute |
| JournalImportEntryService | 10 (incl. exports) | Non-standard: Confirm, Unconfirm, Restore, Ignore |

### Implement Order Group (#5)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| OrderService | 16 (incl. exports) | UpdateStatus, UpdateRecurrence, Continue, Dossier ops |
| BookEntryService | 5 | Standard CRUD |
| OrderCategoryService | 7 | Includes Reorder, ReadStatus |
| DocumentService | 5 | Read, Mail, Update + PDF/ZIP download |
| DocumentTemplateService | 5 | Standard CRUD |
| OrderLayoutService | 5 | NEW -- missing from codebase entirely |
| OrderPaymentService | 2 | NEW -- Create + Download |

### Implement Person Group (#6)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| PersonService | 11 (incl. exports + vCard) | Categorize, UpdateAttachments |
| PersonCategoryService | 6 | Standard tree pattern |
| PersonImportService | 5 | Import workflow: Create, Mapping, MappingFields, Preview, Execute |
| PersonTitleService | 5 | Standard CRUD |

---

## Wave 2: Supporting Entities (3-4 parallel agents)

### Implement Common Group (#7)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| CurrencyService | 6 | Non-standard GetExchangeRate |
| CustomFieldService | 7 | Reorder, GetTypes; List requires type param |
| CustomFieldGroupService | 6 | Reorder; List requires type param |
| RoundingService | 5 | Standard CRUD |
| SequenceNumberService | 6 | Add missing `get` endpoint constant |
| TaxRateService | 5 | Standard CRUD |
| TextTemplateService | 5 | Standard CRUD |
| HistoryService | 1 | NEW -- List only |

### Implement File Group (#8)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| FileService | 14 (incl. exports) | GetContent, Prepare (multipart), Persist, Categorize, EmptyArchive, Restore |
| FileCategoryService | 6 | Standard tree pattern |

### Implement Inventory Group (remaining) (#9)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| FixedAssetService | 10 (incl. exports) | Categorize, UpdateAttachments |
| FixedAssetCategoryService | 6 | Standard tree pattern |
| InventoryImportService | 5 | Import workflow |
| UnitService | 5 | Standard CRUD |
| + ArticleCategoryService.GetTree | 1 | Missing from existing implementation |
| + Article/ArticleCategory exports | 6 | .xlsx/.csv/.pdf for both |

### Implement Meta Group (#10)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| FiscalPeriodService | 15 | Complex: Switch, Result, Depreciations, BookDepreciations, ExchangeDiff, BookExchangeDiff, Complete, Reopen, CompleteMonths, ReopenMonths |
| FiscalPeriodTaskService | 3 | List, Create, Delete |
| LocationService | 5 | Standard CRUD |
| OrganizationService | 1 | GetLogo only (binary) |
| SettingsService | 3 | Read, Get, Update (no Create/Delete) |

---

## Wave 3: Report & Salary

### Implement Report Group (#11)

| Service | Endpoints | Notes |
|---------|-----------|-------|
| ReportService | 1 | GetTree only |
| ReportElementService | 11 (incl. downloads) | Data (JSON/HTML), Meta, Reorder, Download (PDF/CSV/Excel) |
| ReportSetService | 10 (incl. downloads) | API calls it "Collection", codebase calls it "Set" -- fix paths; includes `download_annualreport.pdf` special endpoint |

### Implement Salary Group (#12)

**Prerequisite:** Architect agent scrapes CashCtrl HTML docs and produces a Salary section for api-reference.md with endpoint tables, parameter details, and model specifications.

~15 services, ~80 endpoints. Requires:
- New `ISalaryConnector` interface and `SalaryConnector` class
- New `SalaryEndpoints` constants
- Adding `ISalaryConnector` to `ICashCtrlApiClient` and `CashCtrlApiClient`

---

## Per-Service Implementation Pattern

Every service follows this checklist (derived from `doc/api-reference.md`):

### Step 1: Models

`src/CashCtrlApiNet.Abstractions/Models/{Group}/{Entity}/`

- `{Entity}Create.cs` -- record inheriting `ModelBaseRecord`, all POST parameters
- `{Entity}Update.cs` -- record inheriting `{Entity}Create`, adds `required int Id`
- `{Entity}.cs` / `{Entity}Listed.cs` -- inherits Update, adds read-only server fields
- Enums in shared `Enums/` folder for fixed-value fields
- All properties: `[JsonPropertyName]`, `required` where mandatory, `init` accessors, `[MaxLength]` where specified, `ImmutableArray<T>` for collections

### Step 2: Interface

`src/CashCtrlApiNet/Interfaces/Connectors/{Group}/I{Entity}Service.cs`

- Populate existing empty interface with method signatures
- XML doc comments with `<a href>` to CashCtrl API docs
- Export/download methods return `Task<ApiResult<BinaryResponse>>`

### Step 3: Service

`src/CashCtrlApiNet/Services/Connectors/{Group}/{Entity}Service.cs`

- Primary constructor, endpoint alias, expression-bodied members
- `/// <inheritdoc />` on all methods

### Step 4: Wiring

`src/CashCtrlApiNet/Services/Connectors/{Group}Connector.cs`

- Uncomment/add service instantiation

### Step 5: Endpoints

`src/CashCtrlApiNet/Services/Endpoints/{Group}Endpoints.cs`

- Add any missing endpoint constants

### Step 6: Unit Tests

`src/CashCtrlApiNet.UnitTests/{Group}/{Entity}ServiceTests.cs`

- Mock `ICashCtrlConnectionHandler` via NSubstitute
- Per method: correct endpoint, correct HTTP method, request serialization, response deserialization
- Edge cases: optional params, collections, nulls
- Shouldly assertions

---

## Conventions Checklist

Every `.cs` file must include (from `doc/api-reference.md`):

1. MIT License header (lines 1-24)
2. File-scoped namespace
3. XML doc comments on every public type and member
4. `record` types for all models inheriting `ModelBaseRecord`
5. `[JsonPropertyName("camelCaseApiName")]` matching CashCtrl API exactly
6. `required` on mandatory properties
7. `init` accessors on all record properties
8. `[MaxLength(n)]` where API specifies
9. `ImmutableArray<T>` for collections
10. `[Optional] CancellationToken` on every async method
11. Primary constructors on service classes
12. `using Endpoint = ...Endpoints.{Entity}` alias
13. Expression-bodied service methods
14. `/// <inheritdoc />` on service methods
15. `/// <inheritdoc cref="..." />` on service class declarations
16. `internal static class` for endpoint classes
17. Nullable reference types enabled

---

## Quality Gates

### Agent-side (before PR):

- [ ] `dotnet build` passes with zero warnings
- [ ] All unit tests pass
- [ ] Every public type/member has XML doc comments
- [ ] Every endpoint from api-reference.md table is covered
- [ ] Export/binary endpoints included

### Review gate (TL + verification agent):

- [ ] Model properties match CashCtrl API docs exactly
- [ ] Conventions checklist satisfied
- [ ] No regressions -- existing tests pass
- [ ] Every service method has at least one unit test

### Cross-wave consistency (between waves):

- [ ] Pattern audit after Wave 1 merge
- [ ] Deviations corrected before Wave 2

### Salary special gate:

- [ ] Architect produces api-reference.md Salary section first
- [ ] Section reviewed before implementation

---

## Issue Summary

| Issue | Title | Wave | Services | Est. Endpoints |
|-------|-------|------|----------|-----------|
| [#3](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/3) | Phase 0: Infrastructure, bug fixes & Account golden reference | Pre | 5 | ~43 |
| [#4](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/4) | Implement Journal group | Wave 1 | 3 | ~24 |
| [#5](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/5) | Implement Order group | Wave 1 | 7 | ~45 |
| [#6](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/6) | Implement Person group | Wave 1 | 4 | ~27 |
| [#7](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/7) | Implement Common group | Wave 2 | 8 | ~41 |
| [#8](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/8) | Implement File group | Wave 2 | 2 | ~20 |
| [#9](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/9) | Implement Inventory group (remaining) | Wave 2 | 6 | ~33 |
| [#10](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/10) | Implement Meta group | Wave 2 | 5 | ~27 |
| [#11](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/11) | Implement Report group | Wave 3 | 3 | ~22 |
| [#12](https://github.com/AMANDA-Technology/CashCtrlApiNet/issues/12) | Implement Salary group | Wave 3 | ~15 | ~80 |
| **Total** | | | **~58** | **~362** |

**Labels:** `wave-0`, `wave-1`, `wave-2`, `wave-3`
**Milestone:** "100% API Coverage"
