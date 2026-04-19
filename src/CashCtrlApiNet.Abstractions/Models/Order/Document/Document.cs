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

namespace CashCtrlApiNet.Abstractions.Models.Order.Document;

/// <summary>
/// Order document (detail response from <c>/order/document/read.json</c>). A document represents
/// the print/PDF settings for an order — there is one document per order, identified by the
/// parent order's ID. The read response does <b>not</b> include a top-level <c>id</c> field, only
/// an <c>orderId</c>, so this type is standalone and does not inherit from
/// <see cref="DocumentUpdate"/>.
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/document/read.json">API Doc</a>
/// </summary>
public record Document : ModelBaseRecord
{
    /// <summary>The ID of the parent order this document belongs to.</summary>
    [JsonPropertyName("orderId")]
    public int? OrderId { get; init; }

    /// <summary>The ID of the layout used to render this document.</summary>
    [JsonPropertyName("layoutId")]
    public int? LayoutId { get; init; }

    /// <summary>The ID of the sender location.</summary>
    [JsonPropertyName("orgLocationId")]
    public int? OrgLocationId { get; init; }

    /// <summary>The ID of the organization's bank account.</summary>
    [JsonPropertyName("orgBankAccountId")]
    public int? OrgBankAccountId { get; init; }

    /// <summary>The ID of the recipient's address.</summary>
    [JsonPropertyName("recipientAddressId")]
    public int? RecipientAddressId { get; init; }

    /// <summary>The ID of the recipient's bank account.</summary>
    [JsonPropertyName("recipientBankAccountId")]
    public int? RecipientBankAccountId { get; init; }

    /// <summary>The sender address formatted with line breaks.</summary>
    [JsonPropertyName("orgAddress")]
    public string? OrgAddress { get; init; }

    /// <summary>The organization's IBAN.</summary>
    [JsonPropertyName("orgIban")]
    public string? OrgIban { get; init; }

    /// <summary>The organization's QR-IBAN (Swiss QR invoicing).</summary>
    [JsonPropertyName("orgQrIban")]
    public string? OrgQrIban { get; init; }

    /// <summary>The organization's BIC.</summary>
    [JsonPropertyName("orgBic")]
    public string? OrgBic { get; init; }

    /// <summary>The recipient address formatted with line breaks.</summary>
    [JsonPropertyName("recipientAddress")]
    public string? RecipientAddress { get; init; }

    /// <summary>The recipient's IBAN.</summary>
    [JsonPropertyName("recipientIban")]
    public string? RecipientIban { get; init; }

    /// <summary>The recipient's BIC.</summary>
    [JsonPropertyName("recipientBic")]
    public string? RecipientBic { get; init; }

    /// <summary>Header HTML rendered above the items list.</summary>
    [JsonPropertyName("header")]
    public string? Header { get; init; }

    /// <summary>Footer HTML rendered below the items list.</summary>
    [JsonPropertyName("footer")]
    public string? Footer { get; init; }

    /// <summary>The e-mail recipient ("to" field).</summary>
    [JsonPropertyName("mailTo")]
    public string? MailTo { get; init; }

    /// <summary>The e-mail sender ("from" field).</summary>
    [JsonPropertyName("mailFrom")]
    public string? MailFrom { get; init; }

    /// <summary>The e-mail CC recipients.</summary>
    [JsonPropertyName("mailCc")]
    public string? MailCc { get; init; }

    /// <summary>The e-mail BCC recipients.</summary>
    [JsonPropertyName("mailBcc")]
    public string? MailBcc { get; init; }

    /// <summary>The e-mail subject. Can contain variables like <c>$documentName</c>, <c>$nr</c>, <c>$orgName</c>.</summary>
    [JsonPropertyName("mailSubject")]
    public string? MailSubject { get; init; }

    /// <summary>The e-mail body text.</summary>
    [JsonPropertyName("mailText")]
    public string? MailText { get; init; }

    /// <summary>The language of the document (DE, EN, FR, IT).</summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }

    /// <summary>The date and time the document was created.</summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]
    public DateTime? Created { get; init; }

    /// <summary>The user who created the document.</summary>
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }

    /// <summary>The date and time the document was last updated.</summary>
    [JsonPropertyName("lastUpdated")]
    [JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]
    public DateTime? LastUpdated { get; init; }

    /// <summary>The user who last updated the document.</summary>
    [JsonPropertyName("lastUpdatedBy")]
    public string? LastUpdatedBy { get; init; }
}
