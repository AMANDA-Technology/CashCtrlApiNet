---
title: "ADR-004: Custom DateTime Converter for CashCtrl Format"
tags: [architecture, adr, serialization, datetime]
---

# ADR-004: Custom DateTime Converter for CashCtrl Format

## Context

The CashCtrl API returns datetime values in a non-standard format: `"2024-01-07 20:21:38.0"` (pattern: `yyyy-MM-dd HH:mm:ss.f`). This is neither ISO 8601 nor a standard `System.Text.Json` recognized format.

## Decision

Two custom `JsonConverter<T>` implementations handle this:

- `CashCtrlDateTimeConverter` -- Converts `DateTime` to/from the CashCtrl format.
- `CashCtrlDateTimeNullableConverter` -- Converts `DateTime?` to/from the CashCtrl format (returns `null` for empty/missing values).

Both are registered globally in `CashCtrlSerialization.DefaultSerializerOptions` and applied during deserialization of all API responses.

On parse failure, `CashCtrlDateTimeConverter` returns `DateTime.MinValue` and `CashCtrlDateTimeNullableConverter` returns `null` (silent fallback, no exception).

## Consequences

**Positive:**
- CashCtrl datetime values are transparently handled without any per-property configuration.
- The format string is defined once as a constant (`CashCtrlDateTimeConverter.CashCtrlDateTimeFormat`) and reused.

**Negative:**
- Silent fallback to `DateTime.MinValue` / `null` on parse failure may mask data issues.
- The format only has single-digit fractional second (`.f`), which may not handle all CashCtrl responses if they vary precision.
- These converters apply globally to all `DateTime` / `DateTime?` properties in deserialization, which could cause issues if a non-CashCtrl datetime format is ever encountered.
