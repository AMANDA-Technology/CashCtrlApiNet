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
using System.Web;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Abstractions.Helpers;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Api.Base;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Values;
using CashCtrlApiNet.Interfaces;

namespace CashCtrlApiNet.Services;

/// <inheritdoc />
public class CashCtrlConnectionHandler : ICashCtrlConnectionHandler, IDisposable
{
    /// <summary>
    /// Language to use on all requests
    /// </summary>
    private Language _language;

    /// <summary>
    /// HTTP client for all API requests
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Base address for all API requests
    /// </summary>
    private readonly Uri _baseAddress;

    /// <summary>
    /// Whether this instance owns (and should dispose) the <see cref="_httpClient"/>
    /// </summary>
    private readonly bool _ownsHttpClient;

    /// <summary>
    /// Whether this instance has been disposed
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CashCtrlConnectionHandler"/> class.
    /// This constructor creates a standalone <see cref="HttpClient"/> for non-DI environments.
    /// The created client is owned by this instance and will be disposed when <see cref="Dispose()"/> is called.
    /// </summary>
    /// <param name="configuration">The CashCtrl API configuration</param>
    public CashCtrlConnectionHandler(ICashCtrlConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.BaseUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ApiKey);

        // Configure
        _language = CashCtrlSerialization.TryDeserializeEnum<Language>(configuration.DefaultLanguage, out var language)
            ? language
            : Language.De;

        // Normalize base urls
        var baseUri = configuration.BaseUri;
        if (!baseUri.EndsWith('/'))
            baseUri += '/';

        _baseAddress = new(baseUri);
        _ownsHttpClient = true;

        // Create a new http client for instance
        _httpClient = new(new HttpClientHandler { AllowAutoRedirect = false })
        {
            BaseAddress = _baseAddress
        };

        _httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{configuration.ApiKey}:")));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CashCtrlConnectionHandler"/> class.
    /// This constructor accepts a pre-configured <see cref="HttpClient"/> from typed HttpClient registration in DI environments.
    /// The provided client is NOT owned by this instance and will not be disposed.
    /// </summary>
    /// <param name="httpClient">The pre-configured HTTP client from DI typed registration</param>
    /// <param name="configuration">The CashCtrl API configuration</param>
    public CashCtrlConnectionHandler(HttpClient httpClient, ICashCtrlConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.BaseUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ApiKey);

        // Configure
        _language = CashCtrlSerialization.TryDeserializeEnum<Language>(configuration.DefaultLanguage, out var language)
            ? language
            : Language.De;

        // Normalize base urls
        var baseUri = configuration.BaseUri;
        if (!baseUri.EndsWith('/'))
            baseUri += '/';

