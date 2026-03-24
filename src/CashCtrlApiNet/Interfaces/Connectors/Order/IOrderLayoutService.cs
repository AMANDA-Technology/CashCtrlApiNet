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
using CashCtrlApiNet.Abstractions.Models.Order.Layout;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Order;

/// <summary>
/// CashCtrl order layout service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/layout">API Doc - Order/Layout</a>
/// </summary>
public interface IOrderLayoutService
{
    /// <summary>
    /// Read layout. Returns a single layout by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/layout/read.json">API Doc - Order/Layout/Read</a>
    /// </summary>
    /// <param name="layout">The entry containing the ID of the layout.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<OrderLayout>>> Get(Entry layout, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List layouts. Returns a list of layouts, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/layout/list.json">API Doc - Order/Layout/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<OrderLayout>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new layout. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/layout/create.json">API Doc - Order/Layout/Create</a>
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(OrderLayoutCreate layout, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update layout. Updates an existing layout. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/layout/update.json">API Doc - Order/Layout/Update</a>
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(OrderLayoutUpdate layout, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete layouts. Deletes one or multiple layouts. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/layout/delete.json">API Doc - Order/Layout/Delete</a>
    /// </summary>
    /// <param name="layouts"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries layouts, [Optional] CancellationToken cancellationToken);
}
