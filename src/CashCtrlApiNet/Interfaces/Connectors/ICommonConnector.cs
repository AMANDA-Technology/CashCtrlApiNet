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
/// CashCtrl common service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common</a>
/// </summary>
public interface ICommonConnector
{
    /// <summary>
    /// CashCtrl common currency service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common/Currency</a>
    /// </summary>
    public ICurrencyService Currency { get; }

    /// <summary>
    /// CashCtrl common custom field service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield">API Doc - Common/Custom field</a>
    /// </summary>
    public ICustomFieldService CustomField { get; }

    /// <summary>
    /// CashCtrl common custom field group service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group">API Doc - Common/Custom field group</a>
    /// </summary>
    public ICustomFieldGroupService CustomFieldGroup { get; }

    /// <summary>
    /// CashCtrl common rounding service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/rounding">API Doc - Common/Rounding</a>
    /// </summary>
    public IRoundingService Rounding { get; }

    /// <summary>
    /// CashCtrl common sequence number service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber">API Doc - Common/Sequence number</a>
    /// </summary>
    public ISequenceNumberService SequenceNumber { get; }

    /// <summary>
    /// CashCtrl common sequence number service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/tax">API Doc - Common/Tax rate</a>
    /// </summary>
    public ITaxRateService TaxRate { get; }

    /// <summary>
    /// CashCtrl common sequence number service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/text">API Doc - Common/Text template</a>
    /// </summary>
    public ITextTemplateService TextTemplate { get; }
}
