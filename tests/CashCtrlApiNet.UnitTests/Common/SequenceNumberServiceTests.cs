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
using CashCtrlApiNet.Abstractions.Models.Common.SequenceNumber;
using CashCtrlApiNet.Services.Connectors.Common;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Common;

/// <summary>
/// Unit tests for <see cref="SequenceNumberService"/>
/// </summary>
public class SequenceNumberServiceTests : ServiceTestBase<SequenceNumberService>
{
    /// <inheritdoc />
    protected override SequenceNumberService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<SequenceNumber>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<SequenceNumber>>());

        var result = await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<SequenceNumber>, Entry>(
                CommonEndpoints.SequenceNumber.Read, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<SequenceNumberListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SequenceNumberListed>>());

        var result = await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SequenceNumberListed>>(
                CommonEndpoints.SequenceNumber.List, (ListParams?)null, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<SequenceNumberListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SequenceNumberListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SequenceNumberListed>>(
                CommonEndpoints.SequenceNumber.List, listParams, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<SequenceNumberListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<SequenceNumberListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var sequenceNumber = new SequenceNumberCreate { Name = "Test", Pattern = "{0000}" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SequenceNumberCreate>(Arg.Any<string>(), Arg.Any<SequenceNumberCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(sequenceNumber);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SequenceNumberCreate>(
                CommonEndpoints.SequenceNumber.Create, sequenceNumber, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var sequenceNumber = new SequenceNumberUpdate { Id = 1, Name = "Test", Pattern = "{0000}" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SequenceNumberUpdate>(Arg.Any<string>(), Arg.Any<SequenceNumberUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(sequenceNumber);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SequenceNumberUpdate>(
                CommonEndpoints.SequenceNumber.Update, sequenceNumber, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2, 3] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                CommonEndpoints.SequenceNumber.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetGeneratedNumber_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetPlainTextAsync(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<PlainTextResponse>());

        var result = await Service.GetGeneratedNumber(entry);

        await ConnectionHandler.Received(1)
            .GetPlainTextAsync(CommonEndpoints.SequenceNumber.Get, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
