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
using CashCtrlApiNet.Abstractions.Models.Salary.Type;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary type service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryTypeService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class SalaryTypeE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupTypeId;
    private int _createdTypeId;
    private int _categoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test salary type for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan salary types from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.Type.GetList(),
            t => t.Name,
            t => t.Id,
            ids => CashCtrlApiClient.Salary.Type.Delete(ids));

        // Discover or create a salary category for creating types
        var categoryResult = await CashCtrlApiClient.Salary.Category.GetList();
        if (categoryResult.ResponseData?.Data is { Length: > 0 } categories)
        {
            _categoryId = categories.First().Id;
        }
        else
        {
            var catRes = await CashCtrlApiClient.Salary.Category.Create(new() { Name = $"{_testId}-Cat" });
            _categoryId = AssertCreated(catRes);
            RegisterCleanup(async () => await CashCtrlApiClient.Salary.Category.Delete(new() { Ids = [_categoryId] }));
        }

        // Create primary test salary type (Number max 20 chars)
        var createResult = await CashCtrlApiClient.Salary.Type.Create(new()
        {
            CategoryId = _categoryId,
            Name = _testId,
            Number = _testId[..20],
            Type = SalaryTypeKind.ADD
        });
        _setupTypeId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Type.Delete(new() { Ids = [_setupTypeId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary type by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Type.Get(new() { Id = _setupTypeId });
        var salaryType = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        salaryType.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary types successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Type.GetList();
        var types = AssertSuccess(res);

        types.Length.ShouldBe(res.ResponseData!.Total);
        types.ShouldContain(t => t.Id == _setupTypeId);
    }

    /// <summary>
    /// Create a salary type successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.Type.Create(new()
        {
            CategoryId = _categoryId,
            Name = secondTestId,
            Number = secondTestId[..20],
            Type = SalaryTypeKind.ADD
        });

        _createdTypeId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.Type.Delete(new() { Ids = [_createdTypeId] }));
    }

    /// <summary>
    /// Update a salary type successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Type.Get(new() { Id = _setupTypeId });
        var salaryType = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary type for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Salary.Type.Update((salaryType as SalaryTypeUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.Type.Get(new() { Id = _setupTypeId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Categorize a salary type successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.Salary.Type.Categorize(new()
        {
            Ids = [_setupTypeId],
            TargetCategoryId = _categoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export salary types as Excel successfully
    /// </summary>
    [Test, Order(6)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Type.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export salary types as CSV successfully
    /// </summary>
    [Test, Order(7)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Type.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export salary types as PDF successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Type.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the salary type created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(9)]
    public async Task Delete_Success()
    {
        _createdTypeId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.Type.Delete(new() { Ids = [_createdTypeId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
