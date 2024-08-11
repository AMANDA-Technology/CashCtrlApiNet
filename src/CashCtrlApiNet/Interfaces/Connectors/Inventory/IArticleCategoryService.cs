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
using CashCtrlApiNet.Abstractions.Models.Inventory.ArticleCategory;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory article category service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category">API Doc - Inventory/Article category</a>
/// </summary>
public interface IArticleCategoryService
{
    /// <summary>
    /// Read category. Returns a single category by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category/read.json">API Doc - Inventory/Article category/Read category</a>
    /// </summary>
    /// <param name="articleCategoryId">The ID of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<ArticleCategory>>> Get(Entry articleCategoryId, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List categories. Returns a list of all categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category/list.json">API Doc - Inventory/Article category/List articles</a>
    /// </summary>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<ArticleCategory>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create category. Creates a new category. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category/create.json">API Doc - Inventory/Article category/Create category</a>
    /// </summary>
    /// <param name="articleCategory"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(ArticleCategoryCreate articleCategory, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update category. Updates an existing category. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category/update.json">API Doc - Inventory/Article category/Update category</a>
    /// </summary>
    /// <param name="articleCategory"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(ArticleCategoryUpdate articleCategory, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete categories. Deletes one or multiple existing categories. Note that you can only delete empty categories. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category/delete.json">API Doc - Inventory/Article category/Delete categories</a>
    /// </summary>
    /// <param name="articleCategories"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries articleCategories, [Optional] CancellationToken cancellationToken);
}
