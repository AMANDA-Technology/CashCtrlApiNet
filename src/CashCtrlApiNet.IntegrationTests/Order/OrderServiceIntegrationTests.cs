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
using CashCtrlApiNet.Abstractions.Models.Order;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Order;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Order.OrderService"/>
/// </summary>
public class OrderServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single order with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var order = OrderFakers.Order.Generate();
        Server.StubGetJson("/api/v1/order/read.json",
            CashCtrlResponseFactory.SingleResponse(order));

        // Act
        var result = await Client.Order.Order.Get(new Entry { Id = order.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(order.Id);
        result.ResponseData.Data.AccountId.ShouldBe(order.AccountId);
        result.ResponseData.Data.CategoryId.ShouldBe(order.CategoryId);
        result.ResponseData.Data.Date.ShouldBe(order.Date);
    }

    /// <summary>
    /// Verify GetList returns a list of orders
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var orders = OrderFakers.OrderListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/order/list.json",
            CashCtrlResponseFactory.ListResponse(orders));

        // Act
        var result = await Client.Order.Order.GetList();

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
        var orders = OrderFakers.OrderListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/order/list.json",
            CashCtrlResponseFactory.ListResponse(orders));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.Order.Order.GetList(listParams);

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
        var orderCreate = OrderFakers.OrderCreate.Generate();
        Server.StubPostJson("/api/v1/order/create.json",
            CashCtrlResponseFactory.SuccessResponse("Order created", insertId: 42));

        // Act
        var result = await Client.Order.Order.Create(orderCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var orderUpdate = OrderFakers.OrderUpdate.Generate();
        Server.StubPostJson("/api/v1/order/update.json",
            CashCtrlResponseFactory.SuccessResponse("Order updated"));

        // Act
        var result = await Client.Order.Order.Update(orderUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Order deleted"));

        // Act
        var result = await Client.Order.Order.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order deleted");
    }

    /// <summary>
    /// Verify UpdateStatus sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task UpdateStatus_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/update_status.json",
            CashCtrlResponseFactory.SuccessResponse("Status updated"));

        // Act
        var result = await Client.Order.Order.UpdateStatus(new OrderStatusUpdate
        {
            Id = 1,
            StatusId = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Status updated");
    }

    /// <summary>
    /// Verify UpdateRecurrence sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task UpdateRecurrence_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/update_recurrence.json",
            CashCtrlResponseFactory.SuccessResponse("Recurrence updated"));

        // Act
        var result = await Client.Order.Order.UpdateRecurrence(new OrderRecurrenceUpdate
        {
            Id = 1,
            Recurrence = "{\"type\":\"monthly\",\"interval\":1}"
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Recurrence updated");
    }

    /// <summary>
    /// Verify Continue sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Continue_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/continue.json",
            CashCtrlResponseFactory.SuccessResponse("Order continued"));

        // Act
        var result = await Client.Order.Order.Continue(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order continued");
    }

    /// <summary>
    /// Verify GetDossier returns a list of orders in the dossier
    /// </summary>
    [Fact]
    public async Task GetDossier_ReturnsExpectedResult()
    {
        // Arrange
        var orders = OrderFakers.OrderListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/order/dossier.json",
            CashCtrlResponseFactory.ListResponse(orders));

        // Act
        var result = await Client.Order.Order.GetDossier(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(2);
    }

    /// <summary>
    /// Verify DossierAdd sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task DossierAdd_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/dossier_add.json",
            CashCtrlResponseFactory.SuccessResponse("Order added to dossier"));

        // Act
        var result = await Client.Order.Order.DossierAdd(new OrderDossierModify
        {
            Id = 1,
            DossierId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order added to dossier");
    }

    /// <summary>
    /// Verify DossierRemove sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task DossierRemove_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/dossier_remove.json",
            CashCtrlResponseFactory.SuccessResponse("Order removed from dossier"));

        // Act
        var result = await Client.Order.Order.DossierRemove(new OrderDossierModify
        {
            Id = 1,
            DossierId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Order removed from dossier");
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Order.Order.UpdateAttachments(new EntryAttachments
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
        Server.StubGetBinary("/api/v1/order/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "orders.xlsx");

        // Act
        var result = await Client.Order.Order.ExportExcel();

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
        var csvBytes = "id,date,amount\n1,2024-01-01,100.00"u8.ToArray();
        Server.StubGetBinary("/api/v1/order/list.csv", csvBytes, "text/csv", "orders.csv");

        // Act
        var result = await Client.Order.Order.ExportCsv();

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
        Server.StubGetBinary("/api/v1/order/list.pdf", pdfBytes, "application/pdf", "orders.pdf");

        // Act
        var result = await Client.Order.Order.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
