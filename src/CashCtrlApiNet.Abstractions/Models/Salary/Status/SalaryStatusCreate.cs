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
using CashCtrlApiNet.Abstractions.Enums.Salary;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Salary.Status;

/// <summary>
/// Salary status create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/status/create.json">API Doc</a>
/// </summary>
public record SalaryStatusCreate : ModelBaseRecord
{
    /// <summary>
    /// The icon color of the status.
    /// </summary>
    [JsonPropertyName("icon")]
    public required SalaryStatusIcon Icon { get; init; }

    /// <summary>
    /// The name of the status.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(40)]
    public required string Name { get; init; }

    /// <summary>
    /// The ID of the action to execute when the status is set.
    /// </summary>
    [JsonPropertyName("actionId")]
    public string? ActionId { get; init; }

    /// <summary>
    /// Whether to book the salary when this status is set.
    /// </summary>
    [JsonPropertyName("isBook")]
    public bool? IsBook { get; init; }

    /// <summary>
    /// Whether this status marks the salary as closed.
    /// </summary>
    [JsonPropertyName("isClosed")]
    public bool? IsClosed { get; init; }
}
