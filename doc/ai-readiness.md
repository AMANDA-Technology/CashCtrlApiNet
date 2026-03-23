---
title: AI Readiness Assessment - CashCtrlApiNet
tags: [ai-readiness, assessment, dotnet, api-client]
---

# AI Readiness Assessment: CashCtrlApiNet

**Date:** 2026-03-23
**Repository:** AMANDA-Technology/CashCtrlApiNet
**Codebase size:** 112 C# files, ~7,500 lines of code (including license headers)

---

## Section 1: Documentation Quality

### What exists and is useful

- **CLAUDE.md** -- Comprehensive and well-structured. Contains build commands, solution structure, dependency graph, key conventions, file locations, implementation status table, and known gotchas. This is the single most valuable doc for AI agents and is in excellent shape.
- **Architecture docs (`doc/architecture/`)** -- Full C4 model (context, containers, components), four ADRs, and a domain glossary. These are AI-generated during onboarding but accurate and thorough. The glossary is particularly useful for understanding CashCtrl domain concepts.
- **XML doc comments** -- Every public member has XML documentation, often with links to the CashCtrl API docs. This is excellent for AI agents navigating the code -- IntelliSense-level documentation is available in-source.
- **README.md** -- Minimal but functional. Contains package list and CI badges.

### What is missing or insufficient

- **No usage examples** -- Neither the README nor any other doc shows how a consumer would actually use the library end-to-end. The test base class (`CashCtrlTestBase.cs`) is the only example of constructing the client, but it is not documented as such.
- **No contribution guide** -- No CONTRIBUTING.md or developer workflow documentation (branching strategy, PR conventions, how to add a new domain group).
- **No changelog** -- No CHANGELOG.md or release notes beyond what GitHub releases might contain.
- **Empty XML doc bodies** -- In `Article.cs` (lines 37-38, 49-50, 54-55, etc.), multiple properties in `ArticleListed` have empty `<summary>` tags (e.g., `/// <summary> /// </summary>`). There are approximately 18 properties in `ArticleListed` with empty doc comments. This means the read-only server fields lack any description.
- **No API mapping reference** -- No document maps which CashCtrl API endpoints are implemented vs. pending. The implementation status table in CLAUDE.md helps but does not list individual endpoints.
- **README is too sparse for a public NuGet package** -- No installation instructions (`dotnet add package`), no quick-start code, no compatibility table.

### What needs improvement

- **Build/CI instructions for contributors** -- The CI workflow (`main.yml`) references a PowerShell module `build\GetBuildVersion.psm1` that does not exist in the repository. This would block AI agents attempting to understand or fix the build pipeline.
- **Dependabot config is broken** -- `.github/dependabot.yml` has `package-ecosystem: ""` (empty string), which means Dependabot is not actually configured. This is misleading.
- **Test environment documentation** -- While CLAUDE.md documents the env vars needed, there is no guidance on how to obtain a CashCtrl test account or what data must exist in the account (the tests assume article with ID 1 exists, category with ID 1 exists, etc.).

### Rating: Needs Work

The CLAUDE.md and architecture docs are strong foundations that make AI-assisted development feasible. However, the missing contribution guide, empty XML docs on `ArticleListed`, broken CI references, and lack of usage examples are gaps that will slow AI agents when working on unfamiliar parts of the codebase.

---

## Section 2: Test Coverage

### Test framework and commands

- **Framework:** xUnit 2.9.2 with FluentAssertions 6.12.2 and Coverlet 6.0.2
- **Run command:** `dotnet test src/CashCtrlApiNet.Tests/CashCtrlApiNet.Tests.csproj`
- **Prerequisite:** Requires live CashCtrl API credentials via environment variables:
  - `CashCtrlApiNet__BaseUri` (e.g., `https://yourorg.cashctrl.com/`)
  - `CashCtrlApiNet__ApiKey`
  - `CashCtrlApiNet__Language` (optional, defaults to `de`)

### What IS covered

- **Article CRUD** (`ArticleTests.cs`, 8 tests): Get, GetList, Create (success + duplicate failure), Update, Delete, Categorize, UpdateAttachments. These tests exercise the full lifecycle and verify both success and error cases.
- **ArticleCategory CRUD** (`ArticleCategoryTests.cs`, 5 tests): Get, GetList, Create, Update, Delete. Similar full lifecycle coverage.
- Both test classes cover the complete HTTP request chain: `Service -> ConnectionHandler -> HttpClient -> CashCtrl API -> Deserialization -> ApiResult`.

