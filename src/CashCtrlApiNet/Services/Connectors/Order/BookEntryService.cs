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
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Order;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.OrderEndpoints.BookEntry;

namespace CashCtrlApiNet.Services.Connectors.Order;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Order.IBookEntryService" />
public class BookEntryService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IBookEntryService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<BookEntry>>> Get(Entry bookEntry, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<BookEntry>, Entry>(Endpoint.Read, bookEntry, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<BookEntry>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<BookEntry>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(BookEntryCreate bookEntry, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, BookEntryCreate>(Endpoint.Create, bookEntry, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(BookEntryUpdate bookEntry, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, BookEntryUpdate>(Endpoint.Update, bookEntry, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries bookEntries, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, bookEntries, cancellationToken: cancellationToken);
}
