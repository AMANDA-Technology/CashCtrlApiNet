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

namespace CashCtrlApiNet.Abstractions.Models.Salary.Document;

/// <summary>
/// Salary document mail request. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/mail.json">API Doc</a>
/// </summary>
public record SalaryDocumentMail : ModelBaseRecord
{
    /// <summary>
    /// The IDs of the salary statements to send (comma-separated).
    /// </summary>
    [JsonPropertyName("statementIds")]
    public required string StatementIds { get; init; }

    /// <summary>
    /// The sender email address.
    /// </summary>
    [JsonPropertyName("mailFrom")]
    [MaxLength(255)]
    public required string MailFrom { get; init; }

    /// <summary>
    /// The subject of the email.
    /// </summary>
    [JsonPropertyName("mailSubject")]
    [MaxLength(255)]
    public required string MailSubject { get; init; }

    /// <summary>
    /// The text body of the email.
    /// </summary>
    [JsonPropertyName("mailText")]
    public required string MailText { get; init; }

    /// <summary>
    /// Whether to send a copy to the sender.
    /// </summary>
    [JsonPropertyName("isCopyToMe")]
    public bool? IsCopyToMe { get; init; }

    /// <summary>
    /// The BCC email address.
    /// </summary>
    [JsonPropertyName("mailBcc")]
    public string? MailBcc { get; init; }

    /// <summary>
    /// The CC email address.
    /// </summary>
    [JsonPropertyName("mailCc")]
    public string? MailCc { get; init; }

    /// <summary>
    /// The recipient email address.
    /// </summary>
    [JsonPropertyName("mailTo")]
    public string? MailTo { get; init; }

    /// <summary>
    /// The ID of the sent status.
    /// </summary>
    [JsonPropertyName("sentStatusId")]
    public int? SentStatusId { get; init; }
}