### What is NOT covered

- **Abstractions layer (0 tests):** No unit tests for `CashCtrlSerialization`, `CashCtrlDateTimeConverter`, `CashCtrlDateTimeNullableConverter`, `IntArrayAsCsvJsonConverter`, `HttpStatusCodeMapping`. These are pure functions that could be trivially unit tested without API access.
- **Connection handler (0 unit tests):** `CashCtrlConnectionHandler` is only tested indirectly through integration tests. No tests for edge cases: malformed JSON, network errors, timeout handling, missing headers, invalid base URI formats.
- **Account domain (0 tests):** `AccountService` exists but has no test class. The Account models are stubs with no properties, so testing would be limited.
- **7 of 9 domain groups (0 tests):** Common, File, Journal, Meta, Order, Person, Report have zero test coverage because their services are not implemented.
- **No negative/edge case testing:** No tests for: invalid API keys (401), rate limiting (429), server errors (500), cancelled tokens, concurrent requests, `HttpClient` disposal.
- **No unit tests at all:** Every test requires a live API connection. There are no mocked tests for any component. This means:
  - Tests cannot run in CI (the `main.yml` workflow does not run tests)
  - Tests cannot run without a CashCtrl account
  - Tests are slow, non-deterministic, and order-dependent

### Test quality assessment

The existing tests are **meaningful but fragile**:
- They verify real API behavior, which catches actual integration bugs.
- They use `AlphabeticalOrderer` for sequential execution (Create before Delete), which is a pragmatic but brittle approach -- `Test5_Update_Success` returns `Task?` (nullable Task), which is unusual.
- Delete tests poll for up to 30 seconds waiting for created entities to appear, indicating awareness of eventual consistency.
- Tests hardcode entity IDs (e.g., `Id = 1`, `Id = 6`, `CategoryId = 1`, `FileId = 3`) and expected strings (e.g., `"Dienstleistungen"`, `"Article saved"`), making them specific to a particular CashCtrl account configuration.
- Tests mix xUnit `Assert.NotNull` with FluentAssertions `Should()` in the same test methods (inconsistent assertion style).

### Rating: Minimal Coverage

Only 2 of 39 defined service interfaces have test coverage. There are zero unit tests for any component. All tests require external infrastructure. The test suite cannot run in CI and is tightly coupled to a specific CashCtrl account.

---

## Section 3: Technical Debt & Danger Zones

### 1. Null-reference time bombs in unimplemented connectors

**Files:** All connector files except `InventoryConnector.cs`
- `CommonConnector.cs` (lines 51-69): 7 properties declared but never assigned
- `FileConnector.cs`: 2 properties declared but never assigned
- `JournalConnector.cs`: 3 properties declared but never assigned
- `MetaConnector.cs`: 5 properties declared but never assigned
- `OrderConnector.cs`: 5 properties declared but never assigned
- `PersonConnector.cs`: 4 properties declared but never assigned
- `ReportConnector.cs`: 3 properties declared but never assigned
- `AccountConnector.cs` (lines 52-58): 3 properties (`Category`, `CostCenter`, `CostCenterCategory`) declared but never assigned
- `InventoryConnector.cs` (lines 57-66): 4 properties (`FixedAsset`, `FixedAssetCategory`, `Import`, `Unit`) declared but never assigned

**Why dangerous:** Accessing any of these 36 uninitialized properties throws `NullReferenceException` at runtime. The C# compiler emits `CS8618` warnings (non-nullable property not initialized) but these are apparently suppressed or ignored. A consumer who discovers `client.Common.Currency` through IntelliSense will get a crash with no clear explanation.

**Precautions:** When implementing new domain groups, the service assignment in the connector constructor MUST be uncommented/added simultaneously. Consider adding `throw new NotImplementedException()` in property getters or using explicit null-returning properties until services exist.

### 2. Serialization asymmetry bug in `CashCtrlSerialization`

**File:** `src/CashCtrlApiNet.Abstractions/Helpers/CashCtrlSerialization.cs`, lines 72-73

