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

namespace CashCtrlApiNet.Abstractions.Models.Salary.Type;

/// <summary>
/// Salary type create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type/create.json">API Doc</a>
/// </summary>
public record SalaryTypeCreate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the category.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public required int CategoryId { get; init; }

    /// <summary>
    /// The name of the salary type.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public required string Name { get; init; }

    /// <summary>
    /// The number of the salary type.
    /// </summary>
    [JsonPropertyName("number")]
    [MaxLength(20)]
    public required string Number { get; init; }

    /// <summary>
    /// The kind of salary type (add or subtract).
    /// </summary>
    [JsonPropertyName("type")]
    public required SalaryTypeKind Type { get; init; }

    /// <summary>
    /// Allocations as a JSON string.
    /// </summary>
    [JsonPropertyName("allocations")]
    public string? Allocations { get; init; }

    /// <summary>
    /// The base variable name for calculation.
    /// </summary>
    [JsonPropertyName("base")]
    [MaxLength(32)]
    public string? Base { get; init; }

    /// <summary>
    /// The calculation formula.
    /// </summary>
    [JsonPropertyName("calculation")]
    public string? Calculation { get; init; }

    /// <summary>
    /// The certificate code.
    /// </summary>
    [JsonPropertyName("certificateCode")]
    [MaxLength(10)]
    public string? CertificateCode { get; init; }

    /// <summary>
    /// The ID of the credit account.
    /// </summary>
    [JsonPropertyName("creditId")]
    public int? CreditId { get; init; }

    /// <summary>
    /// The ID of the debit account.
    /// </summary>
    [JsonPropertyName("debitId")]
    public int? DebitId { get; init; }

    /// <summary>
    /// The description of the salary type.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("description")]
    [MaxLength(512)]
    public string? Description { get; init; }

    /// <summary>
    /// The fields configuration as a JSON string.
    /// </summary>
    [JsonPropertyName("fields")]
    public string? Fields { get; init; }

    /// <summary>
    /// The ID of the insurance type.
    /// </summary>
    [JsonPropertyName("insuranceTypeId")]
    public int? InsuranceTypeId { get; init; }

    /// <summary>
    /// Mark the salary type as inactive.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }
}
