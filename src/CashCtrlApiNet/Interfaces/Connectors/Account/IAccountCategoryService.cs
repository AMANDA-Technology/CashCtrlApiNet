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
using CashCtrlApiNet.Abstractions.Models.Account.Category;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Account;

/// <summary>
/// CashCtrl account category service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category">API Doc - Account/Category</a>
/// </summary>
public interface IAccountCategoryService
{
    /// <summary>
    /// Read account category. Returns a single category by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/read.json">API Doc - Account/Category/Read</a>
    /// </summary>
    /// <param name="category">The entry containing the ID of the category.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<AccountCategory>>> Get(Entry category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List account categories. Returns a list of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/list.json">API Doc - Account/Category/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<AccountCategory>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List account categories with filter and pagination parameters. Returns a list of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/list.json">API Doc - Account/Category/List</a>
    /// </summary>
    /// <param name="listParams">The filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<AccountCategory>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get account category tree. Returns a tree of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/tree.json">API Doc - Account/Category/Tree</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<AccountCategory>>> GetTree([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new account category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/create.json">API Doc - Account/Category/Create</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(AccountCategoryCreate category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update account category. Updates an existing category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/update.json">API Doc - Account/Category/Update</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(AccountCategoryUpdate category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete account categories. Deletes one or multiple categories. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/category/delete.json">API Doc - Account/Category/Delete</a>
    /// </summary>
    /// <param name="categories"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries categories, [Optional] CancellationToken cancellationToken);
}
