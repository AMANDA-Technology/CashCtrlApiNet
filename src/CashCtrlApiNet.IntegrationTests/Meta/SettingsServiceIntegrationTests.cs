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
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Meta;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Meta.SettingsService"/>
/// </summary>
public class SettingsServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Read returns settings with correct deserialization
    /// </summary>
    [Fact]
    public async Task Read_ReturnsExpectedResult()
    {
        // Arrange
        var settings = MetaFakers.Settings.Generate();
        Server.StubGetJson("/api/v1/setting/read.json",
            CashCtrlResponseFactory.SingleResponse(settings));

        // Act
        var result = await Client.Meta.Settings.Read();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.DefaultCurrencyId.ShouldBe(settings.DefaultCurrencyId);
        result.ResponseData.Data.IsProMode.ShouldBe(settings.IsProMode);
    }

    /// <summary>
    /// Verify Get returns a single setting value
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var settings = MetaFakers.Settings.Generate();
        Server.StubGetJson("/api/v1/setting/get",
            CashCtrlResponseFactory.SingleResponse(settings));

        // Act
        var result = await Client.Meta.Settings.Get(new SettingGet { Name = "defaultCurrencyId" });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var settingsUpdate = MetaFakers.SettingsUpdate.Generate();
        Server.StubPostJson("/api/v1/setting/update.json",
            CashCtrlResponseFactory.SuccessResponse("Settings updated"));

        // Act
        var result = await Client.Meta.Settings.Update(settingsUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Settings updated");
    }
}
