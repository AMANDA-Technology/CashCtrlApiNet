# Phase 0: Infrastructure, Bug Fixes & Account Golden Reference

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fix known bugs, add binary HTTP support, establish unit test infrastructure with NSubstitute + Shouldly, and fully implement the Account domain group as the golden reference for all subsequent domain groups.

**Architecture:** The existing connector pattern (Endpoint constants -> Interface -> Service -> Connector wiring) is preserved. We add `GetBinaryAsync` to the connection handler for export endpoints, create `ServiceTestBase<TService>` for unit testing with mocked HTTP, and implement 5 Account services (Account, AccountBank, AccountCategory, CostCenter, CostCenterCategory) as the reference pattern.

**Tech Stack:** .NET 9, C# 13, xUnit 2.9, NSubstitute, Shouldly, System.Text.Json

**Reference files:**
- Design spec: `doc/specs/2026-03-23-full-api-implementation-design.md`
- API reference: `doc/api-reference.md` (authoritative source for all endpoint parameters)
- Model pattern: `src/CashCtrlApiNet.Abstractions/Models/Inventory/Article/ArticleCreate.cs`
- Service pattern: `src/CashCtrlApiNet/Services/Connectors/Account/AccountService.cs` (current, has bugs to fix)
- Interface pattern: `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountService.cs`
- Endpoint pattern: `src/CashCtrlApiNet/Services/Endpoints/AccountEndpoints.cs`
- Connection handler: `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs`

---

## File Map

### New files

| File | Purpose |
|------|---------|
| `src/CashCtrlApiNet.Tests/ServiceTestBase.cs` | Base class for unit tests with mocked ICashCtrlConnectionHandler |
| `src/CashCtrlApiNet.Tests/Account/AccountServiceTests.cs` | Unit tests for AccountService |
| `src/CashCtrlApiNet.Tests/Account/AccountBankServiceTests.cs` | Unit tests for AccountBankService |
| `src/CashCtrlApiNet.Tests/Account/AccountCategoryServiceTests.cs` | Unit tests for AccountCategoryService |
| `src/CashCtrlApiNet.Tests/Account/CostCenterServiceTests.cs` | Unit tests for CostCenterService |
| `src/CashCtrlApiNet.Tests/Account/CostCenterCategoryServiceTests.cs` | Unit tests for CostCenterCategoryService |
| `src/CashCtrlApiNet.Abstractions/Models/Account/AccountListed.cs` | Account list response model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Bank/AccountBankCreate.cs` | Bank account create model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Bank/AccountBankUpdate.cs` | Bank account update model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Bank/AccountBank.cs` | Bank account read model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Category/AccountCategoryCreate.cs` | Account category create model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Category/AccountCategoryUpdate.cs` | Account category update model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Category/AccountCategory.cs` | Account category read model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenterCreate.cs` | Cost center create model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenterUpdate.cs` | Cost center update model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenter.cs` | Cost center read model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenterListed.cs` | Cost center list model |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/CostCenterCategoryCreate.cs` | Cost center category create |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/CostCenterCategoryUpdate.cs` | Cost center category update |
| `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/CostCenterCategory.cs` | Cost center category read |
| `src/CashCtrlApiNet.Abstractions/Models/Api/BinaryResponse.cs` | Response wrapper for binary data |
| `src/CashCtrlApiNet.Abstractions/Enums/Account/BankAccountType.cs` | Enum: DEFAULT, ORDER, SALARY, HISTORICAL, OTHER |
| `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountBankService.cs` | Bank account service interface |
| `src/CashCtrlApiNet/Services/Connectors/Account/AccountBankService.cs` | Bank account service |
| `src/CashCtrlApiNet/Services/Connectors/Account/AccountCategoryService.cs` | Account category service |
| `src/CashCtrlApiNet/Services/Connectors/Account/CostCenterService.cs` | Cost center service |
| `src/CashCtrlApiNet/Services/Connectors/Account/CostCenterCategoryService.cs` | Cost center category service |

### Modified files

