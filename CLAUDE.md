---
title: CashCtrlApiNet - Claude Code Instructions
tags: [claude, onboarding, dotnet, api-client]
---

# CashCtrlApiNet

Unofficial .NET 10 API client library for the [CashCtrl REST API v1](https://app.cashctrl.com/static/help/en/api/index.html). CashCtrl is a Swiss cloud ERP for accounting and business management. This library provides a typed C# client with models, JSON serialization, and ASP.NET Core DI integration, distributed as three NuGet packages.

## Tech Stack

| Layer          | Technology                             |
| -------------- | -------------------------------------- |
| Runtime        | .NET 10                                |
| HTTP           | `System.Net.Http.HttpClient` via `IHttpClientFactory` |
| Serialization  | `System.Text.Json`                     |
| DI integration | ASP.NET Core (`Microsoft.Extensions.DependencyInjection`) |
| Unit Testing   | xUnit, NSubstitute 5.3, Shouldly 4.3  |
| Integration Testing | xUnit 2.9, Shouldly 4.3, WireMock.Net 2.0, Bogus 35.6 |
| E2E Testing    | xUnit 2.9, FluentAssertions 6.12     |
| Code Coverage  | Coverlet                               |
| Build          | MSBuild (SDK-style csproj)             |
| CI/CD          | GitHub Actions                         |
| License        | MIT                                    |

## Solution Structure

```
CashCtrlApiNet.sln
  src/
    CashCtrlApiNet.Abstractions/   -- Models, enums, converters, serialization helpers (NuGet package)
    CashCtrlApiNet/                -- API client, connection handler, connectors, endpoints (NuGet package)
    CashCtrlApiNet.AspNetCore/     -- ASP.NET Core DI registration (NuGet package)
    CashCtrlApiNet.UnitTests/          -- Unit tests (NSubstitute + Shouldly) and E2E tests (xUnit + FluentAssertions)
    CashCtrlApiNet.IntegrationTests/ -- Integration tests (WireMock + Shouldly, no live API needed)
```

### Dependency Graph

```
CashCtrlApiNet.UnitTests --> CashCtrlApiNet --> CashCtrlApiNet.Abstractions
CashCtrlApiNet.IntegrationTests --> CashCtrlApiNet --> CashCtrlApiNet.Abstractions
CashCtrlApiNet.AspNetCore --> CashCtrlApiNet --> CashCtrlApiNet.Abstractions
```

## Build Commands

```bash
# Restore dependencies
dotnet restore CashCtrlApiNet.sln

# Build entire solution
dotnet build CashCtrlApiNet.sln

# Build in Release mode (also produces NuGet packages)
dotnet build CashCtrlApiNet.sln -c Release

# Run unit tests only (no credentials needed)
dotnet test src/CashCtrlApiNet.UnitTests/CashCtrlApiNet.UnitTests.csproj --filter "Category!=E2e"

# Run integration tests (WireMock-based, no credentials needed)
dotnet test src/CashCtrlApiNet.IntegrationTests/CashCtrlApiNet.IntegrationTests.csproj

# Run E2E tests (requires live CashCtrl API credentials)
export CashCtrlApiNet__BaseUri="https://yourorg.cashctrl.com/"
export CashCtrlApiNet__ApiKey="your-api-key"
export CashCtrlApiNet__Language="de"
dotnet test src/CashCtrlApiNet.UnitTests/CashCtrlApiNet.UnitTests.csproj --filter "Category=E2e"
```

## API Completeness

As of 2026-03-24, this library implements 375 endpoint methods covering 100% of the CashCtrl REST API v1. All 10 domain groups, 58 services, and 10 connectors are fully operational with 393 unit tests.

Full audit: `doc/analysis/2026-03-24-api-completeness-audit.md`
Design spec: `doc/specs/2026-03-23-full-api-implementation-design.md`

## Implementation Status

| Domain Group | Services | Endpoints | Connector Wired | Unit Tests |
| ------------ | -------- | --------- | --------------- | ---------- |
| Account      | 5        | 43        | YES             | 43         |
| Common       | 8        | 41        | YES             | 41         |
| File         | 2        | 20        | YES             | 20         |
| Inventory    | 6        | 42        | YES             | 43         |
| Journal      | 3        | 24        | YES             | 24         |
| Meta         | 5        | 27        | YES             | 27         |
| Order        | 6        | 39        | YES             | 39         |
| Person       | 4        | 27        | YES             | 27         |
| Report       | 3        | 22        | YES             | 22         |
| Salary       | 16       | 90        | YES             | 90         |
| **TOTAL**    | **58**   | **375**   | **ALL**         | **376+17** |

## Key Conventions

### Architecture Pattern
- **Connector pattern**: The API surface is organized into domain groups (Account, Inventory, Order, etc.). Each group has a `Connector` class that aggregates individual `Service` classes. Services inherit from `ConnectorService` base and call `ConnectionHandler.GetAsync/PostAsync`.
- **Three-tier model hierarchy**: For each entity: `XxxCreate` (POST fields) -> `XxxUpdate` (adds `Id`) -> `Xxx`/`XxxListed` (adds read-only server fields). All are `record` types inheriting from `ModelBaseRecord`.
- **Endpoint constants**: API paths are defined as static string constants in `Services/Endpoints/` classes, built from `Api.V1` + group + `Default.Read/List/Create/Update/Delete`.
- **Binary endpoints**: Export/download endpoints return `Task<ApiResult<BinaryResponse>>` via `GetBinaryAsync`. File uploads use `PostMultipartAsync`.

### Code Style
- **Records everywhere**: All models and API responses are `record` types (immutability by default).
- **Nullable enabled**: All projects use `<Nullable>enable</Nullable>`.
- **XML doc comments**: Every public member has XML documentation with links to CashCtrl API docs.
- **MIT license header**: Every `.cs` file starts with the full MIT license block.
- **`[Optional]` for CancellationToken**: Uses `System.Runtime.InteropServices.Optional` attribute instead of default parameter values.
- **Primary constructors**: Service classes use C# primary constructors (e.g., `ArticleService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler)`).
- **`required` keyword**: Used on properties that must be set (e.g., `required string Name`).
- **`init` accessors**: All record properties use `init` for immutability.
- **`ImmutableArray<T>`**: Used for collection properties instead of mutable lists.
- **`using Endpoint = ...Endpoints.{Entity}` alias**: Every service uses a using alias for its endpoint constants.
- **Expression-bodied service methods**: Service methods are one-liners delegating to connection handler.
- **`/// <inheritdoc />`**: Used on service method implementations.

### Serialization
- `System.Text.Json` with `[JsonPropertyName]` attributes matching the CashCtrl API field names.
- Custom converters: `CashCtrlDateTimeConverter` (format: `yyyy-MM-dd HH:mm:ss.f`), `IntArrayAsCsvJsonConverter` (comma-separated int arrays).
- POST bodies sent as `FormUrlEncodedContent` (not JSON), serialized through `CashCtrlSerialization.ConvertToDictionary`.
- GET query parameters also serialized via the same dictionary conversion.

### Authentication
- HTTP Basic Auth: API key as username, empty password, sent as `Authorization: Basic <base64>` header.
- Base URL pattern: `https://{org}.cashctrl.com/api/v1/...`

### Unit Testing Pattern
- **Base class**: `ServiceTestBase<TService>` provides a mocked `ICashCtrlConnectionHandler` via NSubstitute.
- **Per method**: Verify correct endpoint called, correct HTTP method, request parameter serialization, response deserialization.
- **Assertions**: Shouldly (`result.ShouldBe(expected)`)
- **Mocking**: NSubstitute (`Substitute.For<T>()`)

### Integration Testing Pattern
- **Base class**: `IntegrationTestBase` manages WireMock server and creates a real `CashCtrlConnectionHandler` pointed at it.
- **WireMock helpers**: `WireMockExtensions.StubGetJson/StubPostJson/StubGetBinary` for easy endpoint stubbing.
- **Response factory**: `CashCtrlResponseFactory.SingleResponse/ListResponse/SuccessResponse/ErrorResponse` for building valid CashCtrl JSON payloads.
- **Assertions**: Shouldly (`result.ShouldBe(expected)`)
- **No live API needed**: All responses are stubbed via WireMock.

## Important File Locations

| Purpose                          | Path                                                                  |
| -------------------------------- | --------------------------------------------------------------------- |
| Main client interface            | `src/CashCtrlApiNet/Interfaces/ICashCtrlApiClient.cs`                 |
| Client implementation            | `src/CashCtrlApiNet/Services/CashCtrlApiClient.cs`                    |
| HTTP connection handler          | `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs`            |
| Configuration interface          | `src/CashCtrlApiNet/Interfaces/ICashCtrlConfiguration.cs`             |
| Base connector service           | `src/CashCtrlApiNet/Services/Connectors/Base/ConnectorService.cs`     |
| Endpoint path constants          | `src/CashCtrlApiNet/Services/Endpoints/` (one file per domain group)  |
| Connector interfaces             | `src/CashCtrlApiNet/Interfaces/Connectors/` (one per domain group)    |
| Service interfaces               | `src/CashCtrlApiNet/Interfaces/Connectors/{Group}/` (one per entity)  |
| API response base types          | `src/CashCtrlApiNet.Abstractions/Models/Api/`                         |
| Binary response model            | `src/CashCtrlApiNet.Abstractions/Models/Api/BinaryResponse.cs`        |
| Domain model base types          | `src/CashCtrlApiNet.Abstractions/Models/Base/`                        |
| JSON serialization helper        | `src/CashCtrlApiNet.Abstractions/Helpers/CashCtrlSerialization.cs`    |
| Custom JSON converters           | `src/CashCtrlApiNet.Abstractions/Converters/`                         |
| Unit test base class             | `src/CashCtrlApiNet.UnitTests/ServiceTestBase.cs`                         |
| E2E test base class              | `src/CashCtrlApiNet.UnitTests/CashCtrlE2eTestBase.cs`                     |
| Integration test base class      | `src/CashCtrlApiNet.IntegrationTests/IntegrationTestBase.cs`          |
| WireMock helpers                 | `src/CashCtrlApiNet.IntegrationTests/Helpers/WireMockExtensions.cs`   |
| Response factory                 | `src/CashCtrlApiNet.IntegrationTests/Helpers/CashCtrlResponseFactory.cs` |
| DI registration extensions       | `src/CashCtrlApiNet.AspNetCore/CashCtrlServiceCollectionExtensions.cs`|
| DI options model                 | `src/CashCtrlApiNet.AspNetCore/CashCtrlOptions.cs`                    |
| DI options validator             | `src/CashCtrlApiNet.AspNetCore/CashCtrlOptionsValidator.cs`           |
| DI options adapter               | `src/CashCtrlApiNet.AspNetCore/CashCtrlOptionsAdapter.cs`             |
| API completeness audit           | `doc/analysis/2026-03-24-api-completeness-audit.md`                   |
| Implementation design spec       | `doc/specs/2026-03-23-full-api-implementation-design.md`              |

## Known Constraints and Gotchas

1. **E2E tests require a live CashCtrl account** -- E2E tests (tagged `[Trait("Category", "E2e")]`) call the real API. They require environment variables `CashCtrlApiNet__BaseUri`, `CashCtrlApiNet__ApiKey`, and optionally `CashCtrlApiNet__Language`. Unit tests and integration tests run without credentials.
2. **Test ordering dependency** -- E2E tests use `AlphabeticalOrderer` and are named `Test1_`, `Test2_`, etc. to enforce execution order (Create before Delete).
3. **Integration tests use WireMock** -- The `CashCtrlApiNet.IntegrationTests` project uses WireMock.Net to simulate the CashCtrl API. Tests inherit from `IntegrationTestBase` which manages the WireMock server lifecycle and creates a real `CashCtrlConnectionHandler` pointed at the mock server.
4. **`CashCtrlConnectionHandler` supports dual construction** -- Use `IHttpClientFactory` constructor for DI environments (proper connection pooling/lifetime management), or the standalone `ICashCtrlConfiguration`-only constructor for non-DI usage.
5. **POST uses form-encoded content** -- The CashCtrl API expects `application/x-www-form-urlencoded` for writes, not JSON.
6. **Language query parameter** -- Fixed: the `lang` query parameter is correctly set without a trailing space.
7. **`GeneratePackageOnBuild`** is enabled for all three library projects, so `dotnet build` produces `.nupkg` files in the output.
8. **`GetList` methods lack filter/pagination parameters** -- Most list endpoints accept optional `filter`, `sort`, `dir`, `query` parameters not yet exposed in the service interfaces.
9. **DI registration uses `IHttpClientFactory`** -- `AddCashCtrl` registers `IHttpClientFactory` via `AddHttpClient()` and `CashCtrlConnectionHandler` as scoped, leveraging factory-based `HttpClient` lifetime management.
