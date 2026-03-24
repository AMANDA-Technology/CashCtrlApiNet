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
using CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory fixed asset service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset">API Doc - Inventory/Fixed asset</a>
/// </summary>
public interface IFixedAssetService
{
    /// <summary>
    /// Read fixed asset. Returns a single fixed asset by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/read.json">API Doc - Inventory/Fixed asset/Read fixed asset</a>
    /// </summary>
    /// <param name="fixedAssetId">The ID of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<FixedAsset>>> Get(Entry fixedAssetId, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List fixed assets. Returns a list of fixed assets.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/list.json">API Doc - Inventory/Fixed asset/List fixed assets</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FixedAssetListed>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List fixed assets with filter and pagination parameters. Returns a list of fixed assets.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/list.json">API Doc - Inventory/Fixed asset/List fixed assets</a>
    /// </summary>
    /// <param name="listParams">The filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FixedAssetListed>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create fixed asset. Creates a new fixed asset. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/create.json">API Doc - Inventory/Fixed asset/Create fixed asset</a>
    /// </summary>
    /// <param name="fixedAsset"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(FixedAssetCreate fixedAsset, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update fixed asset. Updates an existing fixed asset. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/update.json">API Doc - Inventory/Fixed asset/Update fixed asset</a>
    /// </summary>
    /// <param name="fixedAsset"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(FixedAssetUpdate fixedAsset, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete fixed assets. Deletes one or multiple fixed assets. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/delete.json">API Doc - Inventory/Fixed asset/Delete fixed assets</a>
    /// </summary>
    /// <param name="fixedAssets"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries fixedAssets, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize fixed assets. Assigns one or multiple fixed assets to the desired category. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/categorize.json">API Doc - Inventory/Fixed asset/Categorize fixed assets</a>
    /// </summary>
    /// <param name="fixedAssetsCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize fixedAssetsCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of a fixed asset. Use the File API to upload a file and then use the file ID here. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/update_attachments.json">API Doc - Inventory/Fixed asset/Update attachments</a>
    /// </summary>
    /// <param name="fixedAssetAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments fixedAssetAttachments, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export fixed assets as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/list.xlsx">API Doc - Inventory/Fixed asset/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export fixed assets as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/list.csv">API Doc - Inventory/Fixed asset/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export fixed assets as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/list.pdf">API Doc - Inventory/Fixed asset/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
