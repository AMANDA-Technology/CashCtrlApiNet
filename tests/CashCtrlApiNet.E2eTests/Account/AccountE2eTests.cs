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

using CashCtrlApiNet.Abstractions.Models.Account;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Account;

/// <summary>
/// E2E tests for account service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Account.IAccountService"/> operations.
/// </summary>
[Category("E2e")]
public class AccountE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupAccountId;
    private int _createdAccountId;
    private int _accountCategoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test account for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan accounts from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Account.Account.GetList(),
            a => a.Name,
            a => a.Id,
            ids => CashCtrlApiClient.Account.Account.Delete(ids));

        // Discover an account category ID for creating accounts
        var categoryResult = await CashCtrlApiClient.Account.Category.GetList();
        _accountCategoryId = categoryResult.ResponseData?.Data.FirstOrDefault()?.Id
                             ?? throw new InvalidOperationException("No account categories found");

        // Create primary test account with unique number
        var createResult = await CashCtrlApiClient.Account.Account.Create(new()
        {
            CategoryId = _accountCategoryId,
            Name = _testId,
            Number = $"{Random.Shared.Next(100000, 999999)}"
        });
        _setupAccountId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Account.Account.Delete(new() { Ids = [_setupAccountId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get an account by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.Get(new() { Id = _setupAccountId });
        var account = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        account.Name.ShouldBe(_testId);
        account.Number.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Get list of accounts successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.GetList();
        var accounts = AssertSuccess(res);

        accounts.Length.ShouldBe(res.ResponseData!.Total);
        accounts.ShouldContain(a => a.Id == _setupAccountId);
    }

    /// <summary>
    /// Get account category tree successfully
    /// </summary>
    [Test, Order(3)]
    public async Task GetCategoryTree_Success()
    {
        var res = await CashCtrlApiClient.Account.Category.GetTree();
        AssertSuccess(res);
    }

    /// <summary>
    /// Get account balance successfully
    /// </summary>
    [Test, Order(4)]
    public async Task GetBalance_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.GetBalance(new() { Id = _setupAccountId });
        AssertSuccess(res);
    }

    /// <summary>
    /// Create an account successfully and store its ID for later tests
    /// </summary>
    [Test, Order(5)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.Create(new()
        {
            CategoryId = _accountCategoryId,
            Name = GenerateTestId(),
            Number = $"{Random.Shared.Next(100000, 999999)}"
        });

        _createdAccountId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Account.Account.Delete(new() { Ids = [_createdAccountId] }));
    }

    /// <summary>
    /// Update an account successfully
    /// </summary>
    [Test, Order(6)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Account.Account.Get(new() { Id = _setupAccountId });
        var account = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get account for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Account.Account.Update((account as AccountUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Account.Account.Get(new() { Id = _setupAccountId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Categorize an account successfully
    /// </summary>
    [Test, Order(7)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.Categorize(new()
        {
            Ids = [_setupAccountId],
            TargetCategoryId = _accountCategoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update account attachments successfully
    /// </summary>
    [Test, Order(8)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.UpdateAttachments(new()
        {
            Id = _setupAccountId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export accounts as Excel successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.Account.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export accounts as CSV successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.Account.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export accounts as PDF successfully
    /// </summary>
    [Test, Order(11)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.Account.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the account created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(12)]
    public async Task Delete_Success()
    {
        _createdAccountId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Account.Account.Delete(new() { Ids = [_createdAccountId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
