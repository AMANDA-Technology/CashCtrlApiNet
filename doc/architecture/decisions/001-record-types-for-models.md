---
title: "ADR-001: Record Types for All Models"
tags: [architecture, adr, models, records]
---

# ADR-001: Record Types for All Models

## Context

The library needs data types to represent CashCtrl API entities (articles, accounts, categories, etc.) and API responses. These types are serialized to/from JSON and used as parameters and return values throughout the client.

Key considerations:
- CashCtrl entities are received from the API (read-only in most contexts).
- Update operations need to modify a few fields and send back.
- Equality comparisons should be value-based (comparing field values, not references).
- The codebase targets C# 13 / .NET 9 with modern language features available.

## Decision

All models and API response types are defined as C# `record` types (not classes or structs). Properties use `init` accessors (not `set`) to enforce immutability after construction. The `required` keyword is used on mandatory properties. Collections use `ImmutableArray<T>`.

Base types enforce this pattern:
- `ModelBaseRecord` -- abstract record base for domain models
- `ApiResponse` -- abstract record base for API response payloads

Both are intentionally empty abstract records that serve as type constraints (similar to marker interfaces but ensuring inheritors are also records).

## Consequences

**Positive:**
- Value-based equality is built-in, useful for comparing API responses.
- `with` expressions allow non-destructive mutation (e.g., casting an `Article` down to `ArticleUpdate` and changing a field).
- `init` accessors prevent accidental mutation after object creation.
- `required` properties catch missing fields at compile time.
- Records generate `ToString()`, `Equals()`, and `GetHashCode()` automatically.

**Negative:**
- Record inheritance has some quirks with `with` expressions across type hierarchies.
- The `new required` keyword on `ArticleUpdate.Nr` shadowing `ArticleCreate.Nr` is unusual and may confuse contributors.
- Positional records (used for `NoContentResponse`, `ResponseError`, `ArticleCategoryAllocation`, `ArticleAttachment`) mix with init-property records, creating two styles in the same codebase.
