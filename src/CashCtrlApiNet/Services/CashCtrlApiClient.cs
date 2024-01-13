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

using System.Diagnostics.CodeAnalysis;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors;

namespace CashCtrlApiNet.Services;

/// <inheritdoc />
public class CashCtrlApiClient : ICashCtrlApiClient
{
    private readonly ICashCtrlConnectionHandler _cashCtrlConnectionHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CashCtrlApiClient"/> class.
    /// </summary>
    /// <param name="cashCtrlConnectionHandler"></param>
    /// <param name="account"></param>
    /// <param name="common"></param>
    /// <param name="file"></param>
    /// <param name="inventory"></param>
    /// <param name="journal"></param>
    /// <param name="meta"></param>
    /// <param name="order"></param>
    /// <param name="person"></param>
    /// <param name="report"></param>
    [SuppressMessage("Sonar", "S107:Methods should not have too many parameters", Justification = "Accepted here, for injecting all services")]
    public CashCtrlApiClient(ICashCtrlConnectionHandler cashCtrlConnectionHandler,
        IAccountConnector account,
        ICommonConnector common,
        IFileConnector file,
        IInventoryConnector inventory,
        IJournalConnector journal,
        IMetaConnector meta,
        IOrderConnector order,
        IPersonConnector person,
        IReportConnector report)
    {
        _cashCtrlConnectionHandler = cashCtrlConnectionHandler;
        Account = account;
        Common = common;
        File = file;
        Inventory = inventory;
        Journal = journal;
        Meta = meta;
        Order = order;
        Person = person;
        Report = report;
    }

    /// <inheritdoc />
    public void SetLanguage(Language language)
        => _cashCtrlConnectionHandler.SetLanguage(language);

    /// <inheritdoc />
    public IAccountConnector Account { get; }

    /// <inheritdoc />
    public ICommonConnector Common { get; }

    /// <inheritdoc />
    public IFileConnector File { get; }

    /// <inheritdoc />
    public IInventoryConnector Inventory { get; }

    /// <inheritdoc />
    public IJournalConnector Journal { get; }

    /// <inheritdoc />
    public IMetaConnector Meta { get; }

    /// <inheritdoc />
    public IOrderConnector Order { get; }

    /// <inheritdoc />
    public IPersonConnector Person { get; }

    /// <inheritdoc />
    public IReportConnector Report { get; }
}
