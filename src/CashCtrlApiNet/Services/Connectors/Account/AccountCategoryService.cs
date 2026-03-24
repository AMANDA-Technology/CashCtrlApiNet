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
using CashCtrlApiNet.Abstractions.Models.Account.Category;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Account;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.AccountEndpoints.AccountCategory;

namespace CashCtrlApiNet.Services.Connectors.Account;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Account.IAccountCategoryService" />
public class AccountCategoryService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IAccountCategoryService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<AccountCategory>>> Get(Entry category, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<AccountCategory>, Entry>(Endpoint.Read, category, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<AccountCategory>>> GetList([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<AccountCategory>>(Endpoint.List, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<AccountCategory>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<AccountCategory>, ListParams>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<AccountCategory>>> GetTree([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<AccountCategory>>(Endpoint.Tree, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(AccountCategoryCreate category, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, AccountCategoryCreate>(Endpoint.Create, category, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(AccountCategoryUpdate category, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, AccountCategoryUpdate>(Endpoint.Update, category, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries categories, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, categories, cancellationToken: cancellationToken);
}
