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

namespace CashCtrlApiNet.Abstractions.Models.Inventory;

// TODO: Cleanup this auto generated mess
public record Article : ModelBaseRecord
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("created")]
    public string? Created { get; set; }

    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }

    [JsonPropertyName("lastUpdated")]
    public string? LastUpdated { get; set; }

    [JsonPropertyName("lastUpdatedBy")]
    public string? LastUpdatedBy { get; set; }

    [JsonPropertyName("categoryId")]
    public object? CategoryId { get; set; }

    [JsonPropertyName("thumbnailFileId")]
    public object? ThumbnailFileId { get; set; }

    [JsonPropertyName("categoryDisplay")]
    public object? CategoryDisplay { get; set; }

    [JsonPropertyName("unitId")]
    public int? UnitId { get; set; }

    [JsonPropertyName("unitName")]
    public string? UnitName { get; set; }

    [JsonPropertyName("salesAccountId")]
    public object?SalesAccountId { get; set; }

    [JsonPropertyName("purchaseAccountId")]
    public object? PurchaseAccountId { get; set; }

    [JsonPropertyName("locationId")]
    public object? LocationId { get; set; }

    [JsonPropertyName("locationName")]
    public object? LocationName { get; set; }

    [JsonPropertyName("currencyId")]
    public object? CurrencyId { get; set; }

    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("dateAdded")]
    public string? DateAdded { get; set; }

    [JsonPropertyName("nr")]
    public string? Nr { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("notes")]
    public object? Notes { get; set; }

    [JsonPropertyName("salesPrice")]
    public double? SalesPrice { get; set; }

    [JsonPropertyName("lastPurchasePrice")]
    public double? LastPurchasePrice { get; set; }

    [JsonPropertyName("stock")]
    public object? Stock { get; set; }

    [JsonPropertyName("minStock")]
    public object? MinStock { get; set; }

    [JsonPropertyName("maxStock")]
    public object? MaxStock { get; set; }

    [JsonPropertyName("binLocation")]
    public object? BinLocation { get; set; }

    [JsonPropertyName("custom")]
    public string? Custom { get; set; }

    [JsonPropertyName("attachmentCount")]
    public int? AttachmentCount { get; set; }

    [JsonPropertyName("allocationCount")]
    public int? AllocationCount { get; set; }

    [JsonPropertyName("costCenterIds")]
    public object? CostCenterIds { get; set; }

    [JsonPropertyName("costCenterNumbers")]
    public object? CostCenterNumbers { get; set; }

    [JsonPropertyName("isStockArticle")]
    public bool? IsStockArticle { get; set; }

    [JsonPropertyName("isInactive")]
    public bool? IsInactive { get; set; }

    [JsonPropertyName("isSalesPriceGross")]
    public bool? IsSalesPriceGross { get; set; }

    [JsonPropertyName("isPurchasePriceGross")]
    public bool? IsPurchasePriceGross { get; set; }
}
