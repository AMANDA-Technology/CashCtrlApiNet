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
using CashCtrlApiNet.Abstractions.Models.Journal.Import.Entry;
using CashCtrlApiNet.Services.Connectors.Journal;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Journal;

/// <summary>
/// Unit tests for <see cref="JournalImportEntryService"/>
/// </summary>
public class JournalImportEntryServiceTests : ServiceTestBase<JournalImportEntryService>
{
    /// <inheritdoc />
    protected override JournalImportEntryService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<JournalImportEntry>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<JournalImportEntry>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<JournalImportEntry>, Entry>(
                JournalEndpoints.ImportEntry.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<JournalImportEntryListed>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<JournalImportEntryListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<JournalImportEntryListed>>(
                JournalEndpoints.ImportEntry.List, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var importEntry = new JournalImportEntryUpdate { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, JournalImportEntryUpdate>(Arg.Any<string>(), Arg.Any<JournalImportEntryUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(importEntry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, JournalImportEntryUpdate>(
                JournalEndpoints.ImportEntry.Update, importEntry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Ignore_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Ignore(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                JournalEndpoints.ImportEntry.Ignore, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Restore_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Restore(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                JournalEndpoints.ImportEntry.Restore, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Confirm_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Confirm(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                JournalEndpoints.ImportEntry.Confirm, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Unconfirm_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Unconfirm(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                JournalEndpoints.ImportEntry.Unconfirm, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(JournalEndpoints.ImportEntry.ListXlsx, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task ExportCsv_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportCsv();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(JournalEndpoints.ImportEntry.ListCsv, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task ExportPdf_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportPdf();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(JournalEndpoints.ImportEntry.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
