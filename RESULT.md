# Phase 3: ListParams Overloads for Common Group Services

## Summary

Added `GetList(ListParams listParams, CancellationToken cancellationToken)` overloads to all 5 Common group services, following the established pattern from Phase 1 (Account) and Phase 2 (Journal/Order/Person).

## Changes Made

### 15 files edited (3 per service x 5 services):

#### 1. CurrencyService
- **Interface:** `src/CashCtrlApiNet/Interfaces/Connectors/Common/ICurrencyService.cs` -- added `GetList(ListParams, CancellationToken)` declaration with XML doc
- **Service:** `src/CashCtrlApiNet/Services/Connectors/Common/CurrencyService.cs` -- added implementation delegating to `ConnectionHandler.GetAsync<ListResponse<CurrencyListed>, ListParams>`
- **Tests:** `src/CashCtrlApiNet.Tests/Common/CurrencyServiceTests.cs` -- added 2 tests: endpoint verification and return value verification

#### 2. RoundingService
- **Interface:** `src/CashCtrlApiNet/Interfaces/Connectors/Common/IRoundingService.cs` -- added `GetList(ListParams, CancellationToken)` declaration with XML doc
- **Service:** `src/CashCtrlApiNet/Services/Connectors/Common/RoundingService.cs` -- added implementation
- **Tests:** `src/CashCtrlApiNet.Tests/Common/RoundingServiceTests.cs` -- added 2 tests

#### 3. SequenceNumberService
- **Interface:** `src/CashCtrlApiNet/Interfaces/Connectors/Common/ISequenceNumberService.cs` -- added `GetList(ListParams, CancellationToken)` declaration with XML doc
- **Service:** `src/CashCtrlApiNet/Services/Connectors/Common/SequenceNumberService.cs` -- added implementation
- **Tests:** `src/CashCtrlApiNet.Tests/Common/SequenceNumberServiceTests.cs` -- added 2 tests

#### 4. TaxRateService
- **Interface:** `src/CashCtrlApiNet/Interfaces/Connectors/Common/ITaxRateService.cs` -- added `GetList(ListParams, CancellationToken)` declaration with XML doc
- **Service:** `src/CashCtrlApiNet/Services/Connectors/Common/TaxRateService.cs` -- added implementation
- **Tests:** `src/CashCtrlApiNet.Tests/Common/TaxRateServiceTests.cs` -- added 2 tests

#### 5. TextTemplateService
- **Interface:** `src/CashCtrlApiNet/Interfaces/Connectors/Common/ITextTemplateService.cs` -- added `GetList(ListParams, CancellationToken)` declaration with XML doc
- **Service:** `src/CashCtrlApiNet/Services/Connectors/Common/TextTemplateService.cs` -- added implementation
- **Tests:** `src/CashCtrlApiNet.Tests/Common/TextTemplateServiceTests.cs` -- added 2 tests

## Verification

- **Build:** `dotnet build CashCtrlApiNet.sln` -- succeeded, 0 warnings, 0 errors
- **Common group tests:** 51 passed, 0 failed (41 existing + 10 new)
- **Full unit test suite:** 440 passed (13 pre-existing integration test failures due to missing API credentials -- not related to this change)

## Test Details

Each service received 2 new tests:
1. `GetList_WithListParams_ShouldCallCorrectEndpoint` -- verifies the correct endpoint and ListParams are passed to the connection handler
2. `GetList_WithListParams_ShouldReturnResult` -- verifies the result from the connection handler is returned correctly
