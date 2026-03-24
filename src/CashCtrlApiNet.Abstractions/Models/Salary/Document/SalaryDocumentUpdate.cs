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
/// Salary document update. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document/update.json">API Doc</a>
/// </summary>
public record SalaryDocumentUpdate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the document to update.
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// The ID of the file.
    /// </summary>
    [JsonPropertyName("fileId")]
    public int? FileId { get; init; }

    /// <summary>
    /// The footer of the document.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("footer")]
    public string? Footer { get; init; }

    /// <summary>
    /// The header of the document.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("header")]
    public string? Header { get; init; }

    /// <summary>
    /// The ID of the layout.
    /// </summary>
    [JsonPropertyName("layoutId")]
    public int? LayoutId { get; init; }

    /// <summary>
    /// The organization address.
    /// </summary>
    [JsonPropertyName("orgAddress")]
    [MaxLength(255)]
    public string? OrgAddress { get; init; }

    /// <summary>
    /// The ID of the organization bank account.
    /// </summary>
    [JsonPropertyName("orgBankAccountId")]
    public int? OrgBankAccountId { get; init; }

    /// <summary>
    /// The ID of the organization location.
    /// </summary>
    [JsonPropertyName("orgLocationId")]
    public int? OrgLocationId { get; init; }

    /// <summary>
    /// The recipient address.
    /// </summary>
    [JsonPropertyName("recipientAddress")]
    [MaxLength(255)]
    public string? RecipientAddress { get; init; }

    /// <summary>
    /// The ID of the recipient address.
    /// </summary>
    [JsonPropertyName("recipientAddressId")]
    public int? RecipientAddressId { get; init; }

    /// <summary>
    /// The ID of the recipient bank account.
    /// </summary>
    [JsonPropertyName("recipientBankAccountId")]
    public int? RecipientBankAccountId { get; init; }
}
