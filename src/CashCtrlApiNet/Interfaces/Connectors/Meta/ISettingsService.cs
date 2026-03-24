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

using System.Runtime.InteropServices;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Meta.Settings;

namespace CashCtrlApiNet.Interfaces.Connectors.Meta;

/// <summary>
/// CashCtrl meta settings service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/setting">API Doc - Meta/Settings</a>
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Read settings. Returns the full settings object.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/setting/read.json">API Doc - Meta/Read settings</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Settings>>> Read([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get setting. Returns a single setting value by name.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/setting/get">API Doc - Meta/Get setting</a>
    /// </summary>
    /// <param name="setting">The query containing the name of the setting to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<Settings>>> Get(SettingGet setting, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update settings. Updates one or more settings. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/setting/update.json">API Doc - Meta/Update settings</a>
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SettingsUpdate settings, [Optional] CancellationToken cancellationToken);
}
