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
using CashCtrlApiNet.Abstractions.Models.Account;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces.Connectors.Account;

/// <summary>
/// CashCtrl account service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/account">API Doc - Account/Account</see>
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Read account. Returns a single account by ID.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/account/read.json">API Doc - Inventory/Account/Read account</see>
    /// </summary>
    /// <param name="accountId">The ID of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Abstractions.Models.Account.Account>>> Get(int accountId, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List accounts. Returns a list of accounts.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/account/list.json">API Doc - Inventory/Account/List accounts</see>
    /// </summary>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<Abstractions.Models.Account.Account>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new account. Returns either a success or multiple error messages (for each issue).
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/account/create.json">API Doc - Inventory/Account/Create account</see>
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(AccountCreate account, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update account. Updates an existing account. Returns either a success or multiple error messages (for each issue).
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/account/update.json">API Doc - Inventory/Account/Update account</see>
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(AccountUpdate account, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete accounts. Deletes one or multiple accounts. Returns either a success or error message.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/account/delete.json">API Doc - Inventory/Account/Delete accounts</see>
    /// </summary>
    /// <param name="accounts"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries accounts, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Categorize accounts. Assigns one or multiple accounts to the desired category. Returns either a success or error message.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/account/categorize.json">API Doc - Inventory/Account/Categorize accounts</see>
    /// </summary>
    /// <param name="accountsCategorize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize accountsCategorize, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update attachments. Updates the file attachments of an account. Use the File API to upload a file and then use the file ID here. Returns either a success or error message.
    /// <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/account/update_attachments.json">API Doc - Inventory/Account/Update attachments</see>
    /// </summary>
    /// <param name="accountAttachments"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments accountAttachments, [Optional] CancellationToken cancellationToken);
}
