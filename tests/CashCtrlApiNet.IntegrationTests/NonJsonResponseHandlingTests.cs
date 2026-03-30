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

using System.Net;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.IntegrationTests.Fakers;
using Shouldly;
using CashCtrlApiNet.IntegrationTests.Helpers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Integration tests for handling non-JSON API responses (rate limits, HTML error pages)
/// </summary>
public class NonJsonResponseHandlingTests : IntegrationTestBase
{
    /// <summary>
    /// Verify that a rate limit plain text response on a typed GET endpoint does not throw
    /// </summary>
    [Test]
    public async Task GetTyped_RateLimitPlainText_ReturnsApiResultWithNullResponseData()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(429)
                .WithHeader("Content-Type", "text/plain")
                .WithHeader("X-CashCtrl-Requests-Left", "0")
                .WithBody("Calls exceeded. Please retry in a moment."));

        // Act
        var result = await Client.Inventory.Article.Get(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe((HttpStatusCode)429);
        result.ResponseData.ShouldBeNull();
        result.RawResponseContent.ShouldBe("Calls exceeded. Please retry in a moment.");
        result.RequestsLeft.ShouldBe(0);
    }

    /// <summary>
    /// Verify that an HTML error page response on a typed GET endpoint does not throw
    /// </summary>
    [Test]
    public async Task GetTyped_HtmlErrorPage_ReturnsApiResultWithNullResponseData()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(502)
                .WithHeader("Content-Type", "text/html")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody("<html><body><h1>502 Bad Gateway</h1></body></html>"));

        // Act
        var result = await Client.Inventory.Article.Get(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe((HttpStatusCode)502);
        result.ResponseData.ShouldBeNull();
        result.RawResponseContent.ShouldBe("<html><body><h1>502 Bad Gateway</h1></body></html>");
    }

    /// <summary>
    /// Verify that a plain text response on a typed POST endpoint does not throw
    /// </summary>
    [Test]
    public async Task PostTyped_PlainTextResponse_ReturnsApiResultWithNullResponseData()
    {
        // Arrange
        var articleCreate = InventoryFakers.ArticleCreate.Generate();
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/create.json").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(503)
                .WithHeader("Content-Type", "text/plain")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody("Service Temporarily Unavailable"));

        // Act
        var result = await Client.Inventory.Article.Create(articleCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        result.ResponseData.ShouldBeNull();
        result.RawResponseContent.ShouldBe("Service Temporarily Unavailable");
    }

    /// <summary>
    /// Verify that a 200 response with non-JSON body does not throw and sets RawResponseContent
    /// </summary>
    [Test]
    public async Task GetTyped_Success200NonJson_ReturnsApiResultWithNullResponseData()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "text/plain")
                .WithHeader("X-CashCtrl-Requests-Left", "50")
                .WithBody("Unexpected plain text on success"));

        // Act
        var result = await Client.Inventory.Article.Get(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldBeNull();
        result.RawResponseContent.ShouldBe("Unexpected plain text on success");
    }

    /// <summary>
    /// Verify that an empty response body on a typed endpoint does not throw
    /// </summary>
    [Test]
    public async Task GetTyped_EmptyBody_ReturnsApiResultWithNullResponseData()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(500)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(""));

        // Act
        var result = await Client.Inventory.Article.Get(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        result.ResponseData.ShouldBeNull();
        result.RawResponseContent.ShouldBe("");
    }

    /// <summary>
    /// Verify that a valid JSON response still deserializes normally (no regression)
    /// </summary>
    [Test]
    public async Task GetTyped_ValidJson_DeserializesNormally()
    {
        // Arrange
        var article = InventoryFakers.Article.Generate();
        Server.StubGetJson("/api/v1/inventory/article/read.json",
            CashCtrlResponseFactory.SingleResponse(article));

        // Act
        var result = await Client.Inventory.Article.Get(new() { Id = article.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.RawResponseContent.ShouldBeNull();
    }

    /// <summary>
    /// Verify that the non-generic GetAsync also handles non-JSON gracefully
    /// </summary>
    [Test]
    public async Task GetNonGeneric_NonJsonResponse_ReturnsApiResultWithRawContent()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(429)
                .WithHeader("Content-Type", "text/plain")
                .WithHeader("X-CashCtrl-Requests-Left", "0")
                .WithBody("Rate limited"));

        // Act
        var result = await ConnectionHandler.GetAsync("api/v1/inventory/article/read.json");

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe((HttpStatusCode)429);
    }
}
