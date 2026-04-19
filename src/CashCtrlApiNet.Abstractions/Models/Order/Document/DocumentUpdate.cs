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

namespace CashCtrlApiNet.Abstractions.Models.Order.Document;

/// <summary>
/// Order document update. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/document/update.json">API Doc</a>
/// </summary>
public record DocumentUpdate : ModelBaseRecord
{
    /// <summary>
    /// The ID of the order whose document to update (documents are keyed 1:1 by order id).
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// The sender address formatted with line breaks (max 255 chars). Required for payment
    /// generation — creating a payment against an order fails with "Sender: Address must be set."
    /// when this is empty.
    /// </summary>
    [JsonPropertyName("orgAddress")]
    public string? OrgAddress { get; init; }

    /// <summary>
    /// The recipient address formatted with line breaks. Similarly required for payment generation.
    /// </summary>
    [JsonPropertyName("recipientAddress")]
    public string? RecipientAddress { get; init; }

    /// <summary>
    /// Header HTML rendered above the items list on the generated document.
    /// </summary>
    [JsonPropertyName("header")]
    public string? Header { get; init; }

    /// <summary>
    /// Footer HTML rendered below the items list on the generated document.
    /// </summary>
    [JsonPropertyName("footer")]
    public string? Footer { get; init; }

    /// <summary>
    /// The layout ID for the generated document.
    /// </summary>
    [JsonPropertyName("layoutId")]
    public int? LayoutId { get; init; }

    /// <summary>
    /// The language of the document. Possible values: <c>DE</c>, <c>EN</c>, <c>FR</c>, <c>IT</c>.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }
}
