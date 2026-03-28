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
using CashCtrlApiNet.Abstractions.Models.Order.Document;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Order;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Order.DocumentService"/>
/// </summary>
public class DocumentServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single document with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var document = OrderFakers.Document.Generate();
        Server.StubGetJson("/api/v1/order/document/read.json",
            CashCtrlResponseFactory.SingleResponse(document));

        // Act
        var result = await Client.Order.Document.Get(new Entry { Id = document.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(document.Id);
        result.ResponseData.Data.Text.ShouldBe(document.Text);
    }

    /// <summary>
    /// Verify DownloadPdf returns binary PDF data
    /// </summary>
    [Test]
    public async Task DownloadPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/order/document/read.pdf", pdfBytes,
            "application/pdf", "document.pdf");

        // Act
        var result = await Client.Order.Document.DownloadPdf(new Entry { Id = 1 });

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
        Server.StubGetBinary("/api/v1/order/document/read.zip", zipBytes,
            "application/zip", "document.zip");

        // Act
        var result = await Client.Order.Document.DownloadZip(new Entry { Id = 1 });

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
        Server.StubPostJson("/api/v1/order/document/mail.json",
            CashCtrlResponseFactory.SuccessResponse("Mail sent"));

        // Act
        var result = await Client.Order.Document.SendMail(new DocumentMail
        {
            Id = 1,
            RecipientEmail = "test@example.com",
            Subject = "Invoice",
            Message = "Please find attached your invoice."
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
        Server.StubPostJson("/api/v1/order/document/update.json",
            CashCtrlResponseFactory.SuccessResponse("Document updated"));

        // Act
        var result = await Client.Order.Document.Update(new DocumentUpdate
        {
            Id = 1,
            Text = "Updated document text"
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Document updated");
    }
}
