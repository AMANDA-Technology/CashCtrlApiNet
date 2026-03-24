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
using CashCtrlApiNet.Abstractions.Models.Common.Currency;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common currency service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common/Currency</a>
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// Read currency. Returns a single currency by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency/read.json">API Doc - Common/Read currency</a>
    /// </summary>
    /// <param name="currency">The entry containing the ID of the currency.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Currency>>> Get(Entry currency, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List currencies. Returns a list of currencies, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency/list.json">API Doc - Common/List currencies</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CurrencyListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create currency. Creates a new currency. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency/create.json">API Doc - Common/Create currency</a>
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(CurrencyCreate currency, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update currency. Updates an existing currency. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency/update.json">API Doc - Common/Update currency</a>
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(CurrencyUpdate currency, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete currencies. Deletes one or multiple currencies. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency/delete.json">API Doc - Common/Delete currencies</a>
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries currencies, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get exchange rate. Returns the exchange rate between two currencies.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency/exchangerate">API Doc - Common/Get exchange rate</a>
    /// </summary>
    /// <param name="exchangeRateRequest">The exchange rate request parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<CurrencyExchangeRate>>> GetExchangeRate(CurrencyExchangeRateRequest exchangeRateRequest, [Optional] CancellationToken cancellationToken);
}
