---
title: "C4 Level 2: Container Diagram - CashCtrlApiNet"
tags: [architecture, c4, containers]
---

# Container Diagram

```mermaid
C4Container
    title CashCtrlApiNet - Container Diagram (NuGet Packages)

    Person(developer, ".NET Developer", "Consumes the library")

    System_Boundary(lib, "CashCtrlApiNet Library") {
        Container(abstractions, "CashCtrlApiNet.Abstractions", ".NET 10 Class Library / NuGet", "Models, enums, converters, serialization helpers. Zero external dependencies.")
        Container(client, "CashCtrlApiNet", ".NET 10 Class Library / NuGet", "API client, connection handler, connectors, endpoints. Core library.")
        Container(aspnetcore, "CashCtrlApiNet.AspNetCore", ".NET 10 Class Library / NuGet", "ASP.NET Core DI registration extensions. NOT YET IMPLEMENTED.")
    }

    System_Boundary(tests, "Test Suite") {
        Container(test, "CashCtrlApiNet.UnitTests", ".NET 10 / xUnit", "Unit tests (NSubstitute + Shouldly) and integration tests against live CashCtrl API. 393 unit tests covering 375 endpoints.")
    }

    System_Ext(cashctrl, "CashCtrl REST API v1", "External API")

    Rel(developer, client, "References", "NuGet")
    Rel(developer, aspnetcore, "References (for DI)", "NuGet")
    Rel(client, abstractions, "Depends on", "ProjectReference")
    Rel(aspnetcore, client, "Depends on", "ProjectReference")
    Rel(test, client, "Tests", "ProjectReference")
    Rel(client, cashctrl, "HTTPS REST", "Basic Auth")
```

## Package Details

### CashCtrlApiNet.Abstractions

| Property        | Value                                              |
| --------------- | -------------------------------------------------- |
| Technology      | .NET 10 Class Library                               |
| NuGet Package   | `CashCtrlApiNet.Abstractions`                      |
| Repo Path       | `src/CashCtrlApiNet.Abstractions/`                 |
| Responsibility  | Domain models, API response types, enums, JSON converters, serialization helpers |
| Dependencies    | None (only `System.Text.Json` from framework)       |
| Interfaces      | No service interfaces -- only data types            |

**Key directories:**
- `Models/Base/` -- `ModelBaseRecord`, `Entry`, `Entries`, `EntriesCategorize`, `EntryAttachments`
- `Models/Api/` -- `ApiResult`, `ApiResponse`, `ListResponse<T>`, `SingleResponse<T>`, `NoContentResponse`, `ResponseError`
- `Models/{Group}/{Entity}/` -- Domain models for all 10 groups (Account, Common, File, Inventory, Journal, Meta, Order, Person, Report, Salary)
- `Converters/` -- `CashCtrlDateTimeConverter`, `CashCtrlDateTimeNullableConverter`, `IntArrayAsCsvJsonConverter`
- `Helpers/` -- `CashCtrlSerialization` (JSON serialize/deserialize, dictionary conversion)
- `Enums/Api/` -- `Language`, `ApiHeaderNames`
- `Values/` -- `HttpStatusCodeMapping`

### CashCtrlApiNet

| Property        | Value                                              |
| --------------- | -------------------------------------------------- |
| Technology      | .NET 10 Class Library                               |
| NuGet Package   | `CashCtrlApiNet`                                   |
| Repo Path       | `src/CashCtrlApiNet/`                              |
| Responsibility  | HTTP client, API connection handling, typed service interfaces, connector aggregation, endpoint path definitions |
| Dependencies    | `CashCtrlApiNet.Abstractions`                      |
| Key Interface   | `ICashCtrlApiClient` -- entry point for consumers  |

**Key directories:**
- `Interfaces/` -- `ICashCtrlApiClient`, `ICashCtrlConfiguration`, `ICashCtrlConnectionHandler`
- `Interfaces/Connectors/` -- Connector group interfaces (e.g., `IInventoryConnector`)
- `Interfaces/Connectors/{Group}/` -- Individual service interfaces (e.g., `IArticleService`)
- `Services/` -- `CashCtrlApiClient`, `CashCtrlConfiguration`, `CashCtrlConnectionHandler`
- `Services/Connectors/` -- Connector implementations (e.g., `InventoryConnector`)
- `Services/Connectors/{Group}/` -- Service implementations (e.g., `ArticleService`)
- `Services/Connectors/Base/` -- `ConnectorService` abstract base class
- `Services/Endpoints/` -- Static endpoint path constants (e.g., `InventoryEndpoints`)
- `Services/Endpoints/Base/` -- `Api` (version root), `Default` (CRUD action suffixes)

### CashCtrlApiNet.AspNetCore

| Property        | Value                                              |
| --------------- | -------------------------------------------------- |
| Technology      | .NET 10 Class Library                               |
| NuGet Package   | `CashCtrlApiNet.AspNetCore`                        |
| Repo Path       | `src/CashCtrlApiNet.AspNetCore/`                   |
| Responsibility  | Dependency injection registration for ASP.NET Core |
| Dependencies    | `CashCtrlApiNet`                                   |
| Status          | **EMPTY** -- only `.csproj` exists, no C# code     |

### CashCtrlApiNet.UnitTests

| Property        | Value                                              |
| --------------- | -------------------------------------------------- |
| Technology      | .NET 10, xUnit, NSubstitute, Shouldly, FluentAssertions, Coverlet |
| Repo Path       | `tests/CashCtrlApiNet.UnitTests/`                      |
| Responsibility  | Integration tests against live CashCtrl API        |
| Dependencies    | `CashCtrlApiNet` (transitively includes Abstractions) |
| Not Packaged    | `<IsPackable>false</IsPackable>`                   |

**Key files:**
- `ServiceTestBase.cs` -- Base class for unit tests with mocked `ICashCtrlConnectionHandler` (NSubstitute)
- `CashCtrlTestBase.cs` -- Base class for integration tests, reads env vars and constructs `CashCtrlApiClient`
- `AlphabeticalOrderer.cs` -- Custom xUnit test orderer for sequential integration test execution
- `{Group}/{Entity}ServiceTests.cs` -- Unit tests per service (393 total across all 10 domain groups)
