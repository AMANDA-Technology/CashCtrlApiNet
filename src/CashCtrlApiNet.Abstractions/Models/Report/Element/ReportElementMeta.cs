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

using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Report.Element;

/// <summary>
/// Response shape for <c>/report/element/meta.json</c>. This is a slim "document header" view
/// used when rendering a single report element — title, text, period label, and display flags.
/// Distinct from <see cref="ReportElement"/> which is the element's configuration record.
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/meta.json">API Doc</a>
/// </summary>
public record ReportElementMeta : ModelBaseRecord
{
    /// <summary>The element title shown as a heading in the rendered report.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>Free-form header text for the element.</summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>Human-readable label for the reporting period (e.g. "2026").</summary>
    [JsonPropertyName("periodLabel")]
    public string? PeriodLabel { get; init; }

    /// <summary>Whether the title should be hidden on the rendered report.</summary>
    [JsonPropertyName("isHideTitle")]
    public bool? IsHideTitle { get; init; }

    /// <summary>Whether this report type is in beta status.</summary>
    [JsonPropertyName("isBeta")]
    public bool? IsBeta { get; init; }

    /// <summary>Whether this report type requires a CashCtrl Pro subscription.</summary>
    [JsonPropertyName("isPro")]
    public bool? IsPro { get; init; }
}
