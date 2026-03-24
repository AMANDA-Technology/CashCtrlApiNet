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

using System.Runtime.InteropServices;
using CashCtrlApiNet.Abstractions.Models.Order;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Order;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.OrderEndpoints.Order;

namespace CashCtrlApiNet.Services.Connectors.Order;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Order.IOrderService" />
public class OrderService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IOrderService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<Abstractions.Models.Order.Order>>> Get(Entry order, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<Abstractions.Models.Order.Order>, Entry>(Endpoint.Read, order, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<OrderListed>>> GetList([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<OrderListed>>(Endpoint.List, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(OrderCreate order, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, OrderCreate>(Endpoint.Create, order, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(OrderUpdate order, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, OrderUpdate>(Endpoint.Update, order, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries orders, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, orders, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateStatus(OrderStatusUpdate status, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, OrderStatusUpdate>(Endpoint.UpdateStatus, status, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateRecurrence(OrderRecurrenceUpdate recurrence, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, OrderRecurrenceUpdate>(Endpoint.UpdateRecurrence, recurrence, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Continue(Entry order, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.Continue, order, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<OrderListed>>> GetDossier(Entry order, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<OrderListed>, Entry>(Endpoint.ReadDossier, order, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> DossierAdd(OrderDossierModify dossier, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, OrderDossierModify>(Endpoint.DossierAdd, dossier, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> DossierRemove(OrderDossierModify dossier, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, OrderDossierModify>(Endpoint.DossierRemove, dossier, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments attachments, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, EntryAttachments>(Endpoint.UpdateAttachments, attachments, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListXlsx, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListCsv, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListPdf, cancellationToken: cancellationToken);
}
