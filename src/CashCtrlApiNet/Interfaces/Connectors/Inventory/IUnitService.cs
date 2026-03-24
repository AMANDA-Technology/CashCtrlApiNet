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
using CashCtrlApiNet.Abstractions.Models.Inventory.Unit;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory unit service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit">API Doc - Inventory/Unit</a>
/// </summary>
public interface IUnitService
{
    /// <summary>
    /// Read unit. Returns a single unit by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit/read.json">API Doc - Inventory/Unit/Read unit</a>
    /// </summary>
    /// <param name="unitId">The ID of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Unit>>> Get(Entry unitId, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List units. Returns a list of all units.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit/list.json">API Doc - Inventory/Unit/List units</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<Unit>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List units with filter and pagination parameters. Returns a list of all units.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit/list.json">API Doc - Inventory/Unit/List units</a>
    /// </summary>
    /// <param name="listParams">The filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<Unit>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create unit. Creates a new unit. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit/create.json">API Doc - Inventory/Unit/Create unit</a>
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(UnitCreate unit, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update unit. Updates an existing unit. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit/update.json">API Doc - Inventory/Unit/Update unit</a>
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(UnitUpdate unit, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete units. Deletes one or multiple units. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit/delete.json">API Doc - Inventory/Unit/Delete units</a>
    /// </summary>
    /// <param name="units"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries units, [Optional] CancellationToken cancellationToken);
}