| File | Changes |
|------|---------|
| `src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj` | Add NSubstitute + Shouldly packages |
| `src/CashCtrlApiNet/Interfaces/ICashCtrlConnectionHandler.cs` | Add `GetBinaryAsync` method |
| `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs` | Implement `GetBinaryAsync` |
| `src/CashCtrlApiNet.Abstractions/Models/Account/AccountCreate.cs` | Populate with properties |
| `src/CashCtrlApiNet.Abstractions/Models/Account/AccountUpdate.cs` | Fix inheritance, add Id |
| `src/CashCtrlApiNet.Abstractions/Models/Account/Account.cs` | Fix inheritance, add server fields |
| `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountService.cs` | Fix Get(Entry), add GetBalance, exports |
| `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountCategoryService.cs` | Populate methods |
| `src/CashCtrlApiNet/Interfaces/Connectors/Account/ICostCenterService.cs` | Populate methods |
| `src/CashCtrlApiNet/Interfaces/Connectors/Account/ICostCenterCategoryService.cs` | Populate methods |
| `src/CashCtrlApiNet/Interfaces/Connectors/IAccountConnector.cs` | Add IAccountBankService property |
| `src/CashCtrlApiNet/Services/Connectors/Account/AccountService.cs` | Fix Get, add GetBalance, exports |
| `src/CashCtrlApiNet/Services/Connectors/AccountConnector.cs` | Fix bugs, wire all services |
| `src/CashCtrlApiNet/Services/Endpoints/AccountEndpoints.cs` | Add Bank, export endpoints |
| `src/CashCtrlApiNet/Services/Endpoints/PersonEndpoints.cs` | Fix bug: rename Order -> Person |
| `src/CashCtrlApiNet/Services/Endpoints/FileEndpoints.cs` | Fix bug: rename ArticleCategory -> FileCategory |
| `src/CashCtrlApiNet/Services/Endpoints/ReportEndpoints.cs` | Fix bug: report/set/ -> report/collection/ |
| `src/CashCtrlApiNet/Services/Connectors/ReportConnector.cs` | Fix bug: IReportService -> ReportService |

---

## Task 1: Add NSubstitute + Shouldly to Test Project

**Files:**
- Modify: `src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj`

- [ ] **Step 1: Add NSubstitute and Shouldly packages**

Add to the `<ItemGroup>` with other PackageReferences:

```xml
<PackageReference Include="NSubstitute" Version="5.3.0" />
<PackageReference Include="Shouldly" Version="4.2.1" />
```

- [ ] **Step 2: Restore and verify build**

Run: `dotnet restore src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj && dotnet build src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj`
Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj
git commit -m "chore: add NSubstitute and Shouldly test packages"
```

---

## Task 2: Create Unit Test Infrastructure

**Files:**
- Create: `src/CashCtrlApiNet.Tests/ServiceTestBase.cs`

- [ ] **Step 1: Create ServiceTestBase**

```csharp
// MIT License header (copy from existing files)

using CashCtrlApiNet.Interfaces;
using NSubstitute;

namespace CashCtrlApiNet.Tests;

/// <summary>
/// Base class for unit tests with mocked ICashCtrlConnectionHandler
/// </summary>
public abstract class ServiceTestBase<TService> where TService : class
{
    /// <summary>
    /// Mocked connection handler
    /// </summary>
    protected readonly ICashCtrlConnectionHandler ConnectionHandler;

    /// <summary>
    /// The service under test
    /// </summary>
    protected readonly TService Service;

    /// <summary>
    /// Setup with mocked connection handler
    /// </summary>
    protected ServiceTestBase()
    {
        ConnectionHandler = Substitute.For<ICashCtrlConnectionHandler>();
        Service = CreateService(ConnectionHandler);
    }

    /// <summary>
    /// Create the service under test with the given connection handler
    /// </summary>
    /// <param name="connectionHandler"></param>
    /// <returns></returns>
    protected abstract TService CreateService(ICashCtrlConnectionHandler connectionHandler);
}
```

- [ ] **Step 2: Verify build**

Run: `dotnet build src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj`
Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add src/CashCtrlApiNet.Tests/ServiceTestBase.cs
git commit -m "feat: add ServiceTestBase for unit tests with mocked connection handler"
```

---

## Task 3: Fix Known Bugs (Non-Account)

**Files:**
- Modify: `src/CashCtrlApiNet/Services/Endpoints/PersonEndpoints.cs` (rename inner class `Order` -> `Person`)
- Modify: `src/CashCtrlApiNet/Services/Endpoints/FileEndpoints.cs` (rename inner class `ArticleCategory` -> `FileCategory`)
- Modify: `src/CashCtrlApiNet/Services/Endpoints/ReportEndpoints.cs` (change `report/set/` -> `report/collection/`)
- Modify: `src/CashCtrlApiNet/Services/Connectors/ReportConnector.cs` (change `new IReportService(...)` -> `new ReportService(...)`)

- [ ] **Step 1: Read the 4 files to identify exact lines**

Read each file. Identify the exact lines with bugs.

- [ ] **Step 2: Fix PersonEndpoints.cs**

In `PersonEndpoints.cs`, find the inner static class named `Order` (should be around line 43) and rename it to `Person`.

- [ ] **Step 3: Fix FileEndpoints.cs**

In `FileEndpoints.cs`, find the inner static class named `ArticleCategory` (should be around line 109) and rename it to `FileCategory`.

- [ ] **Step 4: Fix ReportEndpoints.cs**

