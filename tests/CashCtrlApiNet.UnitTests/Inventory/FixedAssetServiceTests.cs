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
using CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset;
using CashCtrlApiNet.Services.Connectors.Inventory;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Inventory;

/// <summary>
/// Unit tests for <see cref="FixedAssetService"/>
/// </summary>
public class FixedAssetServiceTests : ServiceTestBase<FixedAssetService>
{
    /// <inheritdoc />
    protected override FixedAssetService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<FixedAsset>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<FixedAsset>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<FixedAsset>, Entry>(
                InventoryEndpoints.FixedAsset.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<FixedAssetListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<FixedAssetListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<FixedAssetListed>>(
                InventoryEndpoints.FixedAsset.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<FixedAssetListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<FixedAssetListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<FixedAssetListed>>(
                InventoryEndpoints.FixedAsset.List, listParams, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<FixedAssetListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<FixedAssetListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var fixedAsset = new FixedAssetCreate { Name = "Test Asset", AccountId = 1, PurchaseCreditId = 2, DateAdded = "2026-01-01" };
        ConnectionHandler
            .PostAsync<NoContentResponse, FixedAssetCreate>(Arg.Any<string>(), Arg.Any<FixedAssetCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(fixedAsset);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, FixedAssetCreate>(
                InventoryEndpoints.FixedAsset.Create, fixedAsset, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var fixedAsset = new FixedAssetUpdate { Id = 1, Name = "Updated Asset", AccountId = 1, PurchaseCreditId = 2, DateAdded = "2026-01-01" };
        ConnectionHandler
            .PostAsync<NoContentResponse, FixedAssetUpdate>(Arg.Any<string>(), Arg.Any<FixedAssetUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(fixedAsset);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, FixedAssetUpdate>(
                InventoryEndpoints.FixedAsset.Update, fixedAsset, Arg.Any<CancellationToken>());
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
                InventoryEndpoints.FixedAsset.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Categorize_ShouldPostToCorrectEndpoint()
    {
        var categorize = new EntriesCategorize { Ids = [1], TargetCategoryId = 5 };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntriesCategorize>(Arg.Any<string>(), Arg.Any<EntriesCategorize>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Categorize(categorize);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntriesCategorize>(
                InventoryEndpoints.FixedAsset.Categorize, categorize, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task UpdateAttachments_ShouldPostToCorrectEndpoint()
    {
        var attachments = new EntryAttachments { Id = 1, AttachedFileIds = [10, 20] };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntryAttachments>(Arg.Any<string>(), Arg.Any<EntryAttachments>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateAttachments(attachments);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntryAttachments>(
                InventoryEndpoints.FixedAsset.UpdateAttachments, attachments, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(InventoryEndpoints.FixedAsset.ListXlsx, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task ExportCsv_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.ExportCsv();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(InventoryEndpoints.FixedAsset.ListCsv, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task ExportPdf_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.ExportPdf();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(InventoryEndpoints.FixedAsset.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
