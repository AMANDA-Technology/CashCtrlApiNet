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

using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common history service (read-only).
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.IHistoryService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class HistoryE2eTests : CashCtrlE2eTestBase
{
    private int _accountId;

    /// <summary>
    /// Discovers an existing account ID to query history for
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Get an existing account to use for history queries
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        _accountId = accountResult.ResponseData?.Data.FirstOrDefault()?.Id
                     ?? throw new InvalidOperationException("No accounts found for history query");
    }

    /// <summary>
    /// Get list of history entries for an account successfully
    /// </summary>
    [Test, Order(1)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.History.GetList(new()
        {
            Id = _accountId,
            Type = "ACCOUNT",
            Count = 5
        });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();
    }
}
