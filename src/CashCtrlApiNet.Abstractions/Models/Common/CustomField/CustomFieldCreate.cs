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
using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Common.CustomField;

/// <summary>
/// Custom field create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/create.json">API Doc</a>
/// </summary>
public record CustomFieldCreate : ModelBaseRecord
{
    /// <summary>
    /// The data type of the custom field.
    /// </summary>
    [JsonPropertyName("dataType")]
    public required CustomFieldDataType DataType { get; init; }

    /// <summary>
    /// The row label of the custom field.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("rowLabel")]
    [MaxLength(100)]
    public required string RowLabel { get; init; }

    /// <summary>
    /// The entity type this custom field applies to.
    /// </summary>
    [JsonPropertyName("type")]
    public required CustomFieldType Type { get; init; }

    /// <summary>
    /// Additional information about the field.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("fieldInfo")]
    [MaxLength(240)]
    public string? FieldInfo { get; init; }

    /// <summary>
    /// The label for the field.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("fieldLabel")]
    [MaxLength(100)]
    public string? FieldLabel { get; init; }

    /// <summary>
    /// The text displayed to the right of the field.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("fieldTextRight")]
    [MaxLength(20)]
    public string? FieldTextRight { get; init; }

    /// <summary>
    /// The ID of the custom field group.
    /// </summary>
    [JsonPropertyName("groupId")]
    public int? GroupId { get; init; }

    /// <summary>
    /// Whether the custom field is inactive. Defaults to false.
    /// </summary>
    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; init; }

    /// <summary>
    /// Whether this is a multi-value field. Defaults to false.
    /// </summary>
    [JsonPropertyName("isMulti")]
    public bool? IsMulti { get; init; }

    /// <summary>
    /// The maximum width of the field in pixels (minimum 30).
    /// </summary>
    [JsonPropertyName("maxWidth")]
    public int? MaxWidth { get; init; }

    /// <summary>
    /// The predefined values for COMBOBOX fields as a JSON array.
    /// </summary>
    [JsonPropertyName("values")]
    public string? Values { get; init; }

    /// <summary>
    /// The variable name of the custom field for use in templates.
    /// </summary>
    [JsonPropertyName("variableName")]
    [MaxLength(32)]
    public string? VariableName { get; init; }
}
