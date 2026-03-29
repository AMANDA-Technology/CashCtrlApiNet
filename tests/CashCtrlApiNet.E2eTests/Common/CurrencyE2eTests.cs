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

using CashCtrlApiNet.Abstractions.Models.Common.Currency;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common currency service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.ICurrencyService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class CurrencyE2eTests : CashCtrlE2eTestBase
{
    private int _setupCurrencyId;
    private int _createdCurrencyId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test currency for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Scavenge orphan currencies from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.Currency.GetList(),
            c => c.Description ?? string.Empty,
            c => c.Id,
            ids => CashCtrlApiClient.Common.Currency.Delete(ids));

        // Create primary test currency with unique code
        var testId = GenerateTestId();
        var createResult = await CashCtrlApiClient.Common.Currency.Create(new()
        {
            Code = "ZZZ",
            Description = testId,
            Rate = 1.5
        });
        _setupCurrencyId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.Currency.Delete(new() { Ids = [_setupCurrencyId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a currency by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.Currency.Get(new() { Id = _setupCurrencyId });
        var currency = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        currency.Code.ShouldBe("ZZZ");
    }

    /// <summary>
    /// Get list of currencies successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.Currency.GetList();
        var currencies = AssertSuccess(res);

        currencies.Length.ShouldBe(res.ResponseData!.Total);
        currencies.ShouldContain(c => c.Id == _setupCurrencyId);
    }

    /// <summary>
    /// Create a currency successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var testId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.Currency.Create(new()
        {
            Code = "ZZY",
            Description = testId,
            Rate = 2.0
        });

        _createdCurrencyId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.Currency.Delete(new() { Ids = [_createdCurrencyId] }));
    }

    /// <summary>
    /// Update a currency successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.Currency.Get(new() { Id = _setupCurrencyId });
        var currency = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get currency for update");

        var res = await CashCtrlApiClient.Common.Currency.Update((currency as CurrencyUpdate) with
        {
            Rate = 1.75
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.Currency.Get(new() { Id = _setupCurrencyId });
        verify.ResponseData?.Data?.Rate.ShouldBe(1.75);
    }

    /// <summary>
    /// Get exchange rate between two currencies successfully
    /// </summary>
    [Test, Order(5)]
    public async Task GetExchangeRate_Success()
    {
        var res = await CashCtrlApiClient.Common.Currency.GetExchangeRate(new()
        {
            From = "CHF",
            To = "EUR"
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Delete the currency created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(6)]
    public async Task Delete_Success()
    {
        _createdCurrencyId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.Currency.Delete(new() { Ids = [_createdCurrencyId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
