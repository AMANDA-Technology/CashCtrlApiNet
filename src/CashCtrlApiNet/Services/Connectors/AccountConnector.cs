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

using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors;
using CashCtrlApiNet.Interfaces.Connectors.Account;
using CashCtrlApiNet.Services.Connectors.Account;

namespace CashCtrlApiNet.Services.Connectors;

/// <inheritdoc />
public class AccountConnector : IAccountConnector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountConnector"/> class with all services using the connection handler.
    /// </summary>
    /// <param name="connectionHandler"></param>
    public AccountConnector(ICashCtrlConnectionHandler connectionHandler)
    {
        Account = new AccountService(connectionHandler);
        // Category = new AccountCategoryService(connectionHandler);
        // CostCenter = new CostCenterService(connectionHandler);
        // Account = new CostCenterCategoryService(connectionHandler);
    }

    /// <inheritdoc />
    public IAccountService Account { get; }

    /// <inheritdoc />
    public IAccountCategoryService Category { get; }

    /// <inheritdoc />
    public ICostCenterService CostCenter { get; }

    /// <inheritdoc />
    public ICostCenterCategoryService CostCenterCategory { get; }
}
