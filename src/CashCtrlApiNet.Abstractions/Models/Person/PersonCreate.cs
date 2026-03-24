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

namespace CashCtrlApiNet.Abstractions.Models.Person;

/// <summary>
/// Person create. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/create.json">API Doc</a>
/// </summary>
public record PersonCreate : ModelBaseRecord
{
    /// <summary>
    /// The first name of the person.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("firstName")]
    [MaxLength(50)]
    public string? FirstName { get; init; }

    /// <summary>
    /// The last name of the person.
    /// <br/>This can contain localized text. To add values in multiple languages, use the XML format like this: &lt;values&gt;&lt;de&gt;German text&lt;/de&gt;&lt;en&gt;English text&lt;/en&gt;&lt;/values&gt;
    /// </summary>
    [JsonPropertyName("lastName")]
    [MaxLength(100)]
    public string? LastName { get; init; }

    /// <summary>
    /// The ID of the category. See Person category.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; init; }

    /// <summary>
    /// The company name of the person.
    /// </summary>
    [JsonPropertyName("company")]
    [MaxLength(100)]
    public string? Company { get; init; }

    /// <summary>
    /// The email address of the person.
    /// </summary>
    [JsonPropertyName("email")]
    [MaxLength(100)]
    public string? Email { get; init; }

    /// <summary>
    /// The phone number of the person.
    /// </summary>
    [JsonPropertyName("phone")]
    [MaxLength(20)]
    public string? Phone { get; init; }

    /// <summary>
    /// The mobile phone number of the person.
    /// </summary>
    [JsonPropertyName("mobile")]
    [MaxLength(20)]
    public string? Mobile { get; init; }

    /// <summary>
    /// The fax number of the person.
    /// </summary>
    [JsonPropertyName("fax")]
    [MaxLength(20)]
    public string? Fax { get; init; }

    /// <summary>
    /// The URL / website of the person.
    /// </summary>
    [JsonPropertyName("url")]
    [MaxLength(255)]
    public string? Url { get; init; }

    /// <summary>
    /// Some optional notes.
    /// <br/>This can contain limited HTML for styling.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? NotesHtml { get; init; }

    /// <summary>
    /// Custom field values. They are stored as XML in this parameter.
    /// </summary>
    [JsonPropertyName("custom")]
    public string? CustomXml { get; init; }

    /// <summary>
    /// The ID of the title. See Person title.
    /// </summary>
    [JsonPropertyName("titleId")]
    public int? TitleId { get; init; }

    /// <summary>
    /// The address of the person.
    /// </summary>
    [JsonPropertyName("address")]
    [MaxLength(100)]
    public string? Address { get; init; }

    /// <summary>
    /// The zip code of the person.
    /// </summary>
    [JsonPropertyName("zipcode")]
    [MaxLength(10)]
    public string? Zipcode { get; init; }

    /// <summary>
    /// The city of the person.
    /// </summary>
    [JsonPropertyName("city")]
    [MaxLength(50)]
    public string? City { get; init; }

    /// <summary>
    /// The country of the person.
    /// </summary>
    [JsonPropertyName("country")]
    [MaxLength(50)]
    public string? Country { get; init; }
}
