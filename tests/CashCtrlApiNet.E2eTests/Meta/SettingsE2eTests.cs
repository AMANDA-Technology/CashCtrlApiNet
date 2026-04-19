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

using CashCtrlApiNet.Abstractions.Models.Meta.Settings;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Meta;

/// <summary>
/// E2E tests for meta settings service with state restoration.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Meta.ISettingsService"/> operations.
/// </summary>
[Category("E2e")]
[Ignore("Group 8 (Meta) not yet verified against live API — highest-risk category: fixtures can touch active fiscal period, organization settings, and other tenant-wide state. Settings deserialization is already known to fail (missing `success` property). See doc/analysis/2026-03-29-e2e-test-verification.md. Remove this attribute when the fixture is verified.")]
// ReSharper disable once InconsistentNaming
public class SettingsE2eTests : CashCtrlE2eTestBase
{
    private Settings _originalSettings = null!;

    /// <summary>
    /// Captures original settings values for restoration in teardown
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Capture original settings to restore in teardown
        var readResult = await CashCtrlApiClient.Meta.Settings.Read();
        _originalSettings = readResult.ResponseData?.Data
                            ?? throw new InvalidOperationException("Failed to read original settings");
    }

    /// <summary>
    /// Restores original settings values
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        // Restore original settings
        try
        {
            await CashCtrlApiClient.Meta.Settings.Update((_originalSettings as SettingsUpdate)!);
        }
        catch (Exception ex)
        {
            await TestContext.Out.WriteLineAsync($"Failed to restore original settings: {ex.Message}");
        }

        await RunCleanup();
    }

    /// <summary>
    /// Read all settings successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Read_Success()
    {
        var res = await CashCtrlApiClient.Meta.Settings.Read();
        var settings = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        // Settings object should be populated (at least the defaults exist)
        settings.ShouldNotBeNull();
    }

    /// <summary>
    /// Get a single setting by name successfully
    /// </summary>
    [Test, Order(2)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Meta.Settings.Get(new() { Name = "defaultCurrencyId" });

        res.IsHttpSuccess.ShouldBeTrue();
        res.ResponseData.ShouldNotBeNull();
    }

    /// <summary>
    /// Update settings successfully and verify persistence
    /// </summary>
    [Test, Order(3)]
    public async Task Update_Success()
    {
        // Toggle the firstDayOfWeek setting as a safe, reversible change
        var currentFirstDay = _originalSettings.FirstDayOfWeek ?? 1;
        var newFirstDay = currentFirstDay == 1 ? 0 : 1;

        var res = await CashCtrlApiClient.Meta.Settings.Update(new()
        {
            FirstDayOfWeek = newFirstDay
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Meta.Settings.Read();
        verify.ResponseData?.Data?.FirstDayOfWeek.ShouldBe(newFirstDay);
    }
}
