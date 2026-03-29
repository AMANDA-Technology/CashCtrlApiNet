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

namespace CashCtrlApiNet.Abstractions.Models.Salary.Statement;

/// <summary>
/// Salary statement create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/create.json">API Doc</a>
/// </summary>
public record SalaryStatementCreate : ModelBaseRecord
{
    /// <summary>
    /// The date of the statement (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("date")]
    public required string Date { get; init; }

    /// <summary>
    /// The payment date of the statement (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("datePayment")]
    public required string DatePayment { get; init; }

    /// <summary>
    /// The ID of the person.
    /// </summary>
    [JsonPropertyName("personId")]
    public required int PersonId { get; init; }

    /// <summary>
    /// The ID of the status.
    /// </summary>
    [JsonPropertyName("statusId")]
    public required int StatusId { get; init; }

    /// <summary>
    /// The ID of the salary template.
    /// </summary>
    [JsonPropertyName("templateId")]
    public required int TemplateId { get; init; }

    /// <summary>
    /// The ID of the currency. Leave empty to use the default currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// The currency exchange rate.
    /// </summary>
    [JsonPropertyName("currencyRate")]
    public decimal? CurrencyRate { get; init; }

    /// <summary>
    /// Custom field values. They are stored as XML in this parameter.
    /// </summary>
    [JsonPropertyName("custom")]
    public JsonElement? Custom { get; init; }

    /// <summary>
    /// The number of days before the statement date to send notification.
    /// </summary>
    [JsonPropertyName("daysBefore")]
    public int? DaysBefore { get; init; }

    /// <summary>
    /// The end date for recurring statements (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("endDate")]
    public string? EndDate { get; init; }

    /// <summary>
    /// The insurance configuration as JSON.
    /// </summary>
    [JsonPropertyName("insurances")]
    public JsonElement? Insurances { get; init; }

    /// <summary>
    /// A message to include on the salary statement.
    /// </summary>
    [JsonPropertyName("message")]
    [MaxLength(50)]
    public string? Message { get; init; }

    /// <summary>
    /// Some optional notes.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; init; }

    /// <summary>
    /// The notification email address.
    /// </summary>
    [JsonPropertyName("notifyEmail")]
    public string? NotifyEmail { get; init; }

    /// <summary>
    /// The ID of the person to notify.
    /// </summary>
    [JsonPropertyName("notifyPersonId")]
    public int? NotifyPersonId { get; init; }

    /// <summary>
    /// The notification type.
    /// </summary>
    [JsonPropertyName("notifyType")]
    public string? NotifyType { get; init; }

    /// <summary>
    /// The ID of the user to notify.
    /// </summary>
    [JsonPropertyName("notifyUserId")]
    public int? NotifyUserId { get; init; }

    /// <summary>
    /// The number of the statement.
    /// </summary>
    [JsonPropertyName("nr")]
    [MaxLength(50)]
    public string? Nr { get; init; }

    /// <summary>
    /// Whether to recalculate the statement. Defaults to false.
    /// </summary>
    [JsonPropertyName("recalculate")]
    public bool? Recalculate { get; init; }

    /// <summary>
    /// The recurrence definition.
    /// </summary>
    [JsonPropertyName("recurrence")]
    public string? Recurrence { get; init; }

    /// <summary>
    /// The ID of the sequence number.
    /// </summary>
    [JsonPropertyName("sequenceNumberId")]
    public int? SequenceNumberId { get; init; }

    /// <summary>
    /// The start date for recurring statements (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("startDate")]
    public string? StartDate { get; init; }

    /// <summary>
    /// The salary types configuration as JSON.
    /// </summary>
    [JsonPropertyName("types")]
    public string? Types { get; init; }
}
