# Implementation Plan: Integration Tests (Issue #32)

## 1. Codebase Analysis Summary

### Solution Structure

```
CashCtrlApiNet.sln
  src/
    CashCtrlApiNet.Abstractions/   -- Models, enums, converters, serialization (net10.0)
    CashCtrlApiNet/                -- API client, connection handler, connectors, endpoints (net10.0)
    CashCtrlApiNet.AspNetCore/     -- ASP.NET Core DI registration (net10.0)
    CashCtrlApiNet.Tests/          -- Unit tests + e2e tests (net10.0, xUnit 2.9, NSubstitute 5.3, Shouldly 4.3, FluentAssertions 6.12)
```

Dependency chain: `Tests --> AspNetCore --> CashCtrlApiNet --> Abstractions`

### API Client Architecture

**Connection Handler** (`CashCtrlConnectionHandler`): Central HTTP abstraction implementing `ICashCtrlConnectionHandler`. Supports two construction modes:
- **Standalone**: Creates its own `HttpClient` with `HttpClientHandler { AllowAutoRedirect = false }`.
- **Factory-based**: Accepts `IHttpClientFactory`, calls `CreateClient(nameof(CashCtrlConnectionHandler))`, then sets `BaseAddress` and `Authorization` header on each call.

**Key HTTP methods on ICashCtrlConnectionHandler**:
- `GetAsync(path)` -- simple GET, returns `ApiResult`
- `GetAsync<TResult>(path)` -- GET with typed JSON deserialization
- `GetAsync<TResult>(path, ListParams?)` -- GET with list params
- `GetAsync<TResult, TQuery>(path, query)` -- GET with typed query parameters
- `PostAsync<TResult, TPost>(path, payload)` -- POST with `FormUrlEncodedContent` body
- `GetBinaryAsync(path)` / `GetBinaryAsync<TQuery>(path, query)` -- binary response (files/exports)
- `PostMultipartAsync<TResult>(path, MultipartFormDataContent)` -- file uploads

**Serialization**: `System.Text.Json` with `[JsonPropertyName]` attributes. POST bodies serialized via `CashCtrlSerialization.ConvertToDictionary()` into `FormUrlEncodedContent`. GET query params also go through the same dictionary conversion. Responses deserialized via `CashCtrlSerialization.Deserialize<T>()`.

**Authentication**: HTTP Basic Auth -- API key as username, empty password, Base64-encoded as `Authorization: Basic <token>`. Language sent as `?lang=de` query parameter on every request.

**Connector Pattern**: `ICashCtrlApiClient` exposes 10 connector properties (Account, Common, File, Inventory, Journal, Meta, Order, Person, Report, Salary). Each connector aggregates individual service classes. Services inherit from `ConnectorService` (which holds `ICashCtrlConnectionHandler`) and delegate to the connection handler one-liner methods.

**HttpClient interception point for WireMock**: The `CashCtrlConnectionHandler` factory constructor takes `IHttpClientFactory`. The standalone constructor creates its own `HttpClient(new HttpClientHandler())`. For integration tests using WireMock, the approach is to construct `CashCtrlConnectionHandler` with the standalone constructor pointed at the WireMock server URL (e.g., `http://localhost:{port}/`). This is the simplest and most reliable approach -- no need to intercept the factory. The `CashCtrlConfiguration.BaseUri` is set to the WireMock server URL, and the real HttpClient makes requests to WireMock instead of the CashCtrl API.

### How Existing Tests Work

**Unit tests** (`ServiceTestBase<TService>`): Mock `ICashCtrlConnectionHandler` via NSubstitute. Verify each service method calls the correct endpoint with the correct parameters. 393 unit tests, one per endpoint method. Use Shouldly for assertions.

**E2e tests** (`CashCtrlTestBase`): Create a real `CashCtrlConnectionHandler` with env-var credentials. Call the real CashCtrl API. Use `AlphabeticalOrderer` for test ordering (Test1_, Test2_, etc.). Use FluentAssertions. Only 2 test files exist: `ArticleTests.cs` (8 tests) and `ArticleCategoryTests.cs` (5 tests) -- 13 tests total.

### Key Files

| File | Purpose |
|------|---------|
| `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs` | HTTP engine -- all requests flow through here |
| `src/CashCtrlApiNet/Interfaces/ICashCtrlConnectionHandler.cs` | Interface for the connection handler (mocking seam) |
| `src/CashCtrlApiNet/Services/CashCtrlApiClient.cs` | Top-level client aggregating all connectors |
| `src/CashCtrlApiNet/Services/Connectors/Base/ConnectorService.cs` | Base class for all services |
| `src/CashCtrlApiNet.Abstractions/Helpers/CashCtrlSerialization.cs` | JSON serialization/deserialization helper |
| `src/CashCtrlApiNet.Abstractions/Models/Api/` | Response types: ApiResult, SingleResponse, ListResponse, NoContentResponse, BinaryResponse |
| `src/CashCtrlApiNet.Tests/ServiceTestBase.cs` | Unit test base (NSubstitute-mocked connection handler) |
| `src/CashCtrlApiNet.Tests/CashCtrlTestBase.cs` | E2e test base (real API calls) |

---

## 2. Existing Test Restructuring (E2e Rename)

The existing "integration tests" in `ArticleTests.cs` and `ArticleCategoryTests.cs` are actually **end-to-end tests** that call the real CashCtrl API. They need to be renamed/relocated so the term "integration test" can be used for the new WireMock-based tests.

### Changes Required

1. **Rename files**:
   - `src/CashCtrlApiNet.Tests/Inventory/ArticleTests.cs` --> `src/CashCtrlApiNet.Tests/Inventory/ArticleE2eTests.cs`
   - `src/CashCtrlApiNet.Tests/Inventory/ArticleCategoryTests.cs` --> `src/CashCtrlApiNet.Tests/Inventory/ArticleCategoryE2eTests.cs`

2. **Rename classes** (inside the files):
   - `ArticleTests` --> `ArticleE2eTests`
   - `ArticleCategoryTests` --> `ArticleCategoryE2eTests`

3. **Rename base class**:
   - `src/CashCtrlApiNet.Tests/CashCtrlTestBase.cs` --> `src/CashCtrlApiNet.Tests/CashCtrlE2eTestBase.cs`
   - Class: `CashCtrlTestBase` --> `CashCtrlE2eTestBase`
   - Update the two e2e test files to inherit from `CashCtrlE2eTestBase`

