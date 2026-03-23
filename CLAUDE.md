---
title: CashCtrlApiNet - Claude Code Instructions
tags: [claude, onboarding, dotnet, api-client]
---

# CashCtrlApiNet

Unofficial .NET 9 API client library for the [CashCtrl REST API v1](https://app.cashctrl.com/static/help/en/api/index.html). CashCtrl is a Swiss cloud ERP for accounting and business management. This library provides a typed C# client with models, JSON serialization, and ASP.NET Core DI integration, distributed as three NuGet packages.

## Tech Stack

| Layer          | Technology                             |
| -------------- | -------------------------------------- |
| Runtime        | .NET 9, C# 13                         |
| HTTP           | `System.Net.Http.HttpClient`           |
| Serialization  | `System.Text.Json`                     |
| DI integration | ASP.NET Core (placeholder, not yet implemented) |
| Testing        | xUnit 2.9, FluentAssertions 6.12, Coverlet |
| Build          | MSBuild (SDK-style csproj)             |
| CI/CD          | GitHub Actions                         |
| License        | MIT                                    |

## Solution Structure

```
CashCtrlApiNet.sln
  src/
    CashCtrlApiNet.Abstractions/   -- Models, enums, converters, serialization helpers (NuGet package)
    CashCtrlApiNet/                -- API client, connection handler, connectors, endpoints (NuGet package)
    CashCtrlApiNet.AspNetCore/     -- ASP.NET Core DI registration (NuGet package, EMPTY - not yet implemented)
    CashCtrlApiNet.Tests/          -- Integration tests (xUnit, not packaged)
```

### Dependency Graph

```
CashCtrlApiNet.Tests --> CashCtrlApiNet --> CashCtrlApiNet.Abstractions
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

# Run tests (requires live CashCtrl API credentials via environment variables)
export CashCtrlApiNet__BaseUri="https://yourorg.cashctrl.com/"
export CashCtrlApiNet__ApiKey="your-api-key"
export CashCtrlApiNet__Language="de"
dotnet test src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj
```

## Key Conventions

### Architecture Pattern
- **Connector pattern**: The API surface is organized into domain groups (Account, Inventory, Order, etc.). Each group has a `Connector` class that aggregates individual `Service` classes. Services inherit from `ConnectorService` base and call `ConnectionHandler.GetAsync/PostAsync`.
- **Three-tier model hierarchy**: For each entity: `XxxCreate` (POST fields) -> `XxxUpdate` (adds `Id`) -> `Xxx`/`XxxListed` (adds read-only server fields). All are `record` types inheriting from `ModelBaseRecord`.
- **Endpoint constants**: API paths are defined as static string constants in `Services/Endpoints/` classes, built from `Api.V1` + group + `Default.Read/List/Create/Update/Delete`.

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

### Serialization
- `System.Text.Json` with `[JsonPropertyName]` attributes matching the CashCtrl API field names.
- Custom converters: `CashCtrlDateTimeConverter` (format: `yyyy-MM-dd HH:mm:ss.f`), `IntArrayAsCsvJsonConverter` (comma-separated int arrays).
- POST bodies sent as `FormUrlEncodedContent` (not JSON), serialized through `CashCtrlSerialization.ConvertToDictionary`.
- GET query parameters also serialized via the same dictionary conversion.

### Authentication
- HTTP Basic Auth: API key as username, empty password, sent as `Authorization: Basic <base64>` header.
- Base URL pattern: `https://{org}.cashctrl.com/api/v1/...`

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
| Domain model base types          | `src/CashCtrlApiNet.Abstractions/Models/Base/`                        |
| JSON serialization helper        | `src/CashCtrlApiNet.Abstractions/Helpers/CashCtrlSerialization.cs`    |
| Custom JSON converters           | `src/CashCtrlApiNet.Abstractions/Converters/`                         |
| Test base class                  | `src/CashCtrlApiNet.Tests/CashCtrlTestBase.cs`                        |

## Implementation Status

The library defines interfaces for the full CashCtrl API but only partially implements them. Most connector constructors have commented-out service instantiations.

| Domain Group | Interfaces Defined | Services Implemented |
| ------------ | ------------------ | -------------------- |
| Account      | 4 services         | 1 (AccountService)   |
| Common       | 7 services         | 0                    |
| File         | 2 services         | 0                    |
| Inventory    | 6 services         | 2 (Article, ArticleCategory) |
| Journal      | 3 services         | 0                    |
| Meta         | 5 services         | 0                    |
| Order        | 5 services         | 0                    |
| Person       | 4 services         | 0                    |
| Report       | 3 services         | 0                    |

Models with full properties: Article, ArticleCategory, Account (stub only).

## Known Constraints and Gotchas

1. **Tests require a live CashCtrl account** -- there are no mocked/unit tests. All tests are integration tests that call the real API. They require environment variables `CashCtrlApiNet__BaseUri`, `CashCtrlApiNet__ApiKey`, and optionally `CashCtrlApiNet__Language`.
2. **ASP.NET Core DI project is empty** -- `CashCtrlApiNet.AspNetCore` has only a `.csproj` file, no actual DI registration code.
3. **Many connectors have uninitialized properties** -- Connectors like `CommonConnector`, `FileConnector`, etc. declare interface properties but never assign them (service instantiation is commented out). Accessing these properties at runtime will return `null` and throw `NullReferenceException`.
4. **Test ordering dependency** -- Tests use `AlphabeticalOrderer` and are named `Test1_`, `Test2_`, etc. to enforce execution order (Create before Delete).
5. **`HttpClient` is created directly in `CashCtrlConnectionHandler`** -- not using `IHttpClientFactory`. This is a limitation for DI/testability.
6. **POST uses form-encoded content** -- The CashCtrl API expects `application/x-www-form-urlencoded` for writes, not JSON.
7. **Language query parameter has a trailing space** -- In `CashCtrlConnectionHandler.GetHttpRequestMessage`, the language key is `"lang "` (with a space). This appears to be a bug.
8. **`GeneratePackageOnBuild`** is enabled for all three library projects, so `dotnet build` produces `.nupkg` files in the output.
