# CashCtrlApiNet

Unofficial .NET 10 API client library for the [CashCtrl REST API v1](https://app.cashctrl.com/static/help/en/api/index.html). Provides a fully typed C# client with models, JSON serialization, and ASP.NET Core DI integration.

[CashCtrl](https://cashctrl.com/) is a Swiss cloud ERP for accounting and business management.

[![BuildNuGetAndPublish](https://github.com/AMANDA-Technology/CashCtrlApiNet/actions/workflows/main.yml/badge.svg)](https://github.com/AMANDA-Technology/CashCtrlApiNet/actions/workflows/main.yml)
[![CodeQL](https://github.com/AMANDA-Technology/CashCtrlApiNet/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/AMANDA-Technology/CashCtrlApiNet/actions/workflows/github-code-scanning/codeql)
[![SonarCloud](https://github.com/AMANDA-Technology/CashCtrlApiNet/actions/workflows/sonar-analysis.yml/badge.svg)](https://github.com/AMANDA-Technology/CashCtrlApiNet/actions/workflows/sonar-analysis.yml)

## Features

- 100% CashCtrl REST API v1 coverage (375 endpoints, 58 services, 10 domain groups)
- Immutable record models with three-tier hierarchy (Create, Update, Full)
- ASP.NET Core DI integration with typed `HttpClient` and optional HTTP resilience
- Standalone usage without DI
- Auto-pagination helper (`PaginationHelper.ListAllAsync`)
- Filter, sort, and pagination support on all list endpoints

## Packages

| Package | Description |
|---------|-------------|
| [CashCtrlApiNet](https://www.nuget.org/packages/CashCtrlApiNet/) | API client, connection handler, and all service endpoints |
| [CashCtrlApiNet.Abstractions](https://www.nuget.org/packages/CashCtrlApiNet.Abstractions/) | Models, enums, converters, and serialization helpers |
| [CashCtrlApiNet.AspNetCore](https://www.nuget.org/packages/CashCtrlApiNet.AspNetCore/) | ASP.NET Core dependency injection registration |

## Getting Started

### Installation

```bash
# ASP.NET Core (includes all packages)
dotnet add package CashCtrlApiNet.AspNetCore

# Standalone (no DI)
dotnet add package CashCtrlApiNet
```

### Authentication

CashCtrl uses API key authentication. Generate an API key in your CashCtrl account under *Settings > API*. The base URL follows the pattern `https://{yourorg}.cashctrl.com/`.

### ASP.NET Core (DI)

Register the client in `Program.cs`:

```csharp
builder.Services.AddCashCtrl(options =>
{
    options.BaseUri = "https://myorg.cashctrl.com/";
    options.ApiKey = "your-api-key";
    options.Language = Language.De;       // optional, defaults to German
    options.EnableResilience = true;      // optional, defaults to true (retries, circuit breaker)
});
```

Then inject `ICashCtrlApiClient` wherever you need it:

```csharp
public class MyAccountService(ICashCtrlApiClient cashCtrl)
{
    public async Task<IEnumerable<AccountListed>> GetAllAccounts()
    {
        var result = await cashCtrl.Account.Account.GetList();
        return result.ResponseData!.Data;
    }
}
```

### Standalone (without DI)

```csharp
using var connectionHandler = new CashCtrlConnectionHandler(
    new CashCtrlConfiguration
    {
        BaseUri = "https://myorg.cashctrl.com/",
        ApiKey = "your-api-key",
        DefaultLanguage = "de"
    });

// Use services directly via the connection handler
var accountService = new AccountService(connectionHandler);
var result = await accountService.GetList();

foreach (var account in result.ResponseData!.Data)
    Console.WriteLine($"{account.Number} - {account.Name}");
```

## Usage Examples

### List and filter

```csharp
// List all active articles matching a search query
var result = await cashCtrl.Inventory.Article.GetList(
    new ListParams { Query = "widget", OnlyActive = true });

foreach (var article in result.ResponseData!.Data)
    Console.WriteLine(article.Name);
```

### Create

```csharp
// Create a new person
var result = await cashCtrl.Person.Person.Create(
    new PersonCreate { FirstName = "Jane", LastName = "Doe" });

var newPersonId = result.ResponseData!.InsertId;
```

### Update

```csharp
// Update an existing person
await cashCtrl.Person.Person.Update(
    new PersonUpdate { Id = 42, FirstName = "Jane", LastName = "Smith" });
```

### Auto-pagination

```csharp
// Iterate all journal entries across all pages
await foreach (var entry in PaginationHelper.ListAllAsync(
    cashCtrl.Journal.Journal.GetList,
    new ListParams { Sort = "dateAdded", Dir = "DESC" },
    pageSize: 50))
{
    Console.WriteLine($"{entry.DateAdded} - {entry.Amount}");
}
```

## API Domain Groups

The client is organized into 10 domain groups, accessible via `ICashCtrlApiClient`:

| Property | Domain | Examples |
|----------|--------|----------|
| `Account` | Chart of accounts | Accounts, Categories, Cost Centers, Banks |
| `Common` | Shared resources | Currencies, Tax Rates, Custom Fields, Rounding |
| `File` | File management | Files, File Categories |
| `Inventory` | Products & assets | Articles, Fixed Assets, Units, Imports |
| `Journal` | Bookkeeping | Journal Entries, Imports |
| `Meta` | Organization | Settings, Fiscal Periods, Locations |
| `Order` | Sales & purchases | Orders, Book Entries, Documents, Payments |
| `Person` | Contacts | Persons, Categories, Titles, Imports |
| `Report` | Financial reports | Report Sets, Reports, Report Elements |
| `Salary` | Payroll | Statements, Templates, Certificates, Types |

## License

[MIT](LICENSE)

## Acknowledgements

Special thanks to [CashCtrl AG](https://cashctrl.com/) for the excellent cloud ERP platform.

Built and maintained by [AMANDA Technology GmbH](https://github.com/AMANDA-Technology).
