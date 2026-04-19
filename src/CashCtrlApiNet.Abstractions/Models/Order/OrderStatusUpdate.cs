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

using System.Collections.Immutable;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Converters;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Order;

/// <summary>
/// Request body for <c>/order/update_status.json</c> — changes the status of one or multiple
/// orders in a single call.
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/update_status.json">API Doc</a>
/// </summary>
public record OrderStatusUpdate : ModelBaseRecord
{
    /// <summary>
    /// The IDs of the orders to update, comma-separated on the wire. Mandatory.
    /// </summary>
    [JsonPropertyName("ids")]
    [JsonConverter(typeof(IntArrayAsCsvJsonConverter))]
    public required ImmutableArray<int> Ids { get; init; }

    /// <summary>
    /// The ID of the new status. Mandatory.
    /// </summary>
    [JsonPropertyName("statusId")]
    public required int StatusId { get; init; }
}
