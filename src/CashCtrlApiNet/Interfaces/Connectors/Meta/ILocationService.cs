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
using CashCtrlApiNet.Abstractions.Models.Meta.Location;

namespace CashCtrlApiNet.Interfaces.Connectors.Meta;

/// <summary>
/// CashCtrl meta location service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/location">API Doc - Meta/Location</a>
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Read location. Returns a single location by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/location/read.json">API Doc - Meta/Read location</a>
    /// </summary>
    /// <param name="location">The entry containing the ID of the location.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Location>>> Get(Entry location, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List locations. Returns a list of locations, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/location/list.json">API Doc - Meta/List locations</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<LocationListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create location. Creates a new location. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/location/create.json">API Doc - Meta/Create location</a>
    /// </summary>
    /// <param name="location"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(LocationCreate location, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update location. Updates an existing location. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/location/update.json">API Doc - Meta/Update location</a>
    /// </summary>
    /// <param name="location"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(LocationUpdate location, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete locations. Deletes one or multiple locations. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/location/delete.json">API Doc - Meta/Delete locations</a>
    /// </summary>
    /// <param name="locations"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries locations, [Optional] CancellationToken cancellationToken);
}
