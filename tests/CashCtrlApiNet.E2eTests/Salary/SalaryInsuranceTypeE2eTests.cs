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

using CashCtrlApiNet.Abstractions.Models.Salary.InsuranceType;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary insurance type service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryInsuranceTypeService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class SalaryInsuranceTypeE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupInsuranceTypeId;
    private int _createdInsuranceTypeId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test insurance type for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan salary insurance types from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.InsuranceType.GetList(),
            t => t.Name,
            t => t.Id,
            ids => CashCtrlApiClient.Salary.InsuranceType.Delete(ids));

        // Create primary test insurance type (Name max 40 chars, testId is 36)
        var createResult = await CashCtrlApiClient.Salary.InsuranceType.Create(new()
        {
            Name = _testId
        });
        _setupInsuranceTypeId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.InsuranceType.Delete(new() { Ids = [_setupInsuranceTypeId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary insurance type by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.InsuranceType.Get(new() { Id = _setupInsuranceTypeId });
        var insuranceType = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        insuranceType.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary insurance types successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.InsuranceType.GetList();
        var insuranceTypes = AssertSuccess(res);

        insuranceTypes.Length.ShouldBe(res.ResponseData!.Total);
        insuranceTypes.ShouldContain(t => t.Id == _setupInsuranceTypeId);
    }

    /// <summary>
    /// Create a salary insurance type successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.InsuranceType.Create(new()
        {
            Name = secondTestId
        });

        _createdInsuranceTypeId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.InsuranceType.Delete(new() { Ids = [_createdInsuranceTypeId] }));
    }

    /// <summary>
    /// Update a salary insurance type successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.InsuranceType.Get(new() { Id = _setupInsuranceTypeId });
        var insuranceType = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary insurance type for update");

        var updatedName = $"{_testId[..31]}-Updated";
        var res = await CashCtrlApiClient.Salary.InsuranceType.Update((insuranceType as SalaryInsuranceTypeUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.InsuranceType.Get(new() { Id = _setupInsuranceTypeId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the salary insurance type created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdInsuranceTypeId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.InsuranceType.Delete(new() { Ids = [_createdInsuranceTypeId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
