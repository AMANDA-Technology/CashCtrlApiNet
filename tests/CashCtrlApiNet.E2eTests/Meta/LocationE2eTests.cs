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

using CashCtrlApiNet.Abstractions.Models.Meta.Location;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Meta;

/// <summary>
/// E2E tests for meta location service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Meta.ILocationService"/> operations.
/// </summary>
[Category("E2e")]
public class LocationE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupLocationId;
    private int _createdLocationId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test location for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan locations from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Meta.Location.GetList(),
            l => l.Name,
            l => l.Id,
            ids => CashCtrlApiClient.Meta.Location.Delete(ids));

        // Create primary test location
        var createResult = await CashCtrlApiClient.Meta.Location.Create(new()
        {
            Name = _testId,
            OrgName = _testId
        });
        _setupLocationId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Meta.Location.Delete(new() { Ids = [_setupLocationId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a location by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Meta.Location.Get(new() { Id = _setupLocationId });
        var location = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        location.Name.ShouldBe(_testId);
        location.OrgName.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of locations successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Meta.Location.GetList();
        var locations = AssertSuccess(res);

        locations.Length.ShouldBe(res.ResponseData!.Total);
        locations.ShouldContain(l => l.Id == _setupLocationId);
    }

    /// <summary>
    /// Create a location successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Meta.Location.Create(new()
        {
            Name = secondTestId,
            OrgName = secondTestId
        });

        _createdLocationId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Meta.Location.Delete(new() { Ids = [_createdLocationId] }));
    }

    /// <summary>
    /// Update a location successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Meta.Location.Get(new() { Id = _setupLocationId });
        var location = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get location for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Meta.Location.Update((location as LocationUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Meta.Location.Get(new() { Id = _setupLocationId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the location created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdLocationId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Meta.Location.Delete(new() { Ids = [_createdLocationId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
