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
using CashCtrlApiNet.Abstractions.Models.Inventory.FixedAssetCategory;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory fixed asset category service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category">API Doc - Inventory/Fixed asset category</a>
/// </summary>
public interface IFixedAssetCategoryService
{
    /// <summary>
    /// Read fixed asset category. Returns a single category by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category/read.json">API Doc - Inventory/Fixed asset category/Read</a>
    /// </summary>
    /// <param name="category">The entry containing the ID of the category.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<FixedAssetCategory>>> Get(Entry category, CancellationToken cancellationToken = default);

    /// <summary>
    /// List fixed asset categories. Returns a list of categories, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category/list.json">API Doc - Inventory/Fixed asset category/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FixedAssetCategory>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get fixed asset category tree. Returns a tree of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category/tree.json">API Doc - Inventory/Fixed asset category/Tree</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FixedAssetCategory>>> GetTree(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new fixed asset category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category/create.json">API Doc - Inventory/Fixed asset category/Create</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(FixedAssetCategoryCreate category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update fixed asset category. Updates an existing category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category/update.json">API Doc - Inventory/Fixed asset category/Update</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(FixedAssetCategoryUpdate category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete fixed asset categories. Deletes one or multiple categories. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category/delete.json">API Doc - Inventory/Fixed asset category/Delete</a>
    /// </summary>
    /// <param name="categories"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries categories, CancellationToken cancellationToken = default);
}
