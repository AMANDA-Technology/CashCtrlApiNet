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
using CashCtrlApiNet.Abstractions.Models.Journal;
using CashCtrlApiNet.Services.Connectors.Journal;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Journal;

/// <summary>
/// Unit tests for <see cref="JournalService"/>
/// </summary>
public class JournalServiceTests : ServiceTestBase<JournalService>
{
    /// <inheritdoc />
    protected override JournalService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Abstractions.Models.Journal.Journal>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Abstractions.Models.Journal.Journal>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Abstractions.Models.Journal.Journal>, Entry>(
                JournalEndpoints.Journal.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<JournalListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<JournalListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<JournalListed>>(
                JournalEndpoints.Journal.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var journal = new JournalCreate { DateAdded = "2024-01-01", Title = "Test", SequenceNumberId = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, JournalCreate>(Arg.Any<string>(), Arg.Any<JournalCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(journal);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, JournalCreate>(
                JournalEndpoints.Journal.Create, journal, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var journal = new JournalUpdate { Id = 1, DateAdded = "2024-01-01", Title = "Test", SequenceNumberId = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, JournalUpdate>(Arg.Any<string>(), Arg.Any<JournalUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(journal);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, JournalUpdate>(
                JournalEndpoints.Journal.Update, journal, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2, 3] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                JournalEndpoints.Journal.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAttachments_ShouldPostToCorrectEndpoint()
    {
        var attachments = new EntryAttachments { Id = 1, AttachedFileIds = [10, 20] };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntryAttachments>(Arg.Any<string>(), Arg.Any<EntryAttachments>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateAttachments(attachments);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntryAttachments>(
                JournalEndpoints.Journal.UpdateAttachments, attachments, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRecurrence_ShouldPostToCorrectEndpoint()
    {
        var recurrence = new EntryRecurrence { Id = 1, Recurrence = "{}" };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntryRecurrence>(Arg.Any<string>(), Arg.Any<EntryRecurrence>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateRecurrence(recurrence);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntryRecurrence>(
                JournalEndpoints.Journal.UpdateRecurrence, recurrence, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<JournalListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<JournalListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<JournalListed>>(
                JournalEndpoints.Journal.List, listParams, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<JournalListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<JournalListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Fact]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(JournalEndpoints.Journal.ListXlsx, Arg.Any<CancellationToken>());
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
            .GetBinaryAsync(JournalEndpoints.Journal.ListCsv, Arg.Any<CancellationToken>());
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
            .GetBinaryAsync(JournalEndpoints.Journal.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