4. **Add `[Trait]` attribute** to e2e test classes for filtering:
   ```csharp
   [Trait("Category", "E2e")]
   ```

5. **Update CLAUDE.md** test filter command:
   ```bash
   # Run unit tests only (no credentials needed)
   dotnet test src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj --filter "Category!=E2e"
   ```

No namespace changes needed -- the e2e tests remain in `CashCtrlApiNet.Tests.Inventory`.

---

## 3. New Integration Test Project Design

### Architecture Decision

Create a **new test project** `CashCtrlApiNet.IntegrationTests` rather than adding to the existing `CashCtrlApiNet.Tests` project.

**Rationale**:
- Clean separation between unit tests (mock ICashCtrlConnectionHandler) and integration tests (real HTTP through WireMock).
- Different dependency sets (WireMock.Net, Bogus are not needed by unit tests).
- Independent execution -- integration tests can run in parallel with unit tests.
- The existing test project already has 393 unit tests; mixing in integration tests would make filtering fragile.

### Project Setup

**Project**: `src/CashCtrlApiNet.IntegrationTests/CashCtrlApiNet.IntegrationTests.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <LangVersion>13</LangVersion>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <IsPackable>false</IsPackable>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
        <PackageReference Include="xunit" Version="2.9.2" />
        <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
            <PrivateAssets>all</PrivateAssets>
        </PackageReference>
        <PackageReference Include="Shouldly" Version="4.3.0" />
        <PackageReference Include="WireMock.Net" Version="1.6.12" />
        <PackageReference Include="Bogus" Version="35.6.1" />
        <PackageReference Include="coverlet.collector" Version="6.0.2">
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
            <PrivateAssets>all</PrivateAssets>
        </PackageReference>
    </ItemGroup>

    <ItemGroup>
        <ProjectReference Include="..\CashCtrlApiNet\CashCtrlApiNet.csproj" />
    </ItemGroup>

</Project>
```

**Key decisions**:
- **xUnit** (not NUnit): Matches the existing project convention. While our internal guidelines prefer NUnit, this project already standardized on xUnit, and consistency within a project takes priority.
- **Shouldly** (not FluentAssertions): Matches the unit test project convention. The e2e tests use FluentAssertions, but the unit tests (the much larger body) use Shouldly.
- **WireMock.Net**: Mock HTTP server that the real `CashCtrlConnectionHandler` will call.
- **Bogus**: Generate realistic test data for all entity types.

### Test Infrastructure: WireMock Setup

**Strategy**: Use WireMock.Net as a fake CashCtrl API server. Create a `CashCtrlConnectionHandler` with `BaseUri` pointed at the WireMock URL. The handler makes real HTTP requests to WireMock, which returns pre-configured JSON responses. This tests the full HTTP pipeline: request construction, URL building, query params, form-encoded POST bodies, response deserialization, binary responses.

**Base Test Class** (`IntegrationTestBase.cs`):

```csharp
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services;
using CashCtrlApiNet.Services.Connectors;
using WireMock.Server;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Base class for all integration tests. Manages a WireMock server and CashCtrl API client.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    /// <summary>
    /// WireMock server instance for this test class
    /// </summary>
    protected WireMockServer Server { get; private set; } = null!;

    /// <summary>
    /// CashCtrl API client configured to use the WireMock server
    /// </summary>
    protected ICashCtrlApiClient Client { get; private set; } = null!;

    /// <summary>
    /// The connection handler for direct access in tests
    /// </summary>
    protected ICashCtrlConnectionHandler ConnectionHandler { get; private set; } = null!;

    /// <summary>
    /// Fake API key used in tests
    /// </summary>
    protected const string FakeApiKey = "test-api-key-integration";

    public Task InitializeAsync()
    {
        Server = WireMockServer.Start();

        var configuration = new CashCtrlConfiguration
        {
            BaseUri = Server.Url!,
            ApiKey = FakeApiKey,
            DefaultLanguage = "de"
        };

        ConnectionHandler = new CashCtrlConnectionHandler(configuration);

        Client = new CashCtrlApiClient(
            ConnectionHandler,
            new AccountConnector(ConnectionHandler),
            new CommonConnector(ConnectionHandler),
            new FileConnector(ConnectionHandler),
            new InventoryConnector(ConnectionHandler),
            new JournalConnector(ConnectionHandler),
            new MetaConnector(ConnectionHandler),
            new OrderConnector(ConnectionHandler),
            new PersonConnector(ConnectionHandler),
            new ReportConnector(ConnectionHandler),
            new SalaryConnector(ConnectionHandler));

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Server?.Stop();
        Server?.Dispose();
        return Task.CompletedTask;
    }
}
```

**WireMock Response Helpers** (`Helpers/WireMockExtensions.cs`):

A static helper class with methods to configure common CashCtrl API response patterns:

```csharp
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace CashCtrlApiNet.IntegrationTests.Helpers;

/// <summary>
/// WireMock extension methods for configuring CashCtrl API response stubs
/// </summary>
public static class WireMockExtensions
{
    private const string RequestsLeftHeader = "X-Requests-Left";
    private const int DefaultRequestsLeft = 100;

    /// <summary>
    /// Configures a successful JSON response for a GET endpoint
    /// </summary>
    public static void StubGetJson(this WireMockServer server, string pathPattern, string jsonBody)
    {
        server.Given(Request.Create().WithPath(p => p.Contains(pathPattern)).UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithHeader(RequestsLeftHeader, DefaultRequestsLeft.ToString())
                .WithBody(jsonBody));
    }

    /// <summary>
    /// Configures a successful JSON response for a POST endpoint
    /// </summary>
    public static void StubPostJson(this WireMockServer server, string pathPattern, string jsonBody)
    {
        server.Given(Request.Create().WithPath(p => p.Contains(pathPattern)).UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithHeader(RequestsLeftHeader, DefaultRequestsLeft.ToString())
                .WithBody(jsonBody));
    }

    /// <summary>
    /// Configures a binary response for export/download endpoints
    /// </summary>
    public static void StubGetBinary(this WireMockServer server, string pathPattern,
        byte[] data, string contentType = "application/pdf", string? fileName = null)
    {
        var responseBuilder = Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", contentType)
            .WithHeader(RequestsLeftHeader, DefaultRequestsLeft.ToString())
            .WithBody(data);

        if (fileName is not null)
            responseBuilder.WithHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");

        server.Given(Request.Create().WithPath(p => p.Contains(pathPattern)).UsingGet())
            .RespondWith(responseBuilder);
    }
}
```

