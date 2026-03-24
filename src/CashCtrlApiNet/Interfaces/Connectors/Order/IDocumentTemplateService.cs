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
using CashCtrlApiNet.Abstractions.Models.Order.DocumentTemplate;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Order;

/// <summary>
/// CashCtrl order document template service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/template">API Doc - Order/Document template</a>
/// </summary>
public interface IDocumentTemplateService
{
    /// <summary>
    /// Read document template. Returns a single document template by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/template/read.json">API Doc - Order/Document template/Read</a>
    /// </summary>
    /// <param name="template">The entry containing the ID of the document template.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<DocumentTemplate>>> Get(Entry template, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List document templates. Returns a list of document templates.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/template/list.json">API Doc - Order/Document template/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<DocumentTemplate>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new document template. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/template/create.json">API Doc - Order/Document template/Create</a>
    /// </summary>
    /// <param name="template"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(DocumentTemplateCreate template, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update document template. Updates an existing document template. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/template/update.json">API Doc - Order/Document template/Update</a>
    /// </summary>
    /// <param name="template"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(DocumentTemplateUpdate template, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete document templates. Deletes one or multiple document templates. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/template/delete.json">API Doc - Order/Document template/Delete</a>
    /// </summary>
    /// <param name="templates"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries templates, [Optional] CancellationToken cancellationToken);
}
