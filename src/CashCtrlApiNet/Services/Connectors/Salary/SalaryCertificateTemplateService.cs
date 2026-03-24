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
using CashCtrlApiNet.Abstractions.Models.Salary.CertificateTemplate;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Salary;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.SalaryEndpoints.CertificateTemplate;

namespace CashCtrlApiNet.Services.Connectors.Salary;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryCertificateTemplateService" />
public class SalaryCertificateTemplateService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ISalaryCertificateTemplateService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<SalaryCertificateTemplate>>> Get(Entry template, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<SalaryCertificateTemplate>, Entry>(Endpoint.Read, template, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<SalaryCertificateTemplate>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => listParams is not null
            ? ConnectionHandler.GetAsync<ListResponse<SalaryCertificateTemplate>, ListParams>(Endpoint.List, listParams, cancellationToken)
            : ConnectionHandler.GetAsync<ListResponse<SalaryCertificateTemplate>>(Endpoint.List, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<SalaryCertificateTemplate>>> GetTree([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<SalaryCertificateTemplate>>(Endpoint.Tree, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(SalaryCertificateTemplateCreate template, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryCertificateTemplateCreate>(Endpoint.Create, template, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(SalaryCertificateTemplateUpdate template, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, SalaryCertificateTemplateUpdate>(Endpoint.Update, template, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries templates, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, templates, cancellationToken: cancellationToken);
}
