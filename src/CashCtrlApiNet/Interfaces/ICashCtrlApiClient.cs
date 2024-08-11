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
/// Connector service to call CashCtrl REST API. <a href="https://app.cashctrl.com/static/help/en/api/index.html">API Doc</a>
/// </summary>
public interface ICashCtrlApiClient
{
    /// <summary>
    /// Change the language to use for upcoming requests. <a href="https://app.cashctrl.com/static/help/en/api/index.html#lang">API Doc - Language</a>
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language);

    /// <summary>
    /// CashCtrl account service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/account">API Doc - Account</a>
    /// </summary>
    public IAccountConnector Account { get; }

    /// <summary>
    /// CashCtrl common service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/currency">API Doc - Common</a>
    /// </summary>
    public ICommonConnector Common { get; }

    /// <summary>
    /// CashCtrl file service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/file">API Doc - File</a>
    /// </summary>
    public IFileConnector File { get; }

    /// <summary>
    /// CashCtrl inventory service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory">API Doc - Inventory</a>
    /// </summary>
    public IInventoryConnector Inventory { get; }

    /// <summary>
    /// CashCtrl journal service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/journal">API Doc - Journal</a>
    /// </summary>
    public IJournalConnector Journal { get; }

    /// <summary>
    /// CashCtrl meta service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/fiscalperiod">API Doc - Meta</a>
    /// </summary>
    public IMetaConnector Meta { get; }

    /// <summary>
    /// CashCtrl order service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order">API Doc - Order</a>
    /// </summary>
    public IOrderConnector Order { get; }

    /// <summary>
    /// CashCtrl person service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person">API Doc - Person</a>
    /// </summary>
    public IPersonConnector Person { get; }

    /// <summary>
    /// CashCtrl report service endpoint group. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/report">API Doc - Report</a>
    /// </summary>
    public IReportConnector Report { get; }
}
