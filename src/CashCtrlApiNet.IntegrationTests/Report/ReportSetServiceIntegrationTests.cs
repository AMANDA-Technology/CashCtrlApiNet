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
using CashCtrlApiNet.Abstractions.Models.Report.Set;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Report;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Report.ReportSetService"/>
/// </summary>
public class ReportSetServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single report set with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var reportSet = ReportFakers.ReportSet.Generate();
        Server.StubGetJson("/api/v1/report/collection/read.json",
            CashCtrlResponseFactory.SingleResponse(reportSet));

        // Act
        var result = await Client.Report.Set.Get(new Entry { Id = reportSet.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(reportSet.Id);
        result.ResponseData.Data.Name.ShouldBe(reportSet.Name);
        result.ResponseData.Data.Text.ShouldBe(reportSet.Text);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var reportSetCreate = ReportFakers.ReportSetCreate.Generate();
        Server.StubPostJson("/api/v1/report/collection/create.json",
            CashCtrlResponseFactory.SuccessResponse("Report set created", insertId: 42));

        // Act
        var result = await Client.Report.Set.Create(reportSetCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report set created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var reportSetUpdate = ReportFakers.ReportSetUpdate.Generate();
        Server.StubPostJson("/api/v1/report/collection/update.json",
            CashCtrlResponseFactory.SuccessResponse("Report set updated"));

        // Act
        var result = await Client.Report.Set.Update(reportSetUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report set updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/report/collection/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Report set deleted"));

        // Act
        var result = await Client.Report.Set.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report set deleted");
    }

    /// <summary>
    /// Verify Reorder sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Reorder_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/report/collection/reorder.json",
            CashCtrlResponseFactory.SuccessResponse("Report sets reordered"));

        // Act
        var result = await Client.Report.Set.Reorder(new ReportSetReorder
        {
            Ids = [1, 2, 3],
            Target = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Report sets reordered");
    }

    /// <summary>
    /// Verify GetMeta returns a single report set with metadata
    /// </summary>
    [Test]
    public async Task GetMeta_ReturnsExpectedResult()
    {
        // Arrange
        var reportSet = ReportFakers.ReportSet.Generate();
        Server.StubGetJson("/api/v1/report/collection/meta.json",
            CashCtrlResponseFactory.SingleResponse(reportSet));

        // Act
        var result = await Client.Report.Set.GetMeta(new Entry { Id = reportSet.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(reportSet.Id);
    }

    /// <summary>
    /// Verify DownloadPdf returns binary PDF data
    /// </summary>
    [Test]
    public async Task DownloadPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/collection/download.pdf", pdfBytes,
            "application/pdf", "report-set.pdf");

        // Act
        var result = await Client.Report.Set.DownloadPdf(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }

    /// <summary>
    /// Verify DownloadCsv returns binary CSV data
    /// </summary>
    [Test]
    public async Task DownloadCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,name,text\n1,Report Set,Description"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/collection/download.csv", csvBytes,
            "text/csv", "report-set.csv");

        // Act
        var result = await Client.Report.Set.DownloadCsv(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify DownloadExcel returns binary Excel data
    /// </summary>
    [Test]
    public async Task DownloadExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/collection/download.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report-set.xlsx");

        // Act
        var result = await Client.Report.Set.DownloadExcel(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify DownloadAnnualReport returns binary PDF data
    /// </summary>
    [Test]
    public async Task DownloadAnnualReport_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-annual-report-pdf"u8.ToArray();
        Server.StubGetBinary("/api/v1/report/collection/download_annualreport.pdf", pdfBytes,
            "application/pdf", "annual-report.pdf");

        // Act
        var result = await Client.Report.Set.DownloadAnnualReport(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
