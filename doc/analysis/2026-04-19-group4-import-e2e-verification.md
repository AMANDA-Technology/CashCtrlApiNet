# Group 4 Import E2E Verification — Status Report

**Date:** 2026-04-19
**Scope:** Live E2E verification attempt for `InventoryImportE2eTests` and `PersonImportE2eTests`
**Issue:** #89
**Status:** Blocked on live credentials (see below)

## Summary

The Group 4 import workflow E2E fixtures (`InventoryImportE2eTests`, `PersonImportE2eTests`) could not be exercised against the live CashCtrl API in this run. The sandbox environment used for the task did not have the required credentials (`CashCtrlApiNet__BaseUri`, `CashCtrlApiNet__ApiKey`). Static analysis and pattern review against Groups 1-3 findings did not surface any obvious defects that warrant speculative code changes without live evidence.

No code under `src/` or `tests/CashCtrlApiNet.UnitTests/` or `tests/CashCtrlApiNet.IntegrationTests/` was changed as part of this work. The build remains clean with zero warnings, all 631 unit tests pass, and all 460 integration tests pass.

## Verification Status

| Fixture | Tests | Local build | Unit + Integration | Live E2E |
|---------|-------|-------------|---------------------|---------|
| `InventoryImportE2eTests` | 5 | passing | supported by 5 unit tests + 5 integration tests | **not run (no credentials)** |
| `PersonImportE2eTests` | 5 | passing | supported by 5 unit tests + 5 integration tests | **not run (no credentials)** |

Attempted live runs both fail during fixture constructor with:

```
OneTimeSetUp: SetUp : System.InvalidOperationException : Missing CashCtrlApiNet__BaseUri
  at CashCtrlApiNet.E2eTests.CashCtrlE2eTestBase..ctor() in CashCtrlE2eTestBase.cs:line 58
```

This is the correct defensive behavior of `CashCtrlE2eTestBase` — it fails fast when environment variables are absent rather than silently testing against an unconfigured base URI.

## Static Review of Architect Concerns

The architect blueprint flagged six areas that might require fixes based on Groups 1-3 patterns. Each was reviewed:

### FileId (`int` vs other form)
- `InventoryImportCreate.FileId` and `PersonImportCreate.FileId` are both `required int` with `[JsonPropertyName("fileId")]`.
- The File service's `Prepare → Persist` flow returns a numeric file ID (`FilePrepareEntry.FileId` is `int`), and the E2E base class helper `UploadTestFile` returns `int`.
- **Conclusion:** No change. Wiring between File Persist and Import Create is type-consistent. If the live API rejects a numeric file ID, the form-encoded request will surface the error clearly and the model can be updated then.

### Mapping field (`string` vs `JsonElement`)
- `InventoryImportMapping.Mapping` and `PersonImportMapping.Mapping` are both `required string` with `[JsonPropertyName("mapping")]`.
- The CashCtrl API docs describe this parameter as `TEXT` accepting a JSON string. The POST body is `application/x-www-form-urlencoded`, produced by `CashCtrlSerialization.ConvertToDictionary`.
- Empirical verification of STJ's `Dictionary<string, object?>` round-trip shows that a C# `string` holding JSON content (`"[{\"column\":\"Nr\",\"field\":\"nr\"}]"`) serializes to the form-encoded value `mapping=[{"column":"Nr","field":"nr"}]` — which is the expected wire format for a JSON-text TEXT parameter.
- Switching to `JsonElement?` would also produce compatible output, but offers no semantic advantage on the request side because the Mapping model is write-only (never deserialized from a response).
- **Conclusion:** No change. The Groups 1-3 `string? → JsonElement?` pattern was applied to fields that appear in both create requests **and** read responses (e.g., `Insurances`, `Elements`, `Custom`, `Allocations`, `Values`) where the response returns an array. Import Mapping models are not part of any read response.

