﻿/*
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

using CashCtrlApiNet.Interfaces.Connectors.Person;

namespace CashCtrlApiNet.Interfaces.Connectors;

/// <summary>
/// CashCtrl person service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/person">API Doc - Person</see>
/// </summary>
public interface IPersonConnector
{
    /// <summary>
    /// CashCtrl person service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/person">API Doc - Person/Person</see>
    /// </summary>
    public IPersonService Person { get; set; }

    /// <summary>
    /// CashCtrl person category service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/person/category">API Doc - Person/Category</see>
    /// </summary>
    public IPersonCategoryService Category { get; set; }

    /// <summary>
    /// CashCtrl person import service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import">API Doc - Person/Import</see>
    /// </summary>
    public IPersonImportService Import { get; set; }

    /// <summary>
    /// CashCtrl person title service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title">API Doc - Person/Title</see>
    /// </summary>
    public IPersonTitleService Title { get; set; }
}
