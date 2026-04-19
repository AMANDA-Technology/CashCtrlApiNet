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
using CashCtrlApiNet.Abstractions.Models.Report.Set;

namespace CashCtrlApiNet.Interfaces.Connectors.Report;

/// <summary>
/// CashCtrl report set service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/set">API Doc - Report/Set</a>
/// </summary>
public interface IReportSetService
{
    /// <summary>
    /// Read report set. Returns a single report set by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/read.json">API Doc - Report/Set/Read</a>
    /// </summary>
    /// <param name="set">The entry containing the ID of the report set.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<ReportSet>>> Get(Entry set, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new report set. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/create.json">API Doc - Report/Set/Create</a>
    /// </summary>
    /// <param name="set"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(ReportSetCreate set, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update report set. Updates an existing report set. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/update.json">API Doc - Report/Set/Update</a>
    /// </summary>
    /// <param name="set"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(ReportSetUpdate set, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete report sets. Deletes one or multiple report sets. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/delete.json">API Doc - Report/Set/Delete</a>
    /// </summary>
    /// <param name="sets"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries sets, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reorder report sets. Changes the order of report sets. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/reorder.json">API Doc - Report/Set/Reorder</a>
    /// </summary>
    /// <param name="reorder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Reorder(ReportSetReorder reorder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get report collection meta data (title, text, etc.) for a given time period.
    /// Takes mandatory <c>collectionId</c> — not <c>id</c> like the other read endpoints.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/meta.json">API Doc - Report/Collection/Meta</a>
    /// </summary>
    /// <param name="request">Collection ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<ReportCollectionMeta>>> GetMeta(ReportCollectionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download report collection as PDF for a given time period. Mandatory <c>collectionId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/download.pdf">API Doc - Report/Collection/Download PDF</a>
    /// </summary>
    /// <param name="request">Collection ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadPdf(ReportCollectionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download report collection as a ZIP of CSV files for a given time period.
    /// Mandatory <c>collectionId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/download.csv">API Doc - Report/Collection/Download CSV</a>
    /// </summary>
    /// <param name="request">Collection ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadCsv(ReportCollectionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download report collection as Excel (one worksheet per element) for a given time period.
    /// Mandatory <c>collectionId</c>.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/download.xlsx">API Doc - Report/Collection/Download Excel</a>
    /// </summary>
    /// <param name="request">Collection ID + optional period filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadExcel(ReportCollectionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download the annual report (balance sheet and P&amp;L by default) as PDF. Not scoped to a
    /// specific collection; takes only an optional fiscal-period override.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/download_annualreport.pdf">API Doc - Report/Collection/Download Annual Report</a>
    /// </summary>
    /// <param name="request">Optional fiscal-period override (pass <c>new()</c> for the current period).</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadAnnualReport(ReportAnnualReportRequest request, CancellationToken cancellationToken = default);
}
