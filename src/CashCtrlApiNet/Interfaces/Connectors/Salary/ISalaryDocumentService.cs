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
using CashCtrlApiNet.Abstractions.Models.Salary.Document;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary document service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document">API Doc - Salary/Document</a>
/// </summary>
public interface ISalaryDocumentService
{
    /// <summary>
    /// Read salary document. Returns a single document by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/read.json">API Doc - Salary/Document/Read</a>
    /// </summary>
    /// <param name="document">The entry containing the ID of the document.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryDocument>>> Get(Entry document, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Download salary document as PDF.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/read.pdf">API Doc - Salary/Document/Read PDF</a>
    /// </summary>
    /// <param name="documents">The entries containing the IDs of the documents.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadPdf(Entries documents, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Download salary documents as ZIP.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/read.zip">API Doc - Salary/Document/Read ZIP</a>
    /// </summary>
    /// <param name="documents">The entries containing the IDs of the documents.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> DownloadZip(Entries documents, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Send salary document per email.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/mail.json">API Doc - Salary/Document/Mail</a>
    /// </summary>
    /// <param name="mail">The mail request containing the statement IDs, sender, subject and text.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> SendMail(SalaryDocumentMail mail, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary document. Updates an existing document. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/update.json">API Doc - Salary/Document/Update</a>
    /// </summary>
    /// <param name="document"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryDocumentUpdate document, [Optional] CancellationToken cancellationToken);
}
