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
using CashCtrlApiNet.Abstractions.Models.Common.TextTemplate;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common text template service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text">API Doc - Common/Text template</a>
/// </summary>
public interface ITextTemplateService
{
    /// <summary>
    /// Read text template. Returns a single text template by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text/read.json">API Doc - Common/Read text template</a>
    /// </summary>
    /// <param name="textTemplate">The entry containing the ID of the text template.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<TextTemplate>>> Get(Entry textTemplate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List text templates. Returns a list of text templates.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text/list.json">API Doc - Common/List text templates</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<TextTemplateListed>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List text templates with filter and pagination parameters. Returns a list of text templates.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text/list.json">API Doc - Common/List text templates</a>
    /// </summary>
    /// <param name="listParams">The filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<TextTemplateListed>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create text template. Creates a new text template. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text/create.json">API Doc - Common/Create text template</a>
    /// </summary>
    /// <param name="textTemplate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(TextTemplateCreate textTemplate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update text template. Updates an existing text template. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text/update.json">API Doc - Common/Update text template</a>
    /// </summary>
    /// <param name="textTemplate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(TextTemplateUpdate textTemplate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete text templates. Deletes one or multiple text templates. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text/delete.json">API Doc - Common/Delete text templates</a>
    /// </summary>
    /// <param name="textTemplates"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries textTemplates, [Optional] CancellationToken cancellationToken);
}
