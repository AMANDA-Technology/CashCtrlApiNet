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
using CashCtrlApiNet.Abstractions.Models.Salary.Certificate;
using CashCtrlApiNet.Services.Connectors.Salary;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Salary;

/// <summary>
/// Unit tests for <see cref="SalaryCertificateService"/>
/// </summary>
public class SalaryCertificateServiceTests : ServiceTestBase<SalaryCertificateService>
{
    /// <inheritdoc />
    protected override SalaryCertificateService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<SalaryCertificate>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<SalaryCertificate>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<SalaryCertificate>, Entry>(
                SalaryEndpoints.Certificate.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<SalaryCertificate>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryCertificate>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryCertificate>>(
                SalaryEndpoints.Certificate.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var certificate = new SalaryCertificateUpdate { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryCertificateUpdate>(Arg.Any<string>(), Arg.Any<SalaryCertificateUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(certificate);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryCertificateUpdate>(
                SalaryEndpoints.Certificate.Update, certificate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExportExcel_ShouldCallGetBinaryAsync()
    {
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.ExportExcel();

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(SalaryEndpoints.Certificate.ListXlsx, Arg.Any<CancellationToken>());
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
            .GetBinaryAsync(SalaryEndpoints.Certificate.ListCsv, Arg.Any<CancellationToken>());
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
            .GetBinaryAsync(SalaryEndpoints.Certificate.ListPdf, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<SalaryCertificate>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryCertificate>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryCertificate>>(
                SalaryEndpoints.Certificate.List, listParams, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<SalaryCertificate>>();
        ConnectionHandler
            .GetAsync<ListResponse<SalaryCertificate>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }
}
