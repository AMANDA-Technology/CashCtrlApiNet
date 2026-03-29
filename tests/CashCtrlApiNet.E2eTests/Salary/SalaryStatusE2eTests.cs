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

using CashCtrlApiNet.Abstractions.Enums.Salary;
using CashCtrlApiNet.Abstractions.Models.Salary.Status;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary status service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryStatusService"/> operations.
/// </summary>
[Category("E2e")]
public class SalaryStatusE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupStatusId;
    private int _createdStatusId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test status for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan salary statuses from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.Status.GetList(),
            s => s.Name,
            s => s.Id,
            ids => CashCtrlApiClient.Salary.Status.Delete(ids));

        // Create primary test status (Name max 40 chars, testId is 36)
        var createResult = await CashCtrlApiClient.Salary.Status.Create(new()
        {
            Icon = SalaryStatusIcon.BLUE,
            Name = _testId
        });
        _setupStatusId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Status.Delete(new() { Ids = [_setupStatusId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary status by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Status.Get(new() { Id = _setupStatusId });
        var status = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        status.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary statuses successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Status.GetList();
        var statuses = AssertSuccess(res);

        statuses.Length.ShouldBe(res.ResponseData!.Total);
        statuses.ShouldContain(s => s.Id == _setupStatusId);
    }

    /// <summary>
    /// Create a salary status successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.Status.Create(new()
        {
            Icon = SalaryStatusIcon.GREEN,
            Name = secondTestId
        });

        _createdStatusId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.Status.Delete(new() { Ids = [_createdStatusId] }));
    }

    /// <summary>
    /// Update a salary status successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Status.Get(new() { Id = _setupStatusId });
        var status = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary status for update");

        var updatedName = $"{_testId[..31]}-Updated";
        var res = await CashCtrlApiClient.Salary.Status.Update((status as SalaryStatusUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.Status.Get(new() { Id = _setupStatusId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Reorder salary statuses successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Reorder_Success()
    {
        _createdStatusId.ShouldBeGreaterThan(0, "Create_Success must run before Reorder_Success");

        var res = await CashCtrlApiClient.Salary.Status.Reorder(new()
        {
            Ids = _createdStatusId.ToString(),
            Target = _setupStatusId,
            Before = true
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Delete the salary status created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(6)]
    public async Task Delete_Success()
    {
        _createdStatusId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.Status.Delete(new() { Ids = [_createdStatusId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
