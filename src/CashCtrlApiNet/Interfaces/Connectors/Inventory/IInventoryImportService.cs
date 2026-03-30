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
using CashCtrlApiNet.Abstractions.Models.Inventory.Import;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory import service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import">API Doc - Inventory/Import</a>
/// </summary>
public interface IInventoryImportService
{
    /// <summary>
    /// Create a new inventory import. Requires a file ID of an uploaded file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import/create.json">API Doc - Inventory/Import/Create</a>
    /// </summary>
    /// <param name="importCreate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(InventoryImportCreate importCreate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Define the mapping for an inventory import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import/mapping.json">API Doc - Inventory/Import/Mapping</a>
    /// </summary>
    /// <param name="importMapping"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Mapping(InventoryImportMapping importMapping, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get available mapping fields for an inventory import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import/mapping_combo.json">API Doc - Inventory/Import/Mapping fields</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetMappingFields(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a preview of an inventory import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import/preview.json">API Doc - Inventory/Import/Preview</a>
    /// </summary>
    /// <param name="importPreview"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Preview(InventoryImportPreview importPreview, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute an inventory import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import/execute.json">API Doc - Inventory/Import/Execute</a>
    /// </summary>
    /// <param name="importExecute"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Execute(InventoryImportExecute importExecute, CancellationToken cancellationToken = default);
}
