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

using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Common.CustomFieldGroup;
using CashCtrlApiNet.Services.Connectors.Common;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Common;

/// <summary>
/// Unit tests for <see cref="CustomFieldGroupService"/>
/// </summary>
public class CustomFieldGroupServiceTests : ServiceTestBase<CustomFieldGroupService>
{
    /// <inheritdoc />
    protected override CustomFieldGroupService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<CustomFieldGroup>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<CustomFieldGroup>>());

        var result = await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<CustomFieldGroup>, Entry>(
                CommonEndpoints.CustomFieldGroup.Read, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint_WithTypeParameter()
    {
        var listRequest = new CustomFieldGroupListRequest { Type = CustomFieldType.Order };
        ConnectionHandler
            .GetAsync<ListResponse<CustomFieldGroupListed>, CustomFieldGroupListRequest>(
                Arg.Any<string>(), Arg.Any<CustomFieldGroupListRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CustomFieldGroupListed>>());

        var result = await Service.GetList(listRequest);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CustomFieldGroupListed>, CustomFieldGroupListRequest>(
                CommonEndpoints.CustomFieldGroup.List, listRequest, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var group = new CustomFieldGroupCreate { Name = "Test Group", Type = CustomFieldType.Order };
        ConnectionHandler
            .PostAsync<NoContentResponse, CustomFieldGroupCreate>(Arg.Any<string>(), Arg.Any<CustomFieldGroupCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(group);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CustomFieldGroupCreate>(
                CommonEndpoints.CustomFieldGroup.Create, group, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var group = new CustomFieldGroupUpdate { Id = 1, Name = "Test Group", Type = CustomFieldType.Order };
        ConnectionHandler
            .PostAsync<NoContentResponse, CustomFieldGroupUpdate>(Arg.Any<string>(), Arg.Any<CustomFieldGroupUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(group);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CustomFieldGroupUpdate>(
                CommonEndpoints.CustomFieldGroup.Update, group, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                CommonEndpoints.CustomFieldGroup.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Reorder_ShouldPostToCorrectEndpoint()
    {
        var reorder = new CustomFieldGroupReorder { Type = CustomFieldType.Person, Ids = [1, 2, 3], Target = 5 };
        ConnectionHandler
            .PostAsync<NoContentResponse, CustomFieldGroupReorder>(Arg.Any<string>(), Arg.Any<CustomFieldGroupReorder>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Reorder(reorder);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CustomFieldGroupReorder>(
                CommonEndpoints.CustomFieldGroup.Reorder, reorder, Arg.Any<CancellationToken>());
    }
}
