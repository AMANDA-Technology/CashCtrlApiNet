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
using CashCtrlApiNet.Abstractions.Models.Journal.Import;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Journal;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.JournalEndpoints.Import;

namespace CashCtrlApiNet.Services.Connectors.Journal;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Journal.IJournalImportService" />
public class JournalImportService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), IJournalImportService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<JournalImport>>> Get(Entry journalImport, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<SingleResponse<JournalImport>, Entry>(Endpoint.Read, journalImport, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<ListResponse<JournalImport>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default)
        => ConnectionHandler.GetAsync<ListResponse<JournalImport>>(Endpoint.List, listParams, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Create(JournalImportCreate journalImport, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, JournalImportCreate>(Endpoint.Create, journalImport, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Execute(Entry journalImport, CancellationToken cancellationToken = default)
        => ConnectionHandler.PostAsync<NoContentResponse, Entry>(Endpoint.Execute, journalImport, cancellationToken: cancellationToken);
}
