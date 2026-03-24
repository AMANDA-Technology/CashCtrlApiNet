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

using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Inventory;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Inventory.InventoryImportService"/>
/// </summary>
public class InventoryImportServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var importCreate = InventoryFakers.ImportCreate.Generate();
        Server.StubPostJson("/api/v1/inventory/article/import/create.json",
            CashCtrlResponseFactory.SuccessResponse("Import created", insertId: 77));

        // Act
        var result = await Client.Inventory.Import.Create(importCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Import created");
        result.ResponseData.InsertId.ShouldBe(77);
    }

    /// <summary>
    /// Verify Mapping sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Mapping_ReturnsExpectedResult()
    {
        // Arrange
        var importMapping = InventoryFakers.ImportMapping.Generate();
        Server.StubPostJson("/api/v1/inventory/article/import/mapping.json",
            CashCtrlResponseFactory.SuccessResponse("Mapping saved"));

        // Act
        var result = await Client.Inventory.Import.Mapping(importMapping);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Mapping saved");
    }

    /// <summary>
    /// Verify GetMappingFields returns available mapping fields
    /// </summary>
    [Fact]
    public async Task GetMappingFields_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/import/mapping_combo.json",
            "{\"data\":[{\"value\":\"name\",\"text\":\"Name\"},{\"value\":\"nr\",\"text\":\"Number\"}]}");

        // Act
        var result = await Client.Inventory.Import.GetMappingFields();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
    }

    /// <summary>
    /// Verify Preview sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Preview_ReturnsExpectedResult()
    {
        // Arrange
        var importPreview = InventoryFakers.ImportPreview.Generate();
        Server.StubPostJson("/api/v1/inventory/article/import/preview.json",
            CashCtrlResponseFactory.SuccessResponse("Preview generated"));

        // Act
        var result = await Client.Inventory.Import.Preview(importPreview);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Preview generated");
    }

    /// <summary>
    /// Verify Execute sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Execute_ReturnsExpectedResult()
    {
        // Arrange
        var importExecute = InventoryFakers.ImportExecute.Generate();
        Server.StubPostJson("/api/v1/inventory/article/import/execute.json",
            CashCtrlResponseFactory.SuccessResponse("Import executed"));

        // Act
        var result = await Client.Inventory.Import.Execute(importExecute);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Import executed");
    }
}
