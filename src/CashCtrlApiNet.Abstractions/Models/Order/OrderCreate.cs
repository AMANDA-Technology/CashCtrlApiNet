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

using System.Text.Json;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Order;

/// <summary>
/// Order create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/create.json">API Doc</a>
/// </summary>
public record OrderCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the account.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required int AccountId { get; init; }

    /// <summary>
    /// The ID of the category. See Order category.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public required int CategoryId { get; init; }

    /// <summary>
    /// The date of the order in YYYY-MM-DD format.
    /// </summary>
    [JsonPropertyName("date")]
    public required string Date { get; init; }

    /// <summary>
    /// The ID of the sequence number. Mandatory on create, but absent from read/list responses
    /// (the server returns the generated <c>nr</c> instead) — so the property is nullable to keep
    /// response deserialization working across the shared hierarchy (§3 / §13 pattern).
    /// </summary>
    [JsonPropertyName("sequenceNumberId")]
    public int? SequenceNumberId { get; init; }

    /// <summary>
    /// The ID of the associate (person). See Person.
    /// </summary>
    [JsonPropertyName("associateId")]
    public int? AssociateId { get; init; }

    /// <summary>
    /// A description of the order.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// The order items. On create, set to a JSON array with the order-item properties (CashCtrl
    /// accepts the JSON text serialized form). On read, the API returns a parsed array —
    /// <see cref="JsonElement"/> accepts both shapes. Same pattern as the Journal <c>items</c> field
    /// (§4 / §12 of <c>doc/analysis/2026-03-29-api-doc-discrepancies.md</c>).
    /// </summary>
    [JsonPropertyName("items")]
    public JsonElement? Items { get; init; }

    /// <summary>
    /// The ID of the rounding account.
    /// </summary>
    [JsonPropertyName("roundingId")]
    public int? RoundingId { get; init; }

    /// <summary>
    /// Custom field values. They are stored as XML in this parameter.
    /// </summary>
    [JsonPropertyName("custom")]
    public string? CustomXml { get; init; }

    /// <summary>
    /// Some optional notes.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? NotesHtml { get; init; }
}
