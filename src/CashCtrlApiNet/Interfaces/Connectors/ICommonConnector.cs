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

using CashCtrlApiNet.Interfaces.Connectors.Common;

namespace CashCtrlApiNet.Interfaces.Connectors;

/// <summary>
/// CashCtrl common service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common</see>
/// </summary>
public interface ICommonConnector
{
    /// <summary>
    /// CashCtrl common currency service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common/Currency</see>
    /// </summary>
    public ICurrencyService Currency { get; set; }

    /// <summary>
    /// CashCtrl common custom field service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield">API Doc - Common/Custom field</see>
    /// </summary>
    public ICustomFieldService CustomField { get; set; }

    /// <summary>
    /// CashCtrl common custom field group service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group">API Doc - Common/Custom field group</see>
    /// </summary>
    public ICustomFieldGroupService CustomFieldGroup { get; set; }

    /// <summary>
    /// CashCtrl common rounding service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding">API Doc - Common/Rounding</see>
    /// </summary>
    public IRoundingService Rounding { get; set; }

    /// <summary>
    /// CashCtrl common sequence number service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber">API Doc - Common/Sequence number</see>
    /// </summary>
    public ISequenceNumberService SequenceNumber { get; set; }

    /// <summary>
    /// CashCtrl common sequence number service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/tax">API Doc - Common/Tax rate</see>
    /// </summary>
    public ITaxRateService TaxRate { get; set; }

    /// <summary>
    /// CashCtrl common sequence number service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/text">API Doc - Common/Text template</see>
    /// </summary>
    public ITextTemplateService TextTemplate { get; set; }
}
