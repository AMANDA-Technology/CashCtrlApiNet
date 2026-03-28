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
using CashCtrlApiNet.Abstractions.Models.Common.CustomField;
using CashCtrlApiNet.Services.Connectors.Common;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Common;

/// <summary>
/// Unit tests for <see cref="CustomFieldService"/>
/// </summary>
public class CustomFieldServiceTests : ServiceTestBase<CustomFieldService>
{
    /// <inheritdoc />
    protected override CustomFieldService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<CustomField>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<CustomField>>());

        var result = await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<CustomField>, Entry>(
                CommonEndpoints.CustomField.Read, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint_WithTypeParameter()
    {
        var listRequest = new CustomFieldListRequest { Type = CustomFieldType.JOURNAL };
        ConnectionHandler
            .GetAsync<ListResponse<CustomFieldListed>, CustomFieldListRequest>(
                Arg.Any<string>(), Arg.Any<CustomFieldListRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CustomFieldListed>>());

        var result = await Service.GetList(listRequest);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CustomFieldListed>, CustomFieldListRequest>(
                CommonEndpoints.CustomField.List, listRequest, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var customField = new CustomFieldCreate { DataType = CustomFieldDataType.TEXT, RowLabel = "Test", Type = CustomFieldType.JOURNAL };
        ConnectionHandler
            .PostAsync<NoContentResponse, CustomFieldCreate>(Arg.Any<string>(), Arg.Any<CustomFieldCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(customField);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CustomFieldCreate>(
                CommonEndpoints.CustomField.Create, customField, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var customField = new CustomFieldUpdate { Id = 1, DataType = CustomFieldDataType.TEXT, RowLabel = "Test", Type = CustomFieldType.JOURNAL };
        ConnectionHandler
            .PostAsync<NoContentResponse, CustomFieldUpdate>(Arg.Any<string>(), Arg.Any<CustomFieldUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(customField);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CustomFieldUpdate>(
                CommonEndpoints.CustomField.Update, customField, Arg.Any<CancellationToken>());
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
                CommonEndpoints.CustomField.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Reorder_ShouldPostToCorrectEndpoint()
    {
        var reorder = new CustomFieldReorder { Ids = [1, 2, 3], Target = 5 };
        ConnectionHandler
            .PostAsync<NoContentResponse, CustomFieldReorder>(Arg.Any<string>(), Arg.Any<CustomFieldReorder>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Reorder(reorder);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CustomFieldReorder>(
                CommonEndpoints.CustomField.Reorder, reorder, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetTypes_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult());

        await Service.GetTypes();

        await ConnectionHandler.Received(1)
            .GetAsync(CommonEndpoints.CustomField.Types, Arg.Any<CancellationToken>());
    }
}
