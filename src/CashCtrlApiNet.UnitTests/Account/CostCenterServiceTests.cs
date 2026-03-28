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

using CashCtrlApiNet.Abstractions.Models.Account.CostCenter;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Account;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Account;

/// <summary>
/// Unit tests for <see cref="CostCenterService"/>
/// </summary>
public class CostCenterServiceTests : ServiceTestBase<CostCenterService>
{
    /// <inheritdoc />
    protected override CostCenterService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<CostCenter>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<CostCenter>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<CostCenter>, Entry>(
                AccountEndpoints.CostCenter.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<CostCenterListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CostCenterListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CostCenterListed>>(
                AccountEndpoints.CostCenter.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<CostCenterListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<CostCenterListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<CostCenterListed>>(
                AccountEndpoints.CostCenter.List, listParams, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<CostCenterListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<CostCenterListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Test]
    public async Task GetBalance_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<CostCenter>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<CostCenter>>());

        await Service.GetBalance(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<CostCenter>, Entry>(
                AccountEndpoints.CostCenter.Balance, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var costCenter = new CostCenterCreate { Name = "Test Cost Center" };
        ConnectionHandler
            .PostAsync<NoContentResponse, CostCenterCreate>(Arg.Any<string>(), Arg.Any<CostCenterCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(costCenter);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CostCenterCreate>(
                AccountEndpoints.CostCenter.Create, costCenter, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var costCenter = new CostCenterUpdate { Id = 1, Name = "Updated Cost Center" };
        ConnectionHandler
            .PostAsync<NoContentResponse, CostCenterUpdate>(Arg.Any<string>(), Arg.Any<CostCenterUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(costCenter);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, CostCenterUpdate>(
                AccountEndpoints.CostCenter.Update, costCenter, Arg.Any<CancellationToken>());
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
                AccountEndpoints.CostCenter.Delete, entries, Arg.Any<CancellationToken>());
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
                AccountEndpoints.CostCenter.Categorize, categorize, Arg.Any<CancellationToken>());
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
                AccountEndpoints.CostCenter.UpdateAttachments, attachments, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.CostCenter.ListXlsx, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task ExportCsv_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportCsv();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.CostCenter.ListCsv, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task ExportPdf_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportPdf();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.CostCenter.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
