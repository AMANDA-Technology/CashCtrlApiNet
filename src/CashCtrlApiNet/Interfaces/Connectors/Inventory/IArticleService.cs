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
using CashCtrlApiNet.Abstractions.Models.Inventory.Article;

namespace CashCtrlApiNet.Interfaces.Connectors.Inventory;

/// <summary>
/// CashCtrl inventory article service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article">API Doc - Inventory/Article</a>
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Read article. Returns a single article by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/read.json">API Doc - Inventory/Article/Read article</a>
    /// </summary>
    /// <param name="articleId">The ID of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Article>>> Get(Entry articleId, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List articles. Returns a list of articles.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/list.json">API Doc - Inventory/Article/List articles</a>
    /// </summary>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<ArticleListed>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create article. Creates a new article. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/create.json">API Doc - Inventory/Article/Create article</a>
    /// </summary>
    /// <param name="article"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(ArticleCreate article, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update article. Updates an existing article. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/update.json">API Doc - Inventory/Article/Update article</a>
    /// </summary>
    /// <param name="article"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(ArticleUpdate article, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete articles. Deletes one or multiple articles. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/delete.json">API Doc - Inventory/Article/Delete articles</a>
    /// </summary>
    /// <param name="articles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries articles, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize articles. Assigns one or multiple articles to the desired category. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/categorize.json">API Doc - Inventory/Article/Categorize articles</a>
    /// </summary>
    /// <param name="articlesCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize articlesCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of an article. Use the File API to upload a file and then use the file ID here. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/update_attachments.json">API Doc - Inventory/Article/Update attachments</a>
    /// </summary>
    /// <param name="articleAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments articleAttachments, [Optional] CancellationToken cancellationToken);
}
