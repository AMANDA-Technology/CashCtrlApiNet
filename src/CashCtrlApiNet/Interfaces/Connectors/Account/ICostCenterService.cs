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

using CashCtrlApiNet.Abstractions.Models.Account.CostCenter;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Account;

/// <summary>
/// CashCtrl account cost center service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter">API Doc - Account/Cost center</a>
/// </summary>
public interface ICostCenterService
{
    /// <summary>
    /// Read cost center. Returns a single cost center by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/read.json">API Doc - Account/Cost center/Read</a>
    /// </summary>
    /// <param name="costCenter">The entry containing the ID of the cost center.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<CostCenter>>> Get(Entry costCenter, CancellationToken cancellationToken = default);

    /// <summary>
    /// List cost centers. Returns a list of cost centers, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/list.json">API Doc - Account/Cost center/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CostCenterListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get cost center balance. Returns the balance of a single cost center as a decimal value.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/balance">API Doc - Account/Cost center/Get balance</a>
    /// </summary>
    /// <param name="costCenter">The entry containing the ID of the cost center.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<DecimalResponse>> GetBalance(Entry costCenter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new cost center. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/create.json">API Doc - Account/Cost center/Create</a>
    /// </summary>
    /// <param name="costCenter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(CostCenterCreate costCenter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update cost center. Updates an existing cost center. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/update.json">API Doc - Account/Cost center/Update</a>
    /// </summary>
    /// <param name="costCenter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(CostCenterUpdate costCenter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete cost centers. Deletes one or multiple cost centers. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/delete.json">API Doc - Account/Cost center/Delete</a>
    /// </summary>
    /// <param name="costCenters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries costCenters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Categorize cost centers. Assigns one or multiple cost centers to the desired category. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/categorize.json">API Doc - Account/Cost center/Categorize</a>
    /// </summary>
    /// <param name="costCentersCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize costCentersCategorize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update attachments. Updates the file attachments of a cost center. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/update_attachments.json">API Doc - Account/Cost center/Update attachments</a>
    /// </summary>
    /// <param name="costCenterAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments costCenterAttachments, CancellationToken cancellationToken = default);

    /// <summary>
    /// Export cost centers as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/list.xlsx">API Doc - Account/Cost center/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Export cost centers as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/list.csv">API Doc - Account/Cost center/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv(CancellationToken cancellationToken = default);

    /// <summary>
    /// Export cost centers as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/costcenter/list.pdf">API Doc - Account/Cost center/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf(CancellationToken cancellationToken = default);
}
