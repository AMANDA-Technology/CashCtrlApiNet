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
using CashCtrlApiNet.Abstractions.Models.Journal.Import;

namespace CashCtrlApiNet.Interfaces.Connectors.Journal;

/// <summary>
/// CashCtrl journal import service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import">API Doc - Journal/Import</a>
/// </summary>
public interface IJournalImportService
{
    /// <summary>
    /// Read journal import. Returns a single journal import by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/read.json">API Doc - Journal/Import/Read import</a>
    /// </summary>
    /// <param name="journalImport">The entry containing the ID of the journal import.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<JournalImport>>> Get(Entry journalImport, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List journal imports. Returns a list of journal imports, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/list.json">API Doc - Journal/Import/List imports</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<JournalImport>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new journal import. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/create.json">API Doc - Journal/Import/Create import</a>
    /// </summary>
    /// <param name="journalImport"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(JournalImportCreate journalImport, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Execute journal import. Executes an existing journal import. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/execute.json">API Doc - Journal/Import/Execute import</a>
    /// </summary>
    /// <param name="journalImport">The entry containing the ID of the journal import to execute.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Execute(Entry journalImport, [Optional] CancellationToken cancellationToken);
}
