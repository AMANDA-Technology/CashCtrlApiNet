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

using CashCtrlApiNet.Abstractions.Models.Order.Document;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Order;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Order;

/// <summary>
/// Unit tests for <see cref="DocumentService"/>
/// </summary>
public class DocumentServiceTests : ServiceTestBase<DocumentService>
{
    /// <inheritdoc />
    protected override DocumentService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<Document>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Document>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Document>, Entry>(
                OrderEndpoints.Document.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DownloadPdf_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadPdf(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(OrderEndpoints.Document.ReadPdf, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task DownloadZip_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.DownloadZip(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(OrderEndpoints.Document.ReadZip, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task SendMail_ShouldPostToCorrectEndpoint()
    {
        var mail = new DocumentMail { Id = 1, RecipientEmail = "test@example.com" };
        ConnectionHandler
            .PostAsync<NoContentResponse, DocumentMail>(Arg.Any<string>(), Arg.Any<DocumentMail>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.SendMail(mail);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, DocumentMail>(
                OrderEndpoints.Document.SendMail, mail, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var document = new DocumentUpdate { Id = 1, Text = "Updated text" };
        ConnectionHandler
            .PostAsync<NoContentResponse, DocumentUpdate>(Arg.Any<string>(), Arg.Any<DocumentUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(document);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, DocumentUpdate>(
                OrderEndpoints.Document.Update, document, Arg.Any<CancellationToken>());
    }
}
