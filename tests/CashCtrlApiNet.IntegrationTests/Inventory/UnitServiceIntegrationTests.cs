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

using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Inventory;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Inventory.UnitService"/>
/// </summary>
public class UnitServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single unit with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var unit = InventoryFakers.Unit.Generate();
        Server.StubGetJson("/api/v1/inventory/unit/read.json",
            CashCtrlResponseFactory.SingleResponse(unit));

        // Act
        var result = await Client.Inventory.Unit.Get(new() { Id = unit.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(unit.Id);
        result.ResponseData.Data.Name.ShouldBe(unit.Name);
    }

    /// <summary>
    /// Verify GetList returns a list of units
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var units = InventoryFakers.Unit.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/inventory/unit/list.json",
            CashCtrlResponseFactory.ListResponse(units));

        // Act
        var result = await Client.Inventory.Unit.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Test]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var units = InventoryFakers.Unit.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/inventory/unit/list.json",
            CashCtrlResponseFactory.ListResponse(units));
        var listParams = new ListParams { Limit = 5 };

        // Act
        var result = await Client.Inventory.Unit.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var unitCreate = InventoryFakers.UnitCreate.Generate();
        Server.StubPostJson("/api/v1/inventory/unit/create.json",
            CashCtrlResponseFactory.SuccessResponse("Unit created", insertId: 15));

        // Act
        var result = await Client.Inventory.Unit.Create(unitCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Unit created");
        result.ResponseData.InsertId.ShouldBe(15);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var unitUpdate = InventoryFakers.UnitUpdate.Generate();
        Server.StubPostJson("/api/v1/inventory/unit/update.json",
            CashCtrlResponseFactory.SuccessResponse("Unit updated"));

        // Act
        var result = await Client.Inventory.Unit.Update(unitUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Unit updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/unit/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Unit deleted"));

        // Act
        var result = await Client.Inventory.Unit.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Unit deleted");
    }
}
