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

namespace CashCtrlApiNet.Interfaces.Connectors.Meta;

/// <summary>
/// CashCtrl meta fiscal period service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod">API Doc - Meta/Fiscal period</a>
/// </summary>
public interface IFiscalPeriodService
{
    /// <summary>
    /// Read fiscal period. Returns a single fiscal period by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/read.json">API Doc - Meta/Read fiscal period</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.Meta.FiscalPeriod.FiscalPeriod>>> Get(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List fiscal periods. Returns a list of fiscal periods, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/list.json">API Doc - Meta/List fiscal periods</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FiscalPeriodListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create fiscal period. Creates a new fiscal period. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/create.json">API Doc - Meta/Create fiscal period</a>
    /// </summary>
    /// <param name="fiscalPeriod"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(FiscalPeriodCreate fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update fiscal period. Updates an existing fiscal period. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/update.json">API Doc - Meta/Update fiscal period</a>
    /// </summary>
    /// <param name="fiscalPeriod"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(FiscalPeriodUpdate fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Switch fiscal period. Switches to the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/switch.json">API Doc - Meta/Switch fiscal period</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period to switch to.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Switch(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete fiscal periods. Deletes one or multiple fiscal periods. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/delete.json">API Doc - Meta/Delete fiscal periods</a>
    /// </summary>
    /// <param name="fiscalPeriods"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries fiscalPeriods, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get result of a fiscal period. Returns the result data for the specified fiscal period.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/result">API Doc - Meta/Get result</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.Meta.FiscalPeriod.FiscalPeriod>>> GetResult(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List depreciations of a fiscal period. Returns the depreciations for the specified fiscal period.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/depreciations.json">API Doc - Meta/List depreciations</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FiscalPeriodListed>>> GetDepreciations(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Book depreciations of a fiscal period. Books the depreciations for the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/bookdepreciations.json">API Doc - Meta/Book depreciations</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> BookDepreciations(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List exchange differences of a fiscal period. Returns the exchange differences for the specified fiscal period.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/exchangediff.json">API Doc - Meta/List exchange differences</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FiscalPeriodListed>>> GetExchangeDiff(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Book exchange differences of a fiscal period. Books the exchange differences for the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/bookexchangediff.json">API Doc - Meta/Book exchange differences</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> BookExchangeDiff(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Complete fiscal period. Completes the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/complete.json">API Doc - Meta/Complete fiscal period</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Complete(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Reopen fiscal period. Reopens the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/reopen.json">API Doc - Meta/Reopen fiscal period</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Reopen(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Complete months. Completes the months of the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/complete_months.json">API Doc - Meta/Complete months</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> CompleteMonths(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Reopen months. Reopens the months of the specified fiscal period. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/reopen_months.json">API Doc - Meta/Reopen months</a>
    /// </summary>
    /// <param name="fiscalPeriod">The entry containing the ID of the fiscal period.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> ReopenMonths(Entry fiscalPeriod, [Optional] CancellationToken cancellationToken);
}
