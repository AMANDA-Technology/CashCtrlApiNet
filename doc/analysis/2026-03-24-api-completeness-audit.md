# CashCtrl API Completeness Audit

**Date:** 2026-03-24
**Scope:** Full review of CashCtrlApiNet library against live CashCtrl REST API v1

---

## Executive Summary

**API Coverage: 100% (375/375 live endpoints)**

After removing 6 phantom endpoints that did not exist in the CashCtrl API, the library now provides exact 1:1 coverage of the entire CashCtrl REST API v1.

| Metric | Count |
|--------|-------|
| Live API endpoints | 375 |
| Endpoint constants | 375 |
| Interface methods | 375 |
| Service implementations | 375 |
| Unit tests | 393 |
| Domain groups | 10 |
| Services | 58 |
| Connectors (all wired) | 10 |

---

## Per-Group Breakdown

| Group | Services | API Endpoints | Implemented | Tests | Status |
|-------|----------|---------------|-------------|-------|--------|
| Account | 5 | 43 | 43 | 43 | PASS |
| Common | 8 | 41 | 41 | 41 | PASS |
| File | 2 | 20 | 20 | 20 | PASS |
| Inventory | 6 | 42 | 42 | 43 | PASS |
| Journal | 3 | 24 | 24 | 24 | PASS |
| Meta | 5 | 27 | 27 | 27 | PASS |
| Order | 6 | 39 | 39 | 39 | PASS |
| Person | 4 | 27 | 27 | 27 | PASS |
| Report | 3 | 22 | 22 | 22 | PASS |
| Salary | 16 | 90 | 90 | 90 | PASS |
| **TOTAL** | **58** | **375** | **375** | **376+17 infra** | **PASS** |

---

## Corrections Applied

### Removed: `order/template/*` (5 phantom endpoints)

The `DocumentTemplateService` targeted `order/template/read.json`, `list.json`, `create.json`, `update.json`, `delete.json` -- none of which exist in the CashCtrl API. The entire sub-service (interface, service, models, endpoints, tests) was removed.

### Removed: `order/status_info.json` (1 phantom endpoint)

The `OrderService.GetStatus` method targeted `order/status_info.json` which is not documented in the CashCtrl API. The endpoint constant, interface method, service implementation, and test were removed.

Note: `OrderCategoryService.ReadStatus` (`order/category/read_status.json`) is a valid API endpoint and was correctly retained.

---

## Follow-up Recommendations

### P1 -- Should do

1. **Add filter/pagination parameters to `GetList` methods** -- Many list endpoints accept optional parameters (`filter`, `dir`, `sort`, `query`, `categoryId`, `onlyNotes`, `onlyActive`, `fiscalPeriodId`, etc.) not currently exposed. Services call `GetList()` without parameters.

2. **Update `doc/api-reference.md`** -- The implementation status tables are outdated (show most services as unimplemented). Either update to reflect 100% status or mark as superseded by this audit.

### P2 -- Should consider

3. **Integration test modernization** -- The 13 integration tests use xUnit + FluentAssertions and require live API credentials. Consider adding mocked integration test scenarios using the existing NSubstitute infrastructure.

4. **ASP.NET Core DI package** -- `CashCtrlApiNet.AspNetCore` is still empty. Implement `AddCashCtrl()` extension method for `IServiceCollection`.

5. **`ReportSet` vs `ReportCollection` naming** -- The codebase uses "Set" while the API uses "Collection". Consider renaming for clarity or adding XML doc comments explaining the mapping.

### P3 -- Nice to have

6. **`HttpClient` via `IHttpClientFactory`** -- Currently `HttpClient` is created directly in `CashCtrlConnectionHandler`. Using `IHttpClientFactory` would improve DI integration and connection pooling.

7. **Remove `FluentAssertions` dependency** -- Unit tests use Shouldly. Only integration tests use FluentAssertions. Consider migrating to a single assertion library.

---

## CLAUDE.md Update Suggestions

1. Update Implementation Status table to show 100% across all 10 groups
2. Update Tech Stack: .NET 10, add NSubstitute + Shouldly for unit tests
3. Update Known Constraints: remove "uninitialized properties" point, add note about unit vs integration tests
4. Add API Completeness section referencing this audit
5. Add `ServiceTestBase.cs` to Important File Locations

---

## Endpoint Count by Live API Section

| Section | Count |
|---------|-------|
| Account | 11 |
| Bank Account | 9 |
| Account Category | 6 |
| Cost Center | 11 |
| Cost Center Category | 6 |
| Currency | 6 |
| Custom Field | 7 |
| Custom Field Group | 6 |
| History | 1 |
| Rounding | 5 |
| Sequence Number | 6 |
| Tax Rate | 5 |
| Text Template | 5 |
| File | 14 |
| File Category | 6 |
| Article | 10 |
| Article Category | 6 |
| Fixed Asset | 10 |
| Fixed Asset Category | 6 |
| Article Import | 5 |
| Unit | 5 |
| Journal | 10 |
| Journal Import | 4 |
| Journal Import Entry | 10 |
| Fiscal Period | 15 |
| Fiscal Period Task | 3 |
| Location | 5 |
| Organization | 1 |
| Settings | 3 |
| Order | 15 |
| Order Book Entry | 5 |
| Order Category | 7 |
| Order Document | 5 |
| Order Layout | 5 |
| Order Payment | 2 |
| Person | 11 |
| Person Category | 6 |
| Person Import | 5 |
| Person Title | 5 |
| Report | 1 |
| Report Element | 11 |
| Report Collection | 10 |
| Salary Book Entry | 5 |
| Salary Category | 6 |
| Salary Certificate | 6 |
| Salary Certificate Document | 4 |
| Salary Certificate Template | 6 |
| Salary Document | 5 |
| Salary Field | 2 |
| Salary Insurance Type | 5 |
| Salary Layout | 5 |
| Salary Payment | 2 |
| Salary Setting | 5 |
| Salary Statement | 13 |
| Salary Status | 6 |
| Salary Sum | 5 |
| Salary Template | 6 |
| Salary Type | 9 |
| **TOTAL** | **375** |
