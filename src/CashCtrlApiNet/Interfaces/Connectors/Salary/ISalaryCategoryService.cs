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
using CashCtrlApiNet.Abstractions.Models.Salary.Category;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary category service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category">API Doc - Salary/Category</a>
/// </summary>
public interface ISalaryCategoryService
{
    /// <summary>
    /// Read salary category. Returns a single category by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category/read.json">API Doc - Salary/Category/Read</a>
    /// </summary>
    /// <param name="category">The entry containing the ID of the category.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryCategory>>> Get(Entry category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary categories. Returns a list of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category/list.json">API Doc - Salary/Category/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryCategory>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get salary category tree. Returns a tree of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category/tree.json">API Doc - Salary/Category/Tree</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryCategory>>> GetTree([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new salary category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category/create.json">API Doc - Salary/Category/Create</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalaryCategoryCreate category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary category. Updates an existing category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category/update.json">API Doc - Salary/Category/Update</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryCategoryUpdate category, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete salary categories. Deletes one or multiple categories. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category/delete.json">API Doc - Salary/Category/Delete</a>
    /// </summary>
    /// <param name="categories"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries categories, [Optional] CancellationToken cancellationToken);
}
