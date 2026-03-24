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
using CashCtrlApiNet.Abstractions.Models.Common.CustomField;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common custom field service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield">API Doc - Common/Custom field</a>
/// </summary>
public interface ICustomFieldService
{
    /// <summary>
    /// Read custom field. Returns a single custom field by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/read.json">API Doc - Common/Read custom field</a>
    /// </summary>
    /// <param name="customField">The entry containing the ID of the custom field.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<CustomField>>> Get(Entry customField, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List custom fields. Returns a list of custom fields filtered by type.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/list.json">API Doc - Common/List custom fields</a>
    /// </summary>
    /// <param name="listRequest">The list request containing the type filter.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<CustomFieldListed>>> GetList(CustomFieldListRequest listRequest, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create custom field. Creates a new custom field. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/create.json">API Doc - Common/Create custom field</a>
    /// </summary>
    /// <param name="customField"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(CustomFieldCreate customField, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update custom field. Updates an existing custom field. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/update.json">API Doc - Common/Update custom field</a>
    /// </summary>
    /// <param name="customField"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(CustomFieldUpdate customField, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete custom fields. Deletes one or multiple custom fields. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/delete.json">API Doc - Common/Delete custom fields</a>
    /// </summary>
    /// <param name="customFields"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries customFields, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Reorder custom fields. Reorders custom fields relative to a target. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/reorder.json">API Doc - Common/Reorder custom fields</a>
    /// </summary>
    /// <param name="reorder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Reorder(CustomFieldReorder reorder, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get custom field types. Returns available custom field types.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/customfield/types.json">API Doc - Common/Get custom field types</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetTypes([Optional] CancellationToken cancellationToken);
}
