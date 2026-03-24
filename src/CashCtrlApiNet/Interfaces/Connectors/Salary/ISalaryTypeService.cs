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
using CashCtrlApiNet.Abstractions.Models.Salary.Type;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary type service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type">API Doc - Salary/Type</a>
/// </summary>
public interface ISalaryTypeService
{
    /// <summary>
    /// Read salary type. Returns a single salary type by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/read.json">API Doc - Salary/Type/Read</a>
    /// </summary>
    /// <param name="salaryType">The entry containing the ID of the salary type.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryType>>> Get(Entry salaryType, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary types. Returns a list of salary types.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/list.json">API Doc - Salary/Type/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryType>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new salary type. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/create.json">API Doc - Salary/Type/Create</a>
    /// </summary>
    /// <param name="salaryType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalaryTypeCreate salaryType, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary type. Updates an existing salary type. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/update.json">API Doc - Salary/Type/Update</a>
    /// </summary>
    /// <param name="salaryType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryTypeUpdate salaryType, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize salary types. Assigns one or multiple salary types to the desired category. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/categorize.json">API Doc - Salary/Type/Categorize</a>
    /// </summary>
    /// <param name="salaryTypesCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize salaryTypesCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete salary types. Deletes one or multiple salary types. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/delete.json">API Doc - Salary/Type/Delete</a>
    /// </summary>
    /// <param name="salaryTypes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries salaryTypes, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary types as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/list.xlsx">API Doc - Salary/Type/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary types as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/list.csv">API Doc - Salary/Type/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary types as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/list.pdf">API Doc - Salary/Type/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
