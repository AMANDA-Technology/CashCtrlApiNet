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

namespace CashCtrlApiNet.Abstractions.Models.Journal;

/// <summary>
/// Journal entry create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/create.json">API Doc</a>
/// </summary>
public record JournalCreate : ModelBaseRecord
{
    /// <summary>
    /// The date of the journal entry in YYYY-MM-DD format.
    /// </summary>
    [JsonPropertyName("dateAdded")]
    public required string DateAdded { get; init; }

    /// <summary>
    /// The title (description) of the journal entry.
    /// </summary>
    [JsonPropertyName("title")]
    [MaxLength(100)]
    public required string Title { get; init; }

    /// <summary>
    /// The ID of the sequence number. See Sequence number. Required on create, but absent from
    /// read/list responses (the server returns the generated <c>reference</c> instead) — so the
    /// property is nullable to keep response deserialization working across the shared hierarchy.
    /// </summary>
    [JsonPropertyName("sequenceNumberId")]
    public int? SequenceNumberId { get; init; }

    /// <summary>
    /// The ID of the debit account. See Account.
    /// </summary>
    [JsonPropertyName("debitId")]
    public int? DebitId { get; init; }

    /// <summary>
    /// The ID of the credit account. See Account.
    /// </summary>
    [JsonPropertyName("creditId")]
    public int? CreditId { get; init; }

    /// <summary>
    /// The amount of the journal entry.
    /// </summary>
    [JsonPropertyName("amount")]
    public double? Amount { get; init; }

    /// <summary>
    /// The ID of the tax rate. See Tax rate.
    /// </summary>
    [JsonPropertyName("taxId")]
    public int? TaxId { get; init; }

    /// <summary>
    /// Items for collective entries. On create, set to a JSON array with accountId, debit, and
    /// credit properties (CashCtrl accepts the JSON text serialized form). On read, the API returns
    /// a parsed array — <see cref="JsonElement"/> accepts both shapes.
    /// </summary>
    [JsonPropertyName("items")]
    public JsonElement? Items { get; init; }

    /// <summary>
    /// The ID of the cost center. See Cost center.
    /// </summary>
    [JsonPropertyName("costCenterId")]
    public int? CostCenterId { get; init; }

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

    /// <summary>
    /// A reference text for the journal entry.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }
}
