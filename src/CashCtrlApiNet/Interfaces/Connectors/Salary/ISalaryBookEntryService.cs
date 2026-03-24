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
using CashCtrlApiNet.Abstractions.Models.Salary.BookEntry;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary book entry service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry">API Doc - Salary/Book entry</a>
/// </summary>
public interface ISalaryBookEntryService
{
    /// <summary>
    /// Read salary book entry. Returns a single book entry by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry/read.json">API Doc - Salary/Book entry/Read</a>
    /// </summary>
    /// <param name="bookEntry">The entry containing the ID of the book entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryBookEntry>>> Get(Entry bookEntry, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary book entries. Returns a list of book entries for the specified statement.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry/list.json">API Doc - Salary/Book entry/List</a>
    /// </summary>
    /// <param name="statement">The entry containing the ID of the statement to list book entries for.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryBookEntry>>> GetList(Entry statement, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new salary book entry. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry/create.json">API Doc - Salary/Book entry/Create</a>
    /// </summary>
    /// <param name="bookEntry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalaryBookEntryCreate bookEntry, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary book entry. Updates an existing book entry. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry/update.json">API Doc - Salary/Book entry/Update</a>
    /// </summary>
    /// <param name="bookEntry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryBookEntryUpdate bookEntry, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete salary book entries. Deletes one or multiple book entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry/delete.json">API Doc - Salary/Book entry/Delete</a>
    /// </summary>
    /// <param name="bookEntries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries bookEntries, [Optional] CancellationToken cancellationToken);
}
