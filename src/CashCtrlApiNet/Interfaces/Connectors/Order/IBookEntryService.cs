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

using CashCtrlApiNet.Abstractions.Models.Order.BookEntry;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Order;

/// <summary>
/// CashCtrl order book entry service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry">API Doc - Order/Book entry</a>
/// </summary>
public interface IBookEntryService
{
    /// <summary>
    /// Read book entry. Returns a single book entry by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry/read.json">API Doc - Order/Book entry/Read</a>
    /// </summary>
    /// <param name="bookEntry">The entry containing the ID of the book entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<BookEntry>>> Get(Entry bookEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// List book entries for a given order. The <c>id</c> query parameter (the order's ID) is
    /// mandatory on the underlying API.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry/list.json">API Doc - Order/Book entry/List</a>
    /// </summary>
    /// <param name="request">The list request including the mandatory order ID.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<BookEntry>>> GetList(BookEntryListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new book entry. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry/create.json">API Doc - Order/Book entry/Create</a>
    /// </summary>
    /// <param name="bookEntry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(BookEntryCreate bookEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update book entry. Updates an existing book entry. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry/update.json">API Doc - Order/Book entry/Update</a>
    /// </summary>
    /// <param name="bookEntry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(BookEntryUpdate bookEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete book entries. Deletes one or multiple book entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry/delete.json">API Doc - Order/Book entry/Delete</a>
    /// </summary>
    /// <param name="bookEntries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries bookEntries, CancellationToken cancellationToken = default);
}
