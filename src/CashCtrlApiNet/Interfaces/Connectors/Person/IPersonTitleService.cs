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
using CashCtrlApiNet.Abstractions.Models.Person.Title;

namespace CashCtrlApiNet.Interfaces.Connectors.Person;

/// <summary>
/// CashCtrl person title service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title">API Doc - Person/Title</a>
/// </summary>
public interface IPersonTitleService
{
    /// <summary>
    /// Read person title. Returns a single title by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title/read.json">API Doc - Person/Title/Read</a>
    /// </summary>
    /// <param name="title">The entry containing the ID of the title.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<PersonTitle>>> Get(Entry title, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List person titles. Returns a list of titles, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title/list.json">API Doc - Person/Title/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<PersonTitle>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new person title. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title/create.json">API Doc - Person/Title/Create</a>
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(PersonTitleCreate title, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update person title. Updates an existing title. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title/update.json">API Doc - Person/Title/Update</a>
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(PersonTitleUpdate title, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete person titles. Deletes one or multiple titles. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/title/delete.json">API Doc - Person/Title/Delete</a>
    /// </summary>
    /// <param name="titles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries titles, [Optional] CancellationToken cancellationToken);
}
