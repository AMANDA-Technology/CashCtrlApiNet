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

using CashCtrlApiNet.Abstractions.Models.Salary.Setting;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary setting service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalarySettingService"/> operations.
/// </summary>
[Category("E2e")]
[Ignore("Group 7 (Salary) not yet verified against live API — expect model/parameter discrepancies similar to Groups 1-6. See doc/analysis/2026-03-29-e2e-test-verification.md. Remove this attribute when the fixture is verified.")]
// ReSharper disable once InconsistentNaming
public class SalarySettingE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupSettingId;
    private int _createdSettingId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test setting for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan salary settings from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.Setting.GetList(),
            s => s.Name,
            s => s.Id,
            ids => CashCtrlApiClient.Salary.Setting.Delete(ids));

        // Create primary test setting (VariableName max 32 chars)
        var createResult = await CashCtrlApiClient.Salary.Setting.Create(new()
        {
            Name = _testId,
            VariableName = _testId[..32]
        });
        _setupSettingId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Setting.Delete(new() { Ids = [_setupSettingId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary setting by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Setting.Get(new() { Id = _setupSettingId });
        var setting = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        setting.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary settings successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Setting.GetList();
        var settings = AssertSuccess(res);

        settings.Length.ShouldBe(res.ResponseData!.Total);
        settings.ShouldContain(s => s.Id == _setupSettingId);
    }

    /// <summary>
    /// Create a salary setting successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.Setting.Create(new()
        {
            Name = secondTestId,
            VariableName = secondTestId[..32]
        });

        _createdSettingId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.Setting.Delete(new() { Ids = [_createdSettingId] }));
    }

    /// <summary>
    /// Update a salary setting successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Setting.Get(new() { Id = _setupSettingId });
        var setting = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary setting for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Salary.Setting.Update((setting as SalarySettingUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.Setting.Get(new() { Id = _setupSettingId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the salary setting created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdSettingId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.Setting.Delete(new() { Ids = [_createdSettingId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
