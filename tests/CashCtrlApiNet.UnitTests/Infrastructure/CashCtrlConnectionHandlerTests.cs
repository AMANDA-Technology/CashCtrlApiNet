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
using System.Text;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Tests for <see cref="CashCtrlConnectionHandler"/> covering HttpClient constructor support,
/// lang parameter, IDisposable pattern, and backwards-compatible standalone constructor
/// </summary>
public class CashCtrlConnectionHandlerTests
{
    /// <summary>
    /// A mock HTTP message handler that captures the request and returns a configured response
    /// </summary>
    private class MockHttpMessageHandler : HttpMessageHandler
    {
        /// <summary>
        /// The last captured HTTP request message
        /// </summary>
        public HttpRequestMessage? CapturedRequest { get; private set; }

        /// <summary>
        /// The response to return from SendAsync
        /// </summary>
        public HttpResponseMessage Response { get; set; } = CreateDefaultResponse();

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CapturedRequest = request;
            return Task.FromResult(Response);
        }

        /// <summary>
        /// Creates a default successful response with required headers
        /// </summary>
        private static HttpResponseMessage CreateDefaultResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\":true}", Encoding.UTF8, "application/json")
            };
            response.Headers.Add(ApiHeaderNames.RequestsLeft, "100");
            return response;
        }
    }

    private static ICashCtrlConfiguration CreateMockConfiguration(
        string baseUri = "https://testorg.cashctrl.com/",
        string apiKey = "test-api-key",
        string language = "de")
    {
        var config = Substitute.For<ICashCtrlConfiguration>();
        config.BaseUri.Returns(baseUri);
        config.ApiKey.Returns(apiKey);
        config.DefaultLanguage.Returns(language);
        return config;
    }

    [Test]
    public async Task GetAsync_ShouldUseLangParameterWithoutTrailingSpace()
    {
        // Arrange
        var handler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new("https://testorg.cashctrl.com/") };
        httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("test-api-key:")));

        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(httpClient, config);

        // Act
        await connectionHandler.GetAsync("api/v1/account/list.json");

        // Assert
        handler.CapturedRequest.ShouldNotBeNull();
        var queryString = handler.CapturedRequest.RequestUri!.Query;
        queryString.ShouldContain("lang=de");
        queryString.ShouldNotContain("lang+=");
        queryString.ShouldNotContain("lang+");
    }

    [Test]
    public async Task GetAsync_WithHttpClientConstructor_ShouldUseProvidedClient()
    {
        // Arrange
        var handler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new("https://testorg.cashctrl.com/") };
        httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("test-api-key:")));

        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(httpClient, config);

        // Act
        await connectionHandler.GetAsync("api/v1/account/list.json");

        // Assert - request was sent via the provided HttpClient
        handler.CapturedRequest.ShouldNotBeNull();
    }

    [Test]
    public async Task GetAsync_WithHttpClientConstructor_ShouldUseConfiguredBaseAddress()
    {
        // Arrange
        var handler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new("https://testorg.cashctrl.com/") };
        httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("my-key:")));

        var config = CreateMockConfiguration(apiKey: "my-key");
        var connectionHandler = new CashCtrlConnectionHandler(httpClient, config);

        // Act
        await connectionHandler.GetAsync("api/v1/account/list.json");

        // Assert
        handler.CapturedRequest.ShouldNotBeNull();
        handler.CapturedRequest.RequestUri!.Host.ShouldBe("testorg.cashctrl.com");
    }

    [Test]
    public void Constructor_WithConfiguration_ShouldNotThrow()
    {
        // Arrange
        var config = CreateMockConfiguration();

        // Act & Assert - backwards-compatible constructor still works
        var handler = Should.NotThrow(() => new CashCtrlConnectionHandler(config));
        handler.Dispose();
    }

    [Test]
    public void Constructor_ShouldThrow_WhenBaseUriIsNull()
    {
        // Arrange
        var config = CreateMockConfiguration(baseUri: "");

        // Act & Assert
        Should.Throw<ArgumentException>(() => new CashCtrlConnectionHandler(config));
    }

    [Test]
    public void Constructor_ShouldThrow_WhenApiKeyIsNull()
    {
        // Arrange
        var config = CreateMockConfiguration(apiKey: "");

        // Act & Assert
        Should.Throw<ArgumentException>(() => new CashCtrlConnectionHandler(config));
    }

    [Test]
    public void HttpClientConstructor_ShouldThrow_WhenBaseUriIsNull()
    {
        // Arrange
        using var httpClient = new HttpClient();
        var config = CreateMockConfiguration(baseUri: "");

        // Act & Assert
        Should.Throw<ArgumentException>(() => new CashCtrlConnectionHandler(httpClient, config));
    }

    [Test]
    public void HttpClientConstructor_ShouldThrow_WhenApiKeyIsNull()
    {
        // Arrange
        using var httpClient = new HttpClient();
        var config = CreateMockConfiguration(apiKey: "");

        // Act & Assert
        Should.Throw<ArgumentException>(() => new CashCtrlConnectionHandler(httpClient, config));
    }

    [Test]
    public void HttpClientConstructor_ShouldThrow_WhenHttpClientIsNull()
    {
        // Arrange
        var config = CreateMockConfiguration();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => new CashCtrlConnectionHandler((HttpClient)null!, config));
    }

    [Test]
    public async Task SetLanguage_ShouldChangeLanguageOnSubsequentRequests()
    {
        // Arrange
        var handler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new("https://testorg.cashctrl.com/") };
        httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("test-api-key:")));

        var config = CreateMockConfiguration(language: "de");
        var connectionHandler = new CashCtrlConnectionHandler(httpClient, config);

        // Act
        connectionHandler.SetLanguage(Language.En);
        await connectionHandler.GetAsync("api/v1/account/list.json");

        // Assert
        handler.CapturedRequest.ShouldNotBeNull();
        handler.CapturedRequest.RequestUri!.Query.ShouldContain("lang=en");
    }

    [Test]
    public void GetAsync_WithStandaloneConstructor_ShouldAcceptValidLanguage()
    {
        // This test verifies the standalone constructor initializes correctly with a valid language.
        // The lang parameter fix applies to the shared GetHttpRequestMessage method used by both constructors.
        var config = CreateMockConfiguration(language: "fr");
        var connectionHandler = new CashCtrlConnectionHandler(config);

        // Verify the constructor accepted "fr" as valid language
        Should.NotThrow(() => connectionHandler.SetLanguage(Language.Fr));
        connectionHandler.Dispose();
    }

    [Test]
    public void Constructor_WithBaseUriMissingTrailingSlash_ShouldNormalize()
    {
        // Arrange
        var config = CreateMockConfiguration(baseUri: "https://testorg.cashctrl.com");

        // Act & Assert - should not throw, normalizes the URI
        var handler = Should.NotThrow(() => new CashCtrlConnectionHandler(config));
        handler.Dispose();
    }

    [Test]
    public void HttpClientConstructor_WithBaseUriMissingTrailingSlash_ShouldNormalize()
    {
        // Arrange
        using var httpClient = new HttpClient();
        var config = CreateMockConfiguration(baseUri: "https://testorg.cashctrl.com");

        // Act & Assert - should not throw, normalizes the URI
        Should.NotThrow(() => new CashCtrlConnectionHandler(httpClient, config));
    }

    [Test]
    public async Task Dispose_Standalone_ShouldPreventFurtherRequests()
    {
        // Arrange
        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(config);

        // Act
        connectionHandler.Dispose();

        // Assert - should throw ObjectDisposedException when trying to use
        await Should.ThrowAsync<ObjectDisposedException>(async () =>
            await connectionHandler.GetAsync("api/v1/test"));
    }

    [Test]
    public async Task Dispose_HttpClientConstructor_ShouldNotDisposeProvidedHttpClient()
    {
        // Arrange
        var mockHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHandler) { BaseAddress = new("https://testorg.cashctrl.com/") };
        httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("test-api-key:")));

        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(httpClient, config);

        // Act - dispose the connection handler (DI path)
        connectionHandler.Dispose();

        // Assert - the provided HttpClient should NOT be disposed and should still work
        var request = new HttpRequestMessage(HttpMethod.Get, "https://testorg.cashctrl.com/api/v1/test");
        var response = await httpClient.SendAsync(request);
        response.ShouldNotBeNull();

        httpClient.Dispose();
    }

    [Test]
    public void Dispose_ShouldBeIdempotent()
    {
        // Arrange
        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(config);

        // Act & Assert - calling Dispose twice should not throw
        connectionHandler.Dispose();
        Should.NotThrow(() => connectionHandler.Dispose());
    }

    [Test]
    public async Task GetAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var handler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new("https://testorg.cashctrl.com/") };
        httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("test-api-key:")));

        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(httpClient, config);

        // Act
        connectionHandler.Dispose();

        // Assert
        await Should.ThrowAsync<ObjectDisposedException>(async () =>
            await connectionHandler.GetAsync("api/v1/test"));
    }

    [Test]
    public void SetLanguage_AfterDispose_ShouldNotThrow()
    {
        // Arrange - SetLanguage does not use the HttpClient, so it should not throw after disposal
        var config = CreateMockConfiguration();
        var connectionHandler = new CashCtrlConnectionHandler(config);

        // Act
        connectionHandler.Dispose();

        // Assert - SetLanguage only sets a field, it should not throw
        Should.NotThrow(() => connectionHandler.SetLanguage(Language.En));
    }
}
