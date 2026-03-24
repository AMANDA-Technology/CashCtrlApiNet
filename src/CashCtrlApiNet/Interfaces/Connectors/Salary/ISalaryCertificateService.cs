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
using CashCtrlApiNet.Abstractions.Models.Salary.Certificate;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary certificate service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate">API Doc - Salary/Certificate</a>
/// </summary>
public interface ISalaryCertificateService
{
    /// <summary>
    /// Read salary certificate. Returns a single certificate by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/read.json">API Doc - Salary/Certificate/Read</a>
    /// </summary>
    /// <param name="certificate">The entry containing the ID of the certificate.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryCertificate>>> Get(Entry certificate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary certificates. Returns a list of certificates, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/list.json">API Doc - Salary/Certificate/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryCertificate>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update salary certificate. Updates an existing certificate. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/update.json">API Doc - Salary/Certificate/Update</a>
    /// </summary>
    /// <param name="certificate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryCertificateUpdate certificate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary certificates as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/list.xlsx">API Doc - Salary/Certificate/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary certificates as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/list.csv">API Doc - Salary/Certificate/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export salary certificates as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/list.pdf">API Doc - Salary/Certificate/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
