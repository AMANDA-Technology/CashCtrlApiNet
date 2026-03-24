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

namespace CashCtrlApiNet.Abstractions.Models.Meta.Settings;

/// <summary>
/// Settings read response model. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/setting/read.json">API Doc</a>
/// </summary>
public record Settings : ModelBaseRecord
{
    /// <summary>
    /// The ID of the default currency. See Currency.
    /// </summary>
    [JsonPropertyName("defaultCurrencyId")]
    public int? DefaultCurrencyId { get; init; }

    /// <summary>
    /// The ID of the default opening account. See Account.
    /// </summary>
    [JsonPropertyName("defaultOpeningAccountId")]
    public int? DefaultOpeningAccountId { get; init; }

    /// <summary>
    /// The ID of the default tax. See Tax.
    /// </summary>
    [JsonPropertyName("defaultTaxId")]
    public int? DefaultTaxId { get; init; }

    /// <summary>
    /// The ID of the default inventory asset account. See Account.
    /// </summary>
    [JsonPropertyName("defaultInventoryAssetAccountId")]
    public int? DefaultInventoryAssetAccountId { get; init; }

    /// <summary>
    /// The ID of the default inventory depreciation account. See Account.
    /// </summary>
    [JsonPropertyName("defaultInventoryDepreciationAccountId")]
    public int? DefaultInventoryDepreciationAccountId { get; init; }

    /// <summary>
    /// The ID of the CSV delimiter. See Common/SequenceNumber.
    /// </summary>
    [JsonPropertyName("csvDelimiter")]
    public string? CsvDelimiter { get; init; }

    /// <summary>
    /// The first day of the week (0 = Sunday, 1 = Monday, etc.).
    /// </summary>
    [JsonPropertyName("firstDayOfWeek")]
    public int? FirstDayOfWeek { get; init; }

    /// <summary>
    /// Whether the pro mode is enabled.
    /// </summary>
    [JsonPropertyName("isProMode")]
    public bool? IsProMode { get; init; }
}
