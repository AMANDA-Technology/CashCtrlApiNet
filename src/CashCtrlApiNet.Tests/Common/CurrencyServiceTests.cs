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
using CashCtrlApiNet.Abstractions.Models.Common.Currency;
using CashCtrlApiNet.Services.Connectors.Common;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Common;

/// <summary>
/// Unit tests for <see cref="CurrencyService"/>
/// </summary>
public class CurrencyServiceTests : ServiceTestBase<CurrencyService>
{
    /// <inheritdoc />
    protected override CurrencyService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Currency>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Currency>>());

        var result = await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Currency>, Entry>(
                CommonEndpoints.Currency.Read, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<CurrencyListed>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CurrencyListed>>());

        var result = await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CurrencyListed>>(
                CommonEndpoints.Currency.List, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<CurrencyListed>, ListParams>(
                Arg.Any<string>(), Arg.Any<ListParams>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CurrencyListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CurrencyListed>, ListParams>(
                CommonEndpoints.Currency.List, listParams, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<CurrencyListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<CurrencyListed>, ListParams>(
                Arg.Any<string>(), Arg.Any<ListParams>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var currency = new CurrencyCreate { Code = "USD" };
        ConnectionHandler
            .PostAsync<NoContentResponse, CurrencyCreate>(Arg.Any<string>(), Arg.Any<CurrencyCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(currency);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CurrencyCreate>(
                CommonEndpoints.Currency.Create, currency, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var currency = new CurrencyUpdate { Id = 1, Code = "USD" };
        ConnectionHandler
            .PostAsync<NoContentResponse, CurrencyUpdate>(Arg.Any<string>(), Arg.Any<CurrencyUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(currency);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CurrencyUpdate>(
                CommonEndpoints.Currency.Update, currency, Arg.Any<CancellationToken>());
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
                CommonEndpoints.Currency.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExchangeRate_ShouldCallCorrectEndpoint()
    {
        var request = new CurrencyExchangeRateRequest { From = "CHF", To = "EUR" };
        ConnectionHandler
            .GetAsync<SingleResponse<CurrencyExchangeRate>, CurrencyExchangeRateRequest>(
                Arg.Any<string>(), Arg.Any<CurrencyExchangeRateRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<CurrencyExchangeRate>>());

        var result = await Service.GetExchangeRate(request);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<CurrencyExchangeRate>, CurrencyExchangeRateRequest>(
                CommonEndpoints.Currency.ExchangeRate, request, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
