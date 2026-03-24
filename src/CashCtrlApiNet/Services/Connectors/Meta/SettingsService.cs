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
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors.Meta;
using CashCtrlApiNet.Services.Connectors.Base;
using Endpoint = CashCtrlApiNet.Services.Endpoints.MetaEndpoints.Settings;

namespace CashCtrlApiNet.Services.Connectors.Meta;

/// <inheritdoc cref="CashCtrlApiNet.Interfaces.Connectors.Meta.ISettingsService" />
public class SettingsService(ICashCtrlConnectionHandler connectionHandler) : ConnectorService(connectionHandler), ISettingsService
{
    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<Abstractions.Models.Meta.Settings.Settings>>> Read([Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<Abstractions.Models.Meta.Settings.Settings>>(Endpoint.Read, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<SingleResponse<Abstractions.Models.Meta.Settings.Settings>>> Get(SettingGet setting, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.GetAsync<SingleResponse<Abstractions.Models.Meta.Settings.Settings>, SettingGet>(Endpoint.Get, setting, cancellationToken);

    /// <inheritdoc />
    public Task<ApiResult<NoContentResponse>> Update(SettingsUpdate settings, [Optional] CancellationToken cancellationToken)
        => ConnectionHandler.PostAsync<NoContentResponse, SettingsUpdate>(Endpoint.Update, settings, cancellationToken: cancellationToken);
}
