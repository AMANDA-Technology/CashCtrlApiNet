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
/// Report element create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/create.json">API Doc</a>
/// </summary>
public record ReportElementCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the report.
    /// </summary>
    [JsonPropertyName("reportId")]
    public required int ReportId { get; init; }

    /// <summary>
    /// The ID of the account.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required int AccountId { get; init; }

    /// <summary>
    /// The ID of the cost center category.
    /// </summary>
    [JsonPropertyName("costCenterCategoryId")]
    public int? CostCenterCategoryId { get; init; }

    /// <summary>
    /// The ID of the currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// The ID of the date filter.
    /// </summary>
    [JsonPropertyName("dateFilterId")]
    public int? DateFilterId { get; init; }

    /// <summary>
    /// Whether to include the total.
    /// </summary>
    [JsonPropertyName("includeTotal")]
    public bool? IncludeTotal { get; init; }

    /// <summary>
    /// Whether to negate the amount.
    /// </summary>
    [JsonPropertyName("negateAmount")]
    public bool? NegateAmount { get; init; }
}
