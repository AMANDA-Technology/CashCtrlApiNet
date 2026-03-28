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
/// E2E tests for account service
/// </summary>
[Category("E2e")]
public class AccountE2eTests : CashCtrlE2eTestBase
{
    /// <summary>
    /// Get an account successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Test1_Get_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.Get(new() { Id = 1 });
        res.IsHttpSuccess.ShouldBeTrue();

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.ShouldNotBeNull();
        res.ResponseData.Data.Name.ShouldNotBeNullOrEmpty();
        res.ResponseData.Data.Number.ShouldBeGreaterThan("0");
    }

    /// <summary>
    /// Get list of accounts successfully
    /// </summary>
    [Test, Order(2)]
    public async Task Test2_GetList_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.GetList();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        res.ResponseData.Data.Length.ShouldBe(res.ResponseData.Total);
    }

    /// <summary>
    /// Get account balance successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Test3_GetBalance_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.GetBalance(new() { Id = 1 });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.ShouldNotBeNull();
        res.ResponseData.Data.Name.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Create an account successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Test4_Create_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.Create(new()
        {
            CategoryId = 1,
            Name = "Test E2E Account",
            Number = "9990"
        });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Success.ShouldBeTrue();
        res.ResponseData.Errors.ShouldBeNull();
        res.ResponseData.InsertId.ShouldNotBeNull();
        res.ResponseData.InsertId.Value.ShouldBeGreaterThan(0);
    }

    /// <summary>
    /// Update an account successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Test5_Update_Success()
    {
        // Find the test account we created
        AccountListed? account = null;
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20)))
        {
            while (account is null && !cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cts.Token);
                account = await GetTestAccount(cts.Token);
            }
        }

        account.ShouldNotBeNull();

        var res = await CashCtrlApiClient.Account.Account.Update((account as AccountUpdate) with
        {
            Name = "Test E2E Account Updated"
        });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Success.ShouldBeTrue();
        res.ResponseData.Errors.ShouldBeNull();
    }

    /// <summary>
    /// Delete an account successfully
    /// </summary>
    [Test, Order(6)]
    public async Task Test6_Delete_Success()
    {
        // Wait until test account exists
        AccountListed? account = null;
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20)))
        {
            while (account is null && !cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cts.Token);
                account = await GetTestAccount(cts.Token);
            }
        }

        account.ShouldNotBeNull();

        var res = await CashCtrlApiClient.Account.Account.Delete(new() { Ids = [account.Id] });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Success.ShouldBeTrue();
        res.ResponseData.Errors.ShouldBeNull();
    }

    /// <summary>
    /// Export accounts as Excel successfully
    /// </summary>
    [Test, Order(7)]
    public async Task Test7_ExportExcel_Success()
    {
        var res = await CashCtrlApiClient.Account.Account.ExportExcel();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);
    }

    private async Task<AccountListed?> GetTestAccount(CancellationToken cancellationToken)
        => (await CashCtrlApiClient.Account.Account.GetList(cancellationToken: cancellationToken))
            .ResponseData?.Data.SingleOrDefault(a => a.Number == "9990");
}
