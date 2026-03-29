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

using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Salary;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryDocumentService"/>
/// </summary>
public class SalaryDocumentServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary document with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var document = SalaryFakers.Document.Generate();
        Server.StubGetJson("/api/v1/salary/document/read.json",
            CashCtrlResponseFactory.SingleResponse(document));

        // Act
        var result = await Client.Salary.Document.Get(new() { Id = document.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(document.Id);
        result.ResponseData.Data.Header.ShouldBe(document.Header);
    }

    /// <summary>
    /// Verify DownloadPdf returns binary PDF data
    /// </summary>
    [Test]
    public async Task DownloadPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/document/read.pdf", pdfBytes,
            "application/pdf", "salary-document.pdf");

        // Act
        var result = await Client.Salary.Document.DownloadPdf(new() { Ids = [1] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }

    /// <summary>
    /// Verify DownloadZip returns binary ZIP data
    /// </summary>
    [Test]
    public async Task DownloadZip_ReturnsExpectedResult()
    {
        // Arrange
        var zipBytes = "fake-zip-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/document/read.zip", zipBytes,
            "application/zip", "salary-documents.zip");

        // Act
        var result = await Client.Salary.Document.DownloadZip(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(zipBytes);
    }

    /// <summary>
    /// Verify SendMail sends correct request and returns success
    /// </summary>
    [Test]
    public async Task SendMail_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/document/mail.json",
            CashCtrlResponseFactory.SuccessResponse("Mail sent"));

        // Act
        var result = await Client.Salary.Document.SendMail(new()
        {
            StatementIds = "1,2",
            MailFrom = "sender@example.com",
            MailSubject = "Salary Document",
            MailText = "Please find attached your salary document."
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Mail sent");
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var documentUpdate = SalaryFakers.DocumentUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/document/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary document updated"));

        // Act
        var result = await Client.Salary.Document.Update(documentUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary document updated");
    }
}
