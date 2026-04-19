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
using CashCtrlApiNet.Abstractions.Converters;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Order.Category;

/// <summary>
/// A single status entry within an order category. Returned by
/// <c>/order/category/read_status.json</c>. This is the "sub-status" row that appears inside the
/// status array of an <see cref="OrderCategory"/> — distinct from the category itself.
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/category/read_status.json">API Doc</a>
/// </summary>
public record OrderCategoryStatus : ModelBaseRecord
{
    /// <summary>The ID of the status.</summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>The ID of the order category this status belongs to.</summary>
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; init; }

    /// <summary>An optional action triggered when an order moves into this status (e.g. <c>CATEGORY_9</c>, <c>BOOK_TEMPLATE_10</c>).</summary>
    [JsonPropertyName("actionId")]
    public string? ActionId { get; init; }

    /// <summary>The (possibly localized XML) display name of the status.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>The icon/color of the status (e.g. BLUE, GREEN, RED, YELLOW, ORANGE, BLACK, GRAY, BROWN, VIOLET, PINK).</summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    /// <summary>The position of the status within the category.</summary>
    [JsonPropertyName("pos")]
    public int? Pos { get; init; }

    /// <summary>Whether journal book entries are created when moving to this status.</summary>
    [JsonPropertyName("isBook")]
    public bool? IsBook { get; init; }

    /// <summary>Whether inventory stock is added when moving to this status.</summary>
    [JsonPropertyName("isAddStock")]
    public bool? IsAddStock { get; init; }

    /// <summary>Whether inventory stock is removed when moving to this status.</summary>
    [JsonPropertyName("isRemoveStock")]
    public bool? IsRemoveStock { get; init; }

    /// <summary>Whether this status closes / completes the order object (no longer due).</summary>
    [JsonPropertyName("isClosed")]
    public bool? IsClosed { get; init; }

    /// <summary>The date and time the status was created.</summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]
    public DateTime? Created { get; init; }

    /// <summary>The user who created the status.</summary>
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }

    /// <summary>The date and time the status was last updated.</summary>
    [JsonPropertyName("lastUpdated")]
    [JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]
    public DateTime? LastUpdated { get; init; }

    /// <summary>The user who last updated the status.</summary>
    [JsonPropertyName("lastUpdatedBy")]
    public string? LastUpdatedBy { get; init; }
}
