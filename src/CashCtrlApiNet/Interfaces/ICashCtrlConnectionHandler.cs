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

using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Api.Base;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces;

/// <summary>
/// Connection handler to call CashCtrl REST API
/// </summary>
public interface ICashCtrlConnectionHandler
{
    /// <summary>
    /// Change the language to use for upcoming requests. <a href="https://app.cashctrl.com/static/help/en/api/index.html#lang">API Doc - Language</a>
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language);

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetAsync(string requestPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> GetAsync<TResult>(string requestPath, CancellationToken cancellationToken = default) where TResult : ApiResponse;

    /// <summary>
    /// Base GET request with optional list parameters
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="listParams"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> GetAsync<TResult>(string requestPath, ListParams? listParams, CancellationToken cancellationToken = default) where TResult : ApiResponse;

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> GetAsync<TResult, TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default) where TResult : ApiResponse;

    /// <summary>
    /// Base POST request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> PostAsync<TResult, TPost>(string requestPath, TPost payload, CancellationToken cancellationToken = default) where TResult : ApiResponse;

    /// <summary>
    /// GET request returning a decimal balance value (e.g., account balance, cost center balance)
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<DecimalResponse>> GetBalanceAsync(string requestPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET request returning a decimal balance value with query parameters (e.g., account balance, cost center balance)
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<DecimalResponse>> GetBalanceAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET request returning a plain text value (e.g., generated sequence numbers)
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<PlainTextResponse>> GetPlainTextAsync(string requestPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET request returning a plain text value with query parameters
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<PlainTextResponse>> GetPlainTextAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET request returning binary data (e.g., file downloads, PDF exports)
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> GetBinaryAsync(string requestPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET request returning binary data with query parameters (e.g., file downloads, PDF exports)
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> GetBinaryAsync<TQuery>(string requestPath, TQuery queryParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// POST request with multipart form data content (e.g., file uploads)
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> PostMultipartAsync<TResult>(string requestPath, MultipartFormDataContent content, CancellationToken cancellationToken = default) where TResult : ApiResponse;
}
