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
using CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod;
using CashCtrlApiNet.Services.Connectors.Meta;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Meta;

/// <summary>
/// Unit tests for <see cref="FiscalPeriodService"/>
/// </summary>
public class FiscalPeriodServiceTests : ServiceTestBase<FiscalPeriodService>
{
    /// <inheritdoc />
    protected override FiscalPeriodService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<FiscalPeriod>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<FiscalPeriod>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<FiscalPeriod>, Entry>(
                MetaEndpoints.FiscalPeriod.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<FiscalPeriodListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<FiscalPeriodListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<FiscalPeriodListed>>(
                MetaEndpoints.FiscalPeriod.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<FiscalPeriodListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<FiscalPeriodListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<FiscalPeriodListed>>(
                MetaEndpoints.FiscalPeriod.List, listParams, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<FiscalPeriodListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<FiscalPeriodListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var fiscalPeriod = new FiscalPeriodCreate { StartDate = "2024-01-01", EndDate = "2024-12-31" };
        ConnectionHandler
            .PostAsync<NoContentResponse, FiscalPeriodCreate>(Arg.Any<string>(), Arg.Any<FiscalPeriodCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(fiscalPeriod);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, FiscalPeriodCreate>(
                MetaEndpoints.FiscalPeriod.Create, fiscalPeriod, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var fiscalPeriod = new FiscalPeriodUpdate { Id = 1, StartDate = "2024-01-01", EndDate = "2024-12-31" };
        ConnectionHandler
            .PostAsync<NoContentResponse, FiscalPeriodUpdate>(Arg.Any<string>(), Arg.Any<FiscalPeriodUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(fiscalPeriod);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, FiscalPeriodUpdate>(
                MetaEndpoints.FiscalPeriod.Update, fiscalPeriod, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Switch_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Switch(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.Switch, entry, Arg.Any<CancellationToken>());
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
                MetaEndpoints.FiscalPeriod.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetResult_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<FiscalPeriod>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<FiscalPeriod>>());

        await Service.GetResult(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<FiscalPeriod>, Entry>(
                MetaEndpoints.FiscalPeriod.Result, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetDepreciations_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<ListResponse<FiscalPeriodListed>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<FiscalPeriodListed>>());

        await Service.GetDepreciations(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<FiscalPeriodListed>, Entry>(
                MetaEndpoints.FiscalPeriod.Deprecations, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task BookDepreciations_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.BookDepreciations(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.BookDeprecations, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetExchangeDiff_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<ListResponse<FiscalPeriodListed>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<FiscalPeriodListed>>());

        await Service.GetExchangeDiff(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<FiscalPeriodListed>, Entry>(
                MetaEndpoints.FiscalPeriod.ExchangeDifferences, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task BookExchangeDiff_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.BookExchangeDiff(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.BookExchangeDifferences, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Complete_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Complete(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.Complete, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Reopen_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Reopen(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.Reopen, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task CompleteMonths_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.CompleteMonths(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.CompleteMonths, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ReopenMonths_ShouldPostToCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.ReopenMonths(entry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entry>(
                MetaEndpoints.FiscalPeriod.ReopenMonths, entry, Arg.Any<CancellationToken>());
    }
}
