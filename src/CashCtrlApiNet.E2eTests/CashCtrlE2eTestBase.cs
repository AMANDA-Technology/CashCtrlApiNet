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
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services;
using CashCtrlApiNet.Services.Connectors;

namespace CashCtrlApiNet.E2eTests;

/// <summary>
/// Base class for all CashCtrl E2E tests requiring live API credentials
/// </summary>
public class CashCtrlE2eTestBase
{
    /// <summary>
    /// Default instance of CashCtrl API client
    /// </summary>
    protected readonly ICashCtrlApiClient CashCtrlApiClient;

    /// <summary>
    /// Setup
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    protected CashCtrlE2eTestBase()
    {
        ICashCtrlConnectionHandler connectionHandler = new CashCtrlConnectionHandler(new CashCtrlConfiguration
        {
            BaseUri = Environment.GetEnvironmentVariable("CashCtrlApiNet__BaseUri") ?? throw new InvalidOperationException("Missing CashCtrlApiNet__BaseUri"),
            ApiKey = Environment.GetEnvironmentVariable("CashCtrlApiNet__ApiKey") ?? throw new InvalidOperationException("Missing CashCtrlApiNet__ApiKey"),
            DefaultLanguage = Environment.GetEnvironmentVariable("CashCtrlApiNet__Language") ?? nameof(Language.de)
        });

        CashCtrlApiClient = new CashCtrlApiClient(connectionHandler,
            new AccountConnector(connectionHandler),
            new CommonConnector(connectionHandler),
            new FileConnector(connectionHandler),
            new InventoryConnector(connectionHandler),
            new JournalConnector(connectionHandler),
            new MetaConnector(connectionHandler),
            new OrderConnector(connectionHandler),
            new PersonConnector(connectionHandler),
            new ReportConnector(connectionHandler),
            new SalaryConnector(connectionHandler));
    }

    protected async Task DownloadFile(string fileName, byte[] data)
    {
        if (!IsDownloadsEnabled)
            return;

        await File.WriteAllBytesAsync(Path.Combine(DownloadsFolder, fileName), data);
    }

    private static readonly bool IsDownloadsEnabled = string.Equals(Environment.GetEnvironmentVariable("CashCtrlApiNet__IsDownloadsEnabled"), "true", StringComparison.OrdinalIgnoreCase);

    private static readonly string DownloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
}
