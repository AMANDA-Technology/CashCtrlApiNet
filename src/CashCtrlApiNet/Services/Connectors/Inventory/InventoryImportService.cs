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
using CashCtrlApiNet.Abstractions.Models.Inventory.Import;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Inventory;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.InventoryEndpoints.Import;

namespace CashCtrlApiNet.Services.Connectors.Inventory;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IInventoryImportService" />
public class InventoryImportService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IInventoryImportService
{
    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(InventoryImportCreate importCreate, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, InventoryImportCreate>(Endpoint.Create, importCreate, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Mapping(InventoryImportMapping importMapping, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, InventoryImportMapping>(Endpoint.Mapping, importMapping, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult> GetMappingFields([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync(Endpoint.AvailableMappingFields, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Preview(InventoryImportPreview importPreview, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, InventoryImportPreview>(Endpoint.Preview, importPreview, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Execute(InventoryImportExecute importExecute, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, InventoryImportExecute>(Endpoint.Execute, importExecute, cancellationToken: cancellationToken);
}
