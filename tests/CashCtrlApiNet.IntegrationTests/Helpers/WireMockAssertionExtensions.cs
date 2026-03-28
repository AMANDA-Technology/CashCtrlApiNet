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

using System.Web;
using Shouldly;
using WireMock;
using WireMock.Logging;
using WireMock.Server;

namespace CashCtrlApiNet.IntegrationTests.Helpers;

/// <summary>
/// Extension methods for WireMock server to assert request parameters after a call
/// </summary>
public static class WireMockAssertionExtensions
{
    /// <summary>
    /// Get the last log entry from the WireMock server
    /// </summary>
    /// <param name="server">The WireMock server</param>
    /// <returns>The last log entry</returns>
    /// <exception cref="InvalidOperationException">When no log entries exist</exception>
    public static ILogEntry GetLastLogEntry(this WireMockServer server)
    {
        server.LogEntries.ShouldNotBeEmpty("WireMock server has no log entries");
        return server.LogEntries.Last();
    }

    /// <summary>
    /// Assert that a query parameter has the expected value
    /// </summary>
    /// <param name="request">The request message to inspect</param>
    /// <param name="key">The query parameter name</param>
    /// <param name="expectedValue">The expected value</param>
    public static void ShouldHaveQueryParam(this IRequestMessage request, string key, string expectedValue)
    {
        var query = HttpUtility.ParseQueryString(new Uri(request.Url).Query);
        query[key].ShouldNotBeNull($"Query parameter '{key}' was not found in URL: {request.Url}");
        query[key].ShouldBe(expectedValue, $"Query parameter '{key}' had unexpected value");
    }

    /// <summary>
    /// Assert that a query parameter is absent from the request
    /// </summary>
    /// <param name="request">The request message to inspect</param>
    /// <param name="key">The query parameter name that should be absent</param>
    public static void ShouldNotHaveQueryParam(this IRequestMessage request, string key)
    {
        var query = HttpUtility.ParseQueryString(new Uri(request.Url).Query);
        query[key].ShouldBeNull($"Query parameter '{key}' should not be present in URL: {request.Url}");
    }

    /// <summary>
    /// Assert that a form-encoded body parameter has the expected value
    /// </summary>
    /// <param name="request">The request message to inspect</param>
    /// <param name="key">The form parameter name</param>
    /// <param name="expectedValue">The expected value</param>
    public static void ShouldHaveFormParam(this IRequestMessage request, string key, string expectedValue)
    {
        request.Body.ShouldNotBeNullOrEmpty("Request body is empty");
        var form = HttpUtility.ParseQueryString(request.Body);
        form[key].ShouldNotBeNull($"Form parameter '{key}' was not found in body: {request.Body}");
        form[key].ShouldBe(expectedValue, $"Form parameter '{key}' had unexpected value");
    }

    /// <summary>
    /// Find a log entry matching the given path and HTTP method, and return its request message
    /// </summary>
    /// <param name="server">The WireMock server</param>
    /// <param name="path">The API path to match (e.g., "/api/v1/inventory/article/read.json")</param>
    /// <param name="httpMethod">The HTTP method (e.g., "GET" or "POST")</param>
    /// <returns>The matching request message</returns>
    public static IRequestMessage ShouldHaveReceivedRequest(this WireMockServer server, string path, string httpMethod)
    {
        var matchingRequest = server.LogEntries
            .LastOrDefault(e =>
                string.Equals(e.RequestMessage?.Path, path, StringComparison.Ordinal) &&
                string.Equals(e.RequestMessage?.Method, httpMethod, StringComparison.OrdinalIgnoreCase))
            ?.RequestMessage;

        matchingRequest.ShouldNotBeNull($"No {httpMethod} request found for path '{path}'");
        return matchingRequest;
    }
}
