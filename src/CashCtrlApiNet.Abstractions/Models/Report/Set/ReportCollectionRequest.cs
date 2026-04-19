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

namespace CashCtrlApiNet.Abstractions.Models.Report.Set;

/// <summary>
/// Request parameters for the report collection meta/download endpoints
/// (<c>/report/collection/meta.json</c>, <c>download.pdf</c>, <c>download.csv</c>,
/// <c>download.xlsx</c>). These endpoints all take <c>collectionId</c> as a mandatory query
/// parameter (not <c>id</c> like other endpoints) plus an optional reporting period.
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/collection/meta.json">API Doc</a>
/// </summary>
public record ReportCollectionRequest : ModelBaseRecord
{
    /// <summary>
    /// The ID of the report collection. Mandatory.
    /// </summary>
    [JsonPropertyName("collectionId")]
    public required int CollectionId { get; init; }

    /// <summary>
    /// The start date of the report period in <c>YYYY-MM-DD</c> format.
    /// Overridden if <see cref="FiscalPeriod"/> is set.
    /// </summary>
    [JsonPropertyName("startDate")]
    public string? StartDate { get; init; }

    /// <summary>
    /// The end date of the report period in <c>YYYY-MM-DD</c> format.
    /// Overridden if <see cref="FiscalPeriod"/> is set.
    /// </summary>
    [JsonPropertyName("endDate")]
    public string? EndDate { get; init; }

    /// <summary>
    /// The ID of the fiscal period. Overrides <see cref="StartDate"/> / <see cref="EndDate"/>
    /// — the report period is derived from the fiscal period instead.
    /// </summary>
    [JsonPropertyName("fiscalPeriod")]
    public int? FiscalPeriod { get; init; }

    /// <summary>
    /// The language of the generated report. Possible values: <c>DE</c>, <c>EN</c>, <c>FR</c>, <c>IT</c>.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }

    /// <summary>
    /// The column to sort the elements by.
    /// </summary>
    [JsonPropertyName("sort")]
    public string? Sort { get; init; }
}
