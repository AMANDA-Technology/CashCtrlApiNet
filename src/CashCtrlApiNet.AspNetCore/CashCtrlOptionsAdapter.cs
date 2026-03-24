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

using CashCtrlApiNet.Interfaces;
using Microsoft.Extensions.Options;

namespace CashCtrlApiNet.AspNetCore;

/// <summary>
/// Adapts <see cref="IOptions{CashCtrlOptions}"/> to the <see cref="ICashCtrlConfiguration"/> interface
/// required by the CashCtrl API client library.
/// Eagerly accesses <see cref="IOptions{TOptions}.Value"/> to trigger options validation at resolution time.
/// </summary>
internal sealed class CashCtrlOptionsAdapter : ICashCtrlConfiguration
{
    private readonly CashCtrlOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="CashCtrlOptionsAdapter"/> class.
    /// </summary>
    /// <param name="options">The options instance to adapt</param>
    public CashCtrlOptionsAdapter(IOptions<CashCtrlOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public string BaseUri => _options.BaseUri!;

    /// <inheritdoc />
    public string ApiKey => _options.ApiKey!;

    /// <inheritdoc />
    public string DefaultLanguage => Enum.GetName(_options.Language)!;
}
