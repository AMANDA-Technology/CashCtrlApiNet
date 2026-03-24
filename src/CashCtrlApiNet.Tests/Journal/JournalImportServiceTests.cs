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
using CashCtrlApiNet.Services.Connectors.Journal;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;

namespace CashCtrlApiNet.Tests.Journal;

/// <summary>
/// Unit tests for <see cref="JournalImportService"/>
/// </summary>
public class JournalImportServiceTests : ServiceTestBase<JournalImportService>
{
    /// <inheritdoc />
    protected override JournalImportService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<JournalImport>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<JournalImport>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<JournalImport>, Entry>(
                JournalEndpoints.Import.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<JournalImport>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<JournalImport>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<JournalImport>>(
                JournalEndpoints.Import.List, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var journalImport = new JournalImportCreate { FileId = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, JournalImportCreate>(Arg.Any<string>(), Arg.Any<JournalImportCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(journalImport);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, JournalImportCreate>(
                JournalEndpoints.Import.Create, journalImport, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Execute_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Execute(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                JournalEndpoints.Import.Execute, entry, Arg.Any<CancellationToken>());
    }
}
