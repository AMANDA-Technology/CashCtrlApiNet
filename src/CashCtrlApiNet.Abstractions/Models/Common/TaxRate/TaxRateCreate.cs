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

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Common.TaxRate;

/// <summary>
/// Tax rate create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax/create.json">API Doc</a>
/// </summary>
public record TaxRateCreate : ModelBaseRecord
{
    /// <summary>
    /// The code of the tax rate.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    /// <summary>
    /// The description of the tax rate.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("description")]
    [MaxLength(100)]
    public required string Description { get; init; }

    /// <summary>
    /// The components of this tax rate. Each component specifies an account, calculation type, percentage, and apply rule.
    /// <br/>This is a JSON array [{...},{...},...] with the following parameters: <see cref="TaxRateComponent"/>
    /// </summary>
    [JsonPropertyName("components")]
    public ImmutableArray<TaxRateComponent> Components { get; init; } = [];

    /// <summary>
    /// The document name of the tax rate.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("documentName")]
    [MaxLength(100)]
    public string? DocumentName { get; init; }

    /// <summary>
    /// Whether this tax rate is a display tax rate.
    /// </summary>
    [JsonPropertyName("isDisplayTaxRate")]
    public bool? IsDisplayTaxRate { get; init; }

    /// <summary>
    /// Whether the tax rate is inactive.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }

    /// <summary>
    /// The historical rates for this tax rate. Each rate specifies a validity start date and a percentage.
    /// <br/>This is a JSON array [{...},{...},...] with the following parameters: <see cref="TaxRateHistoricalRate"/>
    /// </summary>
    [JsonPropertyName("rates")]
    public ImmutableArray<TaxRateHistoricalRate> Rates { get; init; } = [];
}
