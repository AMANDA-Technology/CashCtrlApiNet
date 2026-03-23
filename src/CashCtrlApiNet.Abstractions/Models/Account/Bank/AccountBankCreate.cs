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
using CashCtrlApiNet.Abstractions.Enums.Account;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Account.Bank;

/// <summary>
/// Bank account create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account/bank/create.json">API Doc</a>
/// </summary>
public record AccountBankCreate : ModelBaseRecord
{
    /// <summary>
    /// The BIC (Bank Identifier Code) of the bank account.
    /// </summary>
    [JsonPropertyName("bic")]
    [MaxLength(11)]
    public required string Bic { get; init; }

    /// <summary>
    /// The IBAN (International Bank Account Number) of the bank account.
    /// </summary>
    [JsonPropertyName("iban")]
    [MaxLength(32)]
    public required string Iban { get; init; }

    /// <summary>
    /// The name of the bank account.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public required string Name { get; init; }

    /// <summary>
    /// The type of the bank account.
    /// </summary>
    [JsonPropertyName("type")]
    public required BankAccountType Type { get; init; }

    /// <summary>
    /// The ID of the associated account. See Account.
    /// </summary>
    [JsonPropertyName("accountId")]
    public int? AccountId { get; init; }

    /// <summary>
    /// The ID of the currency. Leave empty to use the default currency. See Currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// Mark the bank account as inactive. Defaults to false.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }

    /// <summary>
    /// Some optional notes.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? NotesHtml { get; init; }

    /// <summary>
    /// The first digits of the QR reference number.
    /// </summary>
    [JsonPropertyName("qrFirstDigits")]
    [MaxLength(8)]
    public string? QrFirstDigits { get; init; }

    /// <summary>
    /// The QR-IBAN for QR-Bill payments.
    /// </summary>
    [JsonPropertyName("qrIban")]
    [MaxLength(32)]
    public string? QrIban { get; init; }

    /// <summary>
    /// The URL of the bank's website.
    /// </summary>
    [JsonPropertyName("url")]
    [MaxLength(255)]
    public string? Url { get; init; }
}
