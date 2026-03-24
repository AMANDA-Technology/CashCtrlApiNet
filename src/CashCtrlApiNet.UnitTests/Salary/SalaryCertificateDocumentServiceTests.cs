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
using CashCtrlApiNet.Abstractions.Models.Salary.CertificateDocument;
using CashCtrlApiNet.Services.Connectors.Salary;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Salary;

/// <summary>
/// Unit tests for <see cref="SalaryCertificateDocumentService"/>
/// </summary>
public class SalaryCertificateDocumentServiceTests : ServiceTestBase<SalaryCertificateDocumentService>
{
    /// <inheritdoc />
    protected override SalaryCertificateDocumentService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<SalaryCertificateDocument>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<SalaryCertificateDocument>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<SalaryCertificateDocument>, Entry>(
                SalaryEndpoints.CertificateDocument.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DownloadPdf_ShouldCallGetBinaryAsync_WithEntriesParameter()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadPdf(entries);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(SalaryEndpoints.CertificateDocument.ReadPdf, entries, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task DownloadZip_ShouldCallGetBinaryAsync_WithEntriesParameter()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .GetBinaryAsync(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadZip(entries);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync(SalaryEndpoints.CertificateDocument.ReadZip, entries, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task SendMail_ShouldPostToCorrectEndpoint()
    {
        var mail = new SalaryCertificateDocumentMail
        {
            CertificateIds = "1,2",
            MailFrom = "test@example.com",
            MailSubject = "Test",
            MailText = "Test body"
        };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryCertificateDocumentMail>(Arg.Any<string>(), Arg.Any<SalaryCertificateDocumentMail>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.SendMail(mail);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryCertificateDocumentMail>(
                SalaryEndpoints.CertificateDocument.Mail, mail, Arg.Any<CancellationToken>());
    }
}
