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

using CashCtrlApiNet.Abstractions.Models.Salary.BookEntry;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary book entry service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryBookEntryService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class SalaryBookEntryE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupBookEntryId;
    private int _createdBookEntryId;
    private int _creditAccountId;
    private int _debitAccountId;
    private int _statementId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Discovers required dependencies and creates the primary test book entry for the fixture.
    /// Skips gracefully if no salary statements exist in the test account.
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Discover accounts for CreditId/DebitId
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        var accountData = accountResult.ResponseData?.Data;
        if (accountData is not { Length: >= 2 })
        {
            Assert.Ignore("Need at least 2 accounts for book entry testing");
            return;
        }

        _creditAccountId = accountData.Value[0].Id;
        _debitAccountId = accountData.Value[1].Id;

        // Discover a salary statement
        var statementResult = await CashCtrlApiClient.Salary.Statement.GetList();
        if (statementResult.ResponseData?.Data is not { Length: > 0 } statements)
        {
            Assert.Ignore("No salary statements available");
            return;
        }

        _statementId = statements.First().Id;

        // Create primary test book entry
        var createResult = await CashCtrlApiClient.Salary.BookEntry.Create(new()
        {
            CreditId = _creditAccountId,
            DebitId = _debitAccountId,
            StatementIds = _statementId.ToString(),
            Description = _testId
        });
        _setupBookEntryId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.BookEntry.Delete(new() { Ids = [_setupBookEntryId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary book entry by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.BookEntry.Get(new() { Id = _setupBookEntryId });
        var bookEntry = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        bookEntry.Description.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary book entries for a statement successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.BookEntry.GetList(new() { Id = _statementId });
        var bookEntries = AssertSuccess(res);

        bookEntries.Length.ShouldBe(res.ResponseData!.Total);
        bookEntries.ShouldContain(b => b.Id == _setupBookEntryId);
    }

    /// <summary>
    /// Create a salary book entry successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Salary.BookEntry.Create(new()
        {
            CreditId = _creditAccountId,
            DebitId = _debitAccountId,
            StatementIds = _statementId.ToString(),
            Description = GenerateTestId()
        });

        _createdBookEntryId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.BookEntry.Delete(new() { Ids = [_createdBookEntryId] }));
    }

    /// <summary>
    /// Update a salary book entry successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.BookEntry.Get(new() { Id = _setupBookEntryId });
        var bookEntry = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary book entry for update");

        var updatedDescription = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Salary.BookEntry.Update((bookEntry as SalaryBookEntryUpdate) with
        {
            Description = updatedDescription
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.BookEntry.Get(new() { Id = _setupBookEntryId });
        verify.ResponseData?.Data?.Description.ShouldBe(updatedDescription);
    }

    /// <summary>
    /// Delete the salary book entry created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdBookEntryId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.BookEntry.Delete(new() { Ids = [_createdBookEntryId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
