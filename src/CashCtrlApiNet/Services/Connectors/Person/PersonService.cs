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
using CashCtrlApiNet.Abstractions.Models.Person;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Person;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.PersonEndpoints.Person;

namespace CashCtrlApiNet.Services.Connectors.Person;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Person.IPersonService" />
public class PersonService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IPersonService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<Abstractions.Models.Person.Person>>> Get(Entry person, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<Abstractions.Models.Person.Person>, Entry>(Endpoint.Read, person, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<PersonListed>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<PersonListed>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(PersonCreate person, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, PersonCreate>(Endpoint.Create, person, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(PersonUpdate person, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, PersonUpdate>(Endpoint.Update, person, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Delete(Entries persons, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entries>(Endpoint.Delete, persons, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Categorize(EntriesCategorize personsCategorize, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, EntriesCategorize>(Endpoint.Categorize, personsCategorize, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> UpdateAttachments(EntryAttachments personAttachments, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, EntryAttachments>(Endpoint.UpdateAttachments, personAttachments, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportExcel(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListXlsx, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportCsv(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListCsv, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportPdf(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListPdf, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<BinaryResponse>> ExportVcard(CancellationToken cancellationToken = default)
        => ConnectionHandler.GetBinaryAsync(Endpoint.ListVcf, cancellationToken: cancellationToken);
}
