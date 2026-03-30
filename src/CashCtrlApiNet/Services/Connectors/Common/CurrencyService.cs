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

using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Common.Currency;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Common;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.CommonEndpoints.Currency;

namespace CashCtrlApiNet.Services.Connectors.Common;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Common.ICurrencyService" />
public class CurrencyService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ICurrencyService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<Currency>>> Get(Entry currency, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<Currency>, Entry>(Endpoint.Read, currency, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<CurrencyListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<CurrencyListed>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(CurrencyCreate currency, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, CurrencyCreate>(Endpoint.Create, currency, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(CurrencyUpdate currency, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, CurrencyUpdate>(Endpoint.Update, currency, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries currencies, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, currencies, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<DecimalResponse>> GetExchangeRate(CurrencyExchangeRateRequest exchangeRateRequest, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBalanceAsync(Endpoint.ExchangeRate, exchangeRateRequest, cancellationToken);
}
