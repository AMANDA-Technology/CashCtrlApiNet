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

namespace CashCtrlApiNet.Abstractions.Models.Journal.Import.Entry;

/// <summary>
/// Journal import entry update. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal/import/entry/update.json">API Doc</a>
/// </summary>
public record JournalImportEntryUpdate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the journal import entry to update.
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// The amount of the entry. Mandatory on update per the API; must be positive.
    /// Nullable so the shared hierarchy still deserializes list/read responses correctly.
    /// </summary>
    [JsonPropertyName("amount")]
    public double? Amount { get; init; }

    /// <summary>
    /// The date of the entry in <c>YYYY-MM-DD</c> format. Mandatory on update.
    /// </summary>
    [JsonPropertyName("dateAdded")]
    public string? DateAdded { get; init; }

    /// <summary>
    /// The ID of the debit account. Required on update despite the docs listing
    /// <c>contraAccountId</c> — the live API reports <c>debitId</c> as a mandatory field.
    /// </summary>
    [JsonPropertyName("debitId")]
    public int? DebitId { get; init; }

    /// <summary>
    /// The ID of the credit account. Required on update (see note on <see cref="DebitId"/>).
    /// </summary>
    [JsonPropertyName("creditId")]
    public int? CreditId { get; init; }

    /// <summary>
    /// The ID of the contra account (the opposite side of the target account). Documented as
    /// mandatory; in practice the API also accepts <see cref="DebitId"/>/<see cref="CreditId"/>.
    /// </summary>
    [JsonPropertyName("contraAccountId")]
    public int? ContraAccountId { get; init; }

    /// <summary>
    /// The title / description of the book entry.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// An optional reference / receipt for the book entry.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }

    /// <summary>
    /// The ID of the tax rate.
    /// </summary>
    [JsonPropertyName("taxId")]
    public int? TaxId { get; init; }
}
