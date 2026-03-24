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
using CashCtrlApiNet.Abstractions.Models.Salary.Statement;
using CashCtrlApiNet.Services.Connectors.Salary;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Salary;

/// <summary>
/// Unit tests for <see cref="SalaryStatementService"/>
/// </summary>
public class SalaryStatementServiceTests : ServiceTestBase<SalaryStatementService>
{
    /// <inheritdoc />
    protected override SalaryStatementService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<SalaryStatement>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<SalaryStatement>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<SalaryStatement>, Entry>(
                SalaryEndpoints.Statement.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<SalaryStatement>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryStatement>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryStatement>>(
                SalaryEndpoints.Statement.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var statement = new SalaryStatementCreate
        {
            Date = "2024-01-15",
            DatePayment = "2024-01-25",
            PersonId = 1,
            StatusId = 1,
            TemplateId = 1
        };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryStatementCreate>(Arg.Any<string>(), Arg.Any<SalaryStatementCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(statement);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryStatementCreate>(
                SalaryEndpoints.Statement.Create, statement, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var statement = new SalaryStatementUpdate
        {
            Id = 1,
            Date = "2024-01-15",
            DatePayment = "2024-01-25",
            PersonId = 1,
            StatusId = 1,
            TemplateId = 1
        };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryStatementUpdate>(Arg.Any<string>(), Arg.Any<SalaryStatementUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(statement);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryStatementUpdate>(
                SalaryEndpoints.Statement.Update, statement, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateMultiple_ShouldPostToCorrectEndpoint()
    {
        var statements = new SalaryStatementUpdateMultiple { Ids = "1,2,3" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryStatementUpdateMultiple>(Arg.Any<string>(), Arg.Any<SalaryStatementUpdateMultiple>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateMultiple(statements);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryStatementUpdateMultiple>(
                SalaryEndpoints.Statement.UpdateMultiple, statements, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateStatus_ShouldPostToCorrectEndpoint()
    {
        var status = new SalaryStatementStatusUpdate { Ids = "1,2", StatusId = 5 };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryStatementStatusUpdate>(Arg.Any<string>(), Arg.Any<SalaryStatementStatusUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateStatus(status);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryStatementStatusUpdate>(
                SalaryEndpoints.Statement.UpdateStatus, status, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRecurrence_ShouldPostToCorrectEndpoint()
    {
        var recurrence = new SalaryStatementRecurrenceUpdate { Id = 1, Recurrence = "MONTHLY" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryStatementRecurrenceUpdate>(Arg.Any<string>(), Arg.Any<SalaryStatementRecurrenceUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.UpdateRecurrence(recurrence);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryStatementRecurrenceUpdate>(
                SalaryEndpoints.Statement.UpdateRecurrence, recurrence, Arg.Any<CancellationToken>());
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
                SalaryEndpoints.Statement.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Calculate_ShouldPostToCorrectEndpoint()
    {
        var calculation = new SalaryStatementCalculate { Id = 1, PersonId = 1, TemplateId = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryStatementCalculate>(Arg.Any<string>(), Arg.Any<SalaryStatementCalculate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Calculate(calculation);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryStatementCalculate>(
                SalaryEndpoints.Statement.Calculate, calculation, Arg.Any<CancellationToken>());
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
                SalaryEndpoints.Statement.UpdateAttachments, attachments, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(SalaryEndpoints.Statement.ListXlsx, Arg.Any<CancellationToken>());
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
            .GetBinaryAsync(SalaryEndpoints.Statement.ListCsv, Arg.Any<CancellationToken>());
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
            .GetBinaryAsync(SalaryEndpoints.Statement.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<SalaryStatement>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryStatement>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryStatement>>(
                SalaryEndpoints.Statement.List, listParams, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<SalaryStatement>>();
        ConnectionHandler
            .GetAsync<ListResponse<SalaryStatement>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }
}
