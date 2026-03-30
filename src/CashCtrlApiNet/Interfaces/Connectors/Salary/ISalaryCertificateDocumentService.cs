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
using CashCtrlApiNet.Abstractions.Models.Salary.CertificateDocument;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary certificate document service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/document">API Doc - Salary/Certificate document</a>
/// </summary>
public interface ISalaryCertificateDocumentService
{
    /// <summary>
    /// Read salary certificate document. Returns a single document by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/document/read.json">API Doc - Salary/Certificate document/Read</a>
    /// </summary>
    /// <param name="document">The entry containing the ID of the document.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryCertificateDocument>>> Get(Entry document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download salary certificate document as PDF.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/document/read.pdf">API Doc - Salary/Certificate document/Read PDF</a>
    /// </summary>
    /// <param name="documents">The entries containing the IDs of the documents.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadPdf(Entries documents, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download salary certificate documents as ZIP.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/document/read.zip">API Doc - Salary/Certificate document/Read ZIP</a>
    /// </summary>
    /// <param name="documents">The entries containing the IDs of the documents.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadZip(Entries documents, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send salary certificate document per email.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/document/mail.json">API Doc - Salary/Certificate document/Mail</a>
    /// </summary>
    /// <param name="mail">The mail request containing the certificate IDs, sender, subject and text.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> SendMail(SalaryCertificateDocumentMail mail, CancellationToken cancellationToken = default);
}
