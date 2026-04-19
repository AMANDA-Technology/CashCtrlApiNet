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
using CashCtrlApiNet.Abstractions.Models.Report.Element;

namespace CashCtrlApiNet.Interfaces.Connectors.Report;

/// <summary>
/// CashCtrl report element service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element">API Doc - Report/Element</a>
/// </summary>
public interface IReportElementService
{
    /// <summary>
    /// Read report element. Returns a single report element by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/read.json">API Doc - Report/Element/Read</a>
    /// </summary>
    /// <param name="element">The entry containing the ID of the report element.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<ReportElement>>> Get(Entry element, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new report element. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/create.json">API Doc - Report/Element/Create</a>
    /// </summary>
    /// <param name="element"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(ReportElementCreate element, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update report element. Updates an existing report element. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/update.json">API Doc - Report/Element/Update</a>
    /// </summary>
    /// <param name="element"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(ReportElementUpdate element, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete report elements. Deletes one or multiple report elements. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/delete.json">API Doc - Report/Element/Delete</a>
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries elements, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reorder report elements. Changes the order of report elements. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/reorder.json">API Doc - Report/Element/Reorder</a>
    /// </summary>
    /// <param name="reorder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Reorder(ReportElementReorder reorder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get report element data as JSON for the given time period. Takes mandatory <c>elementId</c>.
    /// The response shape varies by report type (tree structure for ChartOfAccounts, flat list for
    /// Journal, etc.) so this returns the untyped <see cref="ApiResult"/> with
    /// <see cref="ApiResult.RawResponseContent"/> carrying the JSON body — callers parse into the
    /// shape appropriate for their report type.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/data.json">API Doc - Report/Element/Data JSON</a>
    /// </summary>
    /// <param name="request">Element ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetData(ReportElementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get report element data as HTML for the given time period. Takes mandatory <c>elementId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/data.html">API Doc - Report/Element/Data HTML</a>
    /// </summary>
    /// <param name="request">Element ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> GetDataHtml(ReportElementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get report element meta data for the given time period. Takes mandatory <c>elementId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/meta.json">API Doc - Report/Element/Meta</a>
    /// </summary>
    /// <param name="request">Element ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<ReportElementMeta>>> GetMeta(ReportElementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download report element as PDF for the given time period. Takes mandatory <c>elementId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/download.pdf">API Doc - Report/Element/Download PDF</a>
    /// </summary>
    /// <param name="request">Element ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadPdf(ReportElementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download report element as CSV for the given time period. Takes mandatory <c>elementId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/download.csv">API Doc - Report/Element/Download CSV</a>
    /// </summary>
    /// <param name="request">Element ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadCsv(ReportElementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download report element as Excel for the given time period. Takes mandatory <c>elementId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/download.xlsx">API Doc - Report/Element/Download Excel</a>
    /// </summary>
    /// <param name="request">Element ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadExcel(ReportElementRequest request, CancellationToken cancellationToken = default);
}
