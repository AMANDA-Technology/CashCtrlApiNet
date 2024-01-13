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
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Inventory;
using CashCtrlApiNet.Services.Connectors.Base;
using CashCtrlApiNet.Services.Endpoints;

namespace CashCtrlApiNet.Services.Connectors.Inventory;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IArticleService" />
public class ArticleService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IArticleService
{
    /// <inheritdoc />
    public Task<ApiResult<Article>> Get(int articleId, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<Article>(InventoryEndpoints.Article.Read, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ApiListResult<Article>>> GetList([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ApiListResult<Article>>(InventoryEndpoints.Article.List, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ApiResponse>> Create(ArticleCreate article, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<ApiResponse, ArticleCreate>(InventoryEndpoints.Article.Create, article, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ApiResponse>> Update(ArticleUpdate article, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<ApiResponse, ArticleUpdate>(InventoryEndpoints.Article.Update, article, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ApiResponse>> Delete(Articles articles, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<ApiResponse, Articles>(InventoryEndpoints.Article.Delete, articles, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ApiResponse>> Categorize(ArticlesCategorize articlesCategorize, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<ApiResponse, ArticlesCategorize>(InventoryEndpoints.Article.Categorize, articlesCategorize, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ApiResponse>> UpdateAttachments(ArticleAttachments articleAttachments, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<ApiResponse, ArticleAttachments>(InventoryEndpoints.Article.UpdateAttachments, articleAttachments, cancellationToken);
}
