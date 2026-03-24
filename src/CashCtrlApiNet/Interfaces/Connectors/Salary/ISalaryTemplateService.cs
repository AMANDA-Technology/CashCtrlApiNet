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
using CashCtrlApiNet.Abstractions.Models.Salary.Template;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary template service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template">API Doc - Salary/Template</a>
/// </summary>
public interface ISalaryTemplateService
{
    /// <summary>
    /// Read salary template. Returns a single template by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/read.json">API Doc - Salary/Template/Read</a>
    /// </summary>
    /// <param name="template">The entry containing the ID of the template.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryTemplate>>> Get(Entry template, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary templates. Returns a list of templates, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/list.json">API Doc - Salary/Template/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryTemplate>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get salary template tree. Returns a tree of templates.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/tree.json">API Doc - Salary/Template/Tree</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryTemplate>>> GetTree([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new salary template. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/create.json">API Doc - Salary/Template/Create</a>
    /// </summary>
    /// <param name="template"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalaryTemplateCreate template, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary template. Updates an existing template. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/update.json">API Doc - Salary/Template/Update</a>
    /// </summary>
    /// <param name="template"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryTemplateUpdate template, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete salary templates. Deletes one or multiple templates. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/delete.json">API Doc - Salary/Template/Delete</a>
    /// </summary>
    /// <param name="templates"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries templates, [Optional] CancellationToken cancellationToken);
}
