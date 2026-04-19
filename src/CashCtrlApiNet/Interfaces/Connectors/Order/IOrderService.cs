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

namespace CashCtrlApiNet.Interfaces.Connectors.Order;

/// <summary>
/// CashCtrl order service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order">API Doc - Order/Order</a>
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Read order. Returns a single order by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/read.json">API Doc - Order/Read order</a>
    /// </summary>
    /// <param name="order">The entry containing the ID of the order.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.Order.Order>>> Get(Entry order, CancellationToken cancellationToken = default);

    /// <summary>
    /// List orders. Returns a list of orders, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/list.json">API Doc - Order/List orders</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<OrderListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new order. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/create.json">API Doc - Order/Create order</a>
    /// </summary>
    /// <param name="order"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(OrderCreate order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update order. Updates an existing order. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/update.json">API Doc - Order/Update order</a>
    /// </summary>
    /// <param name="order"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(OrderUpdate order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete orders. Deletes one or multiple orders. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/delete.json">API Doc - Order/Delete orders</a>
    /// </summary>
    /// <param name="orders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries orders, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update order status. Updates the status of an order.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/update_status.json">API Doc - Order/Update status</a>
    /// </summary>
    /// <param name="status"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateStatus(OrderStatusUpdate status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update order recurrence. Updates the recurrence of an order.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/update_recurrence.json">API Doc - Order/Update recurrence</a>
    /// </summary>
    /// <param name="recurrence"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateRecurrence(OrderRecurrenceUpdate recurrence, CancellationToken cancellationToken = default);

    /// <summary>
    /// Continue one or multiple orders into a new order in a target category (e.g. convert an
    /// offer into an invoice). The target <c>categoryId</c> and <c>ids</c> are mandatory.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/continue.json">API Doc - Order/Continue</a>
    /// </summary>
    /// <param name="request">The continue request carrying the target categoryId + order IDs.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Continue(OrderContinue request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the dossier an order belongs to (group of related orders with a shared <c>groupId</c>).
    /// Returns a single dossier object <c>{id, items:[...]}</c> — not a list response.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/dossier.json">API Doc - Order/Dossier</a>
    /// </summary>
    /// <param name="order">The entry containing the ID of any order in the dossier.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<OrderDossier>>> GetDossier(Entry order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add order to dossier. Adds an order to a dossier.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/dossier_add.json">API Doc - Order/Dossier add</a>
    /// </summary>
    /// <param name="dossier"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> DossierAdd(OrderDossierModify dossier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove order from dossier. Removes an order from a dossier.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/dossier_remove.json">API Doc - Order/Dossier remove</a>
    /// </summary>
    /// <param name="dossier"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> DossierRemove(OrderDossierModify dossier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update attachments. Updates the file attachments of an order. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/update_attachments.json">API Doc - Order/Update attachments</a>
    /// </summary>
    /// <param name="attachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments attachments, CancellationToken cancellationToken = default);

    /// <summary>
    /// Export orders as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/list.xlsx">API Doc - Order/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Export orders as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/list.csv">API Doc - Order/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv(CancellationToken cancellationToken = default);

    /// <summary>
    /// Export orders as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/list.pdf">API Doc - Order/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf(CancellationToken cancellationToken = default);
}
