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

using System.Collections.Immutable;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Report.Element;
using CashCtrlApiNet.Services.Connectors.Report;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Report;

/// <summary>
/// Unit tests for <see cref="ReportElementService"/>
/// </summary>
public class ReportElementServiceTests : ServiceTestBase<ReportElementService>
{
    /// <inheritdoc />
    protected override ReportElementService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<ReportElement>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<ReportElement>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<ReportElement>, Entry>(
                ReportEndpoints.Element.Read, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var element = new ReportElementCreate { ReportId = 1, AccountId = 2 };
        ConnectionHandler
            .PostAsync<NoContentResponse, ReportElementCreate>(Arg.Any<string>(), Arg.Any<ReportElementCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(element);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, ReportElementCreate>(
                ReportEndpoints.Element.Create, element, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var element = new ReportElementUpdate { Id = 1, ReportId = 1, AccountId = 2 };
        ConnectionHandler
            .PostAsync<NoContentResponse, ReportElementUpdate>(Arg.Any<string>(), Arg.Any<ReportElementUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(element);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, ReportElementUpdate>(
                ReportEndpoints.Element.Update, element, Arg.Any<CancellationToken>());
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
                ReportEndpoints.Element.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Reorder_ShouldPostToCorrectEndpoint()
    {
        var reorder = new ReportElementReorder { Ids = [1, 2], Target = 3 };
        ConnectionHandler
            .PostAsync<NoContentResponse, ReportElementReorder>(Arg.Any<string>(), Arg.Any<ReportElementReorder>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Reorder(reorder);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, ReportElementReorder>(
                ReportEndpoints.Element.Reorder, reorder, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetData_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<ReportElement>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<ReportElement>>());

        await Service.GetData(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<ReportElement>, Entry>(
                ReportEndpoints.Element.ReadJson, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetDataHtml_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.GetDataHtml(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(ReportEndpoints.Element.ReadHtml, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetMeta_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<ReportElement>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<ReportElement>>());

        await Service.GetMeta(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<ReportElement>, Entry>(
                ReportEndpoints.Element.ReadMeta, entry, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DownloadPdf_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadPdf(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(ReportEndpoints.Element.DownloadPdf, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task DownloadCsv_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadCsv(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(ReportEndpoints.Element.DownloadCsv, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task DownloadExcel_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadExcel(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(ReportEndpoints.Element.DownloadXlsx, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
