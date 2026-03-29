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

using CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Inventory;

/// <summary>
/// E2E tests for inventory fixed asset service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IFixedAssetService"/> operations.
/// </summary>
[Category("E2e")]
public class FixedAssetE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupFixedAssetId;
    private int _createdFixedAssetId;
    private int _categoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test fixed asset for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan fixed assets from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Inventory.FixedAsset.GetList(),
            a => a.Name,
            a => a.Id,
            ids => CashCtrlApiClient.Inventory.FixedAsset.Delete(ids));

        // Discover a category ID for the categorize test
        var categoryResult = await CashCtrlApiClient.Inventory.FixedAssetCategory.GetList();
        _categoryId = categoryResult.ResponseData?.Data.FirstOrDefault()?.Id
                      ?? throw new InvalidOperationException("No fixed asset categories found for categorize test");

        // Create primary test fixed asset
        var createResult = await CashCtrlApiClient.Inventory.FixedAsset.Create(new()
        {
            Nr = _testId,
            Name = _testId
        });
        _setupFixedAssetId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Inventory.FixedAsset.Delete(new() { Ids = [_setupFixedAssetId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a fixed asset by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Inventory.FixedAsset.Get(new() { Id = _setupFixedAssetId });
        var fixedAsset = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        fixedAsset.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of fixed assets successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.FixedAsset.GetList();
        var fixedAssets = AssertSuccess(res);

        fixedAssets.Length.ShouldBe(res.ResponseData!.Total);
        fixedAssets.ShouldContain(a => a.Id == _setupFixedAssetId);
    }

    /// <summary>
    /// Create a fixed asset successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Inventory.FixedAsset.Create(new()
        {
            Nr = secondTestId,
            Name = secondTestId
        });

        _createdFixedAssetId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Inventory.FixedAsset.Delete(new() { Ids = [_createdFixedAssetId] }));
    }

    /// <summary>
    /// Update a fixed asset successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Inventory.FixedAsset.Get(new() { Id = _setupFixedAssetId });
        var fixedAsset = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get fixed asset for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Inventory.FixedAsset.Update((fixedAsset as FixedAssetUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Inventory.FixedAsset.Get(new() { Id = _setupFixedAssetId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Categorize a fixed asset successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.Inventory.FixedAsset.Categorize(new()
        {
            Ids = [_setupFixedAssetId],
            TargetCategoryId = _categoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update fixed asset attachments successfully
    /// </summary>
    [Test, Order(6)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Inventory.FixedAsset.UpdateAttachments(new()
        {
            Id = _setupFixedAssetId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export fixed assets as Excel successfully
    /// </summary>
    [Test, Order(7)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Inventory.FixedAsset.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export fixed assets as CSV successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Inventory.FixedAsset.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export fixed assets as PDF successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Inventory.FixedAsset.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the fixed asset created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(10)]
    public async Task Delete_Success()
    {
        _createdFixedAssetId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Inventory.FixedAsset.Delete(new() { Ids = [_createdFixedAssetId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
