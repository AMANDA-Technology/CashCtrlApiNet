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
/// Salary statement calculate. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement/calculate.json">API Doc</a>
/// </summary>
public record SalaryStatementCalculate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// Custom field values. They are stored as XML in this parameter.
    /// </summary>
    [JsonPropertyName("custom")]
    public string? Custom { get; init; }

    /// <summary>
    /// The date of the statement (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// The payment date of the statement (format: YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("datePayment")]
    public string? DatePayment { get; init; }

    /// <summary>
    /// The ID of the statement.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// The ID of the person.
    /// </summary>
    [JsonPropertyName("personId")]
    public int? PersonId { get; init; }

    /// <summary>
    /// Whether to recalculate the statement. Defaults to false.
    /// </summary>
    [JsonPropertyName("recalculate")]
    public bool? Recalculate { get; init; }

    /// <summary>
    /// The ID of the salary template.
    /// </summary>
    [JsonPropertyName("templateId")]
    public int? TemplateId { get; init; }

    /// <summary>
    /// The salary types configuration as JSON.
    /// </summary>
    [JsonPropertyName("types")]
    public string? Types { get; init; }
}
