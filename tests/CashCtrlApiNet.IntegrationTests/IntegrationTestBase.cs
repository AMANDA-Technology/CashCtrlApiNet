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
using CashCtrlApiNet.Services;
using CashCtrlApiNet.Services.Connectors;
using WireMock.Server;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Base class for integration tests using WireMock to simulate the CashCtrl API
/// </summary>
[TestFixture]
public abstract class IntegrationTestBase
{
    /// <summary>
    /// Fake API key used for integration tests
    /// </summary>
    protected const string FakeApiKey = "test-api-key-integration";

    /// <summary>
    /// WireMock server instance
    /// </summary>
    protected WireMockServer Server = null!;

    /// <summary>
    /// CashCtrl API client wired to the WireMock server
    /// </summary>
    protected ICashCtrlApiClient Client = null!;

    /// <summary>
    /// CashCtrl connection handler wired to the WireMock server
    /// </summary>
    protected ICashCtrlConnectionHandler ConnectionHandler = null!;

    /// <summary>
    /// Initializes the WireMock server and creates the CashCtrl client before each test
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        Server = WireMockServer.Start();

        var config = new CashCtrlConfiguration
        {
            BaseUri = Server.Url!,
            ApiKey = FakeApiKey,
            DefaultLanguage = "de"
        };

        ConnectionHandler = new CashCtrlConnectionHandler(config);

        Client = new CashCtrlApiClient(
            ConnectionHandler,
            new AccountConnector(ConnectionHandler),
            new CommonConnector(ConnectionHandler),
            new FileConnector(ConnectionHandler),
            new InventoryConnector(ConnectionHandler),
            new JournalConnector(ConnectionHandler),
            new MetaConnector(ConnectionHandler),
            new OrderConnector(ConnectionHandler),
            new PersonConnector(ConnectionHandler),
            new ReportConnector(ConnectionHandler),
            new SalaryConnector(ConnectionHandler));
    }

    /// <summary>
    /// Stops and disposes the WireMock server after each test
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        Server?.Stop();
        Server?.Dispose();
    }
}
