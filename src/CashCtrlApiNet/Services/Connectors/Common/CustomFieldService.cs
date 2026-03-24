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
using CashCtrlApiNet.Abstractions.Models.Common.CustomField;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Common;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.CommonEndpoints.CustomField;

namespace CashCtrlApiNet.Services.Connectors.Common;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Common.ICustomFieldService" />
public class CustomFieldService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ICustomFieldService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<CustomField>>> Get(Entry customField, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<CustomField>, Entry>(Endpoint.Read, customField, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<CustomFieldListed>>> GetList(CustomFieldListRequest listRequest, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<ListResponse<CustomFieldListed>, CustomFieldListRequest>(Endpoint.List, listRequest, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(CustomFieldCreate customField, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, CustomFieldCreate>(Endpoint.Create, customField, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(CustomFieldUpdate customField, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, CustomFieldUpdate>(Endpoint.Update, customField, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries customFields, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, customFields, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Reorder(CustomFieldReorder reorder, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, CustomFieldReorder>(Endpoint.Reorder, reorder, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult> GetTypes([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync(Endpoint.Types, cancellationToken);
}
