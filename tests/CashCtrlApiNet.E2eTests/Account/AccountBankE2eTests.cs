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

using CashCtrlApiNet.Abstractions.Enums.Account;
using CashCtrlApiNet.Abstractions.Models.Account.Bank;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Account;

/// <summary>
/// E2E tests for account bank service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Account.IAccountBankService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class AccountBankE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupBankAccountId;
    private int _createdBankAccountId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test bank account for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan bank accounts from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Account.Bank.GetList(),
            b => b.Name,
            b => b.Id,
            ids => CashCtrlApiClient.Account.Bank.Delete(ids));

        // Create primary test bank account
        var createResult = await CashCtrlApiClient.Account.Bank.Create(new()
        {
            Bic = "POFICHBEXXX",
            Iban = "CH9300762011623852957",
            Name = _testId,
            Type = BankAccountType.Default
        });
        _setupBankAccountId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Account.Bank.Delete(new() { Ids = [_setupBankAccountId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a bank account by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Account.Bank.Get(new() { Id = _setupBankAccountId });
        var bankAccount = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        bankAccount.Name.ShouldBe(_testId);
        bankAccount.Iban.ShouldNotBeNullOrEmpty();
        bankAccount.Type.ShouldBe(BankAccountType.Default);
    }

    /// <summary>
    /// Get list of bank accounts successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Account.Bank.GetList();
        var bankAccounts = AssertSuccess(res);

        bankAccounts.Length.ShouldBe(res.ResponseData!.Total);
        bankAccounts.ShouldContain(b => b.Id == _setupBankAccountId);
    }

    /// <summary>
    /// Create a bank account successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Account.Bank.Create(new()
        {
            Bic = "POFICHBEXXX",
            Iban = "CH9300762011623852957",
            Name = GenerateTestId(),
            Type = BankAccountType.Default
        });

        _createdBankAccountId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Account.Bank.Delete(new() { Ids = [_createdBankAccountId] }));
    }

    /// <summary>
    /// Update a bank account successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Account.Bank.Get(new() { Id = _setupBankAccountId });
        var bankAccount = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get bank account for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Account.Bank.Update((bankAccount as AccountBankUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Account.Bank.Get(new() { Id = _setupBankAccountId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Update bank account attachments successfully
    /// </summary>
    [Test, Order(5)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Account.Bank.UpdateAttachments(new()
        {
            Id = _setupBankAccountId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export bank accounts as Excel successfully
    /// </summary>
    [Test, Order(6)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.Bank.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export bank accounts as CSV successfully
    /// </summary>
    [Test, Order(7)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.Bank.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export bank accounts as PDF successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.Bank.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the bank account created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(9)]
    public async Task Delete_Success()
    {
        _createdBankAccountId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Account.Bank.Delete(new() { Ids = [_createdBankAccountId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
