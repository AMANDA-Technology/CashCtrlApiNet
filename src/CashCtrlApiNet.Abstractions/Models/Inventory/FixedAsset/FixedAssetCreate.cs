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

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset;

/// <summary>
/// Fixed asset create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/create.json">API Doc</a>
/// </summary>
public record FixedAssetCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the depreciation account (asset account). Required.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required int AccountId { get; init; }

    /// <summary>
    /// The ID of the credit account used for the purchase booking. Required.
    /// </summary>
    [JsonPropertyName("purchaseCreditId")]
    public required int PurchaseCreditId { get; init; }

    /// <summary>
    /// The date the asset was added. Must be within an existing fiscal period. Required.
    /// </summary>
    [JsonPropertyName("dateAdded")]
    public required string DateAdded { get; init; }

    /// <summary>
    /// The ID of the category. See Fixed asset category.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; init; }

    /// <summary>
    /// The name of the fixed asset.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(240)]
    public required string Name { get; init; }

    /// <summary>
    /// A description of the fixed asset.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Some optional notes.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? NotesHtml { get; init; }

    /// <summary>
    /// The fixed asset number. Leave empty to generate the number with sequenceNumberId.
    /// </summary>
    [JsonPropertyName("nr")]
    [MaxLength(50)]
    public string? Nr { get; init; }

    /// <summary>
    /// The ID of the currency. Leave empty to use the default currency. See Currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// Custom field values. They are stored as XML in this parameter.
    /// </summary>
    [JsonPropertyName("custom")]
    public string? CustomXml { get; init; }

    /// <summary>
    /// The purchase price of the fixed asset.
    /// </summary>
    [JsonPropertyName("purchasePrice")]
    public double? PurchasePrice { get; init; }

    /// <summary>
    /// The date of purchase.
    /// </summary>
    [JsonPropertyName("dateOfPurchase")]
    public string? DateOfPurchase { get; init; }

    /// <summary>
    /// The depreciation start date.
    /// </summary>
    [JsonPropertyName("depreciationStart")]
    public string? DepreciationStart { get; init; }

    /// <summary>
    /// Mark the fixed asset as inactive. Defaults to false.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }

    /// <summary>
    /// The ID of the sequence number used to generate the fixed asset number (see nr). See Sequence number.
    /// </summary>
    [JsonPropertyName("sequenceNumberId")]
    public int? SequenceNumberId { get; init; }
}
