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

using System.Runtime.InteropServices;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Interfaces;

/// <summary>
/// Connection handler to call CashCtrl REST API
/// </summary>
public interface ICashCtrlConnectionHandler
{
    /// <summary>
    /// Change the language to use for upcoming requests. <see href="https://app.cashctrl.com/static/help/en/api/index.html#lang">API Doc - Language</see>
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language);

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetAsync(string requestPath, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> GetAsync<TResult>(string requestPath, [Optional] CancellationToken cancellationToken) where TResult : ModelBaseRecord;

    /// <summary>
    /// Base GET request
    /// </summary>
    /// <param name="requestPath"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<TResult>> PostAsync<TResult, TPost>(string requestPath, TPost payload, [Optional] CancellationToken cancellationToken) where TResult : ModelBaseRecord;
}
