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

using CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Meta;

/// <summary>
/// E2E tests for meta fiscal period service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Meta.IFiscalPeriodService"/> operations.
/// </summary>
[Category("E2e")]
[Ignore("Group 8 (Meta) not yet verified against live API — highest-risk category: fixtures can touch active fiscal period, organization settings, and other tenant-wide state. See doc/analysis/2026-03-29-e2e-test-verification.md. Remove this attribute when the fixture is verified.")]
// ReSharper disable once InconsistentNaming
public class FiscalPeriodE2eTests : CashCtrlE2eTestBase
{
    private int _setupFiscalPeriodId;
    private int _createdFiscalPeriodId;
    private int _currentFiscalPeriodId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Discovers the current fiscal period and creates a dedicated test fiscal period for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Discover the current fiscal period for reference and to restore after switch test
        var listResult = await CashCtrlApiClient.Meta.FiscalPeriod.GetList();
        var periods = AssertSuccess(listResult);
        var currentPeriod = periods.FirstOrDefault(p => p.IsCurrent == true)
                            ?? throw new InvalidOperationException("No current fiscal period found");
        _currentFiscalPeriodId = currentPeriod.Id;

        // Use the existing current fiscal period as setup period for read-only tests
        _setupFiscalPeriodId = currentPeriod.Id;

        // Scavenge orphan test periods from previous failed runs (far-future 2090 periods)
        var orphanPeriods = periods
            .Where(p => p.StartDate?.StartsWith("2090", StringComparison.Ordinal) is true)
            .ToArray();
        foreach (var orphan in orphanPeriods)
        {
            try
            {
                // Reopen if completed, then delete
                await CashCtrlApiClient.Meta.FiscalPeriod.Reopen(new() { Id = orphan.Id });
            }
            catch { /* may not be completed */ }

            await CashCtrlApiClient.Meta.FiscalPeriod.Delete(new() { Ids = [orphan.Id] });
        }
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        // Ensure we switch back to the original current fiscal period
        try
        {
            await CashCtrlApiClient.Meta.FiscalPeriod.Switch(new() { Id = _currentFiscalPeriodId });
        }
        catch (Exception ex)
        {
            await TestContext.Out.WriteLineAsync($"Failed to restore current fiscal period: {ex.Message}");
        }

        await RunCleanup();
    }

    /// <summary>
    /// Get a fiscal period by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Get(new() { Id = _setupFiscalPeriodId });
        var period = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        period.StartDate.ShouldNotBeNullOrEmpty();
        period.EndDate.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Get list of fiscal periods successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Meta.FiscalPeriod.GetList();
        var periods = AssertSuccess(res);

        periods.Length.ShouldBe(res.ResponseData!.Total);
        periods.ShouldContain(p => p.Id == _setupFiscalPeriodId);
    }

    /// <summary>
    /// Create a fiscal period successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        // Create a fiscal period far in the future to avoid conflicts
        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Create(new()
        {
            StartDate = "2090-01-01",
            EndDate = "2090-12-31"
        });

        _createdFiscalPeriodId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Meta.FiscalPeriod.Delete(new() { Ids = [_createdFiscalPeriodId] }));
    }

    /// <summary>
    /// Update a fiscal period successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before Update_Success");

        var get = await CashCtrlApiClient.Meta.FiscalPeriod.Get(new() { Id = _createdFiscalPeriodId });
        var period = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get fiscal period for update");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Update((period as FiscalPeriodUpdate) with
        {
            EndDate = "2090-06-30"
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Meta.FiscalPeriod.Get(new() { Id = _createdFiscalPeriodId });
        verify.ResponseData?.Data?.EndDate.ShouldBe("2090-06-30");
    }

    /// <summary>
    /// Switch to the created fiscal period and switch back
    /// </summary>
    [Test, Order(5)]
    public async Task Switch_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before Switch_Success");

        // Switch to the test fiscal period
        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Switch(new() { Id = _createdFiscalPeriodId });
        AssertSuccess(res);

        // Switch back to the original current fiscal period
        var restore = await CashCtrlApiClient.Meta.FiscalPeriod.Switch(new() { Id = _currentFiscalPeriodId });
        AssertSuccess(restore);
    }

    /// <summary>
    /// Get result of a fiscal period successfully
    /// </summary>
    [Test, Order(6)]
    public async Task GetResult_Success()
    {
        var res = await CashCtrlApiClient.Meta.FiscalPeriod.GetResult(new() { Id = _setupFiscalPeriodId });
        var period = AssertSuccess(res);

        period.StartDate.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Get depreciations of the test fiscal period successfully
    /// </summary>
    [Test, Order(7)]
    public async Task GetDepreciations_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before GetDepreciations_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.GetDepreciations(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Book depreciations of the test fiscal period successfully.
    /// Uses the isolated 2090 test period to avoid modifying live accounting data.
    /// </summary>
    [Test, Order(8)]
    public async Task BookDepreciations_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before BookDepreciations_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.BookDepreciations(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Get exchange differences of the test fiscal period successfully
    /// </summary>
    [Test, Order(9)]
    public async Task GetExchangeDiff_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before GetExchangeDiff_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.GetExchangeDiff(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Book exchange differences of the test fiscal period successfully.
    /// Uses the isolated 2090 test period to avoid modifying live accounting data.
    /// </summary>
    [Test, Order(10)]
    public async Task BookExchangeDiff_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before BookExchangeDiff_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.BookExchangeDiff(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Complete and reopen a fiscal period successfully
    /// </summary>
    [Test, Order(11)]
    public async Task Complete_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before Complete_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Complete(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Reopen a completed fiscal period successfully
    /// </summary>
    [Test, Order(12)]
    public async Task Reopen_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before Reopen_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Reopen(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Complete months of a fiscal period successfully
    /// </summary>
    [Test, Order(13)]
    public async Task CompleteMonths_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before CompleteMonths_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.CompleteMonths(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Reopen months of a fiscal period successfully
    /// </summary>
    [Test, Order(14)]
    public async Task ReopenMonths_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before ReopenMonths_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.ReopenMonths(new() { Id = _createdFiscalPeriodId });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Delete the fiscal period created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(15)]
    public async Task Delete_Success()
    {
        _createdFiscalPeriodId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriod.Delete(new() { Ids = [_createdFiscalPeriodId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
