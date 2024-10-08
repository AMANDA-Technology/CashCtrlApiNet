﻿/*
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

using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace CashCtrlApiNet.Abstractions.Models.Inventory.Article;

/// <summary>
/// Article
/// </summary>
public record Article : ArticleListed
{
    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("attachments")]
    public ImmutableArray<ArticleAttachment>? Attachments { get; init; }
}

/// <summary>
/// Article listed
/// </summary>
public record ArticleListed : ArticleUpdate
{
    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("created")]
    public required DateTime Created { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("createdBy")]
    public required string CreatedBy { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("lastUpdated")]
    public DateTime? LastUpdated { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("lastUpdatedBy")]
    public string? LastUpdatedBy { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("thumbnailFileId")]
    public int? ThumbnailFileId { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("categoryDisplay")]
    public string? CategoryDisplay { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("unitName")]
    public string? UnitName { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("salesAccountId")]
    public int? SalesAccountId { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("purchaseAccountId")]
    public int? PurchaseAccountId { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("locationName")]
    public string? LocationName { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("dateAdded")]
    public DateTime? DateAdded { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("attachmentCount")]
    public int? AttachmentCount { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("allocationCount")]
    public int? AllocationCount { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("costCenterIds")]
    public ImmutableArray<int>? CostCenterIds { get; init; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("costCenterNumbers")]
    public ImmutableArray<string>? CostCenterNumbers { get; init; }
}
