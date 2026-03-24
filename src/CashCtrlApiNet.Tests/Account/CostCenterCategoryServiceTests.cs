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

using CashCtrlApiNet.Abstractions.Models.Account.CostCenterCategory;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Account;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Account;

/// <summary>
/// Unit tests for <see cref="CostCenterCategoryService"/>
/// </summary>
public class CostCenterCategoryServiceTests : ServiceTestBase<CostCenterCategoryService>
{
    /// <inheritdoc />
    protected override CostCenterCategoryService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<CostCenterCategory>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<CostCenterCategory>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<CostCenterCategory>, Entry>(
                AccountEndpoints.CostCenterCategory.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<CostCenterCategory>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CostCenterCategory>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CostCenterCategory>>(
                AccountEndpoints.CostCenterCategory.List, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTree_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<CostCenterCategory>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CostCenterCategory>>());

        await Service.GetTree();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CostCenterCategory>>(
                AccountEndpoints.CostCenterCategory.Tree, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var category = new CostCenterCategoryCreate { Name = "Test Category" };
        ConnectionHandler
            .PostAsync<NoContentResponse, CostCenterCategoryCreate>(Arg.Any<string>(), Arg.Any<CostCenterCategoryCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(category);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CostCenterCategoryCreate>(
                AccountEndpoints.CostCenterCategory.Create, category, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var category = new CostCenterCategoryUpdate { Id = 1, Name = "Updated Category" };
        ConnectionHandler
            .PostAsync<NoContentResponse, CostCenterCategoryUpdate>(Arg.Any<string>(), Arg.Any<CostCenterCategoryUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(category);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CostCenterCategoryUpdate>(
                AccountEndpoints.CostCenterCategory.Update, category, Arg.Any<CancellationToken>());
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
                AccountEndpoints.CostCenterCategory.Delete, entries, Arg.Any<CancellationToken>());
    }
}
