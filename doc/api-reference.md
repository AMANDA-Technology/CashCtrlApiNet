> **SUPERSEDED** — This document was written during initial development when the library had partial API coverage. As of 2026-03-24, all 375 endpoints are implemented (100% coverage). For the current authoritative reference, see:
> - **API completeness audit:** [`doc/analysis/2026-03-24-api-completeness-audit.md`](analysis/2026-03-24-api-completeness-audit.md)
> - **Implementation design spec:** [`doc/specs/2026-03-23-full-api-implementation-design.md`](specs/2026-03-23-full-api-implementation-design.md)
> - **Project overview:** [`CLAUDE.md`](../CLAUDE.md)
>
> This file is retained for historical context only. The implementation status tables below are outdated.

# CashCtrl API Reference & Development Instructions

This document maps every CashCtrl REST API endpoint to the codebase, tracks implementation status, and provides step-by-step instructions for completing the client library. It is the authoritative bridge between the [CashCtrl API documentation](https://app.cashctrl.com/static/help/en/api/index.html) (the sole source of truth) and this codebase.

## API Overview

- **OpenAPI/Swagger**: No OpenAPI specification exists. The HTML documentation is the sole source of truth.
- **HTML Docs URL**: https://app.cashctrl.com/static/help/en/api/index.html
- **Base URL Pattern**: `https://{org}.cashctrl.com/api/v1/`
- **Authentication**: HTTP Basic Auth -- API key as username, empty password, sent as `Authorization: Basic <base64(apikey:)>` header.
- **Request Content-Type (POST)**: `application/x-www-form-urlencoded` (NOT JSON).
- **Response Content-Type**: `application/json`.
- **Rate Limiting**: Response header `X-Requests-Left` indicates remaining requests.
- **Language**: Optional `lang` query parameter (`de`, `en`, `fr`, `it`).

---

## Complete Endpoint Catalog

All paths are relative to `api/v1/` (the codebase constant `Api.V1`). Organized by the 9 domain groups in the codebase.

### Account Group

**Account** (Service: `AccountService` -- EXISTS but models are stubs):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `account/read.json` | GET | Read account | YES | YES |
| `account/list.json` | GET | List accounts | YES | YES |
| `account/balance` | GET | Get account balance | YES | YES |
| `account/create.json` | POST | Create account | YES | YES |
| `account/update.json` | POST | Update account | YES | YES |
| `account/delete.json` | POST | Delete accounts | YES | YES |
| `account/categorize.json` | POST | Categorize accounts | YES | YES |
| `account/update_attachments.json` | POST | Update attachments | YES | YES |
| `account/list.xlsx` | GET | Export to Excel | NO | NO |
| `account/list.csv` | GET | Export to CSV | NO | NO |
| `account/list.pdf` | GET | Export to PDF | NO | NO |

Create parameters: `categoryId` (NUMBER, required), `name` (TEXT, required, max 100, localized XML), `number` (NUMBER, required, max 20), `currencyId` (NUMBER), `taxId` (NUMBER), `allocations` (JSON: cost center allocation array with `share` and `toCostCenterId`), `custom` (XML), `notes` (HTML), `targetMin` (NUMBER), `targetMax` (NUMBER), `isInactive` (BOOLEAN).

**Account > Bank Account** (NOT in codebase -- missing interface and endpoints entirely):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `account/bank/read.json` | GET | Read bank account | NO | NO |
| `account/bank/list.json` | GET | List bank accounts | NO | NO |
| `account/bank/create.json` | POST | Create bank account | NO | NO |
| `account/bank/update.json` | POST | Update bank account | NO | NO |
| `account/bank/delete.json` | POST | Delete bank accounts | NO | NO |
| `account/bank/update_attachments.json` | POST | Update attachments | NO | NO |
| `account/bank/list.xlsx` | GET | Export to Excel | NO | NO |
| `account/bank/list.csv` | GET | Export to CSV | NO | NO |
| `account/bank/list.pdf` | GET | Export to PDF | NO | NO |

Create parameters: `bic` (TEXT, required, max 11), `iban` (TEXT, required, max 32), `name` (TEXT, required, max 100, localized XML), `type` (TEXT, required: DEFAULT/ORDER/SALARY/HISTORICAL/OTHER), `accountId` (NUMBER), `currencyId` (NUMBER), `isInactive` (BOOLEAN), `notes` (HTML), `qrFirstDigits` (NUMBER, max 8), `qrIban` (TEXT, max 32), `url` (TEXT, max 255).

**Account > Category**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `account/category/read.json` | GET | Read category | YES | NO |
| `account/category/list.json` | GET | List categories | YES | NO |
| `account/category/tree.json` | GET | Get category tree | YES | NO |
| `account/category/create.json` | POST | Create category | YES | NO |
| `account/category/update.json` | POST | Update category | YES | NO |
| `account/category/delete.json` | POST | Delete categories | YES | NO |

Create parameters: `name` (TEXT, required, max 100, localized XML), `number` (TEXT, max 20), `parentId` (NUMBER).

**Account > Cost Center**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `account/costcenter/read.json` | GET | Read cost center | YES | NO |
| `account/costcenter/list.json` | GET | List cost centers | YES | NO |
| `account/costcenter/balance` | GET | Get balance | YES | NO |
| `account/costcenter/create.json` | POST | Create cost center | YES | NO |
| `account/costcenter/update.json` | POST | Update cost center | YES | NO |
| `account/costcenter/delete.json` | POST | Delete cost centers | YES | NO |
| `account/costcenter/categorize.json` | POST | Categorize | YES | NO |
| `account/costcenter/update_attachments.json` | POST | Update attachments | YES | NO |
| `account/costcenter/list.xlsx` | GET | Export to Excel | NO | NO |
| `account/costcenter/list.csv` | GET | Export to CSV | NO | NO |
| `account/costcenter/list.pdf` | GET | Export to PDF | NO | NO |

**Account > Cost Center Category**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `account/costcenter/category/read.json` | GET | Read category | YES | NO |
| `account/costcenter/category/list.json` | GET | List categories | YES | NO |
| `account/costcenter/category/tree.json` | GET | Get category tree | YES | NO |
| `account/costcenter/category/create.json` | POST | Create category | YES | NO |
| `account/costcenter/category/update.json` | POST | Update category | YES | NO |
| `account/costcenter/category/delete.json` | POST | Delete categories | YES | NO |

### Common Group

**Currency**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `currency/read.json` | GET | Read currency | YES | NO |
| `currency/list.json` | GET | List currencies | YES | NO |
| `currency/create.json` | POST | Create currency | YES | NO |
| `currency/update.json` | POST | Update currency | YES | NO |
| `currency/delete.json` | POST | Delete currencies | YES | NO |
| `currency/exchangerate` | GET | Get exchange rate | YES | NO |

Create parameters: `code` (TEXT, required, 3 chars), `description` (TEXT, max 50, localized XML), `isDefault` (BOOLEAN), `rate` (NUMBER).
Exchange rate parameters: `from` (TEXT, required), `to` (TEXT, required), `date` (DATE).

**Custom Field**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `customfield/read.json` | GET | Read custom field | YES | NO |
| `customfield/list.json` | GET | List custom fields | YES | NO |
| `customfield/create.json` | POST | Create custom field | YES | NO |
| `customfield/update.json` | POST | Update custom field | YES | NO |
| `customfield/delete.json` | POST | Delete custom field | YES | NO |
| `customfield/reorder.json` | POST | Reorder custom fields | YES | NO |
| `customfield/types.json` | GET | Get custom field types | YES | NO |

Create parameters: `dataType` (TEXT, required: TEXT/TEXTAREA/CHECKBOX/DATE/COMBOBOX/NUMBER/ACCOUNT/PERSON), `rowLabel` (TEXT, required, max 100, localized), `type` (TEXT, required: JOURNAL/ACCOUNT/INVENTORY_ARTICLE/INVENTORY_ASSET/ORDER/PERSON/FILE/SALARY_STATEMENT), `fieldInfo` (TEXT, max 240, localized), `fieldLabel` (TEXT, max 100, localized), `fieldTextRight` (TEXT, max 20, localized), `groupId` (TEXT), `isInactive` (BOOLEAN), `isMulti` (BOOLEAN), `maxWidth` (NUMBER, min 30), `values` (TEXT, JSON array for COMBOBOX), `variableName` (TEXT, max 32).

**Custom Field Group**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `customfield/group/read.json` | GET | Read group | YES | NO |
| `customfield/group/list.json` | GET | List groups | YES | NO |
| `customfield/group/create.json` | POST | Create group | YES | NO |
| `customfield/group/update.json` | POST | Update group | YES | NO |
| `customfield/group/delete.json` | POST | Delete group | YES | NO |
| `customfield/group/reorder.json` | POST | Reorder groups | YES | NO |

Create parameters: `name` (TEXT, required, max 50, localized), `type` (TEXT, required: same as Custom Field types).

**Rounding**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `rounding/read.json` | GET | Read rounding | YES | NO |
| `rounding/list.json` | GET | List roundings | YES | NO |
| `rounding/create.json` | POST | Create rounding | YES | NO |
| `rounding/update.json` | POST | Update rounding | YES | NO |
| `rounding/delete.json` | POST | Delete roundings | YES | NO |

**Sequence Number**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `sequencenumber/read.json` | GET | Read sequence number | YES | NO |
| `sequencenumber/list.json` | GET | List sequence numbers | YES | NO |
| `sequencenumber/create.json` | POST | Create sequence number | YES | NO |
| `sequencenumber/update.json` | POST | Update sequence number | YES | NO |
| `sequencenumber/delete.json` | POST | Delete sequence number | YES | NO |
| `sequencenumber/get` | GET | Get generated number | NO (missing) | NO |

**Tax Rate**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `tax/read.json` | GET | Read tax rate | YES | NO |
| `tax/list.json` | GET | List tax rates | YES | NO |
| `tax/create.json` | POST | Create tax rate | YES | NO |
| `tax/update.json` | POST | Update tax rate | YES | NO |
| `tax/delete.json` | POST | Delete tax rate | YES | NO |

**Text Template**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `text/read.json` | GET | Read text template | YES | NO |
| `text/list.json` | GET | List text templates | YES | NO |
| `text/create.json` | POST | Create text template | YES | NO |
| `text/update.json` | POST | Update text template | YES | NO |
| `text/delete.json` | POST | Delete text template | YES | NO |

**History** (NOT in codebase -- missing interface and endpoints):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `history/list.json` | GET | List history | NO | NO |

### File Group

**File**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `file/get` | GET | Get file contents | YES | NO |
| `file/read.json` | GET | Read file metadata | YES | NO |
| `file/list.json` | GET | List files | YES | NO |
| `file/prepare.json` | POST | Prepare files | YES | NO |
| `file/persist.json` | POST | Persist files | YES | NO |
| `file/create.json` | POST | Create file | YES | NO |
| `file/update.json` | POST | Update file | YES | NO |
| `file/delete.json` | POST | Delete files | YES | NO |
| `file/categorize.json` | POST | Categorize files | YES | NO |
| `file/empty_archive.json` | POST | Empty archive | YES | NO |
| `file/restore.json` | POST | Restore files | YES | NO |
| `file/list.xlsx` | GET | Export to Excel | NO | NO |
| `file/list.csv` | GET | Export to CSV | NO | NO |
| `file/list.pdf` | GET | Export to PDF | NO | NO |

Create parameters: `id` (NUMBER, required -- from prepare step), `name` (TEXT), `description` (TEXT), `notes` (HTML), `custom` (XML), `categoryId` (NUMBER).
List parameters: `categoryId` (NUMBER), `dir` (TEXT: ASC/DESC), `filter` (JSON), `onlyNotes` (BOOLEAN), `query` (TEXT), `sort` (TEXT, default: name).

**File > Category**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `file/category/read.json` | GET | Read category | YES | NO |
| `file/category/list.json` | GET | List categories | YES | NO |
| `file/category/tree.json` | GET | Get category tree | YES | NO |
| `file/category/create.json` | POST | Create category | YES | NO |
| `file/category/update.json` | POST | Update category | YES | NO |
| `file/category/delete.json` | POST | Delete categories | YES | NO |

Create parameters: `name` (TEXT, required, max 100, localized), `parentId` (NUMBER).

### Inventory Group

**Article** (FULLY IMPLEMENTED):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `inventory/article/read.json` | GET | Read article | YES | YES |
| `inventory/article/list.json` | GET | List articles | YES | YES |
| `inventory/article/create.json` | POST | Create article | YES | YES |
| `inventory/article/update.json` | POST | Update article | YES | YES |
| `inventory/article/delete.json` | POST | Delete articles | YES | YES |
| `inventory/article/categorize.json` | POST | Categorize articles | YES | YES |
| `inventory/article/update_attachments.json` | POST | Update attachments | YES | YES |
| `inventory/article/list.xlsx` | GET | Export to Excel | NO | NO |
| `inventory/article/list.csv` | GET | Export to CSV | NO | NO |
| `inventory/article/list.pdf` | GET | Export to PDF | NO | NO |

**Article > Category** (IMPLEMENTED):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `inventory/article/category/read.json` | GET | Read category | YES | YES |
| `inventory/article/category/list.json` | GET | List categories | YES | YES |
| `inventory/article/category/tree.json` | GET | Get category tree | YES | NO (endpoint exists, not in service) |
| `inventory/article/category/create.json` | POST | Create category | YES | YES |
| `inventory/article/category/update.json` | POST | Update category | YES | YES |
| `inventory/article/category/delete.json` | POST | Delete categories | YES | YES |

**Fixed Asset**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `inventory/asset/read.json` | GET | Read fixed asset | YES | NO |
| `inventory/asset/list.json` | GET | List fixed assets | YES | NO |
| `inventory/asset/create.json` | POST | Create fixed asset | YES | NO |
| `inventory/asset/update.json` | POST | Update fixed asset | YES | NO |
| `inventory/asset/delete.json` | POST | Delete fixed assets | YES | NO |
| `inventory/asset/categorize.json` | POST | Categorize | YES | NO |
| `inventory/asset/update_attachments.json` | POST | Update attachments | YES | NO |
| `inventory/asset/list.xlsx` | GET | Export to Excel | NO | NO |
| `inventory/asset/list.csv` | GET | Export to CSV | NO | NO |
| `inventory/asset/list.pdf` | GET | Export to PDF | NO | NO |

**Fixed Asset > Category**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `inventory/asset/category/read.json` | GET | Read category | YES | NO |
| `inventory/asset/category/list.json` | GET | List categories | YES | NO |
| `inventory/asset/category/tree.json` | GET | Get category tree | YES | NO |
| `inventory/asset/category/create.json` | POST | Create category | YES | NO |
| `inventory/asset/category/update.json` | POST | Update category | YES | NO |
| `inventory/asset/category/delete.json` | POST | Delete categories | YES | NO |

**Import**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `inventory/article/import/create.json` | POST | Create import | YES | NO |
| `inventory/article/import/mapping.json` | POST | Define mapping | YES | NO |
| `inventory/article/import/mapping_combo.json` | GET | List mapping fields | YES | NO |
| `inventory/article/import/preview.json` | POST | Preview import | YES | NO |
| `inventory/article/import/execute.json` | POST | Execute import | YES | NO |

**Unit**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `inventory/unit/read.json` | GET | Read unit | YES | NO |
| `inventory/unit/list.json` | GET | List units | YES | NO |
| `inventory/unit/create.json` | POST | Create unit | YES | NO |
| `inventory/unit/update.json` | POST | Update unit | YES | NO |
| `inventory/unit/delete.json` | POST | Delete units | YES | NO |

### Journal Group

**Journal**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `journal/read.json` | GET | Read journal entry | YES | NO |
| `journal/list.json` | GET | List journal entries | YES | NO |
| `journal/create.json` | POST | Create journal entry | YES | NO |
| `journal/update.json` | POST | Update journal entry | YES | NO |
| `journal/delete.json` | POST | Delete journal entries | YES | NO |
| `journal/update_attachments.json` | POST | Update attachments | YES | NO |
| `journal/update_recurrence.json` | POST | Update recurrence | YES | NO |
| `journal/list.xlsx` | GET | Export to Excel | NO | NO |
| `journal/list.csv` | GET | Export to CSV | NO | NO |
| `journal/list.pdf` | GET | Export to PDF | NO | NO |

Create parameters: `dateAdded` (DATE, required, YYYY-MM-DD), `title` (TEXT, required, max 100), `sequenceNumberId` (NUMBER, required), `debitId` (NUMBER), `creditId` (NUMBER), `amount` (NUMBER), `taxId` (NUMBER), `items` (JSON: array with accountId, debit, credit for collective entries), `costCenterId` (NUMBER), `custom` (XML), `notes` (HTML), `reference` (TEXT).

**Journal > Import**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `journal/import/read.json` | GET | Read import | YES | NO |
| `journal/import/list.json` | GET | List imports | YES | NO |
| `journal/import/create.json` | POST | Create import | YES | NO |
| `journal/import/execute.json` | POST | Execute import | YES | NO |

Create parameters: `fileId` (NUMBER, required), `name` (TEXT).

**Journal > Import Entry**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `journal/import/entry/read.json` | GET | Read import entry | YES | NO |
| `journal/import/entry/list.json` | GET | List import entries | YES | NO |
| `journal/import/entry/update.json` | POST | Update import entry | YES | NO |
| `journal/import/entry/delete.json` | POST | Ignore import entries | YES | NO |
| `journal/import/entry/restore.json` | POST | Restore import entries | YES | NO |
| `journal/import/entry/confirm.json` | POST | Confirm entries | YES | NO |
| `journal/import/entry/unconfirm.json` | POST | Unconfirm entries | YES | NO |
| `journal/import/entry/list.xlsx` | GET | Export to Excel | NO | NO |
| `journal/import/entry/list.csv` | GET | Export to CSV | NO | NO |
| `journal/import/entry/list.pdf` | GET | Export to PDF | NO | NO |

### Meta Group

**Fiscal Period**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `fiscalperiod/read.json` | GET | Read fiscal period | YES | NO |
| `fiscalperiod/list.json` | GET | List fiscal periods | YES | NO |
| `fiscalperiod/create.json` | POST | Create fiscal period | YES | NO |
| `fiscalperiod/update.json` | POST | Update fiscal period | YES | NO |
| `fiscalperiod/switch.json` | POST | Switch fiscal period | YES | NO |
| `fiscalperiod/delete.json` | POST | Delete fiscal periods | YES | NO |
| `fiscalperiod/result` | GET | Get result | YES | NO |
| `fiscalperiod/depreciations.json` | GET | List depreciations | YES | NO |
| `fiscalperiod/bookdepreciations.json` | POST | Book depreciations | YES | NO |
| `fiscalperiod/exchangediff.json` | GET | List exchange differences | YES | NO |
| `fiscalperiod/bookexchangediff.json` | POST | Book exchange differences | YES | NO |
| `fiscalperiod/complete.json` | POST | Complete fiscal period | YES | NO |
| `fiscalperiod/reopen.json` | POST | Reopen fiscal period | YES | NO |
| `fiscalperiod/complete_months.json` | POST | Complete months | NO (missing) | NO |
| `fiscalperiod/reopen_months.json` | POST | Reopen months | NO (missing) | NO |

Create parameters: `startDate` (DATE, required, YYYY-MM-DD), `endDate` (DATE, required, YYYY-MM-DD).

**Fiscal Period > Task**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `fiscalperiod/task/list.json` | GET | List tasks | YES | NO |
| `fiscalperiod/task/create.json` | POST | Create task | YES | NO |
| `fiscalperiod/task/delete.json` | POST | Delete tasks | YES | NO |

**Location**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `location/read.json` | GET | Read location | YES | NO |
| `location/list.json` | GET | List locations | YES | NO |
| `location/create.json` | POST | Create location | YES | NO |
| `location/update.json` | POST | Update location | YES | NO |
| `location/delete.json` | POST | Delete locations | YES | NO |

Create parameters: `name` (TEXT, required, max 100), `orgName` (TEXT, required, max 100), `logoFileId` (NUMBER).

**Organization**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `domain/current/logo` | GET | Get organization logo | YES | NO |

**Settings**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `setting/read.json` | GET | Read settings | YES | NO |
| `setting/get` | GET | Get setting value | YES | NO |
| `setting/update.json` | POST | Update settings | YES | NO |

### Order Group

**Order**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/read.json` | GET | Read order | YES | NO |
| `order/list.json` | GET | List orders | YES | NO |
| `order/create.json` | POST | Create order | YES | NO |
| `order/update.json` | POST | Update order | YES | NO |
| `order/delete.json` | POST | Delete orders | YES | NO |
| `order/status_info.json` | GET | Read status | YES | NO |
| `order/update_status.json` | POST | Update status | YES | NO |
| `order/update_recurrence.json` | POST | Update recurrence | YES | NO |
| `order/continue.json` | POST | Continue as | YES | NO |
| `order/dossier.json` | GET | Read dossier | YES | NO |
| `order/dossier_add.json` | POST | Add to dossier | YES | NO |
| `order/dossier_remove.json` | POST | Remove from dossier | YES | NO |
| `order/update_attachments.json` | POST | Update attachments | YES | NO |
| `order/list.xlsx` | GET | Export to Excel | NO | NO |
| `order/list.csv` | GET | Export to CSV | NO | NO |
| `order/list.pdf` | GET | Export to PDF | NO | NO |

Create parameters: `accountId` (NUMBER, required), `categoryId` (NUMBER, required), `date` (DATE, required, YYYY-MM-DD), `sequenceNumberId` (NUMBER, required), `associateId` (NUMBER or TEXT), `description` (TEXT), `items` (JSON: array with accountId, taxId, unitId, articleNr, quantity, name, unitPrice), `roundingId` (NUMBER).

**Order > Book Entry**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/bookentry/read.json` | GET | Read book entry | YES | NO |
| `order/bookentry/list.json` | GET | List book entries | YES | NO |
| `order/bookentry/create.json` | POST | Create book entry | YES | NO |
| `order/bookentry/update.json` | POST | Update book entry | YES | NO |
| `order/bookentry/delete.json` | POST | Delete book entries | YES | NO |

Create parameters: `orderId` (NUMBER, required), `accountId` (NUMBER, required), `amount` (NUMBER, required).

**Order > Category**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/category/read.json` | GET | Read category | YES | NO |
| `order/category/list.json` | GET | List categories | YES | NO |
| `order/category/create.json` | POST | Create category | YES | NO |
| `order/category/update.json` | POST | Update category | YES | NO |
| `order/category/delete.json` | POST | Delete categories | YES | NO |
| `order/category/reorder.json` | POST | Reorder categories | YES | NO |
| `order/category/read_status.json` | GET | Read status | YES | NO |

Create parameters: `name` (TEXT, required, max 100, localized).
Reorder parameters: `ids` (CSV, required), `target` (NUMBER, required), `before` (BOOLEAN, default: true).

**Order > Document**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/document/read.json` | GET | Read document | YES | NO |
| `order/document/read.pdf` | GET | Download PDF | NO | NO |
| `order/document/read.zip` | GET | Download ZIP | NO | NO |
| `order/document/mail.json` | POST | Mail document | YES | NO |
| `order/document/update.json` | POST | Update document | YES | NO |

**Order > Document Template**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/template/read.json` | GET | Read template | YES | NO |
| `order/template/list.json` | GET | List templates | YES | NO |
| `order/template/create.json` | POST | Create template | YES | NO |
| `order/template/update.json` | POST | Update template | YES | NO |
| `order/template/delete.json` | POST | Delete templates | YES | NO |

**Order > Layout** (NOT in codebase -- missing interface and endpoints):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/layout/read.json` | GET | Read layout | NO | NO |
| `order/layout/list.json` | GET | List layouts | NO | NO |
| `order/layout/create.json` | POST | Create layout | NO | NO |
| `order/layout/update.json` | POST | Update layout | NO | NO |
| `order/layout/delete.json` | POST | Delete layouts | NO | NO |

**Order > Payment** (NOT in codebase -- missing interface and endpoints):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `order/payment/create.json` | POST | Create payment | NO | NO |
| `order/payment/download` | GET | Download file | NO | NO |

### Person Group

**Person**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `person/read.json` | GET | Read person | YES | NO |
| `person/list.json` | GET | List people | YES | NO |
| `person/create.json` | POST | Create person | YES | NO |
| `person/update.json` | POST | Update person | YES | NO |
| `person/delete.json` | POST | Delete people | YES | NO |
| `person/categorize.json` | POST | Categorize people | YES | NO |
| `person/update_attachments.json` | POST | Update attachments | YES | NO |
| `person/list.xlsx` | GET | Export to Excel | NO | NO |
| `person/list.csv` | GET | Export to CSV | NO | NO |
| `person/list.pdf` | GET | Export to PDF | NO | NO |
| `person/list.vcf` | GET | Export to vCard | NO | NO |

Create parameters: `firstName` (TEXT, max 50, localized XML), `lastName` (TEXT, max 100, localized XML), `categoryId` (NUMBER), `company` (TEXT, max 100), `email` (TEXT, max 100), `phone` (TEXT, max 20), `mobile` (TEXT, max 20), `fax` (TEXT, max 20), `url` (TEXT, max 255), `notes` (HTML), `custom` (XML), `titleId` (NUMBER), `address` (TEXT, max 100), `zipcode` (TEXT, max 10), `city` (TEXT, max 50), `country` (TEXT, max 50).

**Person > Category**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `person/category/read.json` | GET | Read category | YES | NO |
| `person/category/list.json` | GET | List categories | YES | NO |
| `person/category/tree.json` | GET | Get category tree | YES | NO |
| `person/category/create.json` | POST | Create category | YES | NO |
| `person/category/update.json` | POST | Update category | YES | NO |
| `person/category/delete.json` | POST | Delete categories | YES | NO |

Create parameters: `name` (TEXT, required, max 100, localized), `number` (TEXT, max 20), `parentId` (NUMBER).

**Person > Import**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `person/import/create.json` | POST | Create import | YES | NO |
| `person/import/mapping.json` | POST | Define mapping | YES | NO |
| `person/import/mapping_combo.json` | GET | List mapping fields | YES | NO |
| `person/import/preview.json` | POST | Preview import | YES | NO |
| `person/import/execute.json` | POST | Execute import | YES | NO |

Create parameters: `fileId` (NUMBER, required -- vCard .vcf file ID).

**Person > Title**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `person/title/read.json` | GET | Read title | YES | NO |
| `person/title/list.json` | GET | List titles | YES | NO |
| `person/title/create.json` | POST | Create title | YES | NO |
| `person/title/update.json` | POST | Update title | YES | NO |
| `person/title/delete.json` | POST | Delete titles | YES | NO |

Create parameters: `title` (TEXT, required, max 50, localized).

### Report Group

**Report**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `report/tree.json` | GET | Get tree of reports | YES | NO |

**Report > Element**:

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `report/element/read.json` | GET | Read element | YES | NO |
| `report/element/create.json` | POST | Create element | YES | NO |
| `report/element/update.json` | POST | Update element | YES | NO |
| `report/element/delete.json` | POST | Delete elements | YES | NO |
| `report/element/reorder.json` | POST | Reorder elements | YES | NO |
| `report/element/data.json` | GET | Read JSON data | YES | NO |
| `report/element/data.html` | GET | Read HTML data | YES | NO |
| `report/element/meta.json` | GET | Read meta data | YES | NO |
| `report/element/download.pdf` | GET | Download PDF | NO | NO |
| `report/element/download.csv` | GET | Download CSV | NO | NO |
| `report/element/download.xlsx` | GET | Download Excel | NO | NO |

**Report > Collection** (codebase calls it "Set"):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `report/collection/read.json` | GET | Read collection | YES (as Set) | NO |
| `report/collection/create.json` | POST | Create collection | YES (as Set) | NO |
| `report/collection/update.json` | POST | Update collection | YES (as Set) | NO |
| `report/collection/delete.json` | POST | Delete collections | YES (as Set) | NO |
| `report/collection/reorder.json` | POST | Reorder collections | YES (as Set) | NO |
| `report/collection/meta.json` | GET | Read meta data | YES (as Set) | NO |
| `report/collection/download.pdf` | GET | Download PDF | NO | NO |
| `report/collection/download.csv` | GET | Download CSV | NO | NO |
| `report/collection/download.xlsx` | GET | Download Excel | NO | NO |
| `report/collection/download_annualreport.pdf` | GET | Download annual report | NO | NO |

---

## Implementation Status Matrix

| Service Interface | Connector Group | Endpoints Defined | Models Exist | Service Implemented | Connector Wired | Priority |
|---|---|---|---|---|---|---|
| IAccountService | Account | YES | STUB (empty) | YES | YES | **P1** - Fix models |
| IAccountCategoryService | Account | YES | NO | NO | NO | P2 |
| ICostCenterService | Account | YES | NO | NO | NO | P2 |
| ICostCenterCategoryService | Account | YES | NO | NO | NO | P3 |
| ICurrencyService | Common | YES | NO | NO | NO | P2 |
| ICustomFieldService | Common | YES | NO | NO | NO | P2 |
| ICustomFieldGroupService | Common | YES | NO | NO | NO | P3 |
| IRoundingService | Common | YES | NO | NO | NO | P3 |
| ISequenceNumberService | Common | YES | NO | NO | NO | P3 |
| ITaxRateService | Common | YES | NO | NO | NO | P2 |
| ITextTemplateService | Common | YES | NO | NO | NO | P3 |
| IFileService | File | YES | NO | NO | NO | P2 |
| IFileCategoryService | File | YES | NO | NO | NO | P3 |
| IArticleService | Inventory | YES | YES | YES | YES | **DONE** |
| IArticleCategoryService | Inventory | YES | YES | YES | YES | **DONE** |
| IFixedAssetService | Inventory | YES | NO | NO | NO | P2 |
| IFixedAssetCategoryService | Inventory | YES | NO | NO | NO | P3 |
| IInventoryImportService | Inventory | YES | NO | NO | NO | P3 |
| IUnitService | Inventory | YES | NO | NO | NO | P2 |
| IJournalService | Journal | YES | NO | NO | NO | **P1** |
| IJournalImportService | Journal | YES | NO | NO | NO | P3 |
| IJournalImportEntryService | Journal | YES | NO | NO | NO | P3 |
| IFiscalPeriodService | Meta | YES | NO | NO | NO | P2 |
| IFiscalPeriodTaskService | Meta | YES | NO | NO | NO | P3 |
| ILocationService | Meta | YES | NO | NO | NO | P3 |
| IOrganizationService | Meta | YES | NO | NO | NO | P3 |
| ISettingsService | Meta | YES | NO | NO | NO | P3 |
| IOrderService | Order | YES | NO | NO | NO | **P1** |
| IBookEntryService | Order | YES | NO | NO | NO | P2 |
| IOrderCategoryService | Order | YES | NO | NO | NO | P3 |
| IDocumentService | Order | YES | NO | NO | NO | P2 |
| IDocumentTemplateService | Order | YES | NO | NO | NO | P3 |
| IPersonService | Person | YES | NO | NO | NO | **P1** |
| IPersonCategoryService | Person | YES | NO | NO | NO | P3 |
| IPersonImportService | Person | YES | NO | NO | NO | P3 |
| IPersonTitleService | Person | YES | NO | NO | NO | P3 |
| IReportService | Report | YES | NO | NO | NO | P3 |
| IReportElementService | Report | YES | NO | NO | NO | P2 |
| IReportSetService | Report | YES | NO | NO | NO | P3 |

**Priority Key**:
- **P1**: Core business entities -- Account (fix models), Journal, Order, Person.
- **P2**: Important supporting entities -- categories, Currency, TaxRate, CustomField, File, Unit, FixedAsset, BookEntry, Document, FiscalPeriod, ReportElement.
- **P3**: Less commonly used entities -- all remaining services.

---

## Step-by-Step Implementation Guide

### General Pattern for a Standard CRUD Service

Reference implementation: `ArticleService` / `ArticleCategoryService`.

**Step 1: Create Models** in `src/CashCtrlApiNet.Abstractions/Models/{Group}/{Entity}/`

1. Look up the Create endpoint parameters in the [API docs](https://app.cashctrl.com/static/help/en/api/index.html).
2. Create `{Entity}Create.cs` -- a `record` inheriting `ModelBaseRecord` with all POST parameters as properties. Use `[JsonPropertyName]`, `[MaxLength]`, `required`, and `init` accessors exactly as in `ArticleCreate.cs`.
3. Create `{Entity}Update.cs` -- a `record` inheriting `{Entity}Create` that adds `required int Id` and any fields that become required on update.
4. Create `{Entity}.cs` (and optionally `{Entity}Listed.cs`):
   - `{Entity}Listed : {Entity}Update` -- adds read-only server fields (`created`, `createdBy`, `lastUpdated`, `lastUpdatedBy`, etc.).
   - `{Entity} : {Entity}Listed` -- adds detail-only fields (e.g., `attachments`).
   - For simple entities, `{Entity} : {Entity}Update` directly is acceptable if List and Read return the same fields.

**Step 2: Populate Interface** in `src/CashCtrlApiNet/Interfaces/Connectors/{Group}/I{Entity}Service.cs`

1. The interface file already exists (currently empty body).
2. Add method signatures following `IArticleService`:
   - `Get(Entry id, [Optional] CancellationToken)` -> `Task<ApiResult<SingleResponse<Entity>>>`
   - `GetList([Optional] CancellationToken)` -> `Task<ApiResult<ListResponse<EntityListed>>>` (or `Entity` if no Listed variant)
   - `Create(EntityCreate, [Optional] CancellationToken)` -> `Task<ApiResult<NoContentResponse>>`
   - `Update(EntityUpdate, [Optional] CancellationToken)` -> `Task<ApiResult<NoContentResponse>>`
   - `Delete(Entries, [Optional] CancellationToken)` -> `Task<ApiResult<NoContentResponse>>`
   - Add non-standard methods (Categorize, UpdateAttachments, etc.) as applicable.
3. Add XML doc comments with `<a href>` links to API docs on every method.

**Step 3: Create Service** in `src/CashCtrlApiNet/Services/Connectors/{Group}/{Entity}Service.cs`

1. Primary constructor: `public class {Entity}Service(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), I{Entity}Service`
2. Using alias: `using Endpoint = CashCtrlApiNet.Services.Endpoints.{Group}Endpoints.{Entity};`
3. One-liner expression bodies delegating to `ConnectionHandler.GetAsync` or `ConnectionHandler.PostAsync`.

**Step 4: Wire Up Connector** in `src/CashCtrlApiNet/Services/Connectors/{Group}Connector.cs`

1. Uncomment the service instantiation line in the constructor.

**Step 5: Add Missing Endpoint Constants** in `src/CashCtrlApiNet/Services/Endpoints/{Group}Endpoints.cs`

1. Add any missing non-standard endpoint constants.

### Per-Service Special Notes

**AccountService (P1 -- Fix Models)**: Models at `Models/Account/` are empty stubs. Populate with actual properties from API docs. Fix `AccountService.Get()` to use `Entry` instead of `int`. Fix inheritance hierarchy to Create -> Update -> Listed -> Entity.

**AccountCategoryService (P2)**: Follow `ArticleCategoryService` pattern. Include `GetTree` method.

**CostCenterService (P2)**: Includes `GetBalance`, `Categorize`, `UpdateAttachments` beyond standard CRUD.

**CurrencyService (P2)**: Non-standard `GetExchangeRate` method.

**CustomFieldService (P2)**: Non-standard `Reorder` and `GetTypes` methods. List requires `type` parameter.

**CustomFieldGroupService (P3)**: Non-standard `Reorder` method. List requires `type` parameter.

**SequenceNumberService (P3)**: Missing `sequencenumber/get` endpoint constant -- add to `CommonEndpoints.cs`.

**FileService (P2)**: Non-standard: `GetContent`, `Prepare`, `Persist`, `Categorize`, `EmptyArchive`, `Restore`.

**InventoryImportService (P3)**: Non-standard workflow: Create, Mapping, AvailableMappingFields, Preview, Execute. No Read/List/Update/Delete.

**JournalService (P1)**: Non-standard: `UpdateAttachments`, `UpdateRecurrence`. Create requires `items` JSON array for collective entries.

**JournalImportEntryService (P3)**: Non-standard: Read, List, Update, Ignore (delete endpoint), Restore, Confirm, Unconfirm.

**FiscalPeriodService (P2)**: Complex -- 13+ endpoints. Add missing `complete_months.json` and `reopen_months.json` to `MetaEndpoints.cs`.

**OrganizationService (P3)**: Single endpoint -- `GetLogo` only.

**SettingsService (P3)**: Read, Get, Update only (no Create/Delete).

**OrderService (P1)**: Complex -- `UpdateStatus`, `UpdateRecurrence`, `Continue`, `ReadDossier`, `DossierAdd`, `DossierRemove`, `UpdateAttachments`.

**DocumentService (P2)**: Non-standard -- Read, Mail, Update only (plus binary download endpoints).

**OrderCategoryService (P3)**: Includes `Reorder` and `ReadStatus` beyond standard CRUD.

**PersonImportService (P3)**: Same workflow as InventoryImport (Create, Mapping, MappingFields, Preview, Execute).

**ReportSetService (P3)**: API calls it "Collection" but codebase calls it "Set". Verify and fix endpoint paths in `ReportEndpoints.cs`.

---

## Implementation Conventions Checklist

Every new `.cs` file MUST include:

1. **MIT License Header** (lines 1-24) -- exact copy from any existing file.
2. **File-scoped namespace** -- `namespace CashCtrlApiNet.{...};`
3. **XML doc comments** on every public type and member. Interface methods include `<a href="...">` link to specific API endpoint.
4. **`record` types** for all models, inheriting from `ModelBaseRecord`.
5. **`[JsonPropertyName("camelCaseApiName")]`** on every model property, matching the CashCtrl API field name exactly.
6. **`required` keyword** on properties that are mandatory in the API.
7. **`init` accessors** on all record properties.
8. **`[MaxLength(n)]`** on string properties where the API docs specify a maximum length.
9. **`ImmutableArray<T>`** for collection properties (with `using System.Collections.Immutable`).
10. **`[Optional] CancellationToken cancellationToken`** on every async method (with `using System.Runtime.InteropServices`).
11. **Primary constructors** on service classes.
12. **Using alias for endpoints**: `using Endpoint = CashCtrlApiNet.Services.Endpoints.{Group}Endpoints.{Entity};`
13. **Expression-bodied members** for all service method implementations.
14. **`/// <inheritdoc />`** on service methods implementing an interface.
15. **`/// <inheritdoc cref="..." />`** on service class declarations referencing the interface.
16. **`internal static class`** for endpoint definition classes.
17. **Nullable reference types** enabled -- use `?` suffix for optional/nullable properties.

---

## Known Codebase Bugs

These should be tracked and fixed alongside implementation work:

1. **`PersonEndpoints.cs` line 43**: Inner static class named `Order` instead of `Person`. Must rename.
2. **`FileEndpoints.cs` line 109**: File category class named `ArticleCategory` instead of `FileCategory`. Must rename.
3. **`ReportEndpoints.cs`**: Uses `report/set/` path but API uses `report/collection/`. Must verify and fix.
4. **`ReportConnector.cs` lines 41-43**: Commented-out lines use `new IReportService(...)` (interface prefix) instead of `new ReportService(...)` (class).
5. **`AccountService.Get()`**: Takes `int accountId` instead of `Entry` and does not pass the ID as a query parameter. Must fix to match `ArticleService.Get(Entry)` pattern.
6. **`AccountConnector.cs` line 45**: Comment says `Account = new CostCenterCategoryService(...)` -- wrong, should be `CostCenterCategory = new CostCenterCategoryService(...)`.
7. **Account models**: `AccountCreate`, `AccountUpdate`, `Account` are empty stubs with no properties and incorrect inheritance (all directly extend `ModelBaseRecord` instead of Create -> Update -> Listed -> Entity hierarchy).

---

## Recommended Build Sequence

1. Fix Account models (populate `AccountCreate`, `AccountUpdate`, `Account` with properties from API docs).
2. Fix `AccountService.Get()` to use `Entry` parameter.
3. Fix codebase bugs (PersonEndpoints naming, FileEndpoints naming, ReportEndpoints path, ReportConnector comments, AccountConnector comment).
4. Implement **P1** services: `JournalService`, `OrderService`, `PersonService`.
5. Implement **P2** services: `AccountCategoryService`, `CostCenterService`, `CurrencyService`, `CustomFieldService`, `TaxRateService`, `FileService`, `FixedAssetService`, `UnitService`, `BookEntryService`, `DocumentService`, `FiscalPeriodService`, `ReportElementService`.
6. Implement **P3** services: All remaining services.
7. For each service: Models -> Interface methods -> Service class -> Connector wire-up -> Tests.

---

## Testing Strategy

- Follow the existing `ArticleTests.cs` pattern for each new service.
- Test file location: `src/CashCtrlApiNet.Tests/{Group}/{Entity}Tests.cs`.
- Use `[TestCaseOrderer]` and `Test1_`, `Test2_` naming for ordered execution.
- Minimum tests per CRUD service: Get, GetList, Create, Update, Delete.
- All tests are integration tests requiring live API credentials.

---

## Salary Group

The Salary domain group manages payroll processing. It contains 16 sub-entities with ~80 endpoints. Requires a new `ISalaryConnector` interface, `SalaryConnector` class, `SalaryEndpoints` constants, and wiring into `ICashCtrlApiClient`.

### Salary > Book Entry

Book entries are the journal entries created for a salary statement, either automatically from salary types or manually from payments.

**SalaryBookEntryService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/bookentry/read.json` | GET | Read book entry | YES | NO |
| `salary/bookentry/list.json` | GET | List book entries | YES | NO |
| `salary/bookentry/create.json` | POST | Create book entry | YES | NO |
| `salary/bookentry/update.json` | POST | Update book entry | YES | NO |
| `salary/bookentry/delete.json` | POST | Delete book entries | YES | NO |

Create parameters: `creditId` (NUMBER, required -- account credit side), `debitId` (NUMBER, required -- account debit side), `statementIds` (CSV, required -- salary statement IDs), `amount` (NUMBER -- leave empty for open amount), `date` (DATE, format YYYY-MM-DD), `description` (TEXT, max 200), `reference` (TEXT), `statusId` (NUMBER -- new status for statements).

Update parameters: Same as create plus `id` (NUMBER, required).

List parameters: `id` (NUMBER, required -- the statement ID to list book entries for).

### Salary > Category

Standard tree-pattern category for organizing salary types.

**SalaryCategoryService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/category/read.json` | GET | Read category | YES | NO |
| `salary/category/list.json` | GET | List categories | YES | NO |
| `salary/category/tree.json` | GET | Get category tree | YES | NO |
| `salary/category/create.json` | POST | Create category | YES | NO |
| `salary/category/update.json` | POST | Update category | YES | NO |
| `salary/category/delete.json` | POST | Delete categories | YES | NO |

Create parameters: `name` (TEXT, required, max 50, localized XML), `number` (TEXT, max 20 -- numeric ordering value), `parentId` (NUMBER).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Certificate

Salary certificates (e.g., annual wage statements for tax purposes).

**SalaryCertificateService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/certificate/read.json` | GET | Read certificate | YES | NO |
| `salary/certificate/list.json` | GET | List certificates | YES | NO |
| `salary/certificate/update.json` | POST | Update certificate | YES | NO |
| `salary/certificate/list.xlsx` | GET | Export to Excel | YES | NO |
| `salary/certificate/list.csv` | GET | Export to CSV | YES | NO |
| `salary/certificate/list.pdf` | GET | Export to PDF | YES | NO |

Update parameters: `id` (NUMBER, required), `notes` (HTML), `valuesLocal` (JSON -- local value overrides).

List parameters: `dir` (TEXT, ASC/DESC, default DESC), `filter` (JSON), `fiscalPeriodId` (NUMBER), `limit` (NUMBER, default 100), `onlyNotes` (BOOLEAN), `personId` (NUMBER), `query` (TEXT), `sort` (TEXT, default dateEnd), `start` (NUMBER, default 0).

Note: No create or delete -- certificates are generated from statements.

### Salary > Certificate Document

PDF/ZIP generation and mailing for salary certificates.

**SalaryCertificateDocumentService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/certificate/document/read.json` | GET | Read document | YES | NO |
| `salary/certificate/document/read.pdf` | GET | Download PDF | YES | NO |
| `salary/certificate/document/read.zip` | GET | Download ZIP | YES | NO |
| `salary/certificate/document/mail.json` | POST | Mail document | YES | NO |

Read parameters: `id` (NUMBER, required -- salary certificate ID).

Download PDF/ZIP parameters: `ids` (CSV, required -- salary certificate IDs).

Mail parameters: `certificateIds` (CSV, required), `mailFrom` (TEXT, required, max 255), `mailSubject` (TEXT, required, max 255 -- supports variables), `mailText` (TEXT, required -- HTML), `isCopyToMe` (BOOLEAN), `mailBcc` (TEXT), `mailCc` (TEXT), `mailTo` (TEXT -- leave empty for default).

### Salary > Certificate Template

Templates for salary certificate PDF generation.

**SalaryCertificateTemplateService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/certificate/template/read.json` | GET | Read template | YES | NO |
| `salary/certificate/template/list.json` | GET | List templates | YES | NO |
| `salary/certificate/template/tree.json` | GET | Get template tree | YES | NO |
| `salary/certificate/template/create.json` | POST | Create template | YES | NO |
| `salary/certificate/template/update.json` | POST | Update template | YES | NO |
| `salary/certificate/template/delete.json` | POST | Delete templates | YES | NO |

Create parameters: `name` (TEXT, required, max 100), `elements` (JSON -- text elements on form), `fileId` (NUMBER -- PDF form file), `isDefault` (BOOLEAN), `isInactive` (BOOLEAN), `mailSubject` (TEXT, max 255 -- supports variables), `mailTemplateId` (NUMBER -- text template for mail body), `orgLocationId` (NUMBER), `parentId` (NUMBER -- inherit from parent).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Document

PDF/ZIP generation and mailing for salary statement documents.

**SalaryDocumentService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/document/read.json` | GET | Read document | YES | NO |
| `salary/document/read.pdf` | GET | Download PDF | YES | NO |
| `salary/document/read.zip` | GET | Download ZIP | YES | NO |
| `salary/document/mail.json` | POST | Mail document | YES | NO |
| `salary/document/update.json` | POST | Update document | YES | NO |

Read parameters: `id` (NUMBER, required -- salary statement ID).

Download PDF/ZIP parameters: `ids` (CSV, required -- salary statement IDs).

Mail parameters: `statementIds` (CSV, required), `mailFrom` (TEXT, required, max 255), `mailSubject` (TEXT, required, max 255 -- supports variables), `mailText` (TEXT, required -- HTML), `isCopyToMe` (BOOLEAN), `mailBcc` (TEXT), `mailCc` (TEXT), `mailTo` (TEXT -- leave empty for default), `sentStatusId` (NUMBER -- status to set after sending).

Update parameters: `id` (NUMBER, required -- salary statement ID), `fileId` (NUMBER -- appended PDF), `footer` (HTML), `header` (HTML), `layoutId` (NUMBER), `orgAddress` (TEXT, max 255), `orgBankAccountId` (NUMBER), `orgLocationId` (NUMBER), `recipientAddress` (TEXT, max 255), `recipientAddressId` (NUMBER), `recipientBankAccountId` (NUMBER).

### Salary > Field

Read-only salary fields defined on salary types. No create/update/delete.

**SalaryFieldService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/field/read.json` | GET | Read field | YES | NO |
| `salary/field/list.json` | GET | List fields | YES | NO |

Read parameters: `id` (NUMBER, required -- field ID).

List parameters: `typeId` (NUMBER, required -- salary type ID).

### Salary > Insurance Type

Insurance types used in salary statements (e.g., AHV, ALV, BVG).

**SalaryInsuranceTypeService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/insurance/type/read.json` | GET | Read insurance type | YES | NO |
| `salary/insurance/type/list.json` | GET | List insurance types | YES | NO |
| `salary/insurance/type/create.json` | POST | Create insurance type | YES | NO |
| `salary/insurance/type/update.json` | POST | Update insurance type | YES | NO |
| `salary/insurance/type/delete.json` | POST | Delete insurance types | YES | NO |

Create parameters: `name` (TEXT, required, max 40, localized XML), `codes` (JSON -- array of `{name (TEXT, max 10, required), description (TEXT, max 100)}`), `description` (TEXT, max 100, localized XML).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Layout

Layouts for salary statement PDF documents, containing HTML/CSS elements.

**SalaryLayoutService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/layout/read.json` | GET | Read layout | YES | NO |
| `salary/layout/list.json` | GET | List layouts | YES | NO |
| `salary/layout/create.json` | POST | Create layout | YES | NO |
| `salary/layout/update.json` | POST | Update layout | YES | NO |
| `salary/layout/delete.json` | POST | Delete layouts | YES | NO |

Create parameters: `name` (TEXT, required, max 100), `elements` (JSON -- array of `{elementId (TEXT, required), css (TEXT), html (TEXT)}`).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Payment

Create salary payment files (pain.001) and download them.

**SalaryPaymentService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/payment/create.json` | POST | Create payment | YES | NO |
| `salary/payment/download` | GET | Download payment file | YES | NO |

Create parameters: `date` (DATE, required, format YYYY-MM-DD), `statementIds` (CSV, required), `amount` (NUMBER -- partial payment per statement), `isCombine` (BOOLEAN -- merge payments to same payee), `statusId` (NUMBER -- new status for statements), `type` (TEXT -- PAIN/SEPA_PAIN/WIRE_PDF/CASH_PDF, default PAIN).

Download parameters: Same as create.

### Salary > Setting

Global salary settings (variables used in calculations).

**SalarySettingService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/setting/read.json` | GET | Read setting | YES | NO |
| `salary/setting/list.json` | GET | List settings | YES | NO |
| `salary/setting/create.json` | POST | Create setting | YES | NO |
| `salary/setting/update.json` | POST | Update setting | YES | NO |
| `salary/setting/delete.json` | POST | Delete settings | YES | NO |

Create parameters: `name` (TEXT, required, max 100, localized XML), `variableName` (TEXT, required, min 2, max 32 -- must start with $), `boolValue` (BOOLEAN -- for BOOLEAN type), `decimalValue` (NUMBER -- for DECIMAL type), `isPercentage` (BOOLEAN -- for DECIMAL type), `textValue` (TEXT, max 255 -- for TEXT type), `type` (TEXT -- TEXT/BOOLEAN/DECIMAL).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Statement

The main salary statement entity -- the central document for each employee's pay period.

**SalaryStatementService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/statement/read.json` | GET | Read statement | YES | NO |
| `salary/statement/list.json` | GET | List statements | YES | NO |
| `salary/statement/create.json` | POST | Create statement | YES | NO |
| `salary/statement/update.json` | POST | Update statement | YES | NO |
| `salary/statement/update_multiple.json` | POST | Update multiple | YES | NO |
| `salary/statement/update_status.json` | POST | Update status | YES | NO |
| `salary/statement/update_recurrence.json` | POST | Update recurrence | YES | NO |
| `salary/statement/delete.json` | POST | Delete statements | YES | NO |
| `salary/statement/calculate.json` | POST | Calculate | YES | NO |
| `salary/statement/update_attachments.json` | POST | Update attachments | YES | NO |
| `salary/statement/list.xlsx` | GET | Export to Excel | YES | NO |
| `salary/statement/list.csv` | GET | Export to CSV | YES | NO |
| `salary/statement/list.pdf` | GET | Export to PDF | YES | NO |

Create parameters: `date` (DATE, required), `datePayment` (DATE, required), `personId` (NUMBER, required), `statusId` (NUMBER, required), `templateId` (NUMBER, required), `currencyId` (NUMBER), `currencyRate` (NUMBER), `custom` (XML), `daysBefore` (NUMBER -- days before start for next recurrence), `endDate` (DATE -- recurrence end), `insurances` (JSON -- insurance list with `contractId` and `codeId`), `message` (TEXT, max 50 -- payment message), `notes` (HTML), `notifyEmail` (TEXT), `notifyPersonId` (NUMBER), `notifyType` (TEXT), `notifyUserId` (NUMBER), `nr` (TEXT, max 50 -- statement number, auto-generated if empty), `recalculate` (BOOLEAN), `recurrence` (TEXT -- repeat interval), `sequenceNumberId` (NUMBER), `startDate` (DATE -- recurrence start), `types` (JSON -- salary type list with `typeId`, `allocations`, `creditId`, `dateEnd`, `dateStart`, `debitId`, `description`, `onlyMonth`, `rowName`, `valuesLocal`).

Update parameters: Same as create plus `id` (NUMBER, required).

Update multiple parameters: `ids` (CSV, required), `attachments` (TEXT), `custom` (XML), `date` (TEXT), `datePayment` (TEXT), `daysBefore` (NUMBER), `endDate` (DATE), `insurancesToAdd` (JSON), `insurancesToRemove` (JSON), `notes` (HTML), `notifyEmail` (TEXT), `notifyPersonId` (NUMBER), `notifyType` (TEXT), `notifyUserId` (NUMBER), `personId` (TEXT), `recurrence` (TEXT), `startDate` (DATE), `statusId` (TEXT).

Update status parameters: `ids` (CSV, required), `statusId` (NUMBER, required).

Update recurrence parameters: `id` (NUMBER, required), `daysBefore` (NUMBER), `endDate` (DATE), `notifyEmail` (TEXT), `notifyPersonId` (NUMBER), `notifyType` (TEXT), `notifyUserId` (NUMBER), `recurrence` (TEXT), `startDate` (DATE).

Calculate parameters: `currencyId` (NUMBER), `custom` (XML), `date` (DATE), `datePayment` (DATE), `id` (NUMBER -- existing statement), `personId` (NUMBER), `recalculate` (BOOLEAN), `templateId` (NUMBER), `types` (JSON -- same structure as create).

Update attachments parameters: `id` (NUMBER, required), `fileIds` (CSV -- empty to remove all).

List parameters: `dir` (TEXT, ASC/DESC, default DESC), `filter` (JSON), `fiscalPeriodId` (NUMBER), `limit` (NUMBER, default 100), `onlyNotes` (BOOLEAN), `onlyOpen` (BOOLEAN), `onlyOverdue` (BOOLEAN), `personId` (NUMBER), `query` (TEXT), `sort` (TEXT, default date), `start` (NUMBER, default 0).

### Salary > Status

Statuses for salary statements (e.g., Open, Paid, Closed).

**SalaryStatusService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/status/read.json` | GET | Read status | YES | NO |
| `salary/status/list.json` | GET | List statuses | YES | NO |
| `salary/status/create.json` | POST | Create status | YES | NO |
| `salary/status/update.json` | POST | Update status | YES | NO |
| `salary/status/delete.json` | POST | Delete statuses | YES | NO |
| `salary/status/reorder.json` | POST | Reorder statuses | YES | NO |

Create parameters: `icon` (TEXT, required -- BLUE/GREEN/RED/YELLOW/ORANGE/BLACK/GREY), `name` (TEXT, required, max 40, localized XML), `actionId` (TEXT -- action after status change), `isBook` (BOOLEAN -- create journal entries), `isClosed` (BOOLEAN -- closes/completes statement).

Update parameters: Same as create plus `id` (NUMBER, required).

Reorder parameters: `ids` (CSV, required), `target` (NUMBER, required -- target status ID), `before` (BOOLEAN, default true).

### Salary > Sum

Named calculation sums for salary reporting.

**SalarySumService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/sum/read.json` | GET | Read sum | YES | NO |
| `salary/sum/list.json` | GET | List sums | YES | NO |
| `salary/sum/create.json` | POST | Create sum | YES | NO |
| `salary/sum/update.json` | POST | Update sum | YES | NO |
| `salary/sum/delete.json` | POST | Delete sums | YES | NO |

Create parameters: `name` (TEXT, required, max 100, localized XML), `variableName` (TEXT, required, max 32 -- must start with $), `isDisplayColumn` (BOOLEAN -- show in type master report), `number` (TEXT, max 20 -- for sorting).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Template

Salary templates define defaults for salary statements including types, insurances, and layout.

**SalaryTemplateService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/template/read.json` | GET | Read template | YES | NO |
| `salary/template/list.json` | GET | List templates | YES | NO |
| `salary/template/tree.json` | GET | Get template tree | YES | NO |
| `salary/template/create.json` | POST | Create template | YES | NO |
| `salary/template/update.json` | POST | Update template | YES | NO |
| `salary/template/delete.json` | POST | Delete templates | YES | NO |

Create parameters: `name` (TEXT, required, max 100), `currencyId` (NUMBER), `dayOfMonth` (NUMBER, min 1, max 31), `footerTemplateId` (NUMBER -- text template for footer), `headerTemplateId` (NUMBER -- text template for header), `insurances` (JSON), `isDefault` (BOOLEAN), `isInactive` (BOOLEAN), `layoutId` (NUMBER), `mailSubject` (TEXT, max 255 -- supports variables), `mailTemplateId` (NUMBER -- text template for mail body), `message` (TEXT, max 50 -- payment message), `orgLocationId` (NUMBER), `parentId` (NUMBER -- inherit from parent), `paymentDayOfMonth` (NUMBER, min 1, max 31).

Update parameters: Same as create plus `id` (NUMBER, required).

### Salary > Type

Salary types define individual line items on a salary statement (e.g., base salary, AHV deduction).

**SalaryTypeService** (NEW):

| API Path | Method | Operation | Endpoints Defined | Service Implemented |
|---|---|---|---|---|
| `salary/type/read.json` | GET | Read type | YES | NO |
| `salary/type/list.json` | GET | List types | YES | NO |
| `salary/type/create.json` | POST | Create type | YES | NO |
| `salary/type/update.json` | POST | Update type | YES | NO |
| `salary/type/categorize.json` | POST | Categorize types | YES | NO |
| `salary/type/delete.json` | POST | Delete types | YES | NO |
| `salary/type/list.xlsx` | GET | Export to Excel | YES | NO |
| `salary/type/list.csv` | GET | Export to CSV | YES | NO |
| `salary/type/list.pdf` | GET | Export to PDF | YES | NO |

Create parameters: `categoryId` (NUMBER, required), `name` (TEXT, required, max 100, localized XML), `number` (TEXT, required, max 20), `type` (TEXT, required -- ADD/SUBTRACT), `allocations` (JSON -- cost center allocations with `share` and `toCostCenterId`), `base` (TEXT, max 32 -- display-only base amount), `calculation` (TEXT -- formula), `certificateCode` (TEXT, max 10), `creditId` (NUMBER -- credit account), `debitId` (NUMBER -- debit account), `description` (TEXT, max 512, localized XML), `fields` (JSON -- form field definitions), `insuranceTypeId` (NUMBER), `isInactive` (BOOLEAN).

Update parameters: Same as create plus `id` (NUMBER, required).

Categorize parameters: `ids` (CSV, required), `target` (NUMBER, required -- target category ID).

List parameters: `categoryId` (NUMBER), `dir` (TEXT, ASC/DESC, default ASC), `filter` (JSON), `limit` (NUMBER, default 100), `onlyActive` (BOOLEAN), `onlyChildren` (BOOLEAN), `onlyCostCenters` (BOOLEAN), `onlyNotes` (BOOLEAN), `query` (TEXT), `sort` (TEXT, default number), `start` (NUMBER, default 0).

### Salary Group Endpoint Summary

| Sub-entity | Service Name | Endpoints | Pattern |
|---|---|---|---|
| Book Entry | SalaryBookEntryService | 5 | CRUD (non-standard list by statement ID) |
| Category | SalaryCategoryService | 6 | Standard tree CRUD |
| Certificate | SalaryCertificateService | 6 | Read/List/Update + exports (no create/delete) |
| Certificate Document | SalaryCertificateDocumentService | 4 | Read + PDF/ZIP download + Mail |
| Certificate Template | SalaryCertificateTemplateService | 6 | Standard tree CRUD |
| Document | SalaryDocumentService | 5 | Read + PDF/ZIP download + Mail + Update |
| Field | SalaryFieldService | 2 | Read-only (Read + List) |
| Insurance Type | SalaryInsuranceTypeService | 5 | Standard CRUD |
| Layout | SalaryLayoutService | 5 | Standard CRUD |
| Payment | SalaryPaymentService | 2 | Create + Download (binary) |
| Setting | SalarySettingService | 5 | Standard CRUD |
| Statement | SalaryStatementService | 13 | Full CRUD + UpdateMultiple + UpdateStatus + UpdateRecurrence + Calculate + UpdateAttachments + exports |
| Status | SalaryStatusService | 6 | Standard CRUD + Reorder |
| Sum | SalarySumService | 5 | Standard CRUD |
| Template | SalaryTemplateService | 6 | Standard tree CRUD |
| Type | SalaryTypeService | 9 | CRUD + Categorize + exports |
| **Total** | **16 services** | **~90 endpoints** | |

---

## Critical Implementation Notes

1. **POST body encoding**: All POST requests use `application/x-www-form-urlencoded`, NOT JSON. The `CashCtrlSerialization.ConvertToDictionary` method handles this.
2. **JSON property names**: Must match the CashCtrl API exactly (camelCase).
3. **Translated text fields**: Some fields (`name`, `description`) support XML-formatted multilingual values. Currently handled as plain strings with TODO comment -- do not change this pattern.
4. **Custom field values**: The `custom` field uses XML format. Currently handled as a plain string.
5. **Export endpoints** (`.xlsx`, `.csv`, `.pdf`): Return binary data, not JSON. The current `ICashCtrlConnectionHandler` only supports JSON deserialization. Defer or add a new `GetBinaryAsync` method.
6. **File upload**: `file/prepare.json` likely requires `multipart/form-data`. May need special handling beyond `FormUrlEncodedContent`. Defer or document as known limitation.
7. **Immutability**: All models use records with `init` properties. Do not use mutable properties or classes.
