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
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Meta;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.MetaEndpoints.FiscalPeriod;

namespace CashCtrlApiNet.Services.Connectors.Meta;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Meta.IFiscalPeriodService" />
public class FiscalPeriodService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IFiscalPeriodService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<FiscalPeriod>>> Get(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<FiscalPeriod>, Entry>(Endpoint.Read, fiscalPeriod, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<FiscalPeriodListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<FiscalPeriodListed>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(FiscalPeriodCreate fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, FiscalPeriodCreate>(Endpoint.Create, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(FiscalPeriodUpdate fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, FiscalPeriodUpdate>(Endpoint.Update, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Switch(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.Switch, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries fiscalPeriods, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, fiscalPeriods, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<FiscalPeriod>>> GetResult(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<FiscalPeriod>, Entry>(Endpoint.Result, fiscalPeriod, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<FiscalPeriodListed>>> GetDepreciations(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<FiscalPeriodListed>, Entry>(Endpoint.Deprecations, fiscalPeriod, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> BookDepreciations(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.BookDeprecations, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<FiscalPeriodListed>>> GetExchangeDiff(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<FiscalPeriodListed>, Entry>(Endpoint.ExchangeDifferences, fiscalPeriod, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> BookExchangeDiff(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.BookExchangeDifferences, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Complete(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.Complete, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Reopen(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.Reopen, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> CompleteMonths(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.CompleteMonths, fiscalPeriod, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> ReopenMonths(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.ReopenMonths, fiscalPeriod, cancellationToken: cancellationToken);
}
