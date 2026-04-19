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

using CashCtrlApiNet.Abstractions.Models.Journal.Import.Entry;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Journal;

/// <summary>
/// E2E tests for journal import entry service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Journal.IJournalImportEntryService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class JournalImportEntryE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupImportId;
    private int _setupImportEntryId;
    private JournalImportEntryListRequest ListRequest => new() { ImportId = _setupImportId };

    /// <summary>
    /// Scavenges orphan test data and creates the prerequisite journal import with entries for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan journal entries from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Journal.Journal.GetList(),
            j => j.Title,
            j => j.Id,
            ids => CashCtrlApiClient.Journal.Journal.Delete(ids));

        // Upload a CSV file for import via File.Prepare + File.Persist
        var fileId = await UploadImportFile(_testId);

        // Create a journal import (prerequisite for import entries). Mappings JSON is mandatory
        // per API docs — without it the server returns a masked "unexpected error".
        var importResult = await CashCtrlApiClient.Journal.Import.Create(new()
        {
            FileId = fileId,
            Mappings = """[{"columnDate":"Date","columnDescription":"Title","columnDebitName":"Debit","columnCreditName":"Credit","columnAmount":"Amount"}]"""
        });
        _setupImportId = AssertCreated(importResult);

        // Discover the first import entry created by the import
        var entryListResult = await CashCtrlApiClient.Journal.ImportEntry.GetList(ListRequest);
        var entries = entryListResult.ResponseData?.Data;
        entries.ShouldNotBeNull("Import should have produced entries");
        entries.Value.Length.ShouldBeGreaterThan(0, "Import should have at least one entry");
        _setupImportEntryId = entries.Value[0].Id;
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a journal import entry by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Journal.ImportEntry.Get(new() { Id = _setupImportEntryId });
        var entry = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        entry.Id.ShouldBe(_setupImportEntryId);
    }

    /// <summary>
    /// Get list of journal import entries successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Journal.ImportEntry.GetList(ListRequest);
        var entries = AssertSuccess(res);

        entries.Length.ShouldBe(res.ResponseData!.Total);
        entries.ShouldContain(e => e.Id == _setupImportEntryId);
    }

    /// <summary>
    /// Update a journal import entry successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Update_Success()
    {
        // Read the entry first so we can echo back the server-populated debit/credit IDs.
        // The API requires amount, dateAdded, debitId and creditId on update even though the
        // docs only mark contraAccountId as mandatory — the live API reports all four as
        // "cannot be empty" when they're omitted.
        var get = await CashCtrlApiClient.Journal.ImportEntry.Get(new() { Id = _setupImportEntryId });
        var entry = AssertSuccess(get);

        var res = await CashCtrlApiClient.Journal.ImportEntry.Update((entry as JournalImportEntryUpdate) with
        {
            Title = $"{_testId}-Updated",
            // The response DateAdded is a CashCtrl datetime string ("2026-01-01 00:00:00.0").
            // The update endpoint only accepts YYYY-MM-DD, so trim to the date portion.
            DateAdded = entry.DateAdded?.Split(' ')[0]
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Ignore a journal import entry successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Ignore_Success()
    {
        var res = await CashCtrlApiClient.Journal.ImportEntry.Ignore(new() { Ids = [_setupImportEntryId] });
        AssertSuccess(res);
    }

    /// <summary>
    /// Restore a previously ignored journal import entry successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Restore_Success()
    {
        var res = await CashCtrlApiClient.Journal.ImportEntry.Restore(new() { Ids = [_setupImportEntryId] });
        AssertSuccess(res);
    }

    /// <summary>
    /// Confirm a journal import entry successfully
    /// </summary>
    [Test, Order(6)]
    public async Task Confirm_Success()
    {
        var res = await CashCtrlApiClient.Journal.ImportEntry.Confirm(new() { Ids = [_setupImportEntryId] });
        AssertSuccess(res);
    }

    /// <summary>
    /// Unconfirm a previously confirmed journal import entry successfully
    /// </summary>
    [Test, Order(7)]
    public async Task Unconfirm_Success()
    {
        var res = await CashCtrlApiClient.Journal.ImportEntry.Unconfirm(new() { Ids = [_setupImportEntryId] });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export journal import entries as Excel successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Journal.ImportEntry.ExportExcel(ListRequest));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export journal import entries as CSV successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Journal.ImportEntry.ExportCsv(ListRequest));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export journal import entries as PDF successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Journal.ImportEntry.ExportPdf(ListRequest));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Uploads a minimal CSV file for journal import and returns the persisted file ID
    /// </summary>
    /// <param name="name">A name to include in the CSV data for traceability</param>
    /// <returns>The persisted file ID</returns>
    private async Task<int> UploadImportFile(string name)
    {
        var csvContent = $"Date,Title,Debit,Credit,Amount\n2026-01-01,{name},1000,2000,100.00";
        return await UploadTestFile($"{name}.csv", System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv");
    }
}
