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
using CashCtrlApiNet.Abstractions.Models.Common.TaxRate;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common tax rate service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax">API Doc - Common/Tax rate</a>
/// </summary>
public interface ITaxRateService
{
    /// <summary>
    /// Read tax rate. Returns a single tax rate by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax/read.json">API Doc - Common/Read tax rate</a>
    /// </summary>
    /// <param name="taxRate">The entry containing the ID of the tax rate.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<TaxRate>>> Get(Entry taxRate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List tax rates. Returns a list of tax rates.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax/list.json">API Doc - Common/List tax rates</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<TaxRateListed>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create tax rate. Creates a new tax rate. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax/create.json">API Doc - Common/Create tax rate</a>
    /// </summary>
    /// <param name="taxRate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(TaxRateCreate taxRate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update tax rate. Updates an existing tax rate. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax/update.json">API Doc - Common/Update tax rate</a>
    /// </summary>
    /// <param name="taxRate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(TaxRateUpdate taxRate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete tax rates. Deletes one or multiple tax rates. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax/delete.json">API Doc - Common/Delete tax rates</a>
    /// </summary>
    /// <param name="taxRates"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries taxRates, [Optional] CancellationToken cancellationToken);
}
