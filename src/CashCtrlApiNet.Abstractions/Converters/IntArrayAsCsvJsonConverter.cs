using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CashCtrlApiNet.Abstractions.Converters;

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
