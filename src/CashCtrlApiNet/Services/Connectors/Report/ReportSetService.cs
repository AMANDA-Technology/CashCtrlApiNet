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
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Report;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.ReportEndpoints.Set;

namespace CashCtrlApiNet.Services.Connectors.Report;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Report.IReportSetService" />
public class ReportSetService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IReportSetService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<ReportSet>>> Get(Entry set, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<ReportSet>, Entry>(Endpoint.Read, set, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(ReportSetCreate set, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, ReportSetCreate>(Endpoint.Create, set, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(ReportSetUpdate set, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, ReportSetUpdate>(Endpoint.Update, set, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries sets, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, sets, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Reorder(ReportSetReorder reorder, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, ReportSetReorder>(Endpoint.Reorder, reorder, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<ReportSet>>> GetMeta(Entry set, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<ReportSet>, Entry>(Endpoint.ReadMeta, set, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> DownloadPdf(Entry set, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.DownloadPdf, set, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> DownloadCsv(Entry set, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.DownloadCsv, set, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> DownloadExcel(Entry set, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.DownloadXlsx, set, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> DownloadAnnualReport(Entry set, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.DownloadAnnualReport, set, cancellationToken);
}
