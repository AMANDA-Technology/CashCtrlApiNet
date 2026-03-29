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

using CashCtrlApiNet.Abstractions.Models.Order.Layout;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Order;

/// <summary>
/// E2E tests for order layout service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Order.IOrderLayoutService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class OrderLayoutE2eTests : CashCtrlE2eTestBase
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

        // Scavenge orphan order layouts from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Order.Layout.GetList(),
            l => l.Name,
            l => l.Id,
            ids => CashCtrlApiClient.Order.Layout.Delete(ids));

        // Create primary test order layout
        var createResult = await CashCtrlApiClient.Order.Layout.Create(new()
        {
            Name = _testId
        });
        _setupLayoutId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Order.Layout.Delete(new() { Ids = [_setupLayoutId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get an order layout by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Order.Layout.Get(new() { Id = _setupLayoutId });
        var layout = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        layout.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of order layouts successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Order.Layout.GetList();
        var layouts = AssertSuccess(res);

        layouts.Length.ShouldBe(res.ResponseData!.Total);
        layouts.ShouldContain(l => l.Id == _setupLayoutId);
    }

    /// <summary>
    /// Create an order layout successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Order.Layout.Create(new()
        {
            Name = secondTestId
        });

        _createdLayoutId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Order.Layout.Delete(new() { Ids = [_createdLayoutId] }));
    }

    /// <summary>
    /// Update an order layout successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Order.Layout.Get(new() { Id = _setupLayoutId });
        var layout = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get order layout for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Order.Layout.Update((layout as OrderLayoutUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Order.Layout.Get(new() { Id = _setupLayoutId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the order layout created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdLayoutId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Order.Layout.Delete(new() { Ids = [_createdLayoutId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
