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

using CashCtrlApiNet.Abstractions.Models.Journal.Import;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Journal;

/// <summary>
/// E2E tests for journal import service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Journal.IJournalImportService"/> operations.
/// </summary>
[Category("E2e")]
public class JournalImportE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupImportId;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test journal import for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan journal entries from previous failed runs (imports create journal entries)
        await ScavengeOrphans(
            () => CashCtrlApiClient.Journal.Journal.GetList(),
            j => j.Title,
            j => j.Id,
            ids => CashCtrlApiClient.Journal.Journal.Delete(ids));

        // Upload a CSV file for import via File.Prepare + File.Persist
        var fileId = await UploadImportFile(_testId);

        // Create primary test journal import
        var createResult = await CashCtrlApiClient.Journal.Import.Create(new()
        {
            FileId = fileId,
            Name = _testId
        });
        _setupImportId = AssertCreated(createResult);
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a journal import by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Journal.Import.Get(new() { Id = _setupImportId });
        var import = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        import.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of journal imports successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Journal.Import.GetList();
        var imports = AssertSuccess(res);

        imports.Length.ShouldBe(res.ResponseData!.Total);
        imports.ShouldContain(i => i.Id == _setupImportId);
    }

    /// <summary>
    /// Create a journal import successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var fileId = await UploadImportFile(GenerateTestId());

        var res = await CashCtrlApiClient.Journal.Import.Create(new()
        {
            FileId = fileId,
            Name = GenerateTestId()
        });
        AssertCreated(res);
    }

    /// <summary>
    /// Execute a journal import successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Execute_Success()
    {
        var res = await CashCtrlApiClient.Journal.Import.Execute(new() { Id = _setupImportId });
        AssertSuccess(res);
    }

    /// <summary>
    /// Uploads a minimal CSV file for journal import and returns the persisted file ID
    /// </summary>
    /// <param name="name">A name to include in the CSV data for traceability</param>
    /// <returns>The persisted file ID</returns>
    private async Task<int> UploadImportFile(string name)
    {
        var csvContent = $"Date,Title,Debit,Credit,Amount\n2026-01-01,{name},1000,2000,100.00";
        using var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(csvContent)), "file", $"{name}.csv");

        var prepareResult = await CashCtrlApiClient.File.File.Prepare(content);
        var fileId = AssertCreated(prepareResult);

        var persistResult = await CashCtrlApiClient.File.File.Persist(new() { Ids = [fileId] });
        AssertSuccess(persistResult);

        RegisterCleanup(async () => await CashCtrlApiClient.File.File.Delete(new() { Ids = [fileId] }));

        return fileId;
    }
}
