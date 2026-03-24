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

namespace CashCtrlApiNet.Abstractions.Models.Salary.Statement;

/// <summary>
/// Salary statement recurrence update. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/update_recurrence.json">API Doc</a>
/// </summary>
public record SalaryStatementRecurrenceUpdate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the statement.
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

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
    /// The recurrence definition.
    /// </summary>
    [JsonPropertyName("recurrence")]
    public string? Recurrence { get; init; }

    /// <summary>
    /// The start date for recurring statements (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("startDate")]
    public string? StartDate { get; init; }
}
