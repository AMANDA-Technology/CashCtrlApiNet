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

namespace CashCtrlApiNet.Abstractions.Values;

/// <summary>
/// Http status code mapping
/// </summary>
public static class HttpStatusCodeMapping
{
    private const string DescriptionNotFound = "No description found for this http status code";

    private static readonly Dictionary<int, string> Description = new()
    {
        [200] = "Everything worked as expected. However, form validation errors are still possible, see example response",
        [401] = "No valid API key provided.",
        [403] = "The API key doesn't have permissions to perform the request.",
        [404] = "The requested endpoint doesn't exist.",
        [405] = "The HTTP method (GET, POST, etc.) used is not allowed.",
        [418] = "Error you get when CashCtrl is a teapot and not a coffee pot.",
        [429] = "Too many requests hit the API too quickly. We recommend adding delays between your requests. For more information, see our 'https://cashctrl.com/en/about/terms'.",
        [500] = "Something went wrong on our end - not your fault. Please contact support."
    };

    /// <summary>
    /// Get description for http status code
    /// </summary>
    /// <param name="httpStatusCode"></param>
    /// <returns></returns>
    public static string GetDescription(HttpStatusCode httpStatusCode) => (int) httpStatusCode switch
    {
        < 500 => Description.GetValueOrDefault((int) httpStatusCode, DescriptionNotFound),
        500 or 502 or 503 or 504 => Description.GetValueOrDefault(500, DescriptionNotFound),
        _ => DescriptionNotFound
    };
}
