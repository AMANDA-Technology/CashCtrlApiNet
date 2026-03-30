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
using CashCtrlApiNet.Abstractions.Models.Salary.Field;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary field service endpoint (read-only). <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/field">API Doc - Salary/Field</a>
/// </summary>
public interface ISalaryFieldService
{
    /// <summary>
    /// Read salary field. Returns a single field by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/field/read.json">API Doc - Salary/Field/Read</a>
    /// </summary>
    /// <param name="field">The entry containing the ID of the field.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryField>>> Get(Entry field, CancellationToken cancellationToken = default);

    /// <summary>
    /// List salary fields. Returns a list of fields for the specified type, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/field/list.json">API Doc - Salary/Field/List</a>
    /// </summary>
    /// <param name="listRequest">The list request containing the type ID and optional filter/pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryField>>> GetList(SalaryFieldListRequest listRequest, CancellationToken cancellationToken = default);
}
