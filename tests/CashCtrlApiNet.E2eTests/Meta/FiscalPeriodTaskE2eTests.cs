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

using Shouldly;

namespace CashCtrlApiNet.E2eTests.Meta;

/// <summary>
/// E2E tests for meta fiscal period task service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Meta.IFiscalPeriodTaskService"/> operations.
/// </summary>
[Category("E2e")]
public class FiscalPeriodTaskE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _fiscalPeriodId;
    private int _createdTaskId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Discovers the current fiscal period and prepares test context
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Discover the current fiscal period for creating tasks
        var listResult = await CashCtrlApiClient.Meta.FiscalPeriod.GetList();
        var periods = AssertSuccess(listResult);
        var currentPeriod = periods.FirstOrDefault(p => p.IsCurrent == true)
                            ?? throw new InvalidOperationException("No current fiscal period found");
        _fiscalPeriodId = currentPeriod.Id;

        // Scavenge orphan tasks from previous failed runs
        var taskList = await CashCtrlApiClient.Meta.FiscalPeriodTask.GetList();
        if (taskList.ResponseData?.Data is { Length: > 0 } tasks)
        {
            var orphanIds = tasks
                .Where(t => t.Name.StartsWith("E2E-", StringComparison.Ordinal))
                .Select(t => t.Id)
                .ToArray();

            if (orphanIds.Length > 0)
                await CashCtrlApiClient.Meta.FiscalPeriodTask.Delete(new() { Ids = [..orphanIds] });
        }
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get list of fiscal period tasks successfully
    /// </summary>
    [Test, Order(1)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Meta.FiscalPeriodTask.GetList();

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Create a fiscal period task successfully and store its ID for later tests
    /// </summary>
    [Test, Order(2)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Meta.FiscalPeriodTask.Create(new()
        {
            FiscalPeriodId = _fiscalPeriodId,
            Name = _testId
        });

        _createdTaskId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Meta.FiscalPeriodTask.Delete(new() { Ids = [_createdTaskId] }));
    }

    /// <summary>
    /// Delete the fiscal period task created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(3)]
    public async Task Delete_Success()
    {
        _createdTaskId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Meta.FiscalPeriodTask.Delete(new() { Ids = [_createdTaskId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
