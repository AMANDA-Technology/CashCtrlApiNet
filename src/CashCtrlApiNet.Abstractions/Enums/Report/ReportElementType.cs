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

using System.Text.Json.Serialization;

namespace CashCtrlApiNet.Abstractions.Enums.Report;

/// <summary>
/// The type of report rendered by a <c>ReportElement</c>. Mandatory on
/// <c>/report/element/create.json</c>. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element/create.json">API Doc</a>
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ReportElementType>))]
public enum ReportElementType
{
    /// <summary>Journal entries list.</summary>
    [JsonStringEnumMemberName("JOURNAL")] Journal,

    /// <summary>Balance sheet.</summary>
    [JsonStringEnumMemberName("BALANCE")] Balance,

    /// <summary>Profit and loss statement.</summary>
    [JsonStringEnumMemberName("PLS")] Pls,

    /// <summary>Staggered P&amp;L / balance report.</summary>
    [JsonStringEnumMemberName("STAGGERED")] Staggered,

    /// <summary>Cost center profit and loss statement.</summary>
    [JsonStringEnumMemberName("COST_CENTER_PLS")] CostCenterPls,

    /// <summary>Cost center balance.</summary>
    [JsonStringEnumMemberName("COST_CENTER_BALANCE")] CostCenterBalance,

    /// <summary>Cost center allocation.</summary>
    [JsonStringEnumMemberName("COST_CENTER_ALLOCATION")] CostCenterAllocation,

    /// <summary>Cost center target vs. actual.</summary>
    [JsonStringEnumMemberName("COST_CENTER_TARGET")] CostCenterTarget,

    /// <summary>Cost center statements.</summary>
    [JsonStringEnumMemberName("COST_CENTER_STATEMENTS")] CostCenterStatements,

    /// <summary>Chart of accounts.</summary>
    [JsonStringEnumMemberName("CHART_OF_ACCOUNTS")] ChartOfAccounts,

    /// <summary>Open debtors (receivables) list.</summary>
    [JsonStringEnumMemberName("OPEN_DEBTORS")] OpenDebtors,

    /// <summary>Open creditors (payables) list.</summary>
    [JsonStringEnumMemberName("OPEN_CREDITORS")] OpenCreditors,

    /// <summary>People list.</summary>
    [JsonStringEnumMemberName("PEOPLE")] People,

    /// <summary>Organizational chart.</summary>
    [JsonStringEnumMemberName("ORG_CHART")] OrgChart,

    /// <summary>Sales tax statement.</summary>
    [JsonStringEnumMemberName("SALES_TAX")] SalesTax,

    /// <summary>Target (budget) comparison.</summary>
    [JsonStringEnumMemberName("TARGET")] Target,

    /// <summary>Result by article per person.</summary>
    [JsonStringEnumMemberName("RESULT_BY_ARTICLE_PER_PERSON")] ResultByArticlePerPerson,

    /// <summary>Expense by person.</summary>
    [JsonStringEnumMemberName("EXPENSE_BY_PERSON")] ExpenseByPerson,

    /// <summary>Revenue by person.</summary>
    [JsonStringEnumMemberName("REVENUE_BY_PERSON")] RevenueByPerson,

    /// <summary>Revenue by responsible person.</summary>
    [JsonStringEnumMemberName("REVENUE_BY_RESPONSIBLE_PERSON")] RevenueByResponsiblePerson,

    /// <summary>Result by article.</summary>
    [JsonStringEnumMemberName("RESULT_BY_ARTICLE")] ResultByArticle,

    /// <summary>Statements list.</summary>
    [JsonStringEnumMemberName("STATEMENTS")] Statements,

    /// <summary>Balance list.</summary>
    [JsonStringEnumMemberName("BALANCE_LIST")] BalanceList,

    /// <summary>Key figures.</summary>
    [JsonStringEnumMemberName("KEY_FIGURES")] KeyFigures,

    /// <summary>Expense by person chart.</summary>
    [JsonStringEnumMemberName("EXPENSE_BY_PERSON_CHART")] ExpenseByPersonChart,

    /// <summary>Revenue by person chart.</summary>
    [JsonStringEnumMemberName("REVENUE_BY_PERSON_CHART")] RevenueByPersonChart,

    /// <summary>Result by article chart.</summary>
    [JsonStringEnumMemberName("RESULT_BY_ARTICLE_CHART")] ResultByArticleChart,

    /// <summary>Balance progression chart.</summary>
    [JsonStringEnumMemberName("BALANCE_PROG_CHART")] BalanceProgChart,

    /// <summary>Balance share chart.</summary>
    [JsonStringEnumMemberName("BALANCE_SHARE_CHART")] BalanceShareChart,

    /// <summary>Salary book entries.</summary>
    [JsonStringEnumMemberName("SALARY_BOOK_ENTRIES")] SalaryBookEntries,

    /// <summary>Salary list.</summary>
    [JsonStringEnumMemberName("SALARY_LIST")] SalaryList,

    /// <summary>Salary types.</summary>
    [JsonStringEnumMemberName("SALARY_TYPES")] SalaryTypes,

    /// <summary>Salary type recap.</summary>
    [JsonStringEnumMemberName("SALARY_TYPE_RECAP")] SalaryTypeRecap,

    /// <summary>Salary configuration.</summary>
    [JsonStringEnumMemberName("SALARY_CONFIGURATION")] SalaryConfiguration,

    /// <summary>Social insurance settlement.</summary>
    [JsonStringEnumMemberName("SETTLEMENT")] Settlement,

    /// <summary>Withholding tax settlement.</summary>
    [JsonStringEnumMemberName("WITHHOLDING_TAX_SETTLEMENT")] WithholdingTaxSettlement,

    /// <summary>Withholding tax rates.</summary>
    [JsonStringEnumMemberName("WITHHOLDING_TAX_RATES")] WithholdingTaxRates,

    /// <summary>Cash flow chart.</summary>
    [JsonStringEnumMemberName("CASH_FLOW_CHART")] CashFlowChart,

    /// <summary>Free-form text + image block.</summary>
    [JsonStringEnumMemberName("TEXT_IMAGE")] TextImage
}
