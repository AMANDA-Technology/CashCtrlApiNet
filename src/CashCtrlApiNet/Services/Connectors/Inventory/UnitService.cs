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
using CashCtrlApiNet.Abstractions.Models.Inventory.Unit;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Inventory;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.InventoryEndpoints.Unit;

namespace CashCtrlApiNet.Services.Connectors.Inventory;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IUnitService" />
public class UnitService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IUnitService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<Unit>>> Get(Entry unitId, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<Unit>, Entry>(Endpoint.Read, unitId, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<Unit>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<Unit>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(UnitCreate unit, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, UnitCreate>(Endpoint.Create, unit, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(UnitUpdate unit, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, UnitUpdate>(Endpoint.Update, unit, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries units, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, units, cancellationToken: cancellationToken);
}
