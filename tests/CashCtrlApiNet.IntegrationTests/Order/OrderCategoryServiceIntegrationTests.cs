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

namespace CashCtrlApiNet.IntegrationTests.Order;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Order.OrderCategoryService"/>
/// </summary>
public class OrderCategoryServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single order category with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var category = OrderFakers.Category.Generate();
        Server.StubGetJson("/api/v1/order/category/read.json",
            CashCtrlResponseFactory.SingleResponse(category));

        // Act
        var result = await Client.Order.Category.Get(new() { Id = category.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(category.Id);
        result.ResponseData.Data.Name.ShouldBe(category.Name);
    }

    /// <summary>
    /// Verify GetList returns a list of order categories
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var categories = OrderFakers.Category.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/order/category/list.json",
            CashCtrlResponseFactory.ListResponse(categories));

        // Act
        var result = await Client.Order.Category.GetList();

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
        var categories = OrderFakers.Category.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/order/category/list.json",
            CashCtrlResponseFactory.ListResponse(categories));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Order.Category.GetList(listParams);

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
        var categoryCreate = OrderFakers.CategoryCreate.Generate();
        Server.StubPostJson("/api/v1/order/category/create.json",
            CashCtrlResponseFactory.SuccessResponse("Order category created", insertId: 42));

        // Act
        var result = await Client.Order.Category.Create(categoryCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order category created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var categoryUpdate = OrderFakers.CategoryUpdate.Generate();
        Server.StubPostJson("/api/v1/order/category/update.json",
            CashCtrlResponseFactory.SuccessResponse("Order category updated"));

        // Act
        var result = await Client.Order.Category.Update(categoryUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order category updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/category/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Order category deleted"));

        // Act
        var result = await Client.Order.Category.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order category deleted");
    }

    /// <summary>
    /// Verify Reorder sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Reorder_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/category/reorder.json",
            CashCtrlResponseFactory.SuccessResponse("Order categories reordered"));

        // Act
        var result = await Client.Order.Category.Reorder(new()
        {
            Ids = [1, 2, 3],
            Target = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order categories reordered");
    }

    /// <summary>
    /// Verify GetStatus returns a single order category with correct deserialization
    /// </summary>
    [Test]
    public async Task GetStatus_ReturnsExpectedResult()
    {
        // Arrange
        var category = OrderFakers.Category.Generate();
        Server.StubGetJson("/api/v1/order/category/read_status.json",
            CashCtrlResponseFactory.SingleResponse(category));

        // Act
        var result = await Client.Order.Category.GetStatus(new() { Id = category.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(category.Id);
        result.ResponseData.Data.Name.ShouldBe(category.Name);
    }
}
