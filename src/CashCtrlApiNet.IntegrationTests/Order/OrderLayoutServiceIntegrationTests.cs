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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Order.OrderLayoutService"/>
/// </summary>
public class OrderLayoutServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single order layout with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var layout = OrderFakers.Layout.Generate();
        Server.StubGetJson("/api/v1/order/layout/read.json",
            CashCtrlResponseFactory.SingleResponse(layout));

        // Act
        var result = await Client.Order.Layout.Get(new Entry { Id = layout.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(layout.Id);
        result.ResponseData.Data.Name.ShouldBe(layout.Name);
    }

    /// <summary>
    /// Verify GetList returns a list of order layouts
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var layouts = OrderFakers.Layout.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/order/layout/list.json",
            CashCtrlResponseFactory.ListResponse(layouts));

        // Act
        var result = await Client.Order.Layout.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var layoutCreate = OrderFakers.LayoutCreate.Generate();
        Server.StubPostJson("/api/v1/order/layout/create.json",
            CashCtrlResponseFactory.SuccessResponse("Order layout created", insertId: 42));

        // Act
        var result = await Client.Order.Layout.Create(layoutCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order layout created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var layoutUpdate = OrderFakers.LayoutUpdate.Generate();
        Server.StubPostJson("/api/v1/order/layout/update.json",
            CashCtrlResponseFactory.SuccessResponse("Order layout updated"));

        // Act
        var result = await Client.Order.Layout.Update(layoutUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order layout updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/layout/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Order layout deleted"));

        // Act
        var result = await Client.Order.Layout.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order layout deleted");
    }
}
