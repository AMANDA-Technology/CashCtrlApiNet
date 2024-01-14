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

using System.Text.Json;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Converters;

namespace CashCtrlApiNet.Abstractions.Helpers;

/// <summary>
/// Helper for cash ctrl serialization
/// </summary>
public static class CashCtrlSerialization
{
    /// <summary>
    /// Default instance of json serializer options
    /// </summary>
    private static JsonSerializerOptions DefaultSerializerOptions { get; } = CreateSerializerOptions();

    /// <summary>
    /// Json serializer options with default settings and custom converters
    /// </summary>
    /// <returns></returns>
    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new CashCtrlDateTimeConverter());
        options.Converters.Add(new CashCtrlDateTimeNullableConverter());
        return options;
    }

    /// <summary>
    /// Parses the text representing a single JSON value into an instance of the type specified by a generic type parameter.
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, options: DefaultSerializerOptions);

    /// <summary>
    /// Converts the value of a type specified by a generic type parameter into a JSON string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Serialize<TValue>(TValue? data)
        => JsonSerializer.Serialize(data);

    /// <summary>
    /// Convert data object to dictionary
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Dictionary<string, string?>? ConvertToDictionary<TValue>(TValue? data)
        => data is null
            ? null
            : Deserialize<Dictionary<string, object?>>(Serialize(data))
                ?.ToDictionary(e => e.Key, e => e.Value?.ToString());
}
