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
using CashCtrlApiNet.Abstractions.Models.Salary.Statement;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary statement service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement">API Doc - Salary/Statement</a>
/// </summary>
public interface ISalaryStatementService
{
    /// <summary>
    /// Read salary statement. Returns a single statement by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/read.json">API Doc - Salary/Statement/Read</a>
    /// </summary>
    /// <param name="statement">The entry containing the ID of the statement.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryStatement>>> Get(Entry statement, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary statements. Returns a list of statements, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/list.json">API Doc - Salary/Statement/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryStatement>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new salary statement. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/create.json">API Doc - Salary/Statement/Create</a>
    /// </summary>
    /// <param name="statement"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalaryStatementCreate statement, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary statement. Updates an existing statement. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/update.json">API Doc - Salary/Statement/Update</a>
    /// </summary>
    /// <param name="statement"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryStatementUpdate statement, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update multiple salary statements. Updates multiple existing statements at once.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/update_multiple.json">API Doc - Salary/Statement/Update multiple</a>
    /// </summary>
    /// <param name="statements"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateMultiple(SalaryStatementUpdateMultiple statements, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary statement status. Updates the status of one or multiple statements.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/update_status.json">API Doc - Salary/Statement/Update status</a>
    /// </summary>
    /// <param name="status"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateStatus(SalaryStatementStatusUpdate status, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary statement recurrence. Updates the recurrence of a statement.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/update_recurrence.json">API Doc - Salary/Statement/Update recurrence</a>
    /// </summary>
    /// <param name="recurrence"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateRecurrence(SalaryStatementRecurrenceUpdate recurrence, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete salary statements. Deletes one or multiple statements. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/delete.json">API Doc - Salary/Statement/Delete</a>
    /// </summary>
    /// <param name="statements"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries statements, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Calculate salary statement. Calculates the values of a statement.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/calculate.json">API Doc - Salary/Statement/Calculate</a>
    /// </summary>
    /// <param name="calculation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Calculate(SalaryStatementCalculate calculation, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of a salary statement.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/update_attachments.json">API Doc - Salary/Statement/Update attachments</a>
    /// </summary>
    /// <param name="attachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments attachments, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary statements as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/list.xlsx">API Doc - Salary/Statement/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary statements as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/list.csv">API Doc - Salary/Statement/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary statements as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/list.pdf">API Doc - Salary/Statement/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
