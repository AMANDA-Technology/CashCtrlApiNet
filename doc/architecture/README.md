---
title: Architecture Overview - CashCtrlApiNet
tags: [architecture, overview, dotnet]
---

# Architecture Overview

## Purpose

CashCtrlApiNet is an unofficial .NET client library for the [CashCtrl REST API v1](https://app.cashctrl.com/static/help/en/api/index.html). It provides:

- Typed C# models for all CashCtrl API entities
- A connection handler managing HTTP authentication, serialization, and request construction
- A hierarchical client API organized by CashCtrl domain groups (Account, Inventory, Order, etc.)
- ASP.NET Core dependency injection integration (planned, not yet implemented)

## Key Constraints

| Constraint                     | Detail                                                                 |
| ------------------------------ | ---------------------------------------------------------------------- |
| Target framework               | .NET 9 (C# 13)                                                        |
| CashCtrl API version           | v1 only                                                                |
| Authentication                 | HTTP Basic Auth (API key as username, empty password)                   |
| POST encoding                  | `application/x-www-form-urlencoded` (not JSON)                         |
| Serialization                  | `System.Text.Json` with custom converters for CashCtrl date format     |
| Immutability                   | All models are C# `record` types with `init` properties                |
| NuGet distribution             | Three packages: Abstractions, Client, AspNetCore                       |
| License                        | MIT                                                                    |

## Tech Stack

| Component          | Technology                       |
| ------------------ | -------------------------------- |
| Language/Runtime   | C# 13 / .NET 9                   |
| HTTP client        | `System.Net.Http.HttpClient`      |
| JSON               | `System.Text.Json`                |
| Testing            | xUnit 2.9, FluentAssertions 6.12  |
| Code coverage      | Coverlet 6.0                      |
| Build system       | MSBuild (SDK-style)               |
| Package format     | NuGet                             |

## Architecture Documentation

- [[context]] -- C4 Level 1: System context diagram
- [[containers]] -- C4 Level 2: Container (package) diagram
- [[components/cashctrlapinet]] -- C4 Level 3: CashCtrlApiNet client library internals
- [[components/abstractions]] -- C4 Level 3: CashCtrlApiNet.Abstractions internals
- [[decisions/001-record-types-for-models]] -- ADR: Record types for all models
- [[decisions/002-connector-service-pattern]] -- ADR: Hierarchical connector/service pattern
- [[decisions/003-form-encoded-post-bodies]] -- ADR: Form-encoded POST instead of JSON
- [[decisions/004-custom-datetime-converter]] -- ADR: Custom DateTime converter for CashCtrl format
- [[glossary]] -- Domain terminology
