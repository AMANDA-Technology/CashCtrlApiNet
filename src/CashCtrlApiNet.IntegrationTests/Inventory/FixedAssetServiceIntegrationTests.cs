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

using System.Collections.Immutable;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Inventory;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Inventory.FixedAssetService"/>
/// </summary>
public class FixedAssetServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single fixed asset with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var fixedAsset = InventoryFakers.FixedAsset.Generate();
        Server.StubGetJson("/api/v1/inventory/asset/read.json",
            CashCtrlResponseFactory.SingleResponse(fixedAsset));

        // Act
        var result = await Client.Inventory.FixedAsset.Get(new Entry { Id = fixedAsset.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(fixedAsset.Id);
        result.ResponseData.Data.Name.ShouldBe(fixedAsset.Name);
        result.ResponseData.Data.Nr.ShouldBe(fixedAsset.Nr);
        result.ResponseData.Data.CategoryId.ShouldBe(fixedAsset.CategoryId);
    }

    /// <summary>
    /// Verify GetList returns a list of fixed assets
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var fixedAssets = InventoryFakers.FixedAssetListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/inventory/asset/list.json",
            CashCtrlResponseFactory.ListResponse(fixedAssets));

        // Act
        var result = await Client.Inventory.FixedAsset.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Fact]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var fixedAssets = InventoryFakers.FixedAssetListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/inventory/asset/list.json",
            CashCtrlResponseFactory.ListResponse(fixedAssets));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.Inventory.FixedAsset.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var fixedAssetCreate = InventoryFakers.FixedAssetCreate.Generate();
        Server.StubPostJson("/api/v1/inventory/asset/create.json",
            CashCtrlResponseFactory.SuccessResponse("Fixed asset created", insertId: 55));

        // Act
        var result = await Client.Inventory.FixedAsset.Create(fixedAssetCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fixed asset created");
        result.ResponseData.InsertId.ShouldBe(55);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var fixedAssetUpdate = InventoryFakers.FixedAssetUpdate.Generate();
        Server.StubPostJson("/api/v1/inventory/asset/update.json",
            CashCtrlResponseFactory.SuccessResponse("Fixed asset updated"));

        // Act
        var result = await Client.Inventory.FixedAsset.Update(fixedAssetUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fixed asset updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/asset/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Fixed asset deleted"));

        // Act
        var result = await Client.Inventory.FixedAsset.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fixed asset deleted");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/asset/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Fixed assets categorized"));

        // Act
        var result = await Client.Inventory.FixedAsset.Categorize(new EntriesCategorize
        {
            Ids = [1, 2],
            TargetCategoryId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/asset/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Inventory.FixedAsset.UpdateAttachments(new EntryAttachments
        {
            Id = 1,
            AttachedFileIds = [10, 20]
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify ExportExcel returns binary data
    /// </summary>
    [Fact]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/inventory/asset/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "fixed-assets.xlsx");

        // Act
        var result = await Client.Inventory.FixedAsset.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify ExportCsv returns binary data
    /// </summary>
    [Fact]
    public async Task ExportCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,name,nr\n1,Test,FA001"u8.ToArray();
        Server.StubGetBinary("/api/v1/inventory/asset/list.csv", csvBytes, "text/csv", "fixed-assets.csv");

        // Act
        var result = await Client.Inventory.FixedAsset.ExportCsv();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify ExportPdf returns binary data
    /// </summary>
    [Fact]
    public async Task ExportPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/inventory/asset/list.pdf", pdfBytes, "application/pdf", "fixed-assets.pdf");

        // Act
        var result = await Client.Inventory.FixedAsset.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
