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
using CashCtrlApiNet.Abstractions.Models.Report.Set;
using CashCtrlApiNet.Services.Connectors.Report;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Report;

/// <summary>
/// Unit tests for <see cref="ReportSetService"/>
/// </summary>
public class ReportSetServiceTests : ServiceTestBase<ReportSetService>
{
    /// <inheritdoc />
    protected override ReportSetService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<ReportSet>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<ReportSet>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<ReportSet>, Entry>(
                ReportEndpoints.Set.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var set = new ReportSetCreate { Name = "Test" };
        ConnectionHandler
            .PostAsync<NoContentResponse, ReportSetCreate>(Arg.Any<string>(), Arg.Any<ReportSetCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(set);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, ReportSetCreate>(
                ReportEndpoints.Set.Create, set, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var set = new ReportSetUpdate { Id = 1, Name = "Test" };
        ConnectionHandler
            .PostAsync<NoContentResponse, ReportSetUpdate>(Arg.Any<string>(), Arg.Any<ReportSetUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(set);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, ReportSetUpdate>(
                ReportEndpoints.Set.Update, set, Arg.Any<CancellationToken>());
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
                ReportEndpoints.Set.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Reorder_ShouldPostToCorrectEndpoint()
    {
        var reorder = new ReportSetReorder { Ids = [1, 2], Target = 3 };
        ConnectionHandler
            .PostAsync<NoContentResponse, ReportSetReorder>(Arg.Any<string>(), Arg.Any<ReportSetReorder>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Reorder(reorder);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, ReportSetReorder>(
                ReportEndpoints.Set.Reorder, reorder, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetMeta_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<ReportSet>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<ReportSet>>());

        await Service.GetMeta(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<ReportSet>, Entry>(
                ReportEndpoints.Set.ReadMeta, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DownloadPdf_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.DownloadPdf(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(ReportEndpoints.Set.DownloadPdf, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task DownloadCsv_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.DownloadCsv(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(ReportEndpoints.Set.DownloadCsv, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task DownloadExcel_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.DownloadExcel(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(ReportEndpoints.Set.DownloadXlsx, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task DownloadAnnualReport_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new() { Data = [1, 2, 3] } });

        var result = await Service.DownloadAnnualReport(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(ReportEndpoints.Set.DownloadAnnualReport, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
