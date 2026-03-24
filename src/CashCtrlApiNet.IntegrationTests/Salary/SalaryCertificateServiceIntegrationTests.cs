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
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Salary;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryCertificateService"/>
/// </summary>
public class SalaryCertificateServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary certificate with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var certificate = SalaryFakers.Certificate.Generate();
        Server.StubGetJson("/api/v1/salary/certificate/read.json",
            CashCtrlResponseFactory.SingleResponse(certificate));

        // Act
        var result = await Client.Salary.Certificate.Get(new Entry { Id = certificate.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(certificate.Id);
        result.ResponseData.Data.Notes.ShouldBe(certificate.Notes);
    }

    /// <summary>
    /// Verify GetList returns a list of salary certificates
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var certificates = SalaryFakers.Certificate.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/certificate/list.json",
            CashCtrlResponseFactory.ListResponse(certificates));

        // Act
        var result = await Client.Salary.Certificate.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var certificateUpdate = SalaryFakers.CertificateUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/certificate/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary certificate updated"));

        // Act
        var result = await Client.Salary.Certificate.Update(certificateUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary certificate updated");
    }

    /// <summary>
    /// Verify ExportExcel returns binary Excel data
    /// </summary>
    [Fact]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/certificate/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "certificates.xlsx");

        // Act
        var result = await Client.Salary.Certificate.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify ExportCsv returns binary CSV data
    /// </summary>
    [Fact]
    public async Task ExportCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,notes\n1,Test certificate"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/certificate/list.csv", csvBytes,
            "text/csv", "certificates.csv");

        // Act
        var result = await Client.Salary.Certificate.ExportCsv();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify ExportPdf returns binary PDF data
    /// </summary>
    [Fact]
    public async Task ExportPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/certificate/list.pdf", pdfBytes,
            "application/pdf", "certificates.pdf");

        // Act
        var result = await Client.Salary.Certificate.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
