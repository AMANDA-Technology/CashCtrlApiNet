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
using System.Text.Json;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Abstractions.Models.Inventory;

/// <summary>
/// Article IDs
/// </summary>
public record Articles : ModelBaseRecord
{
    /// <summary>
    /// The IDs of the selected entries, comma-separated.
    /// </summary>
    [JsonPropertyName("ids")]
    // [JsonConverter(typeof(IntArrayAsCsvJsonConverter))] // TODO: really a csv? or json array?
    public required ImmutableArray<int> ArticleIds { get; init; }
}

/// <summary>
/// Json converter for immutable array of int, parsed as comma separated values (CSV) in json
/// </summary>
public class IntArrayAsCsvJsonConverter : JsonConverter<ImmutableArray<int>>
{
    /// <inheritdoc />
    public override ImmutableArray<int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()
            ?.Split(',')
            .Select(val => int.Parse(val.Trim()))
            .ToImmutableArray() ?? [];

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ImmutableArray<int> value, JsonSerializerOptions options)
        => writer.WriteStringValue(string.Join(',', value));
}
