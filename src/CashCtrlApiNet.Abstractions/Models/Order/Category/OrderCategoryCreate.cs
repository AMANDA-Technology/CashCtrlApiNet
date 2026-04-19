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
using System.Text.Json;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Order.Category;

/// <summary>
/// Order category create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/category/create.json">API Doc</a>
/// </summary>
public record OrderCategoryCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the account (typically debtors for sales, creditors for purchase). Mandatory on create.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required int AccountId { get; init; }

    /// <summary>
    /// The plural name of the category (e.g. 'Invoices'). Supports localized XML
    /// (<c>&lt;values&gt;&lt;de&gt;...&lt;/de&gt;...&lt;/values&gt;</c>). Mandatory on create.
    /// </summary>
    [JsonPropertyName("namePlural")]
    [MaxLength(100)]
    public required string NamePlural { get; init; }

    /// <summary>
    /// The singular name of the category (e.g. 'Invoice'). Supports localized XML. Mandatory on create.
    /// </summary>
    [JsonPropertyName("nameSingular")]
    [MaxLength(100)]
    public required string NameSingular { get; init; }

    /// <summary>
    /// The status list (e.g. 'Draft', 'Open', 'Paid') for this order category. Mandatory on create
    /// (at least one status must be defined). Expected shape:
    /// <c>[{"icon":"BLUE","name":"Draft"}, ...]</c> — icon values: BLUE, GREEN, RED, YELLOW, ORANGE,
    /// BLACK, GRAY, BROWN, VIOLET, PINK. On read the API returns a parsed array of status objects
    /// with full audit fields — <see cref="JsonElement"/> accepts both shapes (§4 pattern).
    /// </summary>
    [JsonPropertyName("status")]
    public JsonElement? Status { get; init; }

    /// <summary>
    /// The type of category. Defaults to <c>SALES</c>. Possible values: <c>SALES</c>, <c>PURCHASE</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// The ID of the sequence number used for order objects in this category. Note: this parameter
    /// is <c>sequenceNrId</c> (not <c>sequenceNumberId</c> like on regular Order endpoints).
    /// </summary>
    [JsonPropertyName("sequenceNrId")]
    public int? SequenceNrId { get; init; }
}
