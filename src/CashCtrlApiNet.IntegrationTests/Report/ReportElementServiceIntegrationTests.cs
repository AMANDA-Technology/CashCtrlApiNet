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

using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Report.Element;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Report;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Report.ReportElementService"/>
/// </summary>
public class ReportElementServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single report element with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var element = ReportFakers.ReportElement.Generate();
        Server.StubGetJson("/api/v1/report/element/read.json",
            CashCtrlResponseFactory.SingleResponse(element));

        // Act
        var result = await Client.Report.Element.Get(new Entry { Id = element.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(element.Id);
        result.ResponseData.Data.ReportId.ShouldBe(element.ReportId);
        result.ResponseData.Data.AccountId.ShouldBe(element.AccountId);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var elementCreate = ReportFakers.ReportElementCreate.Generate();
        Server.StubPostJson("/api/v1/report/element/create.json",
            CashCtrlResponseFactory.SuccessResponse("Report element created", insertId: 42));

        // Act
        var result = await Client.Report.Element.Create(elementCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report element created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var elementUpdate = ReportFakers.ReportElementUpdate.Generate();
        Server.StubPostJson("/api/v1/report/element/update.json",
            CashCtrlResponseFactory.SuccessResponse("Report element updated"));

        // Act
        var result = await Client.Report.Element.Update(elementUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report element updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/report/element/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Report element deleted"));

        // Act
        var result = await Client.Report.Element.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report element deleted");
    }

    /// <summary>
    /// Verify Reorder sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Reorder_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/report/element/reorder.json",
            CashCtrlResponseFactory.SuccessResponse("Report elements reordered"));

        // Act
        var result = await Client.Report.Element.Reorder(new ReportElementReorder
        {
            Ids = [1, 2, 3],
            Target = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report elements reordered");
    }

    /// <summary>
    /// Verify GetData returns a single report element with data
    /// </summary>
    [Fact]
    public async Task GetData_ReturnsExpectedResult()
    {
        // Arrange
        var element = ReportFakers.ReportElement.Generate();
        Server.StubGetJson("/api/v1/report/element/data.json",
            CashCtrlResponseFactory.SingleResponse(element));

        // Act
        var result = await Client.Report.Element.GetData(new Entry { Id = element.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(element.Id);
    }

    /// <summary>
    /// Verify GetDataHtml returns binary HTML data
    /// </summary>
    [Fact]
    public async Task GetDataHtml_ReturnsExpectedResult()
    {
        // Arrange
        var htmlBytes = "<html><body><h1>Report</h1></body></html>"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/element/data.html", htmlBytes,
            "text/html", "report.html");

        // Act
        var result = await Client.Report.Element.GetDataHtml(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(htmlBytes);
    }

    /// <summary>
    /// Verify GetMeta returns a single report element with metadata
    /// </summary>
    [Fact]
    public async Task GetMeta_ReturnsExpectedResult()
    {
        // Arrange
        var element = ReportFakers.ReportElement.Generate();
        Server.StubGetJson("/api/v1/report/element/meta.json",
            CashCtrlResponseFactory.SingleResponse(element));

        // Act
        var result = await Client.Report.Element.GetMeta(new Entry { Id = element.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(element.Id);
    }

    /// <summary>
    /// Verify DownloadPdf returns binary PDF data
    /// </summary>
    [Fact]
    public async Task DownloadPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/element/download.pdf", pdfBytes,
            "application/pdf", "report-element.pdf");

        // Act
        var result = await Client.Report.Element.DownloadPdf(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }

    /// <summary>
    /// Verify DownloadCsv returns binary CSV data
    /// </summary>
    [Fact]
    public async Task DownloadCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,reportId,accountId\n1,10,20"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/element/download.csv", csvBytes,
            "text/csv", "report-element.csv");

        // Act
        var result = await Client.Report.Element.DownloadCsv(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify DownloadExcel returns binary Excel data
    /// </summary>
    [Fact]
    public async Task DownloadExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/element/download.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report-element.xlsx");

        // Act
        var result = await Client.Report.Element.DownloadExcel(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }
}