        _baseAddress = new(baseUri);
        _ownsHttpClient = false;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets the HTTP client, throwing if this instance has been disposed
    /// </summary>
    /// <returns>The configured <see cref="HttpClient"/></returns>
    /// <exception cref="ObjectDisposedException">Thrown when this instance has been disposed</exception>
    private HttpClient GetHttpClient()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _httpClient;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="CashCtrlConnectionHandler"/> and optionally releases the managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing && _ownsHttpClient)
            _httpClient.Dispose();

        _disposed = true;
    }

    /// <inheritdoc />
    public void SetLanguage(Language language)
        => _language = language;

    /// <inheritdoc />
    public async Task<ApiResult> GetAsync(string requestPath, CancellationToken cancellationToken = default)
        => await GetApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage<object>(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult> GetAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default)
        => await GetApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, queryParameters), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> GetAsync<TResult>(string requestPath, CancellationToken cancellationToken = default) where TResult : ApiResponse
        => await GetApiResult<TResult>(await GetHttpClient().SendAsync(GetHttpRequestMessage<object>(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> GetAsync<TResult>(string requestPath, ListParams? listParams, CancellationToken cancellationToken = default) where TResult : ApiResponse
        => await GetApiResult<TResult>(await GetHttpClient().SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, listParams), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> GetAsync<TResult, TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default) where TResult : ApiResponse
        => await GetApiResult<TResult>(await GetHttpClient().SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, queryParameters), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> PostAsync<TResult, TPost>(string requestPath, TPost payload, CancellationToken cancellationToken = default) where TResult : ApiResponse
        => await GetApiResult<TResult>(await GetHttpClient().SendAsync(GetHttpRequestMessageWithFormData(HttpMethod.Post, requestPath, payload), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<DecimalResponse>> GetBalanceAsync(string requestPath, CancellationToken cancellationToken = default)
        => await GetBalanceApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage<object>(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<DecimalResponse>> GetBalanceAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default)
        => await GetBalanceApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, queryParameters), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<PlainTextResponse>> GetPlainTextAsync(string requestPath, CancellationToken cancellationToken = default)
        => await GetPlainTextApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage<object>(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<PlainTextResponse>> GetPlainTextAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default)
        => await GetPlainTextApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, queryParameters), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<BinaryResponse>> GetBinaryAsync(string requestPath, CancellationToken cancellationToken = default)
        => await GetBinaryApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage<object>(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<BinaryResponse>> GetBinaryAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default)
        => await GetBinaryApiResult(await GetHttpClient().SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath, queryParameters), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> PostMultipartAsync<TResult>(string requestPath, MultipartFormDataContent content, CancellationToken cancellationToken = default) where TResult : ApiResponse
    {
        var httpRequestMessage = GetHttpRequestMessage<object>(HttpMethod.Post, requestPath);
        httpRequestMessage.Content = content;
        return await GetApiResult<TResult>(await GetHttpClient().SendAsync(httpRequestMessage, cancellationToken));
    }

    /// <summary>
    /// Get http request message to send to the API (with body as json from data with type TForms)
    /// </summary>
    /// <param name="httpMethod"></param>
    /// <param name="requestPath"></param>
    /// <param name="payload"></param>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    private HttpRequestMessage GetHttpRequestMessageWithFormData<TForms>(HttpMethod httpMethod, string requestPath, TForms? payload, IEnumerable<KeyValuePair<string, string>>? queryParameters = null)
    {
        var httpRequestMessage = GetHttpRequestMessage(httpMethod, requestPath, queryParameters);

        if (payload is null)
            return httpRequestMessage;

        httpRequestMessage.Content = new FormUrlEncodedContent(
            CashCtrlSerialization.ConvertToDictionary(payload)
            ?? throw new InvalidOperationException("Payload could not be serialized to form data dictionary"));

        return httpRequestMessage;
    }

    /// <summary>
    /// Get http request message to send to the API
    /// </summary>
    /// <param name="httpMethod"></param>
    /// <param name="requestPath"></param>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    private HttpRequestMessage GetHttpRequestMessage<TQuery>(HttpMethod httpMethod, string requestPath, TQuery? queryParameters = default)
    {
        var httpRequestMessage = new HttpRequestMessage { Method = httpMethod };

        var uriBuilder = new UriBuilder(new Uri(_baseAddress, requestPath));

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["lang"] = CashCtrlSerialization.SerializeEnumValue(_language);

        if (CashCtrlSerialization.ConvertToDictionary(queryParameters) is { } dictionary)
            foreach (var (key, value) in dictionary)
                query[key] = value;

        uriBuilder.Query = query.ToString();
        httpRequestMessage.RequestUri = uriBuilder.Uri;

        return httpRequestMessage;
    }

    /// <summary>
    /// Get API result from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <returns></returns>
    private static async Task<ApiResult> GetApiResult(HttpResponseMessage httpResponseMessage)
        => CreateApiResult<ApiResponse>(await GetData(httpResponseMessage));

    /// <summary>
    /// Get API result with decimal balance from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <returns></returns>
    private static async Task<ApiResult<DecimalResponse>> GetBalanceApiResult(HttpResponseMessage httpResponseMessage)
    {
        var responseHeaders = GetResponseHeaders(httpResponseMessage);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        var balanceResponse = new DecimalResponse
        {
            Value = decimal.TryParse(content.Trim().Trim('"'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : 0m
        };

        return new()
        {
            IsHttpSuccess = httpResponseMessage.IsSuccessStatusCode,
            HttpStatusCode = httpResponseMessage.StatusCode,
            CashCtrlHttpStatusCodeDescription = HttpStatusCodeMapping.GetDescription(httpResponseMessage.StatusCode),
            RequestsLeft = (int?)responseHeaders[ApiHeaderNames.RequestsLeft],
            ResponseData = balanceResponse
        };
    }

    /// <summary>
    /// Get API result with plain text value from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <returns></returns>
    private static async Task<ApiResult<PlainTextResponse>> GetPlainTextApiResult(HttpResponseMessage httpResponseMessage)
    {
        var responseHeaders = GetResponseHeaders(httpResponseMessage);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        return new()
        {
            IsHttpSuccess = httpResponseMessage.IsSuccessStatusCode,
            HttpStatusCode = httpResponseMessage.StatusCode,
            CashCtrlHttpStatusCodeDescription = HttpStatusCodeMapping.GetDescription(httpResponseMessage.StatusCode),
            RequestsLeft = (int?)responseHeaders[ApiHeaderNames.RequestsLeft],
            ResponseData = new PlainTextResponse { Value = content.Trim().Trim('"') }
        };
    }

    /// <summary>
    /// Get API result with binary data from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <returns></returns>
    private static async Task<ApiResult<BinaryResponse>> GetBinaryApiResult(HttpResponseMessage httpResponseMessage)
    {
        var responseHeaders = GetResponseHeaders(httpResponseMessage);

        // Follow 302 redirects (e.g., file/get redirects to pre-authenticated cloud storage URL)
        var contentResponse = httpResponseMessage;
        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Redirect
            && httpResponseMessage.Headers.Location is not null)
        {
            using var redirectClient = new HttpClient();
            contentResponse = await redirectClient.GetAsync(httpResponseMessage.Headers.Location);
        }

        var binaryResponse = new BinaryResponse
        {
            Data = await contentResponse.Content.ReadAsByteArrayAsync(),
            ContentType = contentResponse.Content.Headers.ContentType?.MediaType,
            FileName = contentResponse.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                       ?? httpResponseMessage.Headers.Location?.Segments.LastOrDefault()
        };

        return new()
        {
            IsHttpSuccess = contentResponse.IsSuccessStatusCode,
            HttpStatusCode = httpResponseMessage.StatusCode,
            CashCtrlHttpStatusCodeDescription = HttpStatusCodeMapping.GetDescription(httpResponseMessage.StatusCode),
            RequestsLeft = (int?)responseHeaders[ApiHeaderNames.RequestsLeft],
            ResponseData = binaryResponse
        };
    }

    /// <summary>
    /// Get API result with data from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static async Task<ApiResult<T>> GetApiResult<T>(HttpResponseMessage httpResponseMessage) where T : ApiResponse
    {
        var data = await GetData(httpResponseMessage);
        var responseData = CashCtrlSerialization.Deserialize<T>(data.Content);
        return CreateApiResult<T>(data) with
        {
            ResponseData = responseData
        };
    }

    /// <summary>
    /// Create API result from response data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static ApiResult<T> CreateApiResult<T>((bool IsSuccessStatusCode, HttpStatusCode StatusCode, string StatusCodeDescription, IReadOnlyDictionary<string, object?> ResponseHeaders, string Content) data) where T : ApiResponse
        => new()
        {
            IsHttpSuccess = data.IsSuccessStatusCode,
            HttpStatusCode = data.StatusCode,
            CashCtrlHttpStatusCodeDescription = data.StatusCodeDescription,
            RequestsLeft = (int?) data.ResponseHeaders[ApiHeaderNames.RequestsLeft]
        };

    /// <summary>
    /// Get data from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <returns></returns>
    private static async Task<(bool IsSuccessStatusCode, HttpStatusCode StatusCode, string StatusCodeDescription, Dictionary<string, object?> ResponseHeaders, string Content)> GetData(HttpResponseMessage httpResponseMessage)
        => (httpResponseMessage.IsSuccessStatusCode,
            httpResponseMessage.StatusCode,
            HttpStatusCodeMapping.GetDescription(httpResponseMessage.StatusCode),
            GetResponseHeaders(httpResponseMessage),
            await httpResponseMessage.Content.ReadAsStringAsync());

    /// <summary>
    /// Get API headers from response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <returns></returns>
    private static Dictionary<string, object?> GetResponseHeaders(HttpResponseMessage httpResponseMessage)
        => new()
        {
            [ApiHeaderNames.RequestsLeft] = httpResponseMessage.Headers.TryGetValues(ApiHeaderNames.RequestsLeft, out var values) && int.TryParse(values.First(), out var requestsLeft)
                ? requestsLeft
                : null
        };
}
