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
using CashCtrlApiNet.Abstractions.Models.File.Category;

namespace CashCtrlApiNet.Interfaces.Connectors.File;

/// <summary>
/// CashCtrl file category endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category">API Doc - File/File category</a>
/// </summary>
public interface IFileCategoryService
{
    /// <summary>
    /// Read file category. Returns a single category by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category/read.json">API Doc - File/Category/Read</a>
    /// </summary>
    /// <param name="category">The entry containing the ID of the category.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<FileCategory>>> Get(Entry category, CancellationToken cancellationToken = default);

    /// <summary>
    /// List file categories. Returns a list of categories, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category/list.json">API Doc - File/Category/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FileCategory>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get file category tree. Returns a tree of categories.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category/tree.json">API Doc - File/Category/Tree</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<FileCategory>>> GetTree(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new file category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category/create.json">API Doc - File/Category/Create</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(FileCategoryCreate category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update file category. Updates an existing category. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category/update.json">API Doc - File/Category/Update</a>
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(FileCategoryUpdate category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete file categories. Deletes one or multiple categories. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/category/delete.json">API Doc - File/Category/Delete</a>
    /// </summary>
    /// <param name="categories"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries categories, CancellationToken cancellationToken = default);
}
