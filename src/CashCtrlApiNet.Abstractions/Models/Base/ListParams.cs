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

namespace CashCtrlApiNet.Abstractions.Models.Base;

/// <summary>
/// Common subset of optional filter and pagination parameters applicable to many (but not all) CashCtrl API list endpoints.
/// Not all properties apply to every endpoint; properties left as <c>null</c> are safely omitted from the request.
/// Some endpoints accept additional endpoint-specific parameters not covered by this record.
/// Endpoints with mandatory parameters (e.g., <c>importId</c>, <c>id</c>) use dedicated request types instead of <see cref="ListParams"/>.
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html">API Doc</a>
/// </summary>
public record ListParams : ModelBaseRecord
{
    /// <summary>
    /// The ID of the category to filter by.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; init; }

    /// <summary>
    /// The sort direction. Possible values are 'ASC' (ascending) or 'DESC' (descending).
    /// </summary>
    [JsonPropertyName("dir")]
    public string? Dir { get; init; }

    /// <summary>
    /// The filter expression to search for.
    /// </summary>
    [JsonPropertyName("filter")]
    public string? Filter { get; init; }

    /// <summary>
    /// The ID of the fiscal period to filter by.
    /// </summary>
    [JsonPropertyName("fiscalPeriodId")]
    public int? FiscalPeriodId { get; init; }

    /// <summary>
    /// The maximum number of items to return.
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    /// <summary>
    /// Whether to return only active entries.
    /// </summary>
    [JsonPropertyName("onlyActive")]
    public bool? OnlyActive { get; init; }

    /// <summary>
    /// Whether to return only cost centers.
    /// </summary>
    [JsonPropertyName("onlyCostCenters")]
    public bool? OnlyCostCenters { get; init; }

    /// <summary>
    /// Whether to return only notes.
    /// </summary>
    [JsonPropertyName("onlyNotes")]
    public bool? OnlyNotes { get; init; }

    /// <summary>
    /// The full-text search query.
    /// </summary>
    [JsonPropertyName("query")]
    public string? Query { get; init; }

    /// <summary>
    /// The column to sort by.
    /// </summary>
    [JsonPropertyName("sort")]
    public string? Sort { get; init; }

    /// <summary>
    /// The index of the first item to return (for pagination).
    /// </summary>
    [JsonPropertyName("start")]
    public int? Start { get; init; }
}
