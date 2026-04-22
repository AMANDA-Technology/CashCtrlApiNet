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

namespace CashCtrlApiNet.Abstractions.Models.Common.TaxRate;

/// <summary>
/// Tax rate component. Represents a single component of a tax rate with its own code, account, calculation type, and apply rule.
/// <br/>Used as a JSON array element in <see cref="TaxRateCreate.Components"/>.
/// </summary>
public record TaxRateComponent
{
    /// <summary>
    /// The code of this tax rate component (e.g. an account number like "200,303").
    /// </summary>
    [JsonPropertyName("code")]
    [MaxLength(32)]
    public required string Code { get; init; }

    /// <summary>
    /// The ID of the account used for this tax rate component.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required int AccountId { get; init; }

    /// <summary>
    /// The calculation type (NET or GROSS).
    /// </summary>
    [JsonPropertyName("calcType")]
    public required TaxCalcType CalcType { get; init; }

    /// <summary>
    /// The apply rule that determines when this tax component is applied.
    /// </summary>
    [JsonPropertyName("applyRule")]
    public required TaxApplyRule ApplyRule { get; init; }
}
