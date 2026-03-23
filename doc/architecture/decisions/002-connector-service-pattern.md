---
title: "ADR-002: Hierarchical Connector/Service Pattern"
tags: [architecture, adr, connectors, services]
---

# ADR-002: Hierarchical Connector/Service Pattern

## Context

The CashCtrl API is organized into domain groups (Account, Inventory, Order, etc.), each containing multiple entity endpoints (e.g., Inventory has Article, ArticleCategory, FixedAsset, Unit, etc.). The client library needs a structure that mirrors this hierarchy and provides a discoverable API surface.

## Decision

The API is exposed through a three-level hierarchy:

1. **`ICashCtrlApiClient`** -- Top-level facade. Holds properties for each domain group connector.
2. **`I{Group}Connector`** (e.g., `IInventoryConnector`) -- Domain group aggregator. Holds properties for each entity service within the group.
3. **`I{Entity}Service`** (e.g., `IArticleService`) -- Entity-level service. Provides CRUD methods (`Get`, `GetList`, `Create`, `Update`, `Delete`) plus entity-specific operations (`Categorize`, `UpdateAttachments`).

Consumer usage follows the pattern:
```csharp
var result = await client.Inventory.Article.Get(new Entry { Id = 42 });
```

Implementation details:
- All services inherit from `ConnectorService` abstract base class, which holds a `protected ICashCtrlConnectionHandler`.
- Connectors create their child services in the constructor, passing the shared `ICashCtrlConnectionHandler`.
- Services use static `Endpoint` constants and delegate all HTTP work to `ConnectionHandler.GetAsync/PostAsync`.
- Endpoint paths are defined as compile-time string constants in nested static classes (e.g., `InventoryEndpoints.Article.Read`).

## Consequences

**Positive:**
- The API surface is highly discoverable via IntelliSense (`client.` -> group -> service -> method).
- Mirrors the CashCtrl API documentation structure exactly.
- Each service is small and focused (single entity CRUD).
- Adding a new entity requires only: model records, service interface/class, endpoint constants, and registering in the connector.

**Negative:**
- `CashCtrlApiClient` constructor requires 10 parameters (1 connection handler + 9 connectors), suppressed with `[SuppressMessage("S107")]`.
- Connectors that have not yet implemented their services still expose the interface properties, which are `null` at runtime. This will throw `NullReferenceException` if accessed.
- No DI registration yet (ASP.NET Core project is empty), so consumers must manually construct the full object graph as shown in `CashCtrlTestBase`.
