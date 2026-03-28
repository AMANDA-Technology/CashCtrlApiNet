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

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace CashCtrlApiNet.IntegrationTests.Helpers;

/// <summary>
/// Extension methods for WireMock server to simplify stubbing CashCtrl API responses
/// </summary>
public static class WireMockExtensions
{
    /// <summary>
    /// Stub a GET endpoint to return a JSON response
    /// </summary>
    /// <param name="server">The WireMock server</param>
    /// <param name="path">The API path to match (e.g., "/api/v1/inventory/article/read.json")</param>
    /// <param name="jsonBody">The JSON response body</param>
    /// <param name="statusCode">HTTP status code (default 200)</param>
    public static void StubGetJson(this WireMockServer server, string path, string jsonBody, int statusCode = 200)
    {
        server
            .Given(Request.Create().WithPath(path).UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(statusCode)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(jsonBody));
    }

    /// <summary>
    /// Stub a POST endpoint to return a JSON response
    /// </summary>
    /// <param name="server">The WireMock server</param>
    /// <param name="path">The API path to match</param>
    /// <param name="jsonBody">The JSON response body</param>
    /// <param name="statusCode">HTTP status code (default 200)</param>
    public static void StubPostJson(this WireMockServer server, string path, string jsonBody, int statusCode = 200)
    {
        server
            .Given(Request.Create().WithPath(path).UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(statusCode)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(jsonBody));
    }

    /// <summary>
    /// Stub a GET endpoint to return a plain text response (e.g., balance values)
    /// </summary>
    /// <param name="server">The WireMock server</param>
    /// <param name="path">The API path to match (e.g., "/api/v1/account/balance")</param>
    /// <param name="body">The plain text response body</param>
    /// <param name="statusCode">HTTP status code (default 200)</param>
    public static void StubGetPlainText(this WireMockServer server, string path, string body, int statusCode = 200)
    {
        server
            .Given(Request.Create().WithPath(path).UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(statusCode)
                .WithHeader("Content-Type", "text/plain")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(body));
    }

    /// <summary>
    /// Stub a GET endpoint to return a binary response
    /// </summary>
    /// <param name="server">The WireMock server</param>
    /// <param name="path">The API path to match</param>
    /// <param name="body">The binary response body</param>
    /// <param name="contentType">The content type (default "application/pdf")</param>
    /// <param name="fileName">The file name for content-disposition header</param>
    public static void StubGetBinary(this WireMockServer server, string path, byte[] body, string contentType = "application/pdf", string? fileName = null)
    {
        var response = Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", contentType)
            .WithHeader("X-CashCtrl-Requests-Left", "100")
            .WithBody(body);

        if (fileName is not null)
            response = response.WithHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");

        server
            .Given(Request.Create().WithPath(path).UsingGet())
            .RespondWith(response);
    }
}
