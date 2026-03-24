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
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Salary.Setting;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary setting service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting">API Doc - Salary/Setting</a>
/// </summary>
public interface ISalarySettingService
{
    /// <summary>
    /// Read salary setting. Returns a single setting by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting/read.json">API Doc - Salary/Setting/Read</a>
    /// </summary>
    /// <param name="setting">The entry containing the ID of the setting.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalarySetting>>> Get(Entry setting, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List salary settings. Returns a list of settings.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting/list.json">API Doc - Salary/Setting/List</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalarySetting>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new salary setting. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting/create.json">API Doc - Salary/Setting/Create</a>
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalarySettingCreate setting, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update salary setting. Updates an existing setting. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting/update.json">API Doc - Salary/Setting/Update</a>
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalarySettingUpdate setting, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete salary settings. Deletes one or multiple settings. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/setting/delete.json">API Doc - Salary/Setting/Delete</a>
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries settings, [Optional] CancellationToken cancellationToken);
}