**CashCtrl JSON Response Factory** (`Helpers/CashCtrlResponseFactory.cs`):

A static class that builds valid CashCtrl API JSON responses:

```csharp
using System.Text.Json;

namespace CashCtrlApiNet.IntegrationTests.Helpers;

/// <summary>
/// Factory for building CashCtrl API JSON response payloads
/// </summary>
public static class CashCtrlResponseFactory
{
    /// <summary>
    /// Creates a successful single-entity response JSON
    /// </summary>
    public static string SingleResponse<T>(T data) => JsonSerializer.Serialize(new
    {
        success = true,
        data
    });

    /// <summary>
    /// Creates a successful list response JSON
    /// </summary>
    public static string ListResponse<T>(T[] data) => JsonSerializer.Serialize(new
    {
        total = data.Length,
        data
    });

    /// <summary>
    /// Creates a successful no-content response (create/update/delete)
    /// </summary>
    public static string SuccessResponse(string message, int? insertId = null) => JsonSerializer.Serialize(new
    {
        success = true,
        message,
        insertId
    });

    /// <summary>
    /// Creates an error response
    /// </summary>
    public static string ErrorResponse(string field, string message) => JsonSerializer.Serialize(new
    {
        success = false,
        errors = new[] { new { field, message } }
    });
}
```

---

## 4. Bogus Test Data Factories

### Design Pattern

Each domain group gets a `Fakers` static class containing `Faker<T>` instances for its entity types. Fakers follow the three-tier model hierarchy: `XxxCreate`, `XxxUpdate`, `Xxx`/`XxxListed`.

**Location**: `src/CashCtrlApiNet.IntegrationTests/Fakers/` -- one file per domain group.

**Pattern**:

```csharp
using Bogus;
using CashCtrlApiNet.Abstractions.Models.Inventory.Article;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus test data factories for Inventory domain entities
/// </summary>
public static class InventoryFakers
{
    /// <summary>
    /// Faker for generating ArticleCreate payloads
    /// </summary>
    public static readonly Faker<ArticleCreate> ArticleCreate = new Faker<ArticleCreate>()
        .CustomInstantiator(f => new ArticleCreate
        {
            Name = f.Commerce.ProductName(),
            Nr = f.Random.Replace("A-#####")
        });

    /// <summary>
    /// Faker for generating Article response data (used in WireMock response stubs)
    /// </summary>
    public static readonly Faker<Article> Article = new Faker<Article>()
        .CustomInstantiator(f => new Article
        {
            Id = f.Random.Int(1, 10000),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.Replace("A-#####"),
            // ... other required properties
        });
}
```

**Important**: Since all models are `record` types with `init` accessors and some have `required` properties, Bogus fakers MUST use `CustomInstantiator` (not `.RuleFor`) to work with records. The faker creates the record via object initializer syntax.

### Entity Types Needing Factories

Each factory file covers the Create, Update, and Read/Listed variants for its entities. The worker agent should create Faker instances for entity types as needed per endpoint group -- the list below is the full inventory.

| Domain Group | Factory File | Entity Types |
|-------------|-------------|-------------|
| Account | `AccountFakers.cs` | Account, AccountCreate, AccountUpdate, AccountListed, AccountBank, AccountBankCreate, AccountBankUpdate, AccountCategory, AccountCategoryCreate, AccountCategoryUpdate, CostCenter, CostCenterCreate, CostCenterUpdate, CostCenterListed, CostCenterCategory, CostCenterCategoryCreate, CostCenterCategoryUpdate |
| Common | `CommonFakers.cs` | Currency, CurrencyCreate, CurrencyUpdate, CurrencyListed, CustomField, CustomFieldCreate, CustomFieldUpdate, CustomFieldListed, CustomFieldGroup, CustomFieldGroupCreate, CustomFieldGroupUpdate, CustomFieldGroupListed, HistoryEntry, Rounding, RoundingCreate, RoundingUpdate, RoundingListed, SequenceNumber, SequenceNumberCreate, SequenceNumberUpdate, SequenceNumberListed, TaxRate, TaxRateCreate, TaxRateUpdate, TaxRateListed, TextTemplate, TextTemplateCreate, TextTemplateUpdate, TextTemplateListed |
| File | `FileFakers.cs` | File, FileCreate, FileUpdate, FileCategory, FileCategoryCreate, FileCategoryUpdate |
| Inventory | `InventoryFakers.cs` | Article, ArticleCreate, ArticleUpdate, ArticleListed, ArticleCategory, ArticleCategoryCreate, ArticleCategoryUpdate, FixedAsset, FixedAssetCreate, FixedAssetUpdate, FixedAssetListed, FixedAssetCategory, FixedAssetCategoryCreate, FixedAssetCategoryUpdate, Unit, UnitCreate, UnitUpdate |
| Journal | `JournalFakers.cs` | Journal, JournalCreate, JournalUpdate, JournalListed, JournalImport, JournalImportCreate, JournalImportEntry, JournalImportEntryUpdate, JournalImportEntryListed |
| Meta | `MetaFakers.cs` | FiscalPeriod, FiscalPeriodCreate, FiscalPeriodUpdate, FiscalPeriodListed, FiscalPeriodTask, FiscalPeriodTaskCreate, Location, LocationCreate, LocationUpdate, LocationListed, Settings, SettingsUpdate |
| Order | `OrderFakers.cs` | Order, OrderCreate, OrderUpdate, OrderListed, OrderCategory, OrderCategoryCreate, OrderCategoryUpdate, OrderLayout, OrderLayoutCreate, OrderLayoutUpdate, BookEntry, BookEntryCreate, BookEntryUpdate, Document, DocumentUpdate |
| Person | `PersonFakers.cs` | Person, PersonCreate, PersonUpdate, PersonListed, PersonCategory, PersonCategoryCreate, PersonCategoryUpdate, PersonTitle, PersonTitleCreate, PersonTitleUpdate |
| Report | `ReportFakers.cs` | Report, ReportElement, ReportElementCreate, ReportElementUpdate, ReportSet, ReportSetCreate, ReportSetUpdate |
| Salary | `SalaryFakers.cs` | SalaryBookEntry, SalaryBookEntryCreate, SalaryBookEntryUpdate, SalaryCategory, SalaryCategoryCreate, SalaryCategoryUpdate, SalaryCertificate, SalaryCertificateUpdate, SalaryCertificateDocument, SalaryCertificateTemplate, SalaryCertificateTemplateCreate, SalaryCertificateTemplateUpdate, SalaryDocument, SalaryDocumentUpdate, SalaryField, SalaryInsuranceType, SalaryInsuranceTypeCreate, SalaryInsuranceTypeUpdate, SalaryLayout, SalaryLayoutCreate, SalaryLayoutUpdate, SalaryPaymentCreate, SalarySetting, SalarySettingCreate, SalarySettingUpdate, SalaryStatement, SalaryStatementCreate, SalaryStatementUpdate, SalaryStatus, SalaryStatusCreate, SalaryStatusUpdate, SalarySum, SalarySumCreate, SalarySumUpdate, SalaryTemplate, SalaryTemplateCreate, SalaryTemplateUpdate, SalaryType, SalaryTypeCreate, SalaryTypeUpdate |

