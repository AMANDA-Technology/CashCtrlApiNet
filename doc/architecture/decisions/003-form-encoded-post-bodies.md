---
title: "ADR-003: Form-Encoded POST Bodies"
tags: [architecture, adr, http, serialization]
---

# ADR-003: Form-Encoded POST Bodies Instead of JSON

## Context

The CashCtrl REST API accepts POST data as `application/x-www-form-urlencoded` content, not as JSON request bodies. This is an uncommon pattern for modern REST APIs but is what CashCtrl requires.

## Decision

POST requests serialize the payload to `FormUrlEncodedContent` using a two-step process:

1. The payload object is serialized to a JSON string via `CashCtrlSerialization.Serialize<T>()`.
2. The JSON string is deserialized into a `Dictionary<string, object?>` via `CashCtrlSerialization.Deserialize<Dictionary<string, object?>>()`.
3. The dictionary values are converted to strings (`e.Value?.ToString()`).
4. The resulting `Dictionary<string, string?>` is passed to `new FormUrlEncodedContent(dictionary)`.

This round-trip through JSON ensures that `[JsonPropertyName]` attributes on model properties are respected for the form field names, maintaining consistency between GET query parameters and POST body fields.

GET query parameters use the same `ConvertToDictionary` approach, appending the key-value pairs to the URI query string.

## Consequences

**Positive:**
- Single source of truth for field names (`[JsonPropertyName]` attributes).
- Works correctly with the CashCtrl API's expected format.
- Consistent serialization approach for both GET and POST.

**Negative:**
- The JSON round-trip (`serialize -> deserialize as dictionary`) is inefficient compared to direct reflection-based form encoding.
- Complex nested objects (arrays, nested records) may not serialize correctly to form encoding. The `IntArrayAsCsvJsonConverter` handles one case (comma-separated int arrays), but this approach may not generalize to all data types.
- `CashCtrlSerialization.Serialize` does not use the custom `DefaultSerializerOptions` (it calls `JsonSerializer.Serialize(data)` without options), so `WhenWritingNull` and custom converters are not applied during the serialize step. Only `Deserialize` uses the custom options.
