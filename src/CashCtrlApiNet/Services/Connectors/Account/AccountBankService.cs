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
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Account;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.AccountEndpoints.Bank;

namespace CashCtrlApiNet.Services.Connectors.Account;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Account.IAccountBankService" />
public class AccountBankService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IAccountBankService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<AccountBank>>> Get(Entry bankAccount, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<AccountBank>, Entry>(Endpoint.Read, bankAccount, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<AccountBank>>> GetList([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<AccountBank>>(Endpoint.List, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(AccountBankCreate bankAccount, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, AccountBankCreate>(Endpoint.Create, bankAccount, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(AccountBankUpdate bankAccount, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, AccountBankUpdate>(Endpoint.Update, bankAccount, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries bankAccounts, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, bankAccounts, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments bankAccountAttachments, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, EntryAttachments>(Endpoint.UpdateAttachments, bankAccountAttachments, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportExcel([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListXlsx, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportCsv([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListCsv, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportPdf([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListPdf, cancellationToken: cancellationToken);
}
