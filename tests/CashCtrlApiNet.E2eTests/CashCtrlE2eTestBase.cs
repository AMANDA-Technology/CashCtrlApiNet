/*
MIT License

Copyright (c) 2022 Philip Näf <philip.naef@amanda-technology.ch>
Copyright (c) 2022 Manuel Gysin <manuel.gysin@amanda-technology.ch>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Immutable;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Abstractions.Helpers;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services;
using CashCtrlApiNet.Services.Connectors;
using Shouldly;

namespace CashCtrlApiNet.E2eTests;

/// <summary>
/// Base class for all CashCtrl E2E tests requiring live API credentials.
/// Provides lifecycle helpers for test data creation, cleanup, and assertion.
/// </summary>
// ReSharper disable once InconsistentNaming
public class CashCtrlE2eTestBase
{
    /// <summary>
    /// Default instance of CashCtrl API client
    /// </summary>
    protected readonly ICashCtrlApiClient CashCtrlApiClient;

    private readonly Stack<CleanupAction> _cleanupActions = new();

    /// <summary>
    /// Setup
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    protected CashCtrlE2eTestBase()
    {
        ICashCtrlConnectionHandler connectionHandler = new CashCtrlConnectionHandler(new CashCtrlConfiguration
        {
            BaseUri = Environment.GetEnvironmentVariable("CashCtrlApiNet__BaseUri") ?? throw new InvalidOperationException("Missing CashCtrlApiNet__BaseUri"),
            ApiKey = Environment.GetEnvironmentVariable("CashCtrlApiNet__ApiKey") ?? throw new InvalidOperationException("Missing CashCtrlApiNet__ApiKey"),
            DefaultLanguage = Environment.GetEnvironmentVariable("CashCtrlApiNet__Language") ?? CashCtrlSerialization.SerializeEnumValue(Language.De)
        });

        CashCtrlApiClient = new CashCtrlApiClient(connectionHandler,
            new AccountConnector(connectionHandler),
            new CommonConnector(connectionHandler),
            new FileConnector(connectionHandler),
            new InventoryConnector(connectionHandler),
            new JournalConnector(connectionHandler),
            new MetaConnector(connectionHandler),
            new OrderConnector(connectionHandler),
            new PersonConnector(connectionHandler),
            new ReportConnector(connectionHandler),
            new SalaryConnector(connectionHandler));
    }

    /// <summary>
    /// Generates a unique test identifier with "E2E-" prefix for test data isolation
    /// </summary>
    /// <returns>A unique string in the format "E2E-{guid}"</returns>
    protected static string GenerateTestId() => $"E2E-{Guid.NewGuid():N}";

    /// <summary>
    /// Registers a cleanup action to be executed during teardown in LIFO order.
    /// Returns a cancellation delegate that can be called to skip this cleanup action.
    /// </summary>
    /// <param name="cleanupAction">The async cleanup action to register</param>
    /// <returns>An action that, when invoked, cancels this cleanup entry</returns>
    protected Action RegisterCleanup(Func<Task> cleanupAction)
    {
        var node = new CleanupAction(cleanupAction);
        _cleanupActions.Push(node);
        return node.Cancel;
    }

    /// <summary>
    /// Executes all registered cleanup actions in LIFO order, skipping cancelled entries and continuing on individual failures
    /// </summary>
    protected async Task RunCleanup()
    {
        while (_cleanupActions.TryPop(out var action))
        {
            if (action.IsCancelled)
                continue;

            try
            {
                await action.Execute();
            }
            catch (Exception ex)
            {
                await TestContext.Out.WriteLineAsync($"Cleanup action failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Scavenges orphan test data from previous failed runs by listing entities,
    /// filtering those with "E2E-" name prefix, and deleting them
    /// </summary>
    /// <param name="listFunc">Function to list entities</param>
    /// <param name="nameSelector">Selector for the name/title field to match against "E2E-" prefix</param>
    /// <param name="idSelector">Selector for the entity ID</param>
    /// <param name="deleteFunc">Function to delete entities by IDs</param>
    /// <typeparam name="TListed">The listed entity type</typeparam>
    protected static async Task ScavengeOrphans<TListed>(
        Func<Task<ApiResult<ListResponse<TListed>>>> listFunc,
        Func<TListed, string> nameSelector,
        Func<TListed, int> idSelector,
        Func<Entries, Task<ApiResult<NoContentResponse>>> deleteFunc)
        where TListed : ModelBaseRecord
    {
        var listResult = await listFunc();
        if (listResult.ResponseData?.Data is not { Length: > 0 } items)
            return;

        var orphanIds = items
            .Where(item => nameSelector(item).StartsWith("E2E-", StringComparison.Ordinal))
            .Select(idSelector)
            .ToImmutableArray();

        if (orphanIds.Length > 0)
            await deleteFunc(new() { Ids = orphanIds });
    }

    /// <summary>
    /// Asserts that a <see cref="NoContentResponse"/> API result indicates success with no errors
    /// </summary>
    /// <param name="result">The API result to validate</param>
    protected static void AssertSuccess(ApiResult<NoContentResponse> result)
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        var details = FormatNoContentResponseDetails(result.ResponseData);
        result.ResponseData.Success.ShouldBeTrue(details);
        result.ResponseData.Errors.ShouldBeNull(details);
    }

    /// <summary>
    /// Asserts that a <see cref="NoContentResponse"/> API result indicates a successful creation
    /// and returns the new entity's ID
    /// </summary>
    /// <param name="result">The API result to validate</param>
    /// <returns>The InsertId of the created entity</returns>
    protected static int AssertCreated(ApiResult<NoContentResponse> result)
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        var details = FormatNoContentResponseDetails(result.ResponseData);
        result.ResponseData.Success.ShouldBeTrue(details);
        result.ResponseData.Errors.ShouldBeNull(details);
        result.ResponseData.InsertId.ShouldNotBeNull(details);
        result.ResponseData.InsertId.Value.ShouldBeGreaterThan(0, details);
        return result.ResponseData.InsertId.Value;
    }

    private static string FormatNoContentResponseDetails(NoContentResponse response)
    {
        var errors = response.Errors is { Length: > 0 } list
            ? string.Join("; ", list.Select(e => $"[{e.Field}] {e.Message}"))
            : "<none>";
        return $"API response: Message='{response.Message ?? "<null>"}', Errors={errors}";
    }

    /// <summary>
    /// Asserts that a <see cref="SingleResponse{TData}"/> API result indicates success
    /// and returns the response data
    /// </summary>
    /// <param name="result">The API result to validate</param>
    /// <typeparam name="TData">The entity data type</typeparam>
    /// <returns>The deserialized entity data</returns>
    protected static TData AssertSuccess<TData>(ApiResult<SingleResponse<TData>> result)
        where TData : ModelBaseRecord
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull(FormatRawResponseDetails(result));
        result.ResponseData.Data.ShouldNotBeNull();
        return result.ResponseData.Data;
    }

    /// <summary>
    /// Asserts that a <see cref="ListResponse{TData}"/> API result indicates success
    /// and returns the data array
    /// </summary>
    /// <param name="result">The API result to validate</param>
    /// <typeparam name="TData">The entity data type</typeparam>
    /// <returns>The response data array</returns>
    protected static ImmutableArray<TData> AssertSuccess<TData>(ApiResult<ListResponse<TData>> result)
        where TData : ModelBaseRecord
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull(FormatRawResponseDetails(result));
        result.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        return result.ResponseData.Data;
    }

    private static string FormatRawResponseDetails(ApiResult result)
    {
        var raw = result.RawResponseContent is { Length: > 0 } content
            ? content.Length > 500 ? content[..500] + "…<truncated>" : content
            : "<none>";
        return $"Deserialization likely failed (ResponseData=null). Raw response body: {raw}";
    }

    /// <summary>
    /// Asserts that a <see cref="BinaryResponse"/> API result indicates success with valid file data
    /// and returns the binary response
    /// </summary>
    /// <param name="result">The API result to validate</param>
    /// <returns>The binary response data</returns>
    protected static BinaryResponse AssertSuccess(ApiResult<BinaryResponse> result)
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.FileName.ShouldNotBeNullOrEmpty();
        result.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        return result.ResponseData;
    }

    /// <summary>
    /// Asserts that a <see cref="DecimalResponse"/> API result indicates success
    /// and returns the balance response
    /// </summary>
    /// <param name="result">The API result to validate</param>
    /// <returns>The balance response data</returns>
    protected static DecimalResponse AssertSuccess(ApiResult<DecimalResponse> result)
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        return result.ResponseData;
    }

    /// <summary>
    /// Asserts that a <see cref="PlainTextResponse"/> API result indicates success
    /// and returns the plain text response
    /// </summary>
    /// <param name="result">The API result to validate</param>
    /// <returns>The plain text response data</returns>
    protected static PlainTextResponse AssertSuccess(ApiResult<PlainTextResponse> result)
    {
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Value.ShouldNotBeNullOrEmpty();
        return result.ResponseData;
    }

    /// <summary>
    /// Uploads a file via the 3-step CashCtrl workflow: Prepare (metadata) → PUT (binary to writeUrl) → Persist.
    /// Returns the file ID. Registers cleanup to delete the file.
    /// </summary>
    /// <param name="fileName">The file name including extension</param>
    /// <param name="content">The file content bytes</param>
    /// <param name="mimeType">The MIME type of the file</param>
    /// <returns>The persisted file ID</returns>
    protected async Task<int> UploadTestFile(string fileName, byte[] content, string mimeType = "text/plain")
    {
        // Step 1: Prepare — register file metadata, get pre-authenticated write URL
        var prepareResult = await CashCtrlApiClient.File.File.Prepare(new()
        {
            Files = $"[{{\"name\":\"{fileName}\",\"mimeType\":\"{mimeType}\"}}]"
        });
        prepareResult.IsHttpSuccess.ShouldBeTrue();
        prepareResult.ResponseData.ShouldNotBeNull();
        prepareResult.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        var entry = prepareResult.ResponseData.Data[0];

        // Step 2: PUT — upload binary content to the pre-authenticated URL
        var putResponse = await BinaryUploadClient.PutAsync(entry.WriteUrl, new ByteArrayContent(content));
        putResponse.IsSuccessStatusCode.ShouldBeTrue($"File PUT failed: {putResponse.StatusCode}");

        // Step 3: Persist — finalize the file in CashCtrl
        var persistResult = await CashCtrlApiClient.File.File.Persist(new() { Ids = [entry.FileId] });
        AssertSuccess(persistResult);

        RegisterCleanup(async () => await CashCtrlApiClient.File.File.Delete(new() { Ids = [entry.FileId] }));

        return entry.FileId;
    }

    /// <summary>
    /// Downloads a file to the user's Downloads folder if downloads are enabled
    /// </summary>
    /// <param name="fileName">The file name to save as</param>
    /// <param name="data">The file content bytes</param>
    protected static async Task DownloadFile(string fileName, byte[] data)
    {
        if (!IsDownloadsEnabled)
            return;

        await System.IO.File.WriteAllBytesAsync(Path.Combine(DownloadsFolder, fileName), data);
    }

    private static readonly bool IsDownloadsEnabled = string.Equals(Environment.GetEnvironmentVariable("CashCtrlApiNet__IsDownloadsEnabled"), "true", StringComparison.OrdinalIgnoreCase);

    private static readonly string DownloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    private static readonly HttpClient BinaryUploadClient = new();

    private sealed class CleanupAction(Func<Task> action)
    {
        public bool IsCancelled { get; private set; }

        public void Cancel() => IsCancelled = true;

        public Task Execute() => action();
    }
}
