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

namespace CashCtrlApiNet.Abstractions.Models.Inventory.ArticleCategory;

/// <summary>
/// Article category create
/// </summary>
public record ArticleCategoryCreate : ModelBaseRecord
{
    /// <summary>
    /// The name of the category.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: <values><de>German text</de><en>English text</en></values>
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(100)]
    // TODO: Implement type for translated texts
    public required string Name { get; init; }

    /// <summary>
    /// Allocations to cost centers for all articles in this category. These will be used in order items, when selecting the article. If allocations are set here, they override any allocations that would be automatically made by the corresponding sales/purchase account.
    /// <br/>This is a JSON array [{...},{...},...] with the following parameters: <see cref="ArticleCategoryAllocation"/>
    /// </summary>
    [JsonPropertyName("allocations")]
    public ImmutableArray<ArticleCategoryAllocation> Allocations { get; init; } = [];

    /// <summary>
    /// The ID of the parent category.
    /// </summary>
    [JsonPropertyName("parentId")]
    public int? ParentId { get; init; }

    /// <summary>
    /// The ID of the purchase account, which will be used when purchasing articles in this category through orders. See <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order">API Doc - Order</a>
    /// </summary>
    [JsonPropertyName("purchaseAccountId")]
    public int? PurchaseAccountId { get; init; }

    /// <summary>
    /// The ID of the sales account, which will be used when selling articles in this category through Orders. See <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order">API Doc - Order</a>
    /// </summary>
    [JsonPropertyName("salesAccountId")]
    public int? SalesAccountId { get; init; }

    /// <summary>
    /// The ID of the sequence number used for services in this category. If empty, the sequence number of the parent category is inherited.
    /// </summary>
    [JsonPropertyName("sequenceNrId")]
    public int? SequenceNrId { get; init; }
}
