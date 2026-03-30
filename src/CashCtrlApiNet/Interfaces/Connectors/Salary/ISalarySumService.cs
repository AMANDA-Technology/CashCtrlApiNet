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
using CashCtrlApiNet.Abstractions.Models.Salary.Sum;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary sum service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum">API Doc - Salary/Sum</a>
/// </summary>
public interface ISalarySumService
{
    /// <summary>
    /// Read salary sum. Returns a single sum by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum/read.json">API Doc - Salary/Sum/Read</a>
    /// </summary>
    /// <param name="sum">The entry containing the ID of the sum.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalarySum>>> Get(Entry sum, CancellationToken cancellationToken = default);

    /// <summary>
    /// List salary sums. Returns a list of sums, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum/list.json">API Doc - Salary/Sum/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalarySum>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new salary sum. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum/create.json">API Doc - Salary/Sum/Create</a>
    /// </summary>
    /// <param name="sum"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalarySumCreate sum, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update salary sum. Updates an existing sum. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum/update.json">API Doc - Salary/Sum/Update</a>
    /// </summary>
    /// <param name="sum"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalarySumUpdate sum, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete salary sums. Deletes one or multiple sums. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum/delete.json">API Doc - Salary/Sum/Delete</a>
    /// </summary>
    /// <param name="sums"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries sums, CancellationToken cancellationToken = default);
}
