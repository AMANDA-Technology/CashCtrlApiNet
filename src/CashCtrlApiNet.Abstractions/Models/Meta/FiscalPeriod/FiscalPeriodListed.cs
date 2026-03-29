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

namespace CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod;

/// <summary>
/// Fiscal period listed (list response). <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod/list.json">API Doc</a>
/// </summary>
public record FiscalPeriodListed : FiscalPeriodUpdate
{
    /// <summary>
    /// The date and time the fiscal period was created.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]
    public DateTime? Created { get; init; }

    /// <summary>
    /// The user who created the fiscal period.
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }

    /// <summary>
    /// The date and time the fiscal period was last updated.
    /// </summary>
    [JsonPropertyName("lastUpdated")]
    [JsonConverter(typeof(CashCtrlDateTimeNullableConverter))]
    public DateTime? LastUpdated { get; init; }

    /// <summary>
    /// The user who last updated the fiscal period.
    /// </summary>
    [JsonPropertyName("lastUpdatedBy")]
    public string? LastUpdatedBy { get; init; }

    /// <summary>
    /// The start datetime of the fiscal period (from API response).
    /// </summary>
    [JsonPropertyName("start")]
    public string? Start { get; init; }

    /// <summary>
    /// The end datetime of the fiscal period (from API response).
    /// </summary>
    [JsonPropertyName("end")]
    public string? End { get; init; }

    /// <summary>
    /// Whether the fiscal period is complete.
    /// </summary>
    [JsonPropertyName("isComplete")]
    public bool? IsComplete { get; init; }

    /// <summary>
    /// Whether the fiscal period is the current one.
    /// </summary>
    [JsonPropertyName("isCurrent")]
    public bool? IsCurrent { get; init; }

    /// <summary>
    /// The name of the fiscal period.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
