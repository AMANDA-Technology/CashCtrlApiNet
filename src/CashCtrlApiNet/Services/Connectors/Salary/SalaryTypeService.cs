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
using CashCtrlApiNet.Abstractions.Models.Salary.Type;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Salary;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.SalaryEndpoints.Type;

namespace CashCtrlApiNet.Services.Connectors.Salary;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryTypeService" />
public class SalaryTypeService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ISalaryTypeService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<SalaryType>>> Get(Entry salaryType, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<SalaryType>, Entry>(Endpoint.Read, salaryType, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<SalaryType>>> GetList([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<SalaryType>>(Endpoint.List, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(SalaryTypeCreate salaryType, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryTypeCreate>(Endpoint.Create, salaryType, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(SalaryTypeUpdate salaryType, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryTypeUpdate>(Endpoint.Update, salaryType, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize salaryTypesCategorize, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, EntriesCategorize>(Endpoint.Categorize, salaryTypesCategorize, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries salaryTypes, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, salaryTypes, cancellationToken: cancellationToken);

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
