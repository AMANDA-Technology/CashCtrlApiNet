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

using CashCtrlApiNet.Interfaces.Connectors.Report;

namespace CashCtrlApiNet.Interfaces.Connectors;

/// <summary>
/// CashCtrl report service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/report">API Doc - Report</see>
/// </summary>
public interface IReportConnector
{
    /// <summary>
    /// CashCtrl report service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/report">API Doc - Report/Report</see>
    /// </summary>
    public IReportService Report { get; set; }

    /// <summary>
    /// CashCtrl report element service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/report/element">API Doc - Report/Element</see>
    /// </summary>
    public IReportElementService Element { get; set; }

    /// <summary>
    /// CashCtrl report set service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/report/set">API Doc - Report/Set</see>
    /// </summary>
    public IReportSetService Set { get; set; }
}
