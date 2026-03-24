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
using CashCtrlApiNet.Abstractions.Models.Journal.Import.Entry;

namespace CashCtrlApiNet.Interfaces.Connectors.Journal;

/// <summary>
/// CashCtrl journal import entry service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry">API Doc - Journal/Import entry</a>
/// </summary>
public interface IJournalImportEntryService
{
    /// <summary>
    /// Read journal import entry. Returns a single journal import entry by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/read.json">API Doc - Journal/Import entry/Read entry</a>
    /// </summary>
    /// <param name="journalImportEntry">The entry containing the ID of the journal import entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<JournalImportEntry>>> Get(Entry journalImportEntry, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List journal import entries. Returns a list of journal import entries.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/list.json">API Doc - Journal/Import entry/List entries</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<JournalImportEntryListed>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List journal import entries with filter and pagination parameters. Returns a list of journal import entries.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/list.json">API Doc - Journal/Import entry/List entries</a>
    /// </summary>
    /// <param name="listParams">The filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<JournalImportEntryListed>>> GetList(ListParams listParams, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update journal import entry. Updates an existing journal import entry. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/update.json">API Doc - Journal/Import entry/Update entry</a>
    /// </summary>
    /// <param name="journalImportEntry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(JournalImportEntryUpdate journalImportEntry, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Ignore journal import entries. Ignores one or multiple journal import entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/delete.json">API Doc - Journal/Import entry/Ignore entries</a>
    /// </summary>
    /// <param name="journalImportEntries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Ignore(Entries journalImportEntries, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Restore journal import entries. Restores one or multiple ignored journal import entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/restore.json">API Doc - Journal/Import entry/Restore entries</a>
    /// </summary>
    /// <param name="journalImportEntries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Restore(Entries journalImportEntries, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Confirm journal import entries. Confirms one or multiple journal import entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/confirm.json">API Doc - Journal/Import entry/Confirm entries</a>
    /// </summary>
    /// <param name="journalImportEntries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Confirm(Entries journalImportEntries, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Unconfirm journal import entries. Unconfirms one or multiple journal import entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/unconfirm.json">API Doc - Journal/Import entry/Unconfirm entries</a>
    /// </summary>
    /// <param name="journalImportEntries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Unconfirm(Entries journalImportEntries, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export journal import entries as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/list.xlsx">API Doc - Journal/Import entry/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export journal import entries as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/list.csv">API Doc - Journal/Import entry/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export journal import entries as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/list.pdf">API Doc - Journal/Import entry/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
