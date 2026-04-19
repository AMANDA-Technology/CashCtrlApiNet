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

using CashCtrlApiNet.Abstractions.Models.Journal;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Journal;

/// <summary>
/// E2E tests for journal service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Journal.IJournalService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class JournalE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupJournalId;
    private int _createdJournalId;
    private int _debitAccountId;
    private int _creditAccountId;
    private int _sequenceNumberId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test journal entry for the fixture
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

        // Discover debit and credit account IDs
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        var accounts = accountResult.ResponseData?.Data
                       ?? throw new InvalidOperationException("No accounts found");
        _debitAccountId = accounts[0].Id;
        _creditAccountId = accounts.Length > 1 ? accounts[1].Id : accounts[0].Id;

        // Discover a sequence number ID
        var seqResult = await CashCtrlApiClient.Common.SequenceNumber.GetList();
        _sequenceNumberId = seqResult.ResponseData?.Data.FirstOrDefault()?.Id
                            ?? throw new InvalidOperationException("No sequence numbers found");

        // Create primary test journal entry
        var createResult = await CashCtrlApiClient.Journal.Journal.Create(new()
        {
            DateAdded = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Title = _testId,
            SequenceNumberId = _sequenceNumberId,
            DebitId = _debitAccountId,
            CreditId = _creditAccountId,
            Amount = 100.00
        });
        _setupJournalId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Journal.Journal.Delete(new() { Ids = [_setupJournalId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a journal entry by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Journal.Journal.Get(new() { Id = _setupJournalId });
        var journal = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        journal.Title.ShouldBe(_testId);
        // SequenceNumberId does not round-trip — the server returns the generated Reference
        // (e.g. "RE-2604193") in its place. Assert on the derived Reference instead.
        journal.Reference.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Get list of journal entries successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Journal.Journal.GetList();
        var journals = AssertSuccess(res);

        journals.Length.ShouldBe(res.ResponseData!.Total);
        journals.ShouldContain(j => j.Id == _setupJournalId);
    }

    /// <summary>
    /// Create a journal entry successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Journal.Journal.Create(new()
        {
            DateAdded = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Title = GenerateTestId(),
            SequenceNumberId = _sequenceNumberId,
            DebitId = _debitAccountId,
            CreditId = _creditAccountId,
            Amount = 50.00
        });

        _createdJournalId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Journal.Journal.Delete(new() { Ids = [_createdJournalId] }));
    }

    /// <summary>
    /// Update a journal entry successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Journal.Journal.Get(new() { Id = _setupJournalId });
        var journal = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get journal for update");

        var updatedTitle = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Journal.Journal.Update((journal as JournalUpdate) with
        {
            Title = updatedTitle,
            // Per API docs: items must be OMITTED for a regular (debit/credit) entry.
            // The read response returns items as [] which, if echoed back, is interpreted as
            // "collective entry with zero items" → "At least 1 book entry must be created".
            Items = null
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Journal.Journal.Get(new() { Id = _setupJournalId });
        verify.ResponseData?.Data?.Title.ShouldBe(updatedTitle);
    }

    /// <summary>
    /// Update journal entry attachments successfully
    /// </summary>
    [Test, Order(5)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Journal.Journal.UpdateAttachments(new()
        {
            Id = _setupJournalId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update journal entry recurrence successfully
    /// </summary>
    [Test, Order(6)]
    public async Task UpdateRecurrence_Success()
    {
        var res = await CashCtrlApiClient.Journal.Journal.UpdateRecurrence(new()
        {
            Id = _setupJournalId,
            Recurrence = null
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export journal entries as Excel successfully
    /// </summary>
    [Test, Order(7)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Journal.Journal.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export journal entries as CSV successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Journal.Journal.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export journal entries as PDF successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Journal.Journal.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the journal entry created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(10)]
    public async Task Delete_Success()
    {
        _createdJournalId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Journal.Journal.Delete(new() { Ids = [_createdJournalId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
