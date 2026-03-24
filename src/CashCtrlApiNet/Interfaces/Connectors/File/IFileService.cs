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
using CashCtrlApiNet.Abstractions.Models.File;

namespace CashCtrlApiNet.Interfaces.Connectors.File;

/// <summary>
/// CashCtrl file service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file">API Doc - File/File</a>
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Get file content. Returns the binary content of a file by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/get">API Doc - File/Get content</a>
    /// </summary>
    /// <param name="file">The entry containing the ID of the file.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> GetContent(Entry file, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Read file. Returns a single file by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/read.json">API Doc - File/Read file</a>
    /// </summary>
    /// <param name="file">The entry containing the ID of the file.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.File.File>>> Get(Entry file, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List files. Returns a list of files.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/list.json">API Doc - File/List files</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<Abstractions.Models.File.File>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Prepare files. Uploads files using multipart form data. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/prepare.json">API Doc - File/Prepare files</a>
    /// </summary>
    /// <param name="content">The multipart form data content containing the files to upload.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Prepare(MultipartFormDataContent content, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Persist files. Persists one or multiple prepared files. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/persist.json">API Doc - File/Persist files</a>
    /// </summary>
    /// <param name="files">The entries containing the IDs of the files to persist.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Persist(Entries files, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new file. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/create.json">API Doc - File/Create file</a>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(FileCreate file, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update file. Updates an existing file. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/update.json">API Doc - File/Update file</a>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(FileUpdate file, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete files. Deletes one or multiple files. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/delete.json">API Doc - File/Delete files</a>
    /// </summary>
    /// <param name="files"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries files, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize files. Assigns one or multiple files to the desired category. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/categorize.json">API Doc - File/Categorize files</a>
    /// </summary>
    /// <param name="filesCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize filesCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Empty archive. Deletes all archived files. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/empty_archive.json">API Doc - File/Empty archive</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> EmptyArchive([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Restore files. Restores one or multiple files from the archive. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/restore.json">API Doc - File/Restore files</a>
    /// </summary>
    /// <param name="files"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Restore(Entries files, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export files as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/list.xlsx">API Doc - File/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export files as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/list.csv">API Doc - File/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export files as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file/list.pdf">API Doc - File/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
