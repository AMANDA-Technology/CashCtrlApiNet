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

using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Interfaces.Connectors;

namespace CashCtrlApiNet.Interfaces;

/// <summary>
/// Connector service to call CashCtrl REST API. <see href="https://app.cashctrl.com/static/help/en/api/index.html">API Doc</see>
/// </summary>
public interface ICashCtrlApiClient
{
    /// <summary>
    /// Change the language to use for upcoming requests. <see href="https://app.cashctrl.com/static/help/en/api/index.html#lang">API Doc - Language</see>
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language);

    /// <summary>
    /// CashCtrl account service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/account">API Doc - Account</see>
    /// </summary>
    // public IAccountConnector Account { get; set; }

    /// <summary>
    /// CashCtrl common service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common</see>
    /// </summary>
    // public ICommonConnector Common { get; set; }

    /// <summary>
    /// CashCtrl file service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/file">API Doc - File</see>
    /// </summary>
    // public IFileConnector File { get; set; }

    /// <summary>
    /// CashCtrl inventory service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory">API Doc - Inventory</see>
    /// </summary>
    public IInventoryConnector Inventory { get; set; }

    /// <summary>
    /// CashCtrl journal service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/journal">API Doc - Journal</see>
    /// </summary>
    // public IJournalConnector Journal { get; set; }

    /// <summary>
    /// CashCtrl meta service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod">API Doc - Meta</see>
    /// </summary>
    // public IMetaConnector Meta { get; set; }

    /// <summary>
    /// CashCtrl order service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/order">API Doc - Order</see>
    /// </summary>
    // public IOrderConnector Order { get; set; }

    /// <summary>
    /// CashCtrl person service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/person">API Doc - Person</see>
    /// </summary>
    // public IPersonConnector Person { get; set; }

    /// <summary>
    /// CashCtrl report service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/report">API Doc - Report</see>
    /// </summary>
    // public IReportConnector Report { get; set; }
}
