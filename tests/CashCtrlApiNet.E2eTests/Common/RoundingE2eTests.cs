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

using CashCtrlApiNet.Abstractions.Models.Common.Rounding;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common rounding service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.IRoundingService"/> operations.
/// </summary>
[Category("E2e")]
public class RoundingE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupRoundingId;
    private int _createdRoundingId;
    private int _accountId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test rounding for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan roundings from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.Rounding.GetList(),
            r => r.Name,
            r => r.Id,
            ids => CashCtrlApiClient.Common.Rounding.Delete(ids));

        // Discover an account ID for creating roundings (rounding needs an account for differences)
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        _accountId = accountResult.ResponseData?.Data.FirstOrDefault()?.Id
                     ?? throw new InvalidOperationException("No accounts found for rounding test");

        // Create primary test rounding
        var createResult = await CashCtrlApiClient.Common.Rounding.Create(new()
        {
            Name = _testId,
            AccountId = _accountId,
            Value = 0.05
        });
        _setupRoundingId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.Rounding.Delete(new() { Ids = [_setupRoundingId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a rounding by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.Rounding.Get(new() { Id = _setupRoundingId });
        var rounding = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        rounding.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of roundings successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.Rounding.GetList();
        var roundings = AssertSuccess(res);

        roundings.Length.ShouldBe(res.ResponseData!.Total);
        roundings.ShouldContain(r => r.Id == _setupRoundingId);
    }

    /// <summary>
    /// Create a rounding successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.Rounding.Create(new()
        {
            Name = secondTestId,
            AccountId = _accountId,
            Value = 0.10
        });

        _createdRoundingId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.Rounding.Delete(new() { Ids = [_createdRoundingId] }));
    }

    /// <summary>
    /// Update a rounding successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.Rounding.Get(new() { Id = _setupRoundingId });
        var rounding = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get rounding for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Common.Rounding.Update((rounding as RoundingUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.Rounding.Get(new() { Id = _setupRoundingId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the rounding created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdRoundingId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.Rounding.Delete(new() { Ids = [_createdRoundingId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
