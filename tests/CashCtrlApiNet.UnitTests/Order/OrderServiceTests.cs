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

using CashCtrlApiNet.Abstractions.Models.Order;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Order;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Order;

/// <summary>
/// Unit tests for <see cref="OrderService"/>
/// </summary>
public class OrderServiceTests : ServiceTestBase<OrderService>
{
    /// <inheritdoc />
    protected override OrderService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Abstractions.Models.Order.Order>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Abstractions.Models.Order.Order>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Abstractions.Models.Order.Order>, Entry>(
                OrderEndpoints.Order.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<OrderListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<OrderListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<OrderListed>>(
                OrderEndpoints.Order.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<OrderListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<OrderListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<OrderListed>>(
                OrderEndpoints.Order.List, listParams, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<OrderListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<OrderListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var order = new OrderCreate { AccountId = 1, CategoryId = 2, Date = "2024-01-01", SequenceNumberId = 3 };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderCreate>(Arg.Any<string>(), Arg.Any<OrderCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(order);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderCreate>(
                OrderEndpoints.Order.Create, order, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var order = new OrderUpdate { Id = 1, AccountId = 1, CategoryId = 2, Date = "2024-01-01", SequenceNumberId = 3 };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderUpdate>(Arg.Any<string>(), Arg.Any<OrderUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(order);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderUpdate>(
                OrderEndpoints.Order.Update, order, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2, 3] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                OrderEndpoints.Order.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task UpdateStatus_ShouldPostToCorrectEndpoint()
    {
        var status = new OrderStatusUpdate { Id = 1, StatusId = 5 };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderStatusUpdate>(Arg.Any<string>(), Arg.Any<OrderStatusUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateStatus(status);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderStatusUpdate>(
                OrderEndpoints.Order.UpdateStatus, status, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task UpdateRecurrence_ShouldPostToCorrectEndpoint()
    {
        var recurrence = new OrderRecurrenceUpdate { Id = 1, Recurrence = "MONTHLY" };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderRecurrenceUpdate>(Arg.Any<string>(), Arg.Any<OrderRecurrenceUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateRecurrence(recurrence);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderRecurrenceUpdate>(
                OrderEndpoints.Order.UpdateRecurrence, recurrence, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Continue_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Continue(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                OrderEndpoints.Order.Continue, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetDossier_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<ListResponse<OrderListed>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<OrderListed>>());

        await Service.GetDossier(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<OrderListed>, Entry>(
                OrderEndpoints.Order.ReadDossier, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DossierAdd_ShouldPostToCorrectEndpoint()
    {
        var dossier = new OrderDossierModify { Id = 1, DossierId = 10 };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderDossierModify>(Arg.Any<string>(), Arg.Any<OrderDossierModify>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.DossierAdd(dossier);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderDossierModify>(
                OrderEndpoints.Order.DossierAdd, dossier, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DossierRemove_ShouldPostToCorrectEndpoint()
    {
        var dossier = new OrderDossierModify { Id = 1, DossierId = 10 };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderDossierModify>(Arg.Any<string>(), Arg.Any<OrderDossierModify>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.DossierRemove(dossier);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderDossierModify>(
                OrderEndpoints.Order.DossierRemove, dossier, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task UpdateAttachments_ShouldPostToCorrectEndpoint()
    {
        var attachments = new EntryAttachments { Id = 1, AttachedFileIds = [10, 20] };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntryAttachments>(Arg.Any<string>(), Arg.Any<EntryAttachments>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateAttachments(attachments);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntryAttachments>(
                OrderEndpoints.Order.UpdateAttachments, attachments, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(OrderEndpoints.Order.ListXlsx, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task ExportCsv_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.ExportCsv();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(OrderEndpoints.Order.ListCsv, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task ExportPdf_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.ExportPdf();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(OrderEndpoints.Order.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
