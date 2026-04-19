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
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Order;

/// <summary>
/// An order dossier — a group of related orders (e.g. an offer, its continuation invoice, and a
/// credit note) sharing a common <c>groupId</c>. Returned by <c>/order/dossier.json</c> as a
/// single object <c>{id, items: [...]}</c> (not a typical list response).
/// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/dossier.json">API Doc</a>
/// </summary>
public record OrderDossier : ModelBaseRecord
{
    /// <summary>The group ID of the dossier.</summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>The orders that belong to this dossier, ordered by <c>pos</c>.</summary>
    [JsonPropertyName("items")]
    public ImmutableArray<OrderDossierItem> Items { get; init; } = [];
}