---

## 5. Endpoint Group Inventory

### Complete Service List (58 services, 375 methods, 10 domain groups)

#### Account Group (5 services, 43 endpoints)

**AccountService** (`IAccountService`) -- 10 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetBalance(Entry)`, `Create(AccountCreate)`, `Update(AccountUpdate)`, `Delete(Entries)`, `Categorize(EntriesCategorize)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**AccountBankService** (`IAccountBankService`) -- 9 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(AccountBankCreate)`, `Update(AccountBankUpdate)`, `Delete(Entries)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**AccountCategoryService** (`IAccountCategoryService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(AccountCategoryCreate)`, `Update(AccountCategoryUpdate)`, `Delete(Entries)`

**CostCenterService** (`ICostCenterService`) -- 11 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetBalance(Entry)`, `Create(CostCenterCreate)`, `Update(CostCenterUpdate)`, `Delete(Entries)`, `Categorize(EntriesCategorize)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**CostCenterCategoryService** (`ICostCenterCategoryService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(CostCenterCategoryCreate)`, `Update(CostCenterCategoryUpdate)`, `Delete(Entries)`

#### Common Group (8 services, 41 endpoints)

**CurrencyService** (`ICurrencyService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(CurrencyCreate)`, `Update(CurrencyUpdate)`, `Delete(Entries)`, `GetExchangeRate(CurrencyExchangeRateRequest)`

**CustomFieldService** (`ICustomFieldService`) -- 7 methods:
- `Get(Entry)`, `GetList(CustomFieldListRequest)`, `Create(CustomFieldCreate)`, `Update(CustomFieldUpdate)`, `Delete(Entries)`, `Reorder(CustomFieldReorder)`, `GetTypes()`

**CustomFieldGroupService** (`ICustomFieldGroupService`) -- 6 methods:
- `Get(Entry)`, `GetList(CustomFieldGroupListRequest)`, `Create(CustomFieldGroupCreate)`, `Update(CustomFieldGroupUpdate)`, `Delete(Entries)`, `Reorder(CustomFieldGroupReorder)`

**HistoryService** (`IHistoryService`) -- 1 method:
- `GetList(HistoryListRequest)`

**RoundingService** (`IRoundingService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(RoundingCreate)`, `Update(RoundingUpdate)`, `Delete(Entries)`

**SequenceNumberService** (`ISequenceNumberService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SequenceNumberCreate)`, `Update(SequenceNumberUpdate)`, `Delete(Entries)`, `GetGeneratedNumber(Entry)`

**TaxRateService** (`ITaxRateService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(TaxRateCreate)`, `Update(TaxRateUpdate)`, `Delete(Entries)`

**TextTemplateService** (`ITextTemplateService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(TextTemplateCreate)`, `Update(TextTemplateUpdate)`, `Delete(Entries)`

#### File Group (2 services, 20 endpoints)

**FileService** (`IFileService`) -- 14 methods:
- `GetContent(Entry)`, `Get(Entry)`, `GetList(ListParams?)`, `Prepare(MultipartFormDataContent)`, `Persist(Entries)`, `Create(FileCreate)`, `Update(FileUpdate)`, `Delete(Entries)`, `Categorize(EntriesCategorize)`, `EmptyArchive()`, `Restore(Entries)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**FileCategoryService** (`IFileCategoryService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(FileCategoryCreate)`, `Update(FileCategoryUpdate)`, `Delete(Entries)`

#### Inventory Group (6 services, 42 endpoints)

**ArticleService** (`IArticleService`) -- 11 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(ArticleCreate)`, `Update(ArticleUpdate)`, `Delete(Entries)`, `Categorize(EntriesCategorize)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**ArticleCategoryService** (`IArticleCategoryService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(ArticleCategoryCreate)`, `Update(ArticleCategoryUpdate)`, `Delete(Entries)`

**FixedAssetService** (`IFixedAssetService`) -- 11 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(FixedAssetCreate)`, `Update(FixedAssetUpdate)`, `Delete(Entries)`, `Categorize(EntriesCategorize)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**FixedAssetCategoryService** (`IFixedAssetCategoryService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(FixedAssetCategoryCreate)`, `Update(FixedAssetCategoryUpdate)`, `Delete(Entries)`

**UnitService** (`IUnitService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(UnitCreate)`, `Update(UnitUpdate)`, `Delete(Entries)`

**InventoryImportService** (`IInventoryImportService`) -- 5 methods:
- `Create(InventoryImportCreate)`, `Mapping(InventoryImportMapping)`, `GetMappingFields()`, `Preview(InventoryImportPreview)`, `Execute(InventoryImportExecute)`

#### Journal Group (3 services, 24 endpoints)

**JournalService** (`IJournalService`) -- 11 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(JournalCreate)`, `Update(JournalUpdate)`, `Delete(Entries)`, `UpdateAttachments(EntryAttachments)`, `UpdateRecurrence(EntryRecurrence)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**JournalImportService** (`IJournalImportService`) -- 4 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(JournalImportCreate)`, `Execute(Entry)`

**JournalImportEntryService** (`IJournalImportEntryService`) -- 10 methods:
- `Get(Entry)`, `GetList()`, `Update(JournalImportEntryUpdate)`, `Ignore(Entries)`, `Restore(Entries)`, `Confirm(Entries)`, `Unconfirm(Entries)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

#### Meta Group (5 services, 27 endpoints)

**FiscalPeriodService** (`IFiscalPeriodService`) -- 17 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(FiscalPeriodCreate)`, `Update(FiscalPeriodUpdate)`, `Switch(Entry)`, `Delete(Entries)`, `GetResult(Entry)`, `GetDepreciations(Entry)`, `BookDepreciations(Entry)`, `GetExchangeDiff(Entry)`, `BookExchangeDiff(Entry)`, `Complete(Entry)`, `Reopen(Entry)`, `CompleteMonths(Entry)`, `ReopenMonths(Entry)`

**FiscalPeriodTaskService** (`IFiscalPeriodTaskService`) -- 3 methods:
- `GetList(ListParams?)`, `Create(FiscalPeriodTaskCreate)`, `Delete(Entries)`

**LocationService** (`ILocationService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(LocationCreate)`, `Update(LocationUpdate)`, `Delete(Entries)`

**OrganizationService** (`IOrganizationService`) -- 1 method:
- `GetLogo()`

**SettingsService** (`ISettingsService`) -- 3 methods:
- `Read()`, `Get(SettingGet)`, `Update(SettingsUpdate)`

#### Order Group (6 services, 39 endpoints)

**OrderService** (`IOrderService`) -- 17 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(OrderCreate)`, `Update(OrderUpdate)`, `Delete(Entries)`, `UpdateStatus(OrderStatusUpdate)`, `UpdateRecurrence(OrderRecurrenceUpdate)`, `Continue(Entry)`, `GetDossier(Entry)`, `DossierAdd(OrderDossierModify)`, `DossierRemove(OrderDossierModify)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**OrderCategoryService** (`IOrderCategoryService`) -- 7 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(OrderCategoryCreate)`, `Update(OrderCategoryUpdate)`, `Delete(Entries)`, `Reorder(OrderCategoryReorder)`, `GetStatus(Entry)`

**OrderLayoutService** (`IOrderLayoutService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(OrderLayoutCreate)`, `Update(OrderLayoutUpdate)`, `Delete(Entries)`

**BookEntryService** (`IBookEntryService`) -- 5 methods:
- `Get(Entry)`, `GetList()`, `Create(BookEntryCreate)`, `Update(BookEntryUpdate)`, `Delete(Entries)`

**DocumentService** (`IDocumentService`) -- 5 methods:
- `Get(Entry)`, `DownloadPdf(Entry)`, `DownloadZip(Entry)`, `SendMail(DocumentMail)`, `Update(DocumentUpdate)`

**OrderPaymentService** (`IOrderPaymentService`) -- 2 methods:
- `Create(OrderPaymentCreate)`, `Download(Entry)`

#### Person Group (4 services, 27 endpoints)

**PersonService** (`IPersonService`) -- 12 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(PersonCreate)`, `Update(PersonUpdate)`, `Delete(Entries)`, `Categorize(EntriesCategorize)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`, `ExportVcard()`

**PersonCategoryService** (`IPersonCategoryService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(PersonCategoryCreate)`, `Update(PersonCategoryUpdate)`, `Delete(Entries)`

**PersonTitleService** (`IPersonTitleService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(PersonTitleCreate)`, `Update(PersonTitleUpdate)`, `Delete(Entries)`

**PersonImportService** (`IPersonImportService`) -- 5 methods:
- `Create(PersonImportCreate)`, `Mapping(PersonImportMapping)`, `GetMappingFields()`, `Preview(PersonImportPreview)`, `Execute(PersonImportExecute)`

#### Report Group (3 services, 22 endpoints)

**ReportService** (`IReportService`) -- 1 method:
- `GetTree()`

**ReportElementService** (`IReportElementService`) -- 11 methods:
- `Get(Entry)`, `Create(ReportElementCreate)`, `Update(ReportElementUpdate)`, `Delete(Entries)`, `Reorder(ReportElementReorder)`, `GetData(Entry)`, `GetDataHtml(Entry)`, `GetMeta(Entry)`, `DownloadPdf(Entry)`, `DownloadCsv(Entry)`, `DownloadExcel(Entry)`

**ReportSetService** (`IReportSetService`) -- 10 methods:
- `Get(Entry)`, `Create(ReportSetCreate)`, `Update(ReportSetUpdate)`, `Delete(Entries)`, `Reorder(ReportSetReorder)`, `GetMeta(Entry)`, `DownloadPdf(Entry)`, `DownloadCsv(Entry)`, `DownloadExcel(Entry)`, `DownloadAnnualReport(Entry)`

#### Salary Group (16 services, 90 endpoints)

**SalaryBookEntryService** (`ISalaryBookEntryService`) -- 5 methods:
- `Get(Entry)`, `GetList(Entry)`, `Create(SalaryBookEntryCreate)`, `Update(SalaryBookEntryUpdate)`, `Delete(Entries)`

**SalaryCategoryService** (`ISalaryCategoryService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(SalaryCategoryCreate)`, `Update(SalaryCategoryUpdate)`, `Delete(Entries)`

**SalaryCertificateService** (`ISalaryCertificateService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Update(SalaryCertificateUpdate)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**SalaryCertificateDocumentService** (`ISalaryCertificateDocumentService`) -- 4 methods:
- `Get(Entry)`, `DownloadPdf(Entries)`, `DownloadZip(Entries)`, `SendMail(SalaryCertificateDocumentMail)`

**SalaryCertificateTemplateService** (`ISalaryCertificateTemplateService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(SalaryCertificateTemplateCreate)`, `Update(SalaryCertificateTemplateUpdate)`, `Delete(Entries)`

**SalaryDocumentService** (`ISalaryDocumentService`) -- 5 methods:
- `Get(Entry)`, `DownloadPdf(Entries)`, `DownloadZip(Entries)`, `SendMail(SalaryDocumentMail)`, `Update(SalaryDocumentUpdate)`

**SalaryFieldService** (`ISalaryFieldService`) -- 2 methods:
- `Get(Entry)`, `GetList(Entry)`

**SalaryInsuranceTypeService** (`ISalaryInsuranceTypeService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalaryInsuranceTypeCreate)`, `Update(SalaryInsuranceTypeUpdate)`, `Delete(Entries)`

**SalaryLayoutService** (`ISalaryLayoutService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalaryLayoutCreate)`, `Update(SalaryLayoutUpdate)`, `Delete(Entries)`

**SalaryPaymentService** (`ISalaryPaymentService`) -- 2 methods:
- `Create(SalaryPaymentCreate)`, `Download(SalaryPaymentCreate)`

**SalarySettingService** (`ISalarySettingService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalarySettingCreate)`, `Update(SalarySettingUpdate)`, `Delete(Entries)`

**SalaryStatementService** (`ISalaryStatementService`) -- 15 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalaryStatementCreate)`, `Update(SalaryStatementUpdate)`, `UpdateMultiple(SalaryStatementUpdateMultiple)`, `UpdateStatus(SalaryStatementStatusUpdate)`, `UpdateRecurrence(SalaryStatementRecurrenceUpdate)`, `Delete(Entries)`, `Calculate(SalaryStatementCalculate)`, `UpdateAttachments(EntryAttachments)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

**SalaryStatusService** (`ISalaryStatusService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalaryStatusCreate)`, `Update(SalaryStatusUpdate)`, `Delete(Entries)`, `Reorder(SalaryStatusReorder)`

**SalarySumService** (`ISalarySumService`) -- 5 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalarySumCreate)`, `Update(SalarySumUpdate)`, `Delete(Entries)`

**SalaryTemplateService** (`ISalaryTemplateService`) -- 6 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `GetTree()`, `Create(SalaryTemplateCreate)`, `Update(SalaryTemplateUpdate)`, `Delete(Entries)`

**SalaryTypeService** (`ISalaryTypeService`) -- 9 methods:
- `Get(Entry)`, `GetList(ListParams?)`, `Create(SalaryTypeCreate)`, `Update(SalaryTypeUpdate)`, `Categorize(EntriesCategorize)`, `Delete(Entries)`, `ExportExcel()`, `ExportCsv()`, `ExportPdf()`

---

## 6. Sub-Issue Breakdown

### Issue 1: Initial Setup -- Test Infrastructure and E2e Rename

**Scope**:
1. Rename existing integration tests to e2e tests (see Section 2 above)
2. Create new `CashCtrlApiNet.IntegrationTests` project with csproj
3. Add project to `CashCtrlApiNet.sln`
4. Create `GlobalUsings.cs` with `global using Xunit;`
5. Create `IntegrationTestBase.cs` (WireMock server + client setup)
6. Create `Helpers/WireMockExtensions.cs` (response stub helpers)
7. Create `Helpers/CashCtrlResponseFactory.cs` (JSON response builders)
8. Verify the project builds and a single smoke test passes (e.g., test that WireMock server starts and the client can hit it)
9. Update CLAUDE.md with new project info, test run commands, and updated test filter

**Acceptance criteria**:
- `dotnet build CashCtrlApiNet.sln` succeeds
- `dotnet test src/CashCtrlApiNet.IntegrationTests/` runs at least one smoke test
- `dotnet test src/CashCtrlApiNet.Tests/ --filter "Category!=E2e"` runs only unit tests
- Existing unit tests pass (393 tests)
- E2e test classes have `[Trait("Category", "E2e")]`

### Issue 2: Account Group Integration Tests

**Scope**: Integration tests for all 5 Account services (AccountService, AccountBankService, AccountCategoryService, CostCenterService, CostCenterCategoryService). Create `Fakers/AccountFakers.cs`. Create test files in `Account/` directory.

**Test files**: `AccountServiceIntegrationTests.cs`, `AccountBankServiceIntegrationTests.cs`, `AccountCategoryServiceIntegrationTests.cs`, `CostCenterServiceIntegrationTests.cs`, `CostCenterCategoryServiceIntegrationTests.cs`

**Acceptance criteria**: All 43 Account endpoint methods have at least one integration test. Tests verify: correct URL called, correct HTTP method, request parameters/body serialized correctly, response deserialized correctly, binary export endpoints return data.

### Issue 3: Common Group Integration Tests

**Scope**: Integration tests for all 8 Common services. Create `Fakers/CommonFakers.cs`.

**Test files**: `CurrencyServiceIntegrationTests.cs`, `CustomFieldServiceIntegrationTests.cs`, `CustomFieldGroupServiceIntegrationTests.cs`, `HistoryServiceIntegrationTests.cs`, `RoundingServiceIntegrationTests.cs`, `SequenceNumberServiceIntegrationTests.cs`, `TaxRateServiceIntegrationTests.cs`, `TextTemplateServiceIntegrationTests.cs`

**Acceptance criteria**: All 41 Common endpoint methods have at least one integration test.

### Issue 4: File Group Integration Tests

**Scope**: Integration tests for FileService and FileCategoryService. Create `Fakers/FileFakers.cs`. Note: FileService has `Prepare(MultipartFormDataContent)` which tests multipart upload.

**Test files**: `FileServiceIntegrationTests.cs`, `FileCategoryServiceIntegrationTests.cs`

**Acceptance criteria**: All 20 File endpoint methods have at least one integration test, including multipart upload and binary download.

### Issue 5: Inventory Group Integration Tests

**Scope**: Integration tests for all 6 Inventory services. Create `Fakers/InventoryFakers.cs`.

**Test files**: `ArticleServiceIntegrationTests.cs`, `ArticleCategoryServiceIntegrationTests.cs`, `FixedAssetServiceIntegrationTests.cs`, `FixedAssetCategoryServiceIntegrationTests.cs`, `UnitServiceIntegrationTests.cs`, `InventoryImportServiceIntegrationTests.cs`

**Acceptance criteria**: All 42 Inventory endpoint methods have at least one integration test.

### Issue 6: Journal Group Integration Tests

**Scope**: Integration tests for all 3 Journal services. Create `Fakers/JournalFakers.cs`.

**Test files**: `JournalServiceIntegrationTests.cs`, `JournalImportServiceIntegrationTests.cs`, `JournalImportEntryServiceIntegrationTests.cs`

**Acceptance criteria**: All 24 Journal endpoint methods have at least one integration test.

### Issue 7: Meta Group Integration Tests

**Scope**: Integration tests for all 5 Meta services. Create `Fakers/MetaFakers.cs`. Note: FiscalPeriodService has 17 methods (the largest non-Salary service).

**Test files**: `FiscalPeriodServiceIntegrationTests.cs`, `FiscalPeriodTaskServiceIntegrationTests.cs`, `LocationServiceIntegrationTests.cs`, `OrganizationServiceIntegrationTests.cs`, `SettingsServiceIntegrationTests.cs`

**Acceptance criteria**: All 27 Meta endpoint methods have at least one integration test.

### Issue 8: Order Group Integration Tests

**Scope**: Integration tests for all 6 Order services. Create `Fakers/OrderFakers.cs`. Note: OrderService has 17 methods including dossier operations.

**Test files**: `OrderServiceIntegrationTests.cs`, `OrderCategoryServiceIntegrationTests.cs`, `OrderLayoutServiceIntegrationTests.cs`, `BookEntryServiceIntegrationTests.cs`, `DocumentServiceIntegrationTests.cs`, `OrderPaymentServiceIntegrationTests.cs`

**Acceptance criteria**: All 39 Order endpoint methods have at least one integration test.

### Issue 9: Person Group Integration Tests

**Scope**: Integration tests for all 4 Person services. Create `Fakers/PersonFakers.cs`. Note: PersonService includes VCard export.

**Test files**: `PersonServiceIntegrationTests.cs`, `PersonCategoryServiceIntegrationTests.cs`, `PersonTitleServiceIntegrationTests.cs`, `PersonImportServiceIntegrationTests.cs`

**Acceptance criteria**: All 27 Person endpoint methods have at least one integration test.

### Issue 10: Report Group Integration Tests

**Scope**: Integration tests for all 3 Report services. Create `Fakers/ReportFakers.cs`. Note: Many binary download endpoints (PDF, CSV, Excel).

**Test files**: `ReportServiceIntegrationTests.cs`, `ReportElementServiceIntegrationTests.cs`, `ReportSetServiceIntegrationTests.cs`

**Acceptance criteria**: All 22 Report endpoint methods have at least one integration test.

### Issue 11: Salary Group Integration Tests (Part 1: BookEntry through Field)

**Scope**: Integration tests for: SalaryBookEntryService (5), SalaryCategoryService (6), SalaryCertificateService (6), SalaryCertificateDocumentService (4), SalaryCertificateTemplateService (6), SalaryDocumentService (5), SalaryFieldService (2). Create `Fakers/SalaryFakers.cs` (shared across both Salary issues).

**Test files**: `SalaryBookEntryServiceIntegrationTests.cs`, `SalaryCategoryServiceIntegrationTests.cs`, `SalaryCertificateServiceIntegrationTests.cs`, `SalaryCertificateDocumentServiceIntegrationTests.cs`, `SalaryCertificateTemplateServiceIntegrationTests.cs`, `SalaryDocumentServiceIntegrationTests.cs`, `SalaryFieldServiceIntegrationTests.cs`

**Acceptance criteria**: All 34 endpoint methods across these 7 services have at least one integration test.

### Issue 12: Salary Group Integration Tests (Part 2: InsuranceType through Type)

**Scope**: Integration tests for: SalaryInsuranceTypeService (5), SalaryLayoutService (5), SalaryPaymentService (2), SalarySettingService (5), SalaryStatementService (15), SalaryStatusService (6), SalarySumService (5), SalaryTemplateService (6), SalaryTypeService (9). Extend `Fakers/SalaryFakers.cs` as needed.

**Test files**: `SalaryInsuranceTypeServiceIntegrationTests.cs`, `SalaryLayoutServiceIntegrationTests.cs`, `SalaryPaymentServiceIntegrationTests.cs`, `SalarySettingServiceIntegrationTests.cs`, `SalaryStatementServiceIntegrationTests.cs`, `SalaryStatusServiceIntegrationTests.cs`, `SalarySumServiceIntegrationTests.cs`, `SalaryTemplateServiceIntegrationTests.cs`, `SalaryTypeServiceIntegrationTests.cs`

**Acceptance criteria**: All 56 endpoint methods across these 9 services have at least one integration test.

### Issue 13: Finalize -- CI Integration, Coverage, Documentation

**Scope**:
1. Add integration test run to GitHub Actions CI workflow (if `.github/workflows/` exists)
2. Verify full test suite runs: unit tests + integration tests (e2e excluded by default)
3. Update CLAUDE.md with final test counts and commands
4. Update README.md if it references test commands
5. Final review: confirm all 375 endpoint methods have integration tests
6. Verify code coverage configuration includes the new project

**Acceptance criteria**:
- `dotnet test CashCtrlApiNet.sln --filter "Category!=E2e"` runs all unit + integration tests
- All 375 endpoints covered by integration tests
- CI passes with new test project
- Documentation updated

---

## 7. File Structure

```
src/CashCtrlApiNet.IntegrationTests/
    CashCtrlApiNet.IntegrationTests.csproj
    GlobalUsings.cs
    IntegrationTestBase.cs
    Helpers/
        WireMockExtensions.cs
        CashCtrlResponseFactory.cs
    Fakers/
        AccountFakers.cs
        CommonFakers.cs
        FileFakers.cs
        InventoryFakers.cs
        JournalFakers.cs
        MetaFakers.cs
        OrderFakers.cs
        PersonFakers.cs
        ReportFakers.cs
        SalaryFakers.cs
    Account/
        AccountServiceIntegrationTests.cs
        AccountBankServiceIntegrationTests.cs
        AccountCategoryServiceIntegrationTests.cs
        CostCenterServiceIntegrationTests.cs
        CostCenterCategoryServiceIntegrationTests.cs
    Common/
        CurrencyServiceIntegrationTests.cs
        CustomFieldServiceIntegrationTests.cs
        CustomFieldGroupServiceIntegrationTests.cs
        HistoryServiceIntegrationTests.cs
        RoundingServiceIntegrationTests.cs
        SequenceNumberServiceIntegrationTests.cs
        TaxRateServiceIntegrationTests.cs
        TextTemplateServiceIntegrationTests.cs
    File/
        FileServiceIntegrationTests.cs
        FileCategoryServiceIntegrationTests.cs
    Inventory/
        ArticleServiceIntegrationTests.cs
        ArticleCategoryServiceIntegrationTests.cs
        FixedAssetServiceIntegrationTests.cs
        FixedAssetCategoryServiceIntegrationTests.cs
        UnitServiceIntegrationTests.cs
        InventoryImportServiceIntegrationTests.cs
    Journal/
        JournalServiceIntegrationTests.cs
        JournalImportServiceIntegrationTests.cs
        JournalImportEntryServiceIntegrationTests.cs
    Meta/
        FiscalPeriodServiceIntegrationTests.cs
        FiscalPeriodTaskServiceIntegrationTests.cs
        LocationServiceIntegrationTests.cs
        OrganizationServiceIntegrationTests.cs
        SettingsServiceIntegrationTests.cs
    Order/
        OrderServiceIntegrationTests.cs
        OrderCategoryServiceIntegrationTests.cs
        OrderLayoutServiceIntegrationTests.cs
        BookEntryServiceIntegrationTests.cs
        DocumentServiceIntegrationTests.cs
        OrderPaymentServiceIntegrationTests.cs
    Person/
        PersonServiceIntegrationTests.cs
        PersonCategoryServiceIntegrationTests.cs
        PersonTitleServiceIntegrationTests.cs
        PersonImportServiceIntegrationTests.cs
    Report/
        ReportServiceIntegrationTests.cs
        ReportElementServiceIntegrationTests.cs
        ReportSetServiceIntegrationTests.cs
    Salary/
        SalaryBookEntryServiceIntegrationTests.cs
        SalaryCategoryServiceIntegrationTests.cs
        SalaryCertificateServiceIntegrationTests.cs
        SalaryCertificateDocumentServiceIntegrationTests.cs
        SalaryCertificateTemplateServiceIntegrationTests.cs
        SalaryDocumentServiceIntegrationTests.cs
        SalaryFieldServiceIntegrationTests.cs
        SalaryInsuranceTypeServiceIntegrationTests.cs
        SalaryLayoutServiceIntegrationTests.cs
        SalaryPaymentServiceIntegrationTests.cs
        SalarySettingServiceIntegrationTests.cs
        SalaryStatementServiceIntegrationTests.cs
        SalaryStatusServiceIntegrationTests.cs
        SalarySumServiceIntegrationTests.cs
        SalaryTemplateServiceIntegrationTests.cs
        SalaryTypeServiceIntegrationTests.cs
```

---

## 8. Testing Strategy per Endpoint

Each integration test method should follow this pattern:

### For GET endpoints returning typed JSON (Get, GetList, GetTree, GetBalance, etc.)

```csharp
[Fact]
public async Task Get_WithValidId_ShouldReturnDeserializedEntity()
{
    // Arrange -- generate fake data and stub WireMock
    var fakeAccount = AccountFakers.Account.Generate();
    Server.StubGetJson("api/v1/account/read.json",
        CashCtrlResponseFactory.SingleResponse(fakeAccount));

    // Act -- call through the real client
    var result = await Client.Account.Account.Get(new Entry { Id = fakeAccount.Id });

    // Assert -- verify HTTP success and response deserialization
    result.IsHttpSuccess.ShouldBeTrue();
    result.ResponseData.ShouldNotBeNull();
    result.ResponseData.Data.ShouldNotBeNull();
    result.ResponseData.Data.Id.ShouldBe(fakeAccount.Id);

    // Verify WireMock received the request with correct parameters
    Server.LogEntries.ShouldHaveSingleItem();
    var request = Server.LogEntries.First().RequestMessage;
    request.Method.ShouldBe("GET");
    request.Url.ShouldContain("api/v1/account/read.json");
    request.Url.ShouldContain("id=" + fakeAccount.Id);
}
```

### For POST endpoints (Create, Update, Delete, Categorize, etc.)

```csharp
[Fact]
public async Task Create_WithValidPayload_ShouldPostFormEncodedData()
{
    // Arrange
    var createPayload = AccountFakers.AccountCreate.Generate();
    Server.StubPostJson("api/v1/account/create.json",
        CashCtrlResponseFactory.SuccessResponse("Account saved", insertId: 42));

    // Act
    var result = await Client.Account.Account.Create(createPayload);

    // Assert
    result.IsHttpSuccess.ShouldBeTrue();
    result.ResponseData.ShouldNotBeNull();
    result.ResponseData.Success.ShouldBeTrue();
    result.ResponseData.InsertId.ShouldBe(42);

    // Verify form-encoded POST body
    Server.LogEntries.ShouldHaveSingleItem();
    var request = Server.LogEntries.First().RequestMessage;
    request.Method.ShouldBe("POST");
    request.Body.ShouldContain("name=");
}
```

### For binary/export endpoints (ExportExcel, ExportCsv, ExportPdf, DownloadPdf, etc.)

```csharp
[Fact]
public async Task ExportExcel_ShouldReturnBinaryData()
{
    // Arrange
    var fakeData = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // fake XLSX header
    Server.StubGetBinary("api/v1/account/list.xlsx", fakeData,
        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        fileName: "accounts.xlsx");

    // Act
    var result = await Client.Account.Account.ExportExcel();

    // Assert
    result.IsHttpSuccess.ShouldBeTrue();
    result.ResponseData.ShouldNotBeNull();
    result.ResponseData.Data.ShouldBe(fakeData);
}
```

### For multipart upload (File.Prepare)

```csharp
[Fact]
public async Task Prepare_ShouldPostMultipartContent()
{
    // Arrange
    Server.StubPostJson("api/v1/file/prepare.json",
        CashCtrlResponseFactory.SuccessResponse("File prepared", insertId: 99));

    using var content = new MultipartFormDataContent();
    content.Add(new ByteArrayContent(new byte[] { 1, 2, 3 }), "file", "test.pdf");

    // Act
    var result = await Client.File.File.Prepare(content);

    // Assert
    result.IsHttpSuccess.ShouldBeTrue();
}
```

---

## 9. Critical Details

### Error Handling Tests
Each endpoint group should include at least one test for error responses:
- HTTP 200 with `{"success": false, "errors": [...]}` (validation failure)
- HTTP 4xx/5xx status codes

### Request Verification
Integration tests should verify via `Server.LogEntries`:
- Correct HTTP method (GET vs POST)
- Correct URL path (matches endpoint constants)
- Correct query parameters (lang, id, etc.)
- Correct request body format (form-encoded for POST, multipart for uploads)
- Authorization header present

### WireMock Server Lifecycle
- Each test class gets its own WireMock server instance via `IAsyncLifetime`
- Server is started in `InitializeAsync`, stopped in `DisposeAsync`
- Tests within a class run sequentially (xUnit default for non-collection tests)
- Use `Server.Reset()` between tests if needed, or rely on fresh server per class

### Code Style Requirements
All new files MUST follow existing conventions:
- MIT license header on every `.cs` file
- XML doc comments on every public member
- `Nullable` enabled
- Records for data types
- `Shouldly` for assertions (not FluentAssertions)

### Documentation Impact
- **CLAUDE.md**: Update to reference new `CashCtrlApiNet.IntegrationTests` project, add test run commands, update test counts
- **README.md**: If it references test commands, update accordingly
- No impact on `doc/strategy/` or `doc/000_Index.md` (this is specific to the CashCtrlApiNet project, not ai-cockpit infrastructure)

### Build Sequence (Summary)

1. Issue 1: Infrastructure setup + e2e rename
2. Issues 2-12: Endpoint groups (can be parallelized in pairs)
3. Issue 13: Finalize, CI, documentation
