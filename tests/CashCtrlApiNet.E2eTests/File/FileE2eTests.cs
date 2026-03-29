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

using System.Net.Http.Headers;
using CashCtrlApiNet.Abstractions.Models.File;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.File;

/// <summary>
/// E2E tests for file service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.File.IFileService"/> operations
/// including the two-step Prepare/Persist upload workflow and archive management.
/// </summary>
[Category("E2e")]
public class FileE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupFileId;
    private int _categoryId;
    private int _preparedFileId;
    private int _createdFileId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data, creates a file category and uploads the primary test file for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan files from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.File.File.GetList(),
            f => f.Name ?? string.Empty,
            f => f.Id,
            ids => CashCtrlApiClient.File.File.Delete(ids));

        // Scavenge orphan file categories from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.File.FileCategory.GetList(),
            c => c.Name,
            c => c.Id,
            ids => CashCtrlApiClient.File.FileCategory.Delete(ids));

        // Create a file category for the categorize test
        var categoryResult = await CashCtrlApiClient.File.FileCategory.Create(new()
        {
            Name = _testId
        });
        _categoryId = AssertCreated(categoryResult);

        RegisterCleanup(async () => await CashCtrlApiClient.File.FileCategory.Delete(new() { Ids = [_categoryId] }));

        // Upload primary test file via Prepare + Persist
        var content = new MultipartFormDataContent();
        var fileBytes = "E2E test file content"u8.ToArray();
        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        content.Add(byteContent, "file", $"{_testId}.txt");

        var prepareResult = await CashCtrlApiClient.File.File.Prepare(content);
        _setupFileId = AssertCreated(prepareResult);

        var persistResult = await CashCtrlApiClient.File.File.Persist(new() { Ids = [_setupFileId] });
        AssertSuccess(persistResult);

        RegisterCleanup(async () => await CashCtrlApiClient.File.File.Delete(new() { Ids = [_setupFileId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a file by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.File.File.Get(new() { Id = _setupFileId });
        var file = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        file.Id.ShouldBe(_setupFileId);
        file.Name.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Get list of files successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.File.File.GetList();
        var files = AssertSuccess(res);

        files.Length.ShouldBe(res.ResponseData!.Total);
        files.ShouldContain(f => f.Id == _setupFileId);
    }

    /// <summary>
    /// Get file content (binary download) successfully
    /// </summary>
    [Test, Order(3)]
    public async Task GetContent_Success()
    {
        var res = await CashCtrlApiClient.File.File.GetContent(new() { Id = _setupFileId });
        var binary = AssertSuccess(res);

        binary.Data.Length.ShouldBeGreaterThan(0);
        await DownloadFile(binary.FileName!, binary.Data);
    }

    /// <summary>
    /// Prepare (upload) a new file successfully via multipart form data
    /// </summary>
    [Test, Order(4)]
    public async Task Prepare_Success()
    {
        var prepareTestId = GenerateTestId();
        var content = new MultipartFormDataContent();
        var fileBytes = "E2E prepare test content"u8.ToArray();
        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        content.Add(byteContent, "file", $"{prepareTestId}.txt");

        var res = await CashCtrlApiClient.File.File.Prepare(content);
        _preparedFileId = AssertCreated(res);
    }

    /// <summary>
    /// Persist a previously prepared file successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Persist_Success()
    {
        _preparedFileId.ShouldBeGreaterThan(0, "Prepare_Success must run before Persist_Success");

        var res = await CashCtrlApiClient.File.File.Persist(new() { Ids = [_preparedFileId] });
        AssertSuccess(res);

        RegisterCleanup(async () => await CashCtrlApiClient.File.File.Delete(new() { Ids = [_preparedFileId] }));
    }

    /// <summary>
    /// Create a file entry with metadata using a prepared file ID
    /// </summary>
    [Test, Order(6)]
    public async Task Create_Success()
    {
        // First prepare a new file
        var createTestId = GenerateTestId();
        var content = new MultipartFormDataContent();
        var fileBytes = "E2E create test content"u8.ToArray();
        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        content.Add(byteContent, "file", $"{createTestId}.txt");

        var prepareRes = await CashCtrlApiClient.File.File.Prepare(content);
        var preparedId = AssertCreated(prepareRes);

        // Then create with metadata
        var res = await CashCtrlApiClient.File.File.Create(new()
        {
            Id = preparedId,
            Name = createTestId
        });
        AssertSuccess(res);

        _createdFileId = preparedId;
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.File.File.Delete(new() { Ids = [_createdFileId] }));
    }

    /// <summary>
    /// Update a file successfully
    /// </summary>
    [Test, Order(7)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.File.File.Get(new() { Id = _setupFileId });
        var file = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get file for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.File.File.Update((file as FileUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.File.File.Get(new() { Id = _setupFileId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Categorize a file successfully
    /// </summary>
    [Test, Order(8)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.File.File.Categorize(new()
        {
            Ids = [_setupFileId],
            TargetCategoryId = _categoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export files as Excel successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.File.File.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export files as CSV successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.File.File.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export files as PDF successfully
    /// </summary>
    [Test, Order(11)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.File.File.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete a file (moves to archive) successfully
    /// </summary>
    [Test, Order(12)]
    public async Task Delete_Success()
    {
        _createdFileId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.File.File.Delete(new() { Ids = [_createdFileId] });
        AssertSuccess(res);
    }

    /// <summary>
    /// Restore a previously deleted file from the archive successfully
    /// </summary>
    [Test, Order(13)]
    public async Task Restore_Success()
    {
        var res = await CashCtrlApiClient.File.File.Restore(new() { Ids = [_createdFileId] });
        AssertSuccess(res);
    }

    /// <summary>
    /// Delete a file and then empty the archive to permanently remove all archived files
    /// </summary>
    [Test, Order(14)]
    public async Task EmptyArchive_Success()
    {
        // Delete the file again to move it to archive
        var deleteRes = await CashCtrlApiClient.File.File.Delete(new() { Ids = [_createdFileId] });
        AssertSuccess(deleteRes);

        // Empty the archive to permanently delete all archived files
        var res = await CashCtrlApiClient.File.File.EmptyArchive();
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