The `Serialize` method calls `JsonSerializer.Serialize(data)` **without** passing `DefaultSerializerOptions`, while `Deserialize` uses them. This means:
- Custom DateTime converters are NOT applied during serialization (only during deserialization)
- `WhenWritingNull` ignore condition is NOT applied during serialization
- `DictionaryKeyPolicy = CamelCase` is NOT applied during serialization

This asymmetry directly affects `ConvertToDictionary` (line 83), which uses `Serialize` then `Deserialize`. The form-encoded POST bodies may include null values and may not format DateTime fields correctly.

**Why dangerous:** This could cause silent data corruption when creating or updating entities with DateTime fields. The current tests pass because only Article (which has no writable DateTime fields) is tested via Create/Update.

**Precautions:** Any AI agent implementing new domain models with DateTime properties MUST verify that POST serialization handles them correctly. This bug should be fixed before implementing models with date fields.

### 3. Language query parameter trailing space bug

**File:** `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs`, line 140

```csharp
query["lang "] = Enum.GetName(_language);
```

The key is `"lang "` (with a trailing space) instead of `"lang"`. This sends `lang%20=de` instead of `lang=de` in the query string. The CashCtrl API apparently still works (tests pass), possibly because it ignores unknown parameters or trims whitespace, but this is technically incorrect.

**Why dangerous:** A future CashCtrl API version might enforce strict parameter validation and reject this. It also makes API responses default to whatever the CashCtrl org's default language is, rather than the explicitly requested language.

**Precautions:** Fix this before any language-dependent testing or internationalization work.

### 4. `HttpClient` not disposable / not using `IHttpClientFactory`

**File:** `src/CashCtrlApiNet/Services/CashCtrlConnectionHandler.cs`, lines 72-77

`CashCtrlConnectionHandler` creates `new HttpClient(...)` directly and never disposes it. The class does not implement `IDisposable`. The `HttpClient` is stored as a `readonly` field with no lifecycle management.

**Why dangerous:**
- In server applications, creating `HttpClient` instances without `IHttpClientFactory` causes socket exhaustion (the well-known .NET `HttpClient` anti-pattern).
- No way to mock `HttpClient` for unit testing.
- The `AllowAutoRedirect = false` handler configuration cannot be changed per-request.

**Precautions:** When implementing the ASP.NET Core DI package, this MUST be refactored to use `IHttpClientFactory`. Any AI agent working on testability improvements needs to address this first.

### 5. `ApiResult.RequestsLeft` uses `set` instead of `init`

**File:** `src/CashCtrlApiNet.Abstractions/Models/Api/ApiResult.cs`, line 54

```csharp
public int? RequestsLeft { get; set; }
```

This is the only property in the entire codebase using `set` instead of `init`. All other record properties use `init`. This breaks the immutability contract of the record type.

**Why dangerous:** Inconsistency with the project's own design principles. Any code could mutate this property after construction, breaking the value-equality semantics that records are supposed to guarantee.

**Precautions:** Change to `init` accessor. Low risk, as `CreateApiResult` uses object initializer syntax which works with `init`.

### 6. Missing `build/GetBuildVersion.psm1` referenced by CI

**File:** `.github/workflows/main.yml`, line 21

The CI workflow imports `.\build\GetBuildVersion.psm1`, but no `build/` directory exists in the repository. This means the NuGet publish workflow is broken.

**Why dangerous:** The publish pipeline cannot work as-is. If an AI agent tries to fix CI or release a new version, they will encounter this missing dependency with no documentation about what it should contain.

**Precautions:** Either restore the missing file or rewrite the version extraction logic. Check git history for the original file.

### 7. Outdated CI action versions

**File:** `.github/workflows/main.yml`
- `actions/checkout@v2` (current: v4)
- `NuGet/setup-nuget@v1.0.7` (current: v2)
- `actions/setup-dotnet@v1` (current: v4)

**File:** `.github/workflows/sonar-analysis.yml`
- `actions/checkout@v3` (current: v4)
- `actions/setup-java@v3` (current: v4)
- `actions/cache@v3` (current: v4)

**Why dangerous:** Older action versions may use deprecated Node.js runtimes and will eventually stop working on GitHub's runners. `actions/checkout@v2` is particularly old.

---

## Section 4: Backlog Ideas

