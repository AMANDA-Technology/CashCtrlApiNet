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

namespace CashCtrlApiNet.Abstractions.Models.Inventory.Article;

/// <summary>
/// Article create
/// </summary>
public record ArticleCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the category. See Article category.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; init; }

    /// <summary>
    /// The ID of the unit (like pcs., meters, liters, etc.). See Units.
    /// </summary>
    [JsonPropertyName("unitId")]
    public int? UnitId { get; init; }

    /// <summary>
    /// The ID of the location where the article can be found (e.g. a warehouse). See Location. Note that this represents the building whereas the binLocation parameter represents the place within the building. isStockArticle must be true, ignored otherwise.
    /// </summary>
    [JsonPropertyName("locationId")]
    public int? LocationId { get; init; }

    /// <summary>
    /// The ID of the currency. Leave empty to use the default currency. See Currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// The article number. Leave empty to generate the number with sequenceNumberId.
    /// </summary>
    [JsonPropertyName("nr")]
    [MaxLength(50)]
    public string? Nr { get; init; }

    /// <summary>
    /// The name of the article.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: <values><de>German text</de><en>English text</en></values>
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(240)]
    // TODO: Implement type for translated texts
    public required string Name { get; init; }

    /// <summary>
    /// A description of the article.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: <values><de>German text</de><en>English text</en></values>
    /// </summary>
    [JsonPropertyName("description")]
    // TODO: Implement type for translated texts
    public string? Description { get; init; }

    /// <summary>
    /// Some optional notes.
    /// <br/>This can contain limited HTML for styling. Allowed tags: a, p, div, h1, h2, h3, h4, h5, h6, ul, ol, li, blockquote, b, i, s, u, o, sup, sub, ins, del, strong, strike, tt, code, big, small, br, span, em. Allowed inline CSS rules: background, background-color, color, font, font-size, font-weight, font-style, text-align, text-indent, text-decoration.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? NotesHtml { get; init; }

    /// <summary>
    /// The sales price of the article. Net (excluding tax) by default, see isSalesPriceGross.
    /// </summary>
    [JsonPropertyName("salesPrice")]
    public double? SalesPrice { get; init; }

    /// <summary>
    /// The last purchase price of the article. Net (excluding tax) by default, see isPurchasePriceGross. This price is automatically updated by purchase orders.
    /// </summary>
    [JsonPropertyName("lastPurchasePrice")]
    public double? LastPurchasePrice { get; init; }

    /// <summary>
    /// The current stock of the article. isStockArticle must be true, ignored otherwise.
    /// </summary>
    [JsonPropertyName("stock")]
    public int? Stock { get; init; }

    /// <summary>
    /// The desired minimum stock of the article. isStockArticle must be true, ignored otherwise.
    /// </summary>
    [JsonPropertyName("minStock")]
    public int? MinStock { get; init; }

    /// <summary>
    /// The desired maximum stock of the article. isStockArticle must be true, ignored otherwise.
    /// </summary>
    [JsonPropertyName("maxStock")]
    public int? MaxStock { get; init; }

    /// <summary>
    /// The place within the building (e.g. A15, B04, C11, etc.) to locate the article. See locationId. isStockArticle must be true, ignored otherwise.
    /// </summary>
    [JsonPropertyName("binLocation")]
    public string? BinLocation { get; init; }

    /// <summary>
    /// Custom field values. They are stored as XML in this parameter. Example: <values><customField1>My value</customField1><customField2>["value 1","value 2"]</customField2></values>. Look up custom field variable names (e.g. "customField1") in the Custom fields API.
    /// </summary>
    [JsonPropertyName("custom")]
    public string? CustomXml { get; init; }

    /// <summary>
    /// Whether the article has a stock, i.e. the stock should be tracked and updated via sales and purchase orders. Defaults to false. Possible values: true, false.
    /// </summary>
    [JsonPropertyName("isStockArticle")]
    public bool? IsStockArticle { get; init; }

    /// <summary>
    /// Mark the article as inactive. The article will be greyed out. Defaults to false. Possible values: true, false.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }

    /// <summary>
    /// The sales price is net (excluding tax) by default. This defines the sales price as gross (including tax). Defaults to false. Possible values: true, false.
    /// </summary>
    [JsonPropertyName("isSalesPriceGross")]
    public bool? IsSalesPriceGross { get; init; }

    /// <summary>
    /// The purchase price is net (excluding tax) by default. This defines the purchase price as gross (including tax). Defaults to false. Possible values: true, false.
    /// </summary>
    [JsonPropertyName("isPurchasePriceGross")]
    public bool? IsPurchasePriceGross { get; init; }

    /// <summary>
    /// The ID of the sequence number used to generate the article number (see nr). If this is empty, the sequence number will not update when nr is set. See Sequence number.
    /// </summary>
    [JsonPropertyName("sequenceNumberId")]
    public int? SequenceNumberId { get; init; }
}
