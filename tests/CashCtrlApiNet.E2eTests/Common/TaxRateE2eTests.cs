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

using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.Abstractions.Models.Common.TaxRate;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common tax rate service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.ITaxRateService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class TaxRateE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupTaxRateId;
    private int _createdTaxRateId;
    private int _accountId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test tax rate for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan tax rates from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.TaxRate.GetList(),
            t => t.Description,
            t => t.Id,
            ids => CashCtrlApiClient.Common.TaxRate.Delete(ids));

        // Discover an account ID for creating tax rate components
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        _accountId = accountResult.ResponseData?.Data.FirstOrDefault()?.Id
                     ?? throw new InvalidOperationException("No accounts found for tax rate test");

        // Create primary test tax rate
        var createResult = await CashCtrlApiClient.Common.TaxRate.Create(new()
        {
            Description = _testId,
            Code = "E2E",
            Components =
            [
                new TaxRateComponent
                {
                    Code = "E2E-C1",
                    AccountId = _accountId,
                    CalcType = TaxCalcType.Net,
                    ApplyRule = TaxApplyRule.REVENUE
                }
            ],
            Rates =
            [
                new TaxRateHistoricalRate
                {
                    Percentage = 7.7,
                    DateValid = "2020-01-01"
                }
            ]
        });
        _setupTaxRateId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.TaxRate.Delete(new() { Ids = [_setupTaxRateId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a tax rate by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.TaxRate.Get(new() { Id = _setupTaxRateId });
        var taxRate = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        taxRate.Description.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of tax rates successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.TaxRate.GetList();
        var taxRates = AssertSuccess(res);

        taxRates.Length.ShouldBe(res.ResponseData!.Total);
        taxRates.ShouldContain(t => t.Id == _setupTaxRateId);
    }

    /// <summary>
    /// Create a tax rate successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.TaxRate.Create(new()
        {
            Description = secondTestId,
            Code = "E2E2",
            Components =
            [
                new TaxRateComponent
                {
                    Code = "E2E2-C1",
                    AccountId = _accountId,
                    CalcType = TaxCalcType.Net,
                    ApplyRule = TaxApplyRule.REVENUE
                }
            ],
            Rates =
            [
                new TaxRateHistoricalRate
                {
                    Percentage = 2.5,
                    DateValid = "2020-01-01"
                }
            ]
        });

        _createdTaxRateId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.TaxRate.Delete(new() { Ids = [_createdTaxRateId] }));
    }

    /// <summary>
    /// Update a tax rate successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.TaxRate.Get(new() { Id = _setupTaxRateId });
        var taxRate = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get tax rate for update");

        var updatedDescription = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Common.TaxRate.Update((taxRate as TaxRateUpdate) with
        {
            Description = updatedDescription
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.TaxRate.Get(new() { Id = _setupTaxRateId });
        verify.ResponseData?.Data?.Description.ShouldBe(updatedDescription);
    }

    /// <summary>
    /// Delete the tax rate created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdTaxRateId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.TaxRate.Delete(new() { Ids = [_createdTaxRateId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
