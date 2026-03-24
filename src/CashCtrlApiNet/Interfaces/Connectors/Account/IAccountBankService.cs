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
using CashCtrlApiNet.Abstractions.Models.Account.Bank;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Account;

/// <summary>
/// CashCtrl bank account service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank">API Doc - Account/Bank</a>
/// </summary>
public interface IAccountBankService
{
    /// <summary>
    /// Read bank account. Returns a single bank account by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/read.json">API Doc - Account/Bank/Read</a>
    /// </summary>
    /// <param name="bankAccount">The entry containing the ID of the bank account.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<AccountBank>>> Get(Entry bankAccount, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List bank accounts. Returns a list of bank accounts.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/list.json">API Doc - Account/Bank/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<AccountBank>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new bank account. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/create.json">API Doc - Account/Bank/Create</a>
    /// </summary>
    /// <param name="bankAccount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(AccountBankCreate bankAccount, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update bank account. Updates an existing bank account. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/update.json">API Doc - Account/Bank/Update</a>
    /// </summary>
    /// <param name="bankAccount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(AccountBankUpdate bankAccount, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete bank accounts. Deletes one or multiple bank accounts. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/delete.json">API Doc - Account/Bank/Delete</a>
    /// </summary>
    /// <param name="bankAccounts"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries bankAccounts, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of a bank account. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/update_attachments.json">API Doc - Account/Bank/Update attachments</a>
    /// </summary>
    /// <param name="bankAccountAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments bankAccountAttachments, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export bank accounts as Excel file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/list.xlsx">API Doc - Account/Bank/Export Excel</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export bank accounts as CSV file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/list.csv">API Doc - Account/Bank/Export CSV</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Export bank accounts as PDF file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/list.pdf">API Doc - Account/Bank/Export PDF</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken);
}
