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

using CashCtrlApiNet.Interfaces.Connectors.Salary;

namespace CashCtrlApiNet.Interfaces.Connectors;

/// <summary>
/// CashCtrl salary service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry">API Doc - Salary</a>
/// </summary>
public interface ISalaryConnector
{
    /// <summary>
    /// CashCtrl salary book entry service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/bookentry">API Doc - Salary/Book entry</a>
    /// </summary>
    public ISalaryBookEntryService BookEntry { get; }

    /// <summary>
    /// CashCtrl salary category service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/category">API Doc - Salary/Category</a>
    /// </summary>
    public ISalaryCategoryService Category { get; }

    /// <summary>
    /// CashCtrl salary certificate service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate">API Doc - Salary/Certificate</a>
    /// </summary>
    public ISalaryCertificateService Certificate { get; }

    /// <summary>
    /// CashCtrl salary certificate document service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/document">API Doc - Salary/Certificate document</a>
    /// </summary>
    public ISalaryCertificateDocumentService CertificateDocument { get; }

    /// <summary>
    /// CashCtrl salary certificate template service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/certificate/template">API Doc - Salary/Certificate template</a>
    /// </summary>
    public ISalaryCertificateTemplateService CertificateTemplate { get; }

    /// <summary>
    /// CashCtrl salary document service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/document">API Doc - Salary/Document</a>
    /// </summary>
    public ISalaryDocumentService Document { get; }

    /// <summary>
    /// CashCtrl salary field service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/field">API Doc - Salary/Field</a>
    /// </summary>
    public ISalaryFieldService Field { get; }

    /// <summary>
    /// CashCtrl salary insurance type service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type">API Doc - Salary/Insurance type</a>
    /// </summary>
    public ISalaryInsuranceTypeService InsuranceType { get; }

    /// <summary>
    /// CashCtrl salary layout service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/layout">API Doc - Salary/Layout</a>
    /// </summary>
    public ISalaryLayoutService Layout { get; }

    /// <summary>
    /// CashCtrl salary payment service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/payment">API Doc - Salary/Payment</a>
    /// </summary>
    public ISalaryPaymentService Payment { get; }

    /// <summary>
    /// CashCtrl salary setting service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting">API Doc - Salary/Setting</a>
    /// </summary>
    public ISalarySettingService Setting { get; }

    /// <summary>
    /// CashCtrl salary statement service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/statement">API Doc - Salary/Statement</a>
    /// </summary>
    public ISalaryStatementService Statement { get; }

    /// <summary>
    /// CashCtrl salary status service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/status">API Doc - Salary/Status</a>
    /// </summary>
    public ISalaryStatusService Status { get; }

    /// <summary>
    /// CashCtrl salary sum service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/sum">API Doc - Salary/Sum</a>
    /// </summary>
    public ISalarySumService Sum { get; }

    /// <summary>
    /// CashCtrl salary template service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/template">API Doc - Salary/Template</a>
    /// </summary>
    public ISalaryTemplateService Template { get; }

    /// <summary>
    /// CashCtrl salary type service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/type">API Doc - Salary/Type</a>
    /// </summary>
    public ISalaryTypeService Type { get; }
}