| # | Title | Description | Size | Priority |
|---|-------|-------------|------|----------|
| 1 | Fix serialization asymmetry in `CashCtrlSerialization.Serialize` | Pass `DefaultSerializerOptions` to `JsonSerializer.Serialize()` on line 73 of `CashCtrlSerialization.cs`. Currently custom converters and `WhenWritingNull` are not applied during serialization, only deserialization. This is a correctness bug. | S | High |
| 2 | Fix trailing space in language query parameter | Change `"lang "` to `"lang"` on line 140 of `CashCtrlConnectionHandler.cs`. | S | High |
| 3 | Fix `ApiResult.RequestsLeft` setter to `init` | Change `set` to `init` on line 54 of `ApiResult.cs` to match the immutability pattern used everywhere else. | S | Medium |
| 4 | Add unit tests for serialization and converters | Create unit tests for `CashCtrlSerialization`, `CashCtrlDateTimeConverter`, `CashCtrlDateTimeNullableConverter`, `IntArrayAsCsvJsonConverter`, and `HttpStatusCodeMapping`. These are pure functions requiring no API access. | M | High |
| 5 | Implement ASP.NET Core DI registration | Fill in `CashCtrlApiNet.AspNetCore` with `IServiceCollection` extension methods to register all services. This requires refactoring `CashCtrlConnectionHandler` to use `IHttpClientFactory`. | M | High |
| 6 | Refactor `CashCtrlConnectionHandler` to use `IHttpClientFactory` | Replace direct `new HttpClient()` with `IHttpClientFactory` pattern. Add `IDisposable` implementation. This is a prerequisite for proper DI support and testability. | M | High |
| 7 | Fill empty XML doc comments on `ArticleListed` | Add descriptions to the ~18 properties in `Article.cs` / `ArticleListed` that have empty `<summary>` tags, using CashCtrl API documentation as reference. | S | Medium |
| 8 | Implement Account domain models | Fill in `AccountCreate.cs`, `AccountUpdate.cs`, `Account.cs` with actual properties from the CashCtrl API docs. Currently they are empty stubs. | M | Medium |
| 9 | Implement Common domain group | Add models, services, and endpoints for Currency, CustomField, CustomFieldGroup, Rounding, SequenceNumber, TaxRate, TextTemplate. The interfaces already exist. | L | Medium |
| 10 | Implement Person domain group | Add models, services, and endpoints for Person, PersonCategory, PersonTitle, PersonImport. | L | Medium |
| 11 | Implement Order domain group | Add models, services, and endpoints for Order, OrderCategory, BookEntry, Document, DocumentTemplate. | L | Medium |
| 12 | Implement remaining domain groups | File, Journal, Meta, Report. Each follows the same pattern as Inventory. | L | Low |
| 13 | Add `NotImplementedException` to uninitialized connector properties | Replace silent null returns with explicit exceptions for the 36 uninitialized service properties across all connectors. | S | Medium |
| 14 | Implement `TranslatedText` type | Replace `string` usage for multi-language fields (marked with `// TODO: Implement type for translated texts`) with a proper type that handles the `<values><de>...</de></values>` XML format. Affects `ArticleCreate.Name`, `ArticleCreate.Description`, `ArticleCategoryCreate.Name`. | M | Low |
| 15 | Fix and modernize CI/CD pipeline | Fix missing `build/GetBuildVersion.psm1`, update action versions to v4, add test step (with secrets for API credentials), consider using `dotnet pack` instead of `GeneratePackageOnBuild`. | M | High |
| 16 | Fix Dependabot configuration | Set `package-ecosystem: "nuget"` in `.github/dependabot.yml` (currently empty string). Add `github-actions` ecosystem for workflow updates. | S | Medium |
| 17 | Add usage examples to README | Add NuGet installation instructions, quick-start code snippet, configuration example, and link to API docs. Essential for a public NuGet package. | S | Medium |
| 18 | Add CONTRIBUTING.md | Document the branching strategy, how to add a new domain group (step-by-step recipe), code style requirements, and test environment setup. | S | Medium |
| 19 | Add `IDisposable` to `CashCtrlConnectionHandler` | The class owns an `HttpClient` but never disposes it. Should implement `IDisposable` to clean up the `HttpClientHandler`. | S | Medium |
| 20 | Standardize test assertion style | Tests mix `Assert.NotNull()` with FluentAssertions `Should()`. Pick one approach (FluentAssertions recommended) and use it consistently. | S | Low |
