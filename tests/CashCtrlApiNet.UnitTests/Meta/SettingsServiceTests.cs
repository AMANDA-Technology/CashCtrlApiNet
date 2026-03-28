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

using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Meta.Settings;
using CashCtrlApiNet.Services.Connectors.Meta;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Meta;

/// <summary>
/// Unit tests for <see cref="SettingsService"/>
/// </summary>
public class SettingsServiceTests : ServiceTestBase<SettingsService>
{
    /// <inheritdoc />
    protected override SettingsService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Read_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<SingleResponse<Settings>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Settings>>());

        await Service.Read();

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Settings>>(
                MetaEndpoints.Settings.Read, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithSettingGetParameter()
    {
        var setting = new SettingGet { Name = "defaultCurrencyId" };
        ConnectionHandler
            .GetAsync<SingleResponse<Settings>, SettingGet>(
                Arg.Any<string>(), Arg.Any<SettingGet>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<Settings>>());

        await Service.Get(setting);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<Settings>, SettingGet>(
                MetaEndpoints.Settings.Get, setting, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var settings = new SettingsUpdate { DefaultCurrencyId = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, SettingsUpdate>(Arg.Any<string>(), Arg.Any<SettingsUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(settings);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SettingsUpdate>(
                MetaEndpoints.Settings.Update, settings, Arg.Any<CancellationToken>());
    }
}
