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
using CashCtrlApiNet.Abstractions.Models.Person;

namespace CashCtrlApiNet.Interfaces.Connectors.Person;

/// <summary>
/// CashCtrl person service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person">API Doc - Person/Person</a>
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Read person. Returns a single person by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/read.json">API Doc - Person/Read person</a>
    /// </summary>
    /// <param name="person">The entry containing the ID of the person.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.Person.Person>>> Get(Entry person, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List persons. Returns a list of persons, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/list.json">API Doc - Person/List persons</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<PersonListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new person. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/create.json">API Doc - Person/Create person</a>
    /// </summary>
    /// <param name="person"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(PersonCreate person, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update person. Updates an existing person. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/update.json">API Doc - Person/Update person</a>
    /// </summary>
    /// <param name="person"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(PersonUpdate person, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete persons. Deletes one or multiple persons. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/delete.json">API Doc - Person/Delete persons</a>
    /// </summary>
    /// <param name="persons"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries persons, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize persons. Assigns one or multiple persons to the desired category. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/categorize.json">API Doc - Person/Categorize persons</a>
    /// </summary>
    /// <param name="personsCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize personsCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of a person. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/update_attachments.json">API Doc - Person/Update attachments</a>
    /// </summary>
    /// <param name="personAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments personAttachments, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export persons as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/list.xlsx">API Doc - Person/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export persons as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/list.csv">API Doc - Person/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export persons as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/list.pdf">API Doc - Person/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export persons as vCard file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/list.vcf">API Doc - Person/Export vCard</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportVcard([Optional] CancellationToken cancellationToken);
}
