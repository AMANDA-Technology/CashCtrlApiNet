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
using CashCtrlApiNet.Abstractions.Models.Salary.Statement;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Salary;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.SalaryEndpoints.Statement;

namespace CashCtrlApiNet.Services.Connectors.Salary;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryStatementService" />
public class SalaryStatementService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ISalaryStatementService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<SalaryStatement>>> Get(Entry statement, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<SalaryStatement>, Entry>(Endpoint.Read, statement, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<SalaryStatement>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<SalaryStatement>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(SalaryStatementCreate statement, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryStatementCreate>(Endpoint.Create, statement, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(SalaryStatementUpdate statement, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryStatementUpdate>(Endpoint.Update, statement, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateMultiple(SalaryStatementUpdateMultiple statements, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryStatementUpdateMultiple>(Endpoint.UpdateMultiple, statements, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateStatus(SalaryStatementStatusUpdate status, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryStatementStatusUpdate>(Endpoint.UpdateStatus, status, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateRecurrence(SalaryStatementRecurrenceUpdate recurrence, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryStatementRecurrenceUpdate>(Endpoint.UpdateRecurrence, recurrence, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries statements, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, statements, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Calculate(SalaryStatementCalculate calculation, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryStatementCalculate>(Endpoint.Calculate, calculation, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments attachments, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, EntryAttachments>(Endpoint.UpdateAttachments, attachments, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportExcel(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListXlsx, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportCsv(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListCsv, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportPdf(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListPdf, cancellationToken: cancellationToken);
}
