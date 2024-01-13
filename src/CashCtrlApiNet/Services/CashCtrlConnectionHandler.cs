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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Values;
using CashCtrlApiNet.Interfaces;

namespace CashCtrlApiNet.Services;

/// <inheritdoc />
public class CashCtrlConnectionHandler : ICashCtrlConnectionHandler
{
    /// <summary>
    /// Language to use on all requests
    /// </summary>
    private Language _language;

    private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Holds the http client with some basic settings, to be used for all connectors
    /// </summary>
    private readonly HttpClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="CashCtrlConnectionHandler"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public CashCtrlConnectionHandler(ICashCtrlConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.BaseUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ApiKey);

        // Configure
        _language = Enum.TryParse<Language>(configuration.DefaultLanguage, out var language)
            ? language
            : Language.de;

        // Normalize base urls
        var baseUri = configuration.BaseUri;
        if (!baseUri.EndsWith('/'))
            baseUri += '/';

        // Create a new http client for instance
        _client = new(new HttpClientHandler { AllowAutoRedirect = false })
        {
            BaseAddress = new(baseUri)
        };

        _client.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{configuration.ApiKey}:")));
    }

    /// <inheritdoc />
    public void SetLanguage(Language language)
        => _language = language;


    /// <inheritdoc />
    public async Task<ApiResult> GetAsync(string requestPath, [Optional] CancellationToken cancellationToken)
        => await GetApiResult(await _client.SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> GetAsync<TResult>(string requestPath, [Optional] CancellationToken cancellationToken) where TResult : ModelBaseRecord
        => await GetApiResult<TResult>(await _client.SendAsync(GetHttpRequestMessage(HttpMethod.Get, requestPath), cancellationToken));

    /// <inheritdoc />
    public async Task<ApiResult<TResult>> PostAsync<TResult, TPost>(string requestPath, TPost payload, [Optional] CancellationToken cancellationToken) where TResult : ModelBaseRecord
        => await GetApiResult<TResult>(await _client.SendAsync(GetHttpRequestMessageWithFormData(HttpMethod.Post, requestPath, payload), cancellationToken));

    /// <summary>
    /// Get http request message to send to the API (with body as json from data with type T)
    /// </summary>
    /// <param name="httpMethod"></param>
    /// <param name="requestPath"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    private HttpRequestMessage GetHttpRequestMessageWithFormData<T>(HttpMethod httpMethod, string requestPath, T? payload)
    {
        var httpRequestMessage = GetHttpRequestMessage(httpMethod, requestPath);

        // TODO: A bit hacky generic solution.. but who uses Form data for such payloads?
        var asJson = JsonSerializer.Serialize(payload, DefaultJsonSerializerOptions);
        var asDict = JsonSerializer.Deserialize<Dictionary<string, object?>>(asJson);

        httpRequestMessage.Content = new FormUrlEncodedContent(asDict
                                                               ?.ToDictionary(e => e.Key, e => e.Value?.ToString())
                                                               ?? throw new InvalidOperationException("Payload could not be serialized to form data dictionary"));

        return httpRequestMessage;
    }

    /// <summary>
    /// Get http request message to send to the API
    /// </summary>
    /// <param name="httpMethod"></param>
    /// <param name="requestPath"></param>
    /// <returns></returns>
    private HttpRequestMessage GetHttpRequestMessage(HttpMethod httpMethod, string requestPath)
    {
        var httpRequestMessage = new HttpRequestMessage { Method = httpMethod };

        var uriBuilder = new UriBuilder(new Uri(_client.BaseAddress!, requestPath));

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["lang "] = Enum.GetName(_language);

        // foreach (var (key, value) in GetRequestHeaders(apiRequest, idempotencyKey))
        //     httpRequestMessage.Headers.Add(key, value);

        // foreach (var (key, value) in GetQueryParameters(apiRequest))
        //     query[key] = value;

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
        => CreateApiResult<ModelBaseRecord>(await GetData(httpResponseMessage));

    /// <summary>
    /// Get API result with data from http response
    /// </summary>
    /// <param name="httpResponseMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static async Task<ApiResult<T>> GetApiResult<T>(HttpResponseMessage httpResponseMessage) where T : ModelBaseRecord
    {
        var data = await GetData(httpResponseMessage);
        return CreateApiResult<T>(data) with
        {
            ResponseData = JsonSerializer.Deserialize<T>(data.Content)
        };
    }

    /// <summary>
    /// Create API result from response data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static ApiResult<T> CreateApiResult<T>((bool IsSuccessStatusCode, HttpStatusCode StatusCode, string StatusCodeDescription, IReadOnlyDictionary<string, object?> ResponseHeaders, string Content) data) where T : ModelBaseRecord
        => new()
        {
            IsSuccess = data.IsSuccessStatusCode,
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
