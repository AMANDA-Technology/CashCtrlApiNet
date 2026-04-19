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
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Converters;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Order.BookEntry;

/// <summary>
/// Create request for <c>/order/bookentry/create.json</c>. A book entry is typically a payment or
/// receipt posted against one or more orders (e.g. marking an invoice as paid).
/// The order(s) must be in a status with <c>isBook=true</c>, otherwise the API rejects with
/// <c>"This document does not allow book entries."</c>
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/bookentry/create.json">API Doc</a>
/// </summary>
public record BookEntryCreate : ModelBaseRecord
{
    /// <summary>
    /// The IDs of one or multiple orders to book against, comma-separated on the wire. Mandatory.
    /// </summary>
    [JsonPropertyName("orderIds")]
    [JsonConverter(typeof(IntArrayAsCsvJsonConverter))]
    public required ImmutableArray<int> OrderIds { get; init; }

    /// <summary>
    /// The ID of the account for the book entry (typically the contra account, e.g. a bank account).
    /// Mandatory.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required int AccountId { get; init; }

    /// <summary>
    /// The amount of the book entry. Leave empty to use the order's open amount. Documented as
    /// optional; the live API also treats it as optional.
    /// </summary>
    [JsonPropertyName("amount")]
    public double? Amount { get; init; }

    /// <summary>
    /// The date of the book entry in <c>YYYY-MM-DD</c> format. Documented as optional but the live
    /// API rejects the call with <c>[date] This field cannot be empty</c> when omitted.
    /// Always provide a date within an existing fiscal period.
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// A description of the book entry (max 200 chars).
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// An optional reference / receipt for the book entry (max 100 chars).
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }

    /// <summary>
    /// The ID of the currency. Leave empty to use the default currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// The exchange rate from foreign currency to main currency. Only required if the currency is
    /// not supported or if you want to use a different rate than suggested.
    /// </summary>
    [JsonPropertyName("currencyRate")]
    public double? CurrencyRate { get; init; }

    /// <summary>
    /// The ID of the tax rate.
    /// </summary>
    [JsonPropertyName("taxId")]
    public int? TaxId { get; init; }

    /// <summary>
    /// The ID of the book entry template from the order category.
    /// </summary>
    [JsonPropertyName("templateId")]
    public int? TemplateId { get; init; }
}
