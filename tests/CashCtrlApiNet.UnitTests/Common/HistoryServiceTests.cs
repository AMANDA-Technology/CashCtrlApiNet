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
using CashCtrlApiNet.Abstractions.Models.Common.History;
using CashCtrlApiNet.Services.Connectors.Common;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Common;

/// <summary>
/// Unit tests for <see cref="HistoryService"/>
/// </summary>
public class HistoryServiceTests : ServiceTestBase<HistoryService>
{
    /// <inheritdoc />
    protected override HistoryService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint_WithRequestParameter()
    {
        var request = new HistoryListRequest { Id = 42, Type = "JOURNAL" };
        ConnectionHandler
            .GetAsync<ListResponse<HistoryEntry>, HistoryListRequest>(
                Arg.Any<string>(), Arg.Any<HistoryListRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<HistoryEntry>>());

        var result = await Service.GetList(request);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<HistoryEntry>, HistoryListRequest>(
                CommonEndpoints.History.List, request, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
