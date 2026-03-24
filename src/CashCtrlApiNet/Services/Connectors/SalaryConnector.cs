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

using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors;
using CashCtrlApiNet.Interfaces.Connectors.Salary;
using CashCtrlApiNet.Services.Connectors.Salary;

namespace CashCtrlApiNet.Services.Connectors;

/// <inheritdoc />
public class SalaryConnector : ISalaryConnector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalaryConnector"/> class with all services using the connection handler.
    /// </summary>
    /// <param name="connectionHandler"></param>
    public SalaryConnector(ICashCtrlConnectionHandler connectionHandler)
    {
        BookEntry = new SalaryBookEntryService(connectionHandler);
        Category = new SalaryCategoryService(connectionHandler);
        Certificate = new SalaryCertificateService(connectionHandler);
        CertificateDocument = new SalaryCertificateDocumentService(connectionHandler);
        CertificateTemplate = new SalaryCertificateTemplateService(connectionHandler);
        Document = new SalaryDocumentService(connectionHandler);
        Field = new SalaryFieldService(connectionHandler);
        InsuranceType = new SalaryInsuranceTypeService(connectionHandler);
        Layout = new SalaryLayoutService(connectionHandler);
        Payment = new SalaryPaymentService(connectionHandler);
        Setting = new SalarySettingService(connectionHandler);
        Statement = new SalaryStatementService(connectionHandler);
        Status = new SalaryStatusService(connectionHandler);
        Sum = new SalarySumService(connectionHandler);
        Template = new SalaryTemplateService(connectionHandler);
        Type = new SalaryTypeService(connectionHandler);
    }

    /// <inheritdoc />
    public ISalaryBookEntryService BookEntry { get; }

    /// <inheritdoc />
    public ISalaryCategoryService Category { get; }

    /// <inheritdoc />
    public ISalaryCertificateService Certificate { get; }

    /// <inheritdoc />
    public ISalaryCertificateDocumentService CertificateDocument { get; }

    /// <inheritdoc />
    public ISalaryCertificateTemplateService CertificateTemplate { get; }

    /// <inheritdoc />
    public ISalaryDocumentService Document { get; }

    /// <inheritdoc />
    public ISalaryFieldService Field { get; }

    /// <inheritdoc />
    public ISalaryInsuranceTypeService InsuranceType { get; }

    /// <inheritdoc />
    public ISalaryLayoutService Layout { get; }

    /// <inheritdoc />
    public ISalaryPaymentService Payment { get; }

    /// <inheritdoc />
    public ISalarySettingService Setting { get; }

    /// <inheritdoc />
    public ISalaryStatementService Statement { get; }

    /// <inheritdoc />
    public ISalaryStatusService Status { get; }

    /// <inheritdoc />
    public ISalarySumService Sum { get; }

    /// <inheritdoc />
    public ISalaryTemplateService Template { get; }

    /// <inheritdoc />
    public ISalaryTypeService Type { get; }
}
