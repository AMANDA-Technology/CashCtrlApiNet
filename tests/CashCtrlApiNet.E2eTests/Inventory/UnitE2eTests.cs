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

using CashCtrlApiNet.Abstractions.Models.Inventory.Unit;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Inventory;

/// <summary>
/// E2E tests for inventory unit service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IUnitService"/> operations.
/// </summary>
[Category("E2e")]
public class UnitE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupUnitId;
    private int _createdUnitId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test unit for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan units from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Inventory.Unit.GetList(),
            u => u.Name,
            u => u.Id,
            ids => CashCtrlApiClient.Inventory.Unit.Delete(ids));

        // Create primary test unit
        var createResult = await CashCtrlApiClient.Inventory.Unit.Create(new()
        {
            Name = _testId
        });
        _setupUnitId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Inventory.Unit.Delete(new() { Ids = [_setupUnitId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a unit by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Unit.Get(new() { Id = _setupUnitId });
        var unit = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        unit.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of units successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Unit.GetList();
        var units = AssertSuccess(res);

        units.Length.ShouldBe(res.ResponseData!.Total);
        units.ShouldContain(u => u.Id == _setupUnitId);
    }

    /// <summary>
    /// Create a unit successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Inventory.Unit.Create(new()
        {
            Name = secondTestId
        });

        _createdUnitId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Inventory.Unit.Delete(new() { Ids = [_createdUnitId] }));
    }

    /// <summary>
    /// Update a unit successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Inventory.Unit.Get(new() { Id = _setupUnitId });
        var unit = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get unit for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Inventory.Unit.Update((unit as UnitUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Inventory.Unit.Get(new() { Id = _setupUnitId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the unit created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdUnitId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Inventory.Unit.Delete(new() { Ids = [_createdUnitId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
