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

using CashCtrlApiNet.Abstractions.Models.Account;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Account;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Account;

/// <summary>
/// Unit tests for <see cref="AccountService"/>
/// </summary>
public class AccountServiceTests : ServiceTestBase<AccountService>
{
    /// <inheritdoc />
    protected override AccountService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Abstractions.Models.Account.Account>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                AccountEndpoints.Account.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<AccountListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<AccountListed>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<AccountListed>>(
                AccountEndpoints.Account.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<AccountListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<AccountListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<AccountListed>>(
                AccountEndpoints.Account.List, listParams, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<AccountListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<AccountListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Fact]
    public async Task GetBalance_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Abstractions.Models.Account.Account>>());

        await Service.GetBalance(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Abstractions.Models.Account.Account>, Entry>(
                AccountEndpoints.Account.Balance, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var account = new AccountCreate { CategoryId = 1, Name = "Test", Number = 1000 };
        ConnectionHandler
            .PostAsync<NoContentResponse, AccountCreate>(Arg.Any<string>(), Arg.Any<AccountCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(account);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, AccountCreate>(
                AccountEndpoints.Account.Create, account, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var account = new AccountUpdate { Id = 1, CategoryId = 1, Name = "Test", Number = 1000 };
        ConnectionHandler
            .PostAsync<NoContentResponse, AccountUpdate>(Arg.Any<string>(), Arg.Any<AccountUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(account);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, AccountUpdate>(
                AccountEndpoints.Account.Update, account, Arg.Any<CancellationToken>());
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
                AccountEndpoints.Account.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Categorize_ShouldPostToCorrectEndpoint()
    {
        var categorize = new EntriesCategorize { Ids = [1], TargetCategoryId = 5 };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntriesCategorize>(Arg.Any<string>(), Arg.Any<EntriesCategorize>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Categorize(categorize);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntriesCategorize>(
                AccountEndpoints.Account.Categorize, categorize, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAttachments_ShouldPostToCorrectEndpoint()
    {
        var attachments = new EntryAttachments { Id = 1, AttachedFileIds = [10, 20] };
        ConnectionHandler
            .PostAsync<NoContentResponse, EntryAttachments>(Arg.Any<string>(), Arg.Any<EntryAttachments>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateAttachments(attachments);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, EntryAttachments>(
                AccountEndpoints.Account.UpdateAttachments, attachments, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.Account.ListXlsx, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task ExportCsv_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportCsv();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.Account.ListCsv, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task ExportPdf_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportPdf();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(AccountEndpoints.Account.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
