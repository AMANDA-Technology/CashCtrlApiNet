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
using CashCtrlApiNet.Abstractions.Models.Inventory;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory article service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article">API Doc - Inventory/Article</see>
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Read article. Returns a single article by ID.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/read.json">API Doc - Inventory/Article/Read article</see>
    /// </summary>
    /// <param name="articleId">The ID of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<Article>> Get(int articleId, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List articles. Returns a list of articles.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/list.json">API Doc - Inventory/Article/List articles</see>
    /// </summary>
    /// <returns></returns>
    public Task<ApiResult<ApiListResult<Article>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new article. Returns either a success or multiple error messages (for each issue).
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/create.json">API Doc - Inventory/Article/Create article</see>
    /// </summary>
    /// <param name="article"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ApiResponse>> Create(ArticleCreate article, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update article. Updates an existing article. Returns either a success or multiple error messages (for each issue).
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/update.json">API Doc - Inventory/Article/Update article</see>
    /// </summary>
    /// <param name="article"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ApiResponse>> Update(ArticleUpdate article, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete articles. Deletes one or multiple articles. Returns either a success or error message.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/delete.json">API Doc - Inventory/Article/Delete articles</see>
    /// </summary>
    /// <param name="articles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ApiResponse>> Delete(Articles articles, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize articles. Assigns one or multiple articles to the desired category. Returns either a success or error message.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/categorize.json">API Doc - Inventory/Article/Categorize articles</see>
    /// </summary>
    /// <param name="articlesCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ApiResponse>> Categorize(ArticlesCategorize articlesCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of an article. Use the File API to upload a file and then use the file ID here. Returns either a success or error message.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/update_attachments.json">API Doc - Inventory/Article/Update attachments</see>
    /// </summary>
    /// <param name="articleAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ApiResponse>> UpdateAttachments(ArticleAttachments articleAttachments, [Optional] CancellationToken cancellationToken);
}
