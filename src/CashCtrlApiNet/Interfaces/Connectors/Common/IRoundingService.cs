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
using CashCtrlApiNet.Abstractions.Models.Common.Rounding;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common rounding service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding">API Doc - Common/Rounding</a>
/// </summary>
public interface IRoundingService
{
    /// <summary>
    /// Read rounding. Returns a single rounding by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding/read.json">API Doc - Common/Read rounding</a>
    /// </summary>
    /// <param name="rounding">The entry containing the ID of the rounding.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Rounding>>> Get(Entry rounding, CancellationToken cancellationToken = default);

    /// <summary>
    /// List roundings. Returns a list of roundings, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding/list.json">API Doc - Common/List roundings</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<RoundingListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create rounding. Creates a new rounding. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding/create.json">API Doc - Common/Create rounding</a>
    /// </summary>
    /// <param name="rounding"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(RoundingCreate rounding, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update rounding. Updates an existing rounding. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding/update.json">API Doc - Common/Update rounding</a>
    /// </summary>
    /// <param name="rounding"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(RoundingUpdate rounding, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete roundings. Deletes one or multiple roundings. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding/delete.json">API Doc - Common/Delete roundings</a>
    /// </summary>
    /// <param name="roundings"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries roundings, CancellationToken cancellationToken = default);
}
