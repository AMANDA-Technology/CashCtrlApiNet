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
using CashCtrlApiNet.Abstractions.Models.Account.CostCenterCategory;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Account;

/// <summary>
/// CashCtrl account cost center category service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category">API Doc - Account/Cost center category</a>
/// </summary>
public interface ICostCenterCategoryService
{
    /// <summary>
    /// Read cost center category. Returns a single category by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/read.json">API Doc - Account/Cost center category/Read</a>
    /// </summary>
    /// <param name="category">The entry containing the ID of the category.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<CostCenterCategory>>> Get(Entry category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List cost center categories. Returns a list of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/list.json">API Doc - Account/Cost center category/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CostCenterCategory>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List cost center categories with filter and pagination parameters. Returns a list of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/list.json">API Doc - Account/Cost center category/List</a>
    /// </summary>
    /// <param name="listParams">The filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CostCenterCategory>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get cost center category tree. Returns a tree of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/tree.json">API Doc - Account/Cost center category/Tree</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CostCenterCategory>>> GetTree([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new cost center category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/create.json">API Doc - Account/Cost center category/Create</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(CostCenterCategoryCreate category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update cost center category. Updates an existing category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/update.json">API Doc - Account/Cost center category/Update</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(CostCenterCategoryUpdate category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete cost center categories. Deletes one or multiple categories. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/category/delete.json">API Doc - Account/Cost center category/Delete</a>
    /// </summary>
    /// <param name="categories"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries categories, [Optional] CancellationToken cancellationToken);
}
