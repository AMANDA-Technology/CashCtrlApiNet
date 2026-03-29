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

using CashCtrlApiNet.Abstractions.Models.Account.CostCenter;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Account;

/// <summary>
/// E2E tests for cost center service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Account.ICostCenterService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class CostCenterE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupCostCenterId;
    private int _createdCostCenterId;
    private int _costCenterCategoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test cost center for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan cost centers from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Account.CostCenter.GetList(),
            c => c.Name,
            c => c.Id,
            ids => CashCtrlApiClient.Account.CostCenter.Delete(ids));

        // Discover or create a cost center category for categorize tests
        var categoryResult = await CashCtrlApiClient.Account.CostCenterCategory.GetList();
        if (categoryResult.ResponseData?.Data is { Length: > 0 } categories)
        {
            _costCenterCategoryId = categories[0].Id;
        }
        else
        {
            var catCreateResult = await CashCtrlApiClient.Account.CostCenterCategory.Create(new()
            {
                Name = $"{_testId}-Category"
            });
            _costCenterCategoryId = AssertCreated(catCreateResult);
            RegisterCleanup(async () => await CashCtrlApiClient.Account.CostCenterCategory.Delete(new() { Ids = [_costCenterCategoryId] }));
        }

        // Create primary test cost center
        var createResult = await CashCtrlApiClient.Account.CostCenter.Create(new()
        {
            Name = _testId,
            CategoryId = _costCenterCategoryId
        });
        _setupCostCenterId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Account.CostCenter.Delete(new() { Ids = [_setupCostCenterId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a cost center by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Account.CostCenter.Get(new() { Id = _setupCostCenterId });
        var costCenter = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        costCenter.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of cost centers successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Account.CostCenter.GetList();
        var costCenters = AssertSuccess(res);

        costCenters.Length.ShouldBe(res.ResponseData!.Total);
        costCenters.ShouldContain(c => c.Id == _setupCostCenterId);
    }

    /// <summary>
    /// Get cost center balance successfully
    /// </summary>
    [Test, Order(3)]
    public async Task GetBalance_Success()
    {
        var res = await CashCtrlApiClient.Account.CostCenter.GetBalance(new() { Id = _setupCostCenterId });
        AssertSuccess(res);
    }

    /// <summary>
    /// Create a cost center successfully and store its ID for later tests
    /// </summary>
    [Test, Order(4)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Account.CostCenter.Create(new()
        {
            Name = GenerateTestId()
        });

        _createdCostCenterId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Account.CostCenter.Delete(new() { Ids = [_createdCostCenterId] }));
    }

    /// <summary>
    /// Update a cost center successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Account.CostCenter.Get(new() { Id = _setupCostCenterId });
        var costCenter = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get cost center for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Account.CostCenter.Update((costCenter as CostCenterUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Account.CostCenter.Get(new() { Id = _setupCostCenterId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Categorize a cost center successfully
    /// </summary>
    [Test, Order(6)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.Account.CostCenter.Categorize(new()
        {
            Ids = [_setupCostCenterId],
            TargetCategoryId = _costCenterCategoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update cost center attachments successfully
    /// </summary>
    [Test, Order(7)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Account.CostCenter.UpdateAttachments(new()
        {
            Id = _setupCostCenterId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export cost centers as Excel successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.CostCenter.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export cost centers as CSV successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.CostCenter.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export cost centers as PDF successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Account.CostCenter.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the cost center created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(11)]
    public async Task Delete_Success()
    {
        _createdCostCenterId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Account.CostCenter.Delete(new() { Ids = [_createdCostCenterId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
