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

using CashCtrlApiNet.Abstractions.Models.Order.Layout;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Order;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;

namespace CashCtrlApiNet.Tests.Order;

/// <summary>
/// Unit tests for <see cref="OrderLayoutService"/>
/// </summary>
public class OrderLayoutServiceTests : ServiceTestBase<OrderLayoutService>
{
    /// <inheritdoc />
    protected override OrderLayoutService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<OrderLayout>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<OrderLayout>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<OrderLayout>, Entry>(
                OrderEndpoints.Layout.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<OrderLayout>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<OrderLayout>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<OrderLayout>>(
                OrderEndpoints.Layout.List, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var layout = new OrderLayoutCreate { Name = "Test Layout" };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderLayoutCreate>(Arg.Any<string>(), Arg.Any<OrderLayoutCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(layout);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderLayoutCreate>(
                OrderEndpoints.Layout.Create, layout, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var layout = new OrderLayoutUpdate { Id = 1, Name = "Updated Layout" };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderLayoutUpdate>(Arg.Any<string>(), Arg.Any<OrderLayoutUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(layout);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderLayoutUpdate>(
                OrderEndpoints.Layout.Update, layout, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                OrderEndpoints.Layout.Delete, entries, Arg.Any<CancellationToken>());
    }
}
