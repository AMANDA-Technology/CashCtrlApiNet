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

namespace CashCtrlApiNet.Abstractions.Models.Salary.Template;

/// <summary>
/// Salary template create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template/create.json">API Doc</a>
/// </summary>
public record SalaryTemplateCreate : ModelBaseRecord
{
    /// <summary>
    /// The name of the template.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public required string Name { get; init; }

    /// <summary>
    /// The ID of the currency. Leave empty to use the default currency.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public int? CurrencyId { get; init; }

    /// <summary>
    /// The day of the month for the salary statement (1-31).
    /// </summary>
    [JsonPropertyName("dayOfMonth")]
    public int? DayOfMonth { get; init; }

    /// <summary>
    /// The ID of the footer template.
    /// </summary>
    [JsonPropertyName("footerTemplateId")]
    public int? FooterTemplateId { get; init; }

    /// <summary>
    /// The ID of the header template.
    /// </summary>
    [JsonPropertyName("headerTemplateId")]
    public int? HeaderTemplateId { get; init; }

    /// <summary>
    /// The insurance configuration as JSON.
    /// </summary>
    [JsonPropertyName("insurances")]
    public string? Insurances { get; init; }

    /// <summary>
    /// Whether this is the default template. Defaults to false.
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool? IsDefault { get; init; }

    /// <summary>
    /// Mark the template as inactive. Defaults to false.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }

    /// <summary>
    /// The ID of the layout.
    /// </summary>
    [JsonPropertyName("layoutId")]
    public int? LayoutId { get; init; }

    /// <summary>
    /// The subject of the mail.
    /// </summary>
    [JsonPropertyName("mailSubject")]
    [MaxLength(255)]
    public string? MailSubject { get; init; }

    /// <summary>
    /// The ID of the mail template.
    /// </summary>
    [JsonPropertyName("mailTemplateId")]
    public int? MailTemplateId { get; init; }

    /// <summary>
    /// A message to include on the salary statement.
    /// </summary>
    [JsonPropertyName("message")]
    [MaxLength(50)]
    public string? Message { get; init; }

    /// <summary>
    /// The ID of the organization location.
    /// </summary>
    [JsonPropertyName("orgLocationId")]
    public int? OrgLocationId { get; init; }

    /// <summary>
    /// The ID of the parent template.
    /// </summary>
    [JsonPropertyName("parentId")]
    public int? ParentId { get; init; }

    /// <summary>
    /// The day of the month for the payment (1-31).
    /// </summary>
    [JsonPropertyName("paymentDayOfMonth")]
    public int? PaymentDayOfMonth { get; init; }
}