Change all `report/set/` path references to `report/collection/`. The inner class can stay named `Set` (codebase convention) but the URL paths must use `collection`.

- [ ] **Step 5: Fix ReportConnector.cs**

In the commented-out lines (around lines 41-43), change `new IReportService(...)`, `new IReportElementService(...)`, `new IReportSetService(...)` to `new ReportService(...)`, `new ReportElementService(...)`, `new ReportSetService(...)`. Keep them commented out (they'll be uncommented when Report group is implemented).

- [ ] **Step 6: Verify build**

Run: `dotnet build CashCtrlApiNet.sln`
Expected: Build succeeded with no warnings.

- [ ] **Step 7: Commit**

```bash
git add src/CashCtrlApiNet/Services/Endpoints/PersonEndpoints.cs \
       src/CashCtrlApiNet/Services/Endpoints/FileEndpoints.cs \
       src/CashCtrlApiNet/Services/Endpoints/ReportEndpoints.cs \
       src/CashCtrlApiNet/Services/Connectors/ReportConnector.cs
git commit -m "fix: correct naming bugs in PersonEndpoints, FileEndpoints, ReportEndpoints, and ReportConnector"
```

---

## Task 4: Add Binary HTTP Support

**Files:**
- Create: `src/CashCtrlApiNet.Abstractions/Models/Api/BinaryResponse.cs`
- Modify: `src/CashCtrlApiNet/Interfaces/ICashCtrlConnectionHandler.cs`
- Modify: `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs`

- [ ] **Step 1: Write failing test for GetBinaryAsync**

Create `src/CashCtrlApiNet.Tests/Infrastructure/ConnectionHandlerBinaryTests.cs`:

```csharp
// MIT License header

using CashCtrlApiNet.Interfaces;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Infrastructure;

/// <summary>
/// Tests for binary HTTP support on ICashCtrlConnectionHandler
/// </summary>
public class ConnectionHandlerBinaryTests
{
    [Fact]
    public void ICashCtrlConnectionHandler_ShouldDeclare_GetBinaryAsync()
    {
        // Verify the method exists on the interface via reflection
        var method = typeof(ICashCtrlConnectionHandler).GetMethod("GetBinaryAsync");
        method.ShouldNotBeNull();
        method.ReturnType.ShouldBe(typeof(Task<byte[]>));
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "ConnectionHandlerBinaryTests"`
Expected: FAIL -- method not found.

- [ ] **Step 3: Create BinaryResponse**

```csharp
// MIT License header

using CashCtrlApiNet.Abstractions.Models.Api.Base;

namespace CashCtrlApiNet.Abstractions.Models.Api;

/// <summary>
/// API response wrapper for binary data (export endpoints: .xlsx, .csv, .pdf)
/// </summary>
public record BinaryResponse : ApiResponse
{
    /// <summary>
    /// The raw binary data from the response
    /// </summary>
    public required byte[] Data { get; init; }

    /// <summary>
    /// The content type of the response (e.g., application/pdf)
    /// </summary>
    public string? ContentType { get; init; }

    /// <summary>
    /// The suggested filename from Content-Disposition header, if present
    /// </summary>
    public string? FileName { get; init; }
}
```

- [ ] **Step 4: Add GetBinaryAsync to ICashCtrlConnectionHandler**

Add to the interface:

```csharp
/// <summary>
/// Base GET request for binary content (export endpoints: .xlsx, .csv, .pdf)
/// </summary>
/// <param name="requestPath"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
public Task<byte[]> GetBinaryAsync(string requestPath, [Optional] CancellationToken cancellationToken);

/// <summary>
/// Base GET request for binary content with query parameters
/// </summary>
/// <param name="requestPath"></param>
/// <param name="queryParameters"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
public Task<byte[]> GetBinaryAsync<TQuery>(string requestPath, TQuery queryParameters, [Optional] CancellationToken cancellationToken);
```

- [ ] **Step 5: Implement GetBinaryAsync in CashCtrlConnectionHandler**

Add to `CashCtrlConnectionHandler.cs`:

```csharp
/// <inheritdoc />
public async Task<byte[]> GetBinaryAsync(string requestPath, [Optional] CancellationToken cancellationToken)
{
    var response = await _client.SendAsync(GetHttpRequestMessage<object>(HttpMethod.Get, requestPath), cancellationToken);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsByteArrayAsync(cancellationToken);
}

/// <inheritdoc />
public async Task<byte[]> GetBinaryAsync<TQuery>(string requestPath, TQuery queryParameters, [Optional] CancellationToken cancellationToken)
{
    var response = await _client.SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, queryParameters), cancellationToken);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsByteArrayAsync(cancellationToken);
}
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "ConnectionHandlerBinaryTests"`
Expected: PASS.

- [ ] **Step 7: Commit**

```bash
git add src/CashCtrlApiNet.Abstractions/Models/Api/BinaryResponse.cs \
       src/CashCtrlApiNet/Interfaces/ICashCtrlConnectionHandler.cs \
       src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs \
       src/CashCtrlApiNet.Tests/Infrastructure/ConnectionHandlerBinaryTests.cs
git commit -m "feat: add GetBinaryAsync for export/download endpoints"
```

---

## Task 5: Populate Account Models

**Files:**
- Modify: `src/CashCtrlApiNet.Abstractions/Models/Account/AccountCreate.cs`
- Modify: `src/CashCtrlApiNet.Abstractions/Models/Account/AccountUpdate.cs`
- Modify: `src/CashCtrlApiNet.Abstractions/Models/Account/Account.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/AccountListed.cs`

Refer to `doc/api-reference.md` section "Account > Account" for the full parameter list.

- [ ] **Step 1: Populate AccountCreate**

Replace the empty stub with properties from the API docs. Follow the `ArticleCreate.cs` pattern exactly:

Required properties:
- `categoryId` (NUMBER, required) -> `required int CategoryId`
- `name` (TEXT, required, max 100, localized XML) -> `required string Name` with `[MaxLength(100)]`
- `number` (NUMBER, required, max 20) -> `required int Number`

Optional properties:
- `currencyId` (NUMBER) -> `int? CurrencyId`
- `taxId` (NUMBER) -> `int? TaxId`
- `allocations` (JSON) -> `string? AllocationsJson`
- `custom` (XML) -> `string? CustomXml`
- `notes` (HTML) -> `string? NotesHtml`
- `targetMin` (NUMBER) -> `double? TargetMin`
- `targetMax` (NUMBER) -> `double? TargetMax`
- `isInactive` (BOOLEAN) -> `bool? IsInactive`

Every property gets `[JsonPropertyName("camelCaseApiName")]` and `{ get; init; }`.

- [ ] **Step 2: Fix AccountUpdate**

Change inheritance from `ModelBaseRecord` to `AccountCreate`. Add `required int Id` with `[JsonPropertyName("id")]`.

- [ ] **Step 3: Create AccountListed**

Create `AccountListed : AccountUpdate` with read-only server fields:
- `created` (DateTime?) -> `DateTime? Created`
- `createdBy` (string?) -> `string? CreatedBy`
- `lastUpdated` (DateTime?) -> `DateTime? LastUpdated`
- `lastUpdatedBy` (string?) -> `string? LastUpdatedBy`

Use `[JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]` on DateTime fields.

- [ ] **Step 4: Fix Account model**

Change `Account : AccountListed` and add detail-only fields (e.g., `attachments` as `ImmutableArray<int>?`).

- [ ] **Step 5: Verify build**

Run: `dotnet build CashCtrlApiNet.sln`
Expected: Build succeeded.

- [ ] **Step 6: Commit**

```bash
git add src/CashCtrlApiNet.Abstractions/Models/Account/
git commit -m "feat: populate Account models with properties from API docs"
```

---

## Task 6: Fix AccountService & Interface

**Files:**
- Modify: `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountService.cs`
- Modify: `src/CashCtrlApiNet/Services/Connectors/Account/AccountService.cs`
- Modify: `src/CashCtrlApiNet/Services/Endpoints/AccountEndpoints.cs`

- [ ] **Step 1: Write failing tests for AccountService**

Create `src/CashCtrlApiNet.Tests/Account/AccountServiceTests.cs`:

```csharp
// MIT License header

using CashCtrlApiNet.Abstractions.Models.Account;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services.Connectors.Account;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Account;

/// <summary>
/// Unit tests for AccountService
/// </summary>
public class AccountServiceTests : ServiceTestBase<AccountService>
{
    /// <inheritdoc />
    protected override AccountService CreateService(ICashCtrlConnectionHandler connectionHandler)
        => new(connectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        // Arrange
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Abstractions.Models.Account.Account>>());

        // Act
        await Service.Get(entry);

        // Assert
        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                AccountEndpoints.Account.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        // Arrange
        ConnectionHandler
            .GetAsync<ListResponse<AccountListed>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<AccountListed>>());

        // Act
        await Service.GetList();

        // Assert
        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<AccountListed>>(
                AccountEndpoints.Account.List, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        // Arrange
        var account = new AccountCreate { CategoryId = 1, Name = "Test", Number = 1000 };
        ConnectionHandler
            .PostAsync<NoContentResponse, AccountCreate>(Arg.Any<string>(), Arg.Any<AccountCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        // Act
        await Service.Create(account);

        // Assert
        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, AccountCreate>(
                AccountEndpoints.Account.Create, account, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        // Arrange
        var account = new AccountUpdate { Id = 1, CategoryId = 1, Name = "Test", Number = 1000 };
        ConnectionHandler
            .PostAsync<NoContentResponse, AccountUpdate>(Arg.Any<string>(), Arg.Any<AccountUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        // Act
        await Service.Update(account);

        // Assert
        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, AccountUpdate>(
                AccountEndpoints.Account.Update, account, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        // Arrange
        var entries = new Entries { Ids = [1, 2, 3] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        // Act
        await Service.Delete(entries);

        // Assert
        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                AccountEndpoints.Account.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBalance_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Abstractions.Models.Account.Account>>());

        // Act
        await Service.GetBalance(entry);

        // Assert
        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                AccountEndpoints.Account.Balance, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        // Arrange
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new byte[] { 1, 2, 3 });

        // Act
        var result = await Service.ExportExcel();

        // Assert
        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.Account.ListXlsx, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
```

- [ ] **Step 2: Run tests to verify they fail**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "AccountServiceTests"`
Expected: FAIL -- methods don't exist yet.

- [ ] **Step 3: Add export endpoint constants to AccountEndpoints.cs**

Add to the `Account` inner class:

```csharp
/// <summary>
/// Endpoint to export accounts as Excel
/// </summary>
public const string ListXlsx = $"{Root}/list.xlsx";

/// <summary>
/// Endpoint to export accounts as CSV
/// </summary>
public const string ListCsv = $"{Root}/list.csv";

/// <summary>
/// Endpoint to export accounts as PDF
/// </summary>
public const string ListPdf = $"{Root}/list.pdf";
```

Also add similar export constants to `CostCenter` inner class.

- [ ] **Step 4: Fix IAccountService**

- Change `Get(int accountId, ...)` to `Get(Entry account, ...)`
- Change GetList return type from `Account` to `AccountListed`
- Add `GetBalance(Entry account, ...)` -> `Task<ApiResult<SingleResponse<Account>>>`
- Add `ExportExcel(...)` -> `Task<byte[]>`
- Add `ExportCsv(...)` -> `Task<byte[]>`
- Add `ExportPdf(...)` -> `Task<byte[]>`

- [ ] **Step 5: Fix AccountService implementation**

- Fix `Get` to use `Entry` and pass it as query parameter via `GetAsync<TResult, TQuery>`
- Change `GetList` to return `AccountListed`
- Add `GetBalance` using `AccountEndpoints.Account.Balance`
- Add export methods using `ConnectionHandler.GetBinaryAsync`

```csharp
/// <inheritdoc />
public Task<ApiResult<SingleResponse<Abstractions.Models.Account.Account>>> Get(Entry account, [Optional] CancellationToken cancellationToken)
    => ConnectionHandler.GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(Endpoint.Read, account, cancellationToken: cancellationToken);

/// <inheritdoc />
public Task<ApiResult<ListResponse<AccountListed>>> GetList([Optional] CancellationToken cancellationToken)
    => ConnectionHandler.GetAsync<ListResponse<AccountListed>>(Endpoint.List, cancellationToken: cancellationToken);

/// <inheritdoc />
public Task<ApiResult<SingleResponse<Abstractions.Models.Account.Account>>> GetBalance(Entry account, [Optional] CancellationToken cancellationToken)
    => ConnectionHandler.GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(Endpoint.Balance, account, cancellationToken: cancellationToken);

/// <inheritdoc />
public Task<byte[]> ExportExcel([Optional] CancellationToken cancellationToken)
    => ConnectionHandler.GetBinaryAsync(Endpoint.ListXlsx, cancellationToken: cancellationToken);

/// <inheritdoc />
public Task<byte[]> ExportCsv([Optional] CancellationToken cancellationToken)
    => ConnectionHandler.GetBinaryAsync(Endpoint.ListCsv, cancellationToken: cancellationToken);

/// <inheritdoc />
public Task<byte[]> ExportPdf([Optional] CancellationToken cancellationToken)
    => ConnectionHandler.GetBinaryAsync(Endpoint.ListPdf, cancellationToken: cancellationToken);
```

Add the endpoint alias at top of file: `using Endpoint = CashCtrlApiNet.Services.Endpoints.AccountEndpoints.Account;`

- [ ] **Step 6: Run tests to verify they pass**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "AccountServiceTests"`
Expected: All PASS.

- [ ] **Step 7: Commit**

```bash
git add src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountService.cs \
       src/CashCtrlApiNet/Services/Connectors/Account/AccountService.cs \
       src/CashCtrlApiNet/Services/Endpoints/AccountEndpoints.cs \
       src/CashCtrlApiNet.Tests/Account/AccountServiceTests.cs
git commit -m "fix: fix AccountService Get() to use Entry, add GetBalance and export methods"
```

---

## Task 7: Implement AccountCategoryService

**Files:**
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/Category/AccountCategoryCreate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/Category/AccountCategoryUpdate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/Category/AccountCategory.cs`
- Modify: `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountCategoryService.cs`
- Create: `src/CashCtrlApiNet/Services/Connectors/Account/AccountCategoryService.cs`
- Create: `src/CashCtrlApiNet.Tests/Account/AccountCategoryServiceTests.cs`

Refer to `doc/api-reference.md` section "Account > Category" for parameters:
- Create: `name` (required, max 100, localized), `number` (max 20), `parentId`

- [ ] **Step 1: Write failing tests**

Create `AccountCategoryServiceTests.cs` following the `AccountServiceTests` pattern. Test methods:
- `Get_ShouldCallCorrectEndpoint` (using `AccountEndpoints.AccountCategory.Read`)
- `GetList_ShouldCallCorrectEndpoint` (using `AccountEndpoints.AccountCategory.List`)
- `GetTree_ShouldCallCorrectEndpoint` (using `AccountEndpoints.AccountCategory.Tree`)
- `Create_ShouldPostToCorrectEndpoint`
- `Update_ShouldPostToCorrectEndpoint`
- `Delete_ShouldPostToCorrectEndpoint`

- [ ] **Step 2: Run tests to verify they fail**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "AccountCategoryServiceTests"`
Expected: FAIL.

- [ ] **Step 3: Create AccountCategory models**

`AccountCategoryCreate.cs`:
```csharp
public record AccountCategoryCreate : ModelBaseRecord
{
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public required string Name { get; init; }

    [JsonPropertyName("number")]
    [MaxLength(20)]
    public string? Number { get; init; }

    [JsonPropertyName("parentId")]
    public int? ParentId { get; init; }
}
```

`AccountCategoryUpdate.cs`: inherits `AccountCategoryCreate`, adds `required int Id`.

`AccountCategory.cs`: inherits `AccountCategoryUpdate`, adds server fields (created, lastUpdated, etc.) + `path` (string?), `fullName` (string?).

- [ ] **Step 4: Populate IAccountCategoryService**

Add methods following `IAccountService` pattern:
- `Get(Entry category, ...)` -> `Task<ApiResult<SingleResponse<AccountCategory>>>`
- `GetList(...)` -> `Task<ApiResult<ListResponse<AccountCategory>>>`
- `GetTree(...)` -> `Task<ApiResult<ListResponse<AccountCategory>>>`
- `Create(AccountCategoryCreate, ...)` -> `Task<ApiResult<NoContentResponse>>`
- `Update(AccountCategoryUpdate, ...)` -> `Task<ApiResult<NoContentResponse>>`
- `Delete(Entries, ...)` -> `Task<ApiResult<NoContentResponse>>`

- [ ] **Step 5: Create AccountCategoryService**

Follow `AccountService` pattern with primary constructor, endpoint alias `using Endpoint = AccountEndpoints.AccountCategory;`, expression-bodied members.

- [ ] **Step 6: Run tests to verify they pass**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "AccountCategoryServiceTests"`
Expected: All PASS.

- [ ] **Step 7: Commit**

```bash
git add src/CashCtrlApiNet.Abstractions/Models/Account/Category/ \
       src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountCategoryService.cs \
       src/CashCtrlApiNet/Services/Connectors/Account/AccountCategoryService.cs \
       src/CashCtrlApiNet.Tests/Account/AccountCategoryServiceTests.cs
git commit -m "feat: implement AccountCategoryService with models and unit tests"
```

---

## Task 8: Implement AccountBankService (NEW)

**Files:**
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/Bank/AccountBankCreate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/Bank/AccountBankUpdate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/Bank/AccountBank.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Enums/Account/BankAccountType.cs`
- Create: `src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountBankService.cs`
- Create: `src/CashCtrlApiNet/Services/Connectors/Account/AccountBankService.cs`
- Modify: `src/CashCtrlApiNet/Services/Endpoints/AccountEndpoints.cs` (add Bank inner class)
- Create: `src/CashCtrlApiNet.Tests/Account/AccountBankServiceTests.cs`

Refer to `doc/api-reference.md` section "Account > Bank Account" for parameters:
- Create: `bic` (required, max 11), `iban` (required, max 32), `name` (required, max 100, localized), `type` (required: DEFAULT/ORDER/SALARY/HISTORICAL/OTHER), `accountId`, `currencyId`, `isInactive`, `notes` (HTML), `qrFirstDigits` (max 8), `qrIban` (max 32), `url` (max 255)

- [ ] **Step 1: Write failing tests**

Create `AccountBankServiceTests.cs` with tests for: Get, GetList, Create, Update, Delete, UpdateAttachments, ExportExcel, ExportCsv, ExportPdf.

- [ ] **Step 2: Run tests to verify they fail**

- [ ] **Step 3: Create BankAccountType enum**

```csharp
namespace CashCtrlApiNet.Abstractions.Enums.Account;

/// <summary>
/// Type of bank account
/// </summary>
public enum BankAccountType
{
    DEFAULT,
    ORDER,
    SALARY,
    HISTORICAL,
    OTHER
}
```

- [ ] **Step 4: Create AccountBank models**

`AccountBankCreate.cs` with all properties from api-reference.md. Use `[JsonPropertyName]` on every property. Use `BankAccountType` for the type field.

`AccountBankUpdate.cs`: inherits `AccountBankCreate`, adds `required int Id`.

`AccountBank.cs`: inherits `AccountBankUpdate`, adds server fields.

- [ ] **Step 5: Add Bank endpoints to AccountEndpoints.cs**

Add new inner class:

```csharp
public static class Bank
{
    private const string Root = $"{GroupRoot}/bank";
    public const string Read = $"{Root}/{Default.Read}";
    public const string List = $"{Root}/{Default.List}";
    public const string Create = $"{Root}/{Default.Create}";
    public const string Update = $"{Root}/{Default.Update}";
    public const string Delete = $"{Root}/{Default.Delete}";
    public const string UpdateAttachments = $"{Root}/update_attachments.json";
    public const string ListXlsx = $"{Root}/list.xlsx";
    public const string ListCsv = $"{Root}/list.csv";
    public const string ListPdf = $"{Root}/list.pdf";
}
```

- [ ] **Step 6: Create IAccountBankService interface**

Methods: Get, GetList, Create, Update, Delete, UpdateAttachments, ExportExcel, ExportCsv, ExportPdf.

- [ ] **Step 7: Create AccountBankService**

Primary constructor, endpoint alias `using Endpoint = AccountEndpoints.Bank;`, expression-bodied members.

- [ ] **Step 8: Run tests to verify they pass**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --filter "AccountBankServiceTests"`
Expected: All PASS.

- [ ] **Step 9: Commit**

```bash
git add src/CashCtrlApiNet.Abstractions/Models/Account/Bank/ \
       src/CashCtrlApiNet.Abstractions/Enums/Account/ \
       src/CashCtrlApiNet/Interfaces/Connectors/Account/IAccountBankService.cs \
       src/CashCtrlApiNet/Services/Connectors/Account/AccountBankService.cs \
       src/CashCtrlApiNet/Services/Endpoints/AccountEndpoints.cs \
       src/CashCtrlApiNet.Tests/Account/AccountBankServiceTests.cs
git commit -m "feat: implement AccountBankService with models, endpoints, and unit tests"
```

---

## Task 9: Implement CostCenterService

**Files:**
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenterCreate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenterUpdate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenterListed.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/CostCenter.cs`
- Modify: `src/CashCtrlApiNet/Interfaces/Connectors/Account/ICostCenterService.cs`
- Create: `src/CashCtrlApiNet/Services/Connectors/Account/CostCenterService.cs`
- Create: `src/CashCtrlApiNet.Tests/Account/CostCenterServiceTests.cs`

Refer to `doc/api-reference.md` section "Account > Cost Center" for parameters. Note: CostCenter has `GetBalance`, `Categorize`, `UpdateAttachments`, and exports beyond standard CRUD.

- [ ] **Step 1: Write failing tests**

Test methods: Get, GetList, GetBalance, Create, Update, Delete, Categorize, UpdateAttachments, ExportExcel, ExportCsv, ExportPdf.

- [ ] **Step 2: Run tests to verify they fail**

- [ ] **Step 3: Create CostCenter models**

Follow the same Create -> Update -> Listed -> Entity hierarchy as Account. Properties from api-reference.md.

- [ ] **Step 4: Add export endpoints to AccountEndpoints.CostCenter**

Add `ListXlsx`, `ListCsv`, `ListPdf` constants.

- [ ] **Step 5: Populate ICostCenterService**

Methods: Get, GetList, GetBalance, Create, Update, Delete, Categorize, UpdateAttachments, ExportExcel, ExportCsv, ExportPdf.

- [ ] **Step 6: Create CostCenterService**

- [ ] **Step 7: Run tests to verify they pass**

- [ ] **Step 8: Commit**

```bash
git add src/CashCtrlApiNet.Abstractions/Models/Account/CostCenter/ \
       src/CashCtrlApiNet/Interfaces/Connectors/Account/ICostCenterService.cs \
       src/CashCtrlApiNet/Services/Connectors/Account/CostCenterService.cs \
       src/CashCtrlApiNet.Tests/Account/CostCenterServiceTests.cs
git commit -m "feat: implement CostCenterService with models and unit tests"
```

---

## Task 10: Implement CostCenterCategoryService

**Files:**
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/CostCenterCategoryCreate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/CostCenterCategoryUpdate.cs`
- Create: `src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/CostCenterCategory.cs`
- Modify: `src/CashCtrlApiNet/Interfaces/Connectors/Account/ICostCenterCategoryService.cs`
- Create: `src/CashCtrlApiNet/Services/Connectors/Account/CostCenterCategoryService.cs`
- Create: `src/CashCtrlApiNet.Tests/Account/CostCenterCategoryServiceTests.cs`

Refer to `doc/api-reference.md` section "Account > Cost Center Category". Standard tree pattern (Get, GetList, GetTree, Create, Update, Delete).

- [ ] **Step 1: Write failing tests**

- [ ] **Step 2: Run tests to verify they fail**

- [ ] **Step 3: Create CostCenterCategory models**

Same structure as AccountCategory. Properties: `name` (required, max 100, localized), `parentId`.

- [ ] **Step 4: Populate ICostCenterCategoryService**

- [ ] **Step 5: Create CostCenterCategoryService**

- [ ] **Step 6: Run tests to verify they pass**

- [ ] **Step 7: Commit**

```bash
git add src/CashCtrlApiNet.Abstractions/Models/Account/CostCenterCategory/ \
       src/CashCtrlApiNet/Interfaces/Connectors/Account/ICostCenterCategoryService.cs \
       src/CashCtrlApiNet/Services/Connectors/Account/CostCenterCategoryService.cs \
       src/CashCtrlApiNet.Tests/Account/CostCenterCategoryServiceTests.cs
git commit -m "feat: implement CostCenterCategoryService with models and unit tests"
```

---

## Task 11: Wire Up AccountConnector & Fix Bugs

**Files:**
- Modify: `src/CashCtrlApiNet/Interfaces/Connectors/IAccountConnector.cs`
- Modify: `src/CashCtrlApiNet/Services/Connectors/AccountConnector.cs`

- [ ] **Step 1: Add IAccountBankService to IAccountConnector**

Add property:
```csharp
/// <summary>
/// CashCtrl bank account service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank">API Doc - Account/Bank</a>
/// </summary>
public IAccountBankService Bank { get; }
```

- [ ] **Step 2: Fix AccountConnector**

- Fix the bug on line 45 (comment says `Account = new CostCenterCategoryService(...)` -> fix comment)
- Uncomment all service instantiations
- Add `Bank = new AccountBankService(connectionHandler);`
- Wire all 5 services:

```csharp
public AccountConnector(ICashCtrlConnectionHandler connectionHandler)
{
    Account = new AccountService(connectionHandler);
    Bank = new AccountBankService(connectionHandler);
    Category = new AccountCategoryService(connectionHandler);
    CostCenter = new CostCenterService(connectionHandler);
    CostCenterCategory = new CostCenterCategoryService(connectionHandler);
}
```

- [ ] **Step 3: Verify full solution builds**

Run: `dotnet build CashCtrlApiNet.sln`
Expected: Build succeeded with no warnings.

- [ ] **Step 4: Run all tests**

Run: `dotnet test src/CashCtrlApiNet.Tests/`
Expected: All tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/CashCtrlApiNet/Interfaces/Connectors/IAccountConnector.cs \
       src/CashCtrlApiNet/Services/Connectors/AccountConnector.cs
git commit -m "feat: wire up all Account services in AccountConnector, fix comment bug"
```

---

## Task 12: Final Build Verification & PR

- [ ] **Step 1: Full solution build**

Run: `dotnet build CashCtrlApiNet.sln`
Expected: Build succeeded, 0 warnings.

- [ ] **Step 2: Run all tests**

Run: `dotnet test src/CashCtrlApiNet.Tests/ --verbosity normal`
Expected: All tests pass. Count should be 30+ (5 services x ~6 tests each + infrastructure tests).

- [ ] **Step 3: Verify endpoint coverage**

Check that every endpoint from the api-reference.md Account group tables shows YES/YES:
- AccountService: 11 endpoints (8 JSON + 3 export)
- AccountBankService: 9 endpoints (6 JSON + 3 export)
- AccountCategoryService: 6 endpoints
- CostCenterService: 11 endpoints (8 JSON + 3 export)
- CostCenterCategoryService: 6 endpoints
- Total: 43 endpoints

- [ ] **Step 4: Create PR**

Create PR targeting `main` with title: "Phase 0: Infrastructure, bug fixes & Account golden reference"

PR body should include:
- Summary of all changes
- Bug fixes list
- New infrastructure (NSubstitute, Shouldly, ServiceTestBase, GetBinaryAsync)
- Account services implemented with endpoint counts
- Test count
