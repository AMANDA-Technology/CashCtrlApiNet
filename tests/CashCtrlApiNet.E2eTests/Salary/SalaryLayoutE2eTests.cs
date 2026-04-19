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

using CashCtrlApiNet.Abstractions.Models.Salary.Layout;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary layout service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryLayoutService"/> operations.
/// </summary>
[Category("E2e")]
[Ignore("Group 7 (Salary) not yet verified against live API — expect model/parameter discrepancies similar to Groups 1-6. See doc/analysis/2026-03-29-e2e-test-verification.md. Remove this attribute when the fixture is verified.")]
// ReSharper disable once InconsistentNaming
public class SalaryLayoutE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupLayoutId;
    private int _createdLayoutId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test layout for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan salary layouts from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.Layout.GetList(),
            l => l.Name,
            l => l.Id,
            ids => CashCtrlApiClient.Salary.Layout.Delete(ids));

        // Create primary test layout
        var createResult = await CashCtrlApiClient.Salary.Layout.Create(new()
        {
            Name = _testId
        });
        _setupLayoutId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Layout.Delete(new() { Ids = [_setupLayoutId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary layout by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Layout.Get(new() { Id = _setupLayoutId });
        var layout = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        layout.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary layouts successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Layout.GetList();
        var layouts = AssertSuccess(res);

        layouts.Length.ShouldBe(res.ResponseData!.Total);
        layouts.ShouldContain(l => l.Id == _setupLayoutId);
    }

    /// <summary>
    /// Create a salary layout successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.Layout.Create(new()
        {
            Name = secondTestId
        });

        _createdLayoutId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.Layout.Delete(new() { Ids = [_createdLayoutId] }));
    }

    /// <summary>
    /// Update a salary layout successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Layout.Get(new() { Id = _setupLayoutId });
        var layout = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary layout for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Salary.Layout.Update((layout as SalaryLayoutUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.Layout.Get(new() { Id = _setupLayoutId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the salary layout created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdLayoutId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.Layout.Delete(new() { Ids = [_createdLayoutId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
