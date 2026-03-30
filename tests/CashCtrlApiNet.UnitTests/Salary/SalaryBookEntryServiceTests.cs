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
using CashCtrlApiNet.Abstractions.Models.Salary.BookEntry;
using CashCtrlApiNet.Services.Connectors.Salary;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;

namespace CashCtrlApiNet.UnitTests.Salary;

/// <summary>
/// Unit tests for <see cref="SalaryBookEntryService"/>
/// </summary>
public class SalaryBookEntryServiceTests : ServiceTestBase<SalaryBookEntryService>
{
    /// <inheritdoc />
    protected override SalaryBookEntryService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<SalaryBookEntry>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<SalaryBookEntry>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<SalaryBookEntry>, Entry>(
                SalaryEndpoints.BookEntry.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint_WithListRequest()
    {
        var listRequest = new SalaryBookEntryListRequest { Id = 10 };
        ConnectionHandler
            .GetAsync<ListResponse<SalaryBookEntry>, SalaryBookEntryListRequest>(
                Arg.Any<string>(), Arg.Any<SalaryBookEntryListRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryBookEntry>>());

        await Service.GetList(listRequest);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryBookEntry>, SalaryBookEntryListRequest>(
                SalaryEndpoints.BookEntry.List, listRequest, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var bookEntry = new SalaryBookEntryCreate { CreditId = 1, DebitId = 2, StatementIds = "1,2" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryBookEntryCreate>(Arg.Any<string>(), Arg.Any<SalaryBookEntryCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(bookEntry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryBookEntryCreate>(
                SalaryEndpoints.BookEntry.Create, bookEntry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var bookEntry = new SalaryBookEntryUpdate { Id = 1, CreditId = 1, DebitId = 2, StatementIds = "1,2" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryBookEntryUpdate>(Arg.Any<string>(), Arg.Any<SalaryBookEntryUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(bookEntry);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryBookEntryUpdate>(
                SalaryEndpoints.BookEntry.Update, bookEntry, Arg.Any<CancellationToken>());
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
                SalaryEndpoints.BookEntry.Delete, entries, Arg.Any<CancellationToken>());
    }
}
