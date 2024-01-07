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

namespace CashCtrlApiNet.Interfaces;

/// <summary>
/// Configuration for accessing CashCtrl API
/// </summary>
public interface ICashCtrlConfiguration
{
    /// <summary>
    /// Base URL for accessing the service. <see href="https://app.cashctrl.com/static/help/en/api/index.html#intro">API Doc - Introduction</see>
    /// <br/>E.g. "https://myorg.cashctrl.com/api/v1/"
    /// </summary>
    public string BaseUri { get; set; }

    /// <summary>
    /// API key for authenticating the service. <see href="https://app.cashctrl.com/static/help/en/api/index.html#auth">API Doc - Authentication</see>
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Default language to use. Must be name of <see cref="Language"/>. Can be changed later using <see cref="ICashCtrlApiClient.SetLanguage"/>. <see href="https://app.cashctrl.com/static/help/en/api/index.html#lang">API Doc - Language</see>
    /// </summary>
    public string DefaultLanguage { get; set; }
}