### Response types (`NoContentResponse`)
- The services currently deserialize `Create`, `Mapping`, `Preview`, and `Execute` responses into `ApiResult<NoContentResponse>`.
- `NoContentResponse` has fields `{ success, errors, message, insertId }`. Any extra JSON fields in the response (e.g., preview rows, execution stats) are silently ignored by `System.Text.Json` defaults.
- The existing E2E fixtures only call `AssertSuccess(res)` / `AssertCreated(res)` — they do not need preview rows or stats. Integration tests confirm the success-case deserialization works.
- **Conclusion:** No change pending live evidence. If future work wants to surface preview rows or execute stats to callers, dedicated response records can be introduced without breaking existing callers.

### Id fields (`"id"` vs `"importId"`)
- `InventoryImportMapping.Id`, `InventoryImportPreview.Id`, `InventoryImportExecute.Id`, and Person counterparts all use `[JsonPropertyName("id")]` with `int` type.
- These are request-side fields only (write-only models). `[JsonNumberHandling]` is relevant for response deserialization only, which does not apply here.
- The API docs for `/inventory/article/import/mapping.json`, `/inventory/article/import/preview.json`, `/inventory/article/import/execute.json` (and Person equivalents) all document the parameter as `id`, not `importId`.
- **Conclusion:** No change. If the live API rejects `id` and expects `importId`, that will be obvious in the 400 response body and fix is a one-liner.

### Array vs string response payloads
- None of the Import models (Create/Mapping/Preview/Execute) are read-response models. They are all write-only request bodies.
- The `GetMappingFields` endpoint returns JSON that is currently consumed as `ApiResult` (untyped). Integration test confirms deserialization succeeds for `{"data":[{"value":"name","text":"Name"}]}`.
- **Conclusion:** No change pending live evidence on `GetMappingFields`.

### String IDs (`JsonNumberHandling.AllowReadingFromString`)
- Not applicable — see "Id fields" above.

## What Can Still Go Wrong on Live API (and how to fix)

Because live verification was not possible, the following risks remain open. Each is cheap to fix if it materializes:

1. **API returns `400 Bad Request` on Create.** Inspect the error payload — likely a required field not documented (e.g., `source`, `name`, `type`). Add to `InventoryImportCreate` / `PersonImportCreate` as `required`.
2. **API rejects `id` in Mapping/Preview/Execute.** Rename to `importId` in `[JsonPropertyName]` (Abstractions project only, no service/interface changes needed).
3. **API returns complex JSON from Preview/Execute that breaks deserialization.** Unlikely given STJ's default behavior of ignoring extra properties, but if it returns a non-object top-level (e.g., an array directly), introduce dedicated response types.
4. **Mapping JSON format differs.** The Inventory test uses an array-of-objects format `[{"column":"Nr","field":"nr"}]` while the Person test uses a flat object `{"lastName":"lastName","firstName":"firstName"}`. One of these may be wrong. The API docs do not specify the exact shape.
5. **VCF import requires `source` parameter.** Some CashCtrl import endpoints accept a `source` (e.g., `outlook`, `apple`). If the Person Create fails, this is a likely culprit.

## Recommended Next Steps for the Team Leader

1. Run E2E tests with live credentials:
   ```bash
   export CashCtrlApiNet__BaseUri="https://<org>.cashctrl.com/"
   export CashCtrlApiNet__ApiKey="<apiKey>"
   dotnet test tests/CashCtrlApiNet.E2eTests/CashCtrlApiNet.E2eTests.csproj \
     --filter "FullyQualifiedName~InventoryImportE2eTests|FullyQualifiedName~PersonImportE2eTests"
   ```
2. Capture any failures and apply the fix patterns summarized above. If the needed fix is outside the "cheap one-liner" category, hand a focused issue back to the developer agent.
3. Append findings to this document and `doc/analysis/2026-03-29-e2e-test-verification.md` (Group 4 row) once the live run is complete.

## Environment Notes

- .NET SDK: 10.0
- Build command: `dotnet build CashCtrlApiNet.sln -p:TreatWarningsAsErrors=true -m:1`
- Unit tests: `dotnet test tests/CashCtrlApiNet.UnitTests/CashCtrlApiNet.UnitTests.csproj --no-build`
- Integration tests: `dotnet test tests/CashCtrlApiNet.IntegrationTests/CashCtrlApiNet.IntegrationTests.csproj --no-build`
- NuGet packages: no outdated packages (confirmed via `dotnet list package --outdated`)
