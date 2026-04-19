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

namespace CashCtrlApiNet.Abstractions.Models.Order;

/// <summary>
/// Order recurrence update. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/update_recurrence.json">API Doc</a>
/// </summary>
public record OrderRecurrenceUpdate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the order.
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// The interval of how often the order should be repeated. Omit (null) to remove recurrence.
    /// Possible values: <c>MONTHLY</c>, <c>WEEKLY</c>, <c>DAILY</c>, <c>YEARLY</c>, <c>SEMESTRAL</c>,
    /// <c>QUARTERLY</c>, <c>BI_MONTHLY</c>, <c>BI_WEEKLY</c>.
    /// </summary>
    [JsonPropertyName("recurrence")]
    public string? Recurrence { get; init; }

    /// <summary>
    /// The start date for the recurrence in <c>YYYY-MM-DD</c> format. Documented as optional, but
    /// the live API rejects the call with <c>[startDate] This field cannot be empty</c> whenever
    /// <see cref="Recurrence"/> is set. Always provide it together with a non-null recurrence.
    /// </summary>
    [JsonPropertyName("startDate")]
    public string? StartDate { get; init; }

    /// <summary>
    /// Optional end date for the recurrence in <c>YYYY-MM-DD</c> format. Repetition stops after this date.
    /// </summary>
    [JsonPropertyName("endDate")]
    public string? EndDate { get; init; }

    /// <summary>
    /// The next order will be created this many days before the start date. Leave empty or 0 to
    /// create on the start date itself.
    /// </summary>
    [JsonPropertyName("daysBefore")]
    public int? DaysBefore { get; init; }

    /// <summary>
    /// Notification target type. Possible values: <c>NONE</c>, <c>USER</c>, <c>PERSON</c>,
    /// <c>RESPONSIBLE_PERSON</c>, <c>EMAIL</c>.
    /// </summary>
    [JsonPropertyName("notifyType")]
    public string? NotifyType { get; init; }

    /// <summary>The ID of the user to notify (for <c>notifyType=USER</c>).</summary>
    [JsonPropertyName("notifyUserId")]
    public int? NotifyUserId { get; init; }

    /// <summary>The ID of the person to notify (for <c>notifyType=PERSON</c>).</summary>
    [JsonPropertyName("notifyPersonId")]
    public int? NotifyPersonId { get; init; }

    /// <summary>The e-mail address to notify (for <c>notifyType=EMAIL</c>).</summary>
    [JsonPropertyName("notifyEmail")]
    public string? NotifyEmail { get; init; }
}
