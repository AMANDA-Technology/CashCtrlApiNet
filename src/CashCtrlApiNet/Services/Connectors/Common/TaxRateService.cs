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
using CashCtrlApiNet.Abstractions.Models.Common.TaxRate;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Common;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.CommonEndpoints.TaxRate;

namespace CashCtrlApiNet.Services.Connectors.Common;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Common.ITaxRateService" />
public class TaxRateService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ITaxRateService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<TaxRate>>> Get(Entry taxRate, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<TaxRate>, Entry>(Endpoint.Read, taxRate, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<TaxRateListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<TaxRateListed>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(TaxRateCreate taxRate, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, TaxRateCreate>(Endpoint.Create, taxRate, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(TaxRateUpdate taxRate, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, TaxRateUpdate>(Endpoint.Update, taxRate, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries taxRates, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, taxRates, cancellationToken: cancellationToken);
}
