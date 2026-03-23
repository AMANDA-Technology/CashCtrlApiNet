---
title: Domain Glossary - CashCtrlApiNet
tags: [architecture, glossary, domain]
---

# Domain Glossary

## CashCtrl Platform Concepts

| Term              | Definition                                                                                             |
| ----------------- | ------------------------------------------------------------------------------------------------------ |
| CashCtrl          | A Swiss cloud ERP platform for accounting, invoicing, inventory management, and business administration. Website: [cashctrl.com](https://cashctrl.com/) |
| Organization      | A CashCtrl tenant. Each organization has its own subdomain (e.g., `myorg.cashctrl.com`) and isolated data. |
| API Key           | Authentication credential for the CashCtrl REST API. Used as the username in HTTP Basic Auth with an empty password. |
| Requests Left     | Rate limiting counter returned in the `X-CashCtrl-Requests-Left` response header. Indicates remaining API calls before throttling. |

## CashCtrl API Domain Groups

| Group     | API Path Prefix            | Description                                                    |
| --------- | -------------------------- | -------------------------------------------------------------- |
| Account   | `/api/v1/account/`         | Chart of accounts, account categories, cost centers.           |
| Common    | `/api/v1/`                 | Currencies, custom fields, rounding rules, sequence numbers, tax rates, text templates. |
| File      | `/api/v1/file/`            | File uploads and file categories.                              |
| Inventory | `/api/v1/inventory/`       | Articles, article categories, fixed assets, units, imports.    |
| Journal   | `/api/v1/journal/`         | Journal entries, journal imports.                               |
| Meta      | `/api/v1/`                 | Fiscal periods, locations, organization settings.              |
| Order     | `/api/v1/order/`           | Orders (invoices, quotes, etc.), book entries, document templates. |
| Person    | `/api/v1/person/`          | Contacts/persons, person categories, titles, imports.          |
| Report    | `/api/v1/report/`          | Reports, report elements, report sets.                         |

## CashCtrl API Operations

| Operation           | HTTP Method | URL Suffix         | Description                                      |
| ------------------- | ----------- | ------------------ | ------------------------------------------------ |
| Read                | GET         | `read.json`        | Get a single entity by ID.                       |
| List                | GET         | `list.json`        | Get all entities (optionally with query params). |
| Create              | POST        | `create.json`      | Create a new entity. Returns `insertId`.         |
| Update              | POST        | `update.json`      | Update an existing entity by ID.                 |
| Delete              | POST        | `delete.json`      | Delete one or more entities by IDs.              |
| Categorize          | POST        | `categorize.json`  | Move entities to a target category.              |
| Update Attachments  | POST        | `update_attachments.json` | Set file attachments on an entity.        |
| Tree                | GET         | `tree.json`        | Get hierarchical tree of categories.             |

## Library-Specific Concepts

| Term               | Definition                                                                                            |
| ------------------ | ----------------------------------------------------------------------------------------------------- |
| Connector          | A domain group aggregator class (e.g., `InventoryConnector`) that holds references to related entity services. Mirrors CashCtrl API group structure. |
| Service            | An entity-level class (e.g., `ArticleService`) that provides typed CRUD methods for a single CashCtrl entity. Inherits from `ConnectorService`. |
| ConnectionHandler  | The `CashCtrlConnectionHandler` class that manages the `HttpClient`, authentication, request construction, and response parsing. Shared across all services. |
| Endpoint           | A compile-time string constant representing the URL path for a specific API operation (e.g., `"api/v1/inventory/article/read.json"`). |
| ApiResult          | Wrapper record containing HTTP status metadata and optionally the deserialized response data. All service methods return `Task<ApiResult<T>>`. |
| ModelBaseRecord    | Abstract base record for all domain entity models. Enforces that all models are immutable records.     |
| ApiResponse        | Abstract base record for all API response payloads (what comes back in the JSON body).                 |

## CashCtrl Entity Concepts

| Term              | Definition                                                                                             |
| ----------------- | ------------------------------------------------------------------------------------------------------ |
| Article           | An inventory item (product or service) that can be sold or purchased through orders.                   |
| Article Category  | A hierarchical grouping of articles. Inherits sales/purchase accounts and sequence numbers from parents. |
| Fixed Asset       | A long-term asset tracked in inventory (e.g., equipment, vehicles).                                    |
| Account           | A ledger account in the chart of accounts (e.g., revenue, expense, asset accounts).                    |
| Cost Center       | A department or project used for cost allocation and reporting.                                         |
| Journal Entry     | A bookkeeping entry recording a financial transaction.                                                 |
| Order             | A business document: invoice, quote, credit note, delivery note, etc.                                  |
| Person            | A contact (customer, supplier, employee) in the CashCtrl address book.                                |
| Fiscal Period     | An accounting period (typically a financial year) for reporting boundaries.                             |
| Location          | A physical location (e.g., warehouse) where stock articles are stored.                                 |
| Bin Location      | A specific place within a location (e.g., shelf A15, row B04).                                         |
| Sequence Number   | An auto-incrementing number generator used for article numbers, invoice numbers, etc.                  |
| Custom Field      | A user-defined field that extends CashCtrl entities. Stored as XML in the `custom` property.           |
| Allocation        | A cost center allocation rule, distributing costs by share/percentage.                                 |
| Translated Text   | CashCtrl supports multi-language text using XML format: `<values><de>German</de><en>English</en></values>`. This library currently passes these as raw strings (see `// TODO: Implement type for translated texts`). |
