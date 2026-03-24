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
using CashCtrlApiNet.Abstractions.Models.Journal;

namespace CashCtrlApiNet.Interfaces.Connectors.Journal;

/// <summary>
/// CashCtrl journal service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal">API Doc - Journal/Journal</a>
/// </summary>
public interface IJournalService
{
    /// <summary>
    /// Read journal entry. Returns a single journal entry by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/read.json">API Doc - Journal/Read journal entry</a>
    /// </summary>
    /// <param name="journal">The entry containing the ID of the journal entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.Journal.Journal>>> Get(Entry journal, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List journal entries. Returns a list of journal entries, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/list.json">API Doc - Journal/List journal entries</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<JournalListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new journal entry. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/create.json">API Doc - Journal/Create journal entry</a>
    /// </summary>
    /// <param name="journal"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(JournalCreate journal, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update journal entry. Updates an existing journal entry. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/update.json">API Doc - Journal/Update journal entry</a>
    /// </summary>
    /// <param name="journal"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(JournalUpdate journal, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete journal entries. Deletes one or multiple journal entries. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/delete.json">API Doc - Journal/Delete journal entries</a>
    /// </summary>
    /// <param name="journals"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries journals, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of a journal entry. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/update_attachments.json">API Doc - Journal/Update attachments</a>
    /// </summary>
    /// <param name="journalAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments journalAttachments, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update recurrence. Updates the recurrence of a journal entry. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/update_recurrence.json">API Doc - Journal/Update recurrence</a>
    /// </summary>
    /// <param name="journalRecurrence"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateRecurrence(EntryRecurrence journalRecurrence, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export journal entries as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/list.xlsx">API Doc - Journal/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export journal entries as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/list.csv">API Doc - Journal/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export journal entries as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/list.pdf">API Doc - Journal/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
