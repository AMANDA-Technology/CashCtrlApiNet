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
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Common.CustomFieldGroup;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common custom field group service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group">API Doc - Common/Custom field group</a>
/// </summary>
public interface ICustomFieldGroupService
{
    /// <summary>
    /// Read custom field group. Returns a single custom field group by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group/read.json">API Doc - Common/Read custom field group</a>
    /// </summary>
    /// <param name="customFieldGroup">The entry containing the ID of the custom field group.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<CustomFieldGroup>>> Get(Entry customFieldGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// List custom field groups. Returns a list of custom field groups filtered by type.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group/list.json">API Doc - Common/List custom field groups</a>
    /// </summary>
    /// <param name="listRequest">The list request containing the type filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CustomFieldGroupListed>>> GetList(CustomFieldGroupListRequest listRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create custom field group. Creates a new custom field group. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group/create.json">API Doc - Common/Create custom field group</a>
    /// </summary>
    /// <param name="customFieldGroup"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(CustomFieldGroupCreate customFieldGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update custom field group. Updates an existing custom field group. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group/update.json">API Doc - Common/Update custom field group</a>
    /// </summary>
    /// <param name="customFieldGroup"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(CustomFieldGroupUpdate customFieldGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete custom field groups. Deletes one or multiple custom field groups. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group/delete.json">API Doc - Common/Delete custom field groups</a>
    /// </summary>
    /// <param name="customFieldGroups"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries customFieldGroups, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reorder custom field groups. Reorders custom field groups relative to a target. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/group/reorder.json">API Doc - Common/Reorder custom field groups</a>
    /// </summary>
    /// <param name="reorder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Reorder(CustomFieldGroupReorder reorder, CancellationToken cancellationToken = default);
}
