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

namespace CashCtrlApiNet.Abstractions.Models.Salary.BookEntry;

/// <summary>
/// Salary book entry create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry/create.json">API Doc</a>
/// </summary>
public record SalaryBookEntryCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the credit account.
    /// </summary>
    [JsonPropertyName("creditId")]
    public required int CreditId { get; init; }

    /// <summary>
    /// The ID of the debit account.
    /// </summary>
    [JsonPropertyName("debitId")]
    public required int DebitId { get; init; }

    /// <summary>
    /// The IDs of the salary statements (comma-separated).
    /// </summary>
    [JsonPropertyName("statementIds")]
    public required string StatementIds { get; init; }

    /// <summary>
    /// The amount of the book entry.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }

    /// <summary>
    /// The date of the book entry (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// A description for the book entry.
    /// </summary>
    [JsonPropertyName("description")]
    [MaxLength(200)]
    public string? Description { get; init; }

    /// <summary>
    /// A reference for the book entry.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }

    /// <summary>
    /// The ID of the status.
    /// </summary>
    [JsonPropertyName("statusId")]
    public int? StatusId { get; init; }
}
