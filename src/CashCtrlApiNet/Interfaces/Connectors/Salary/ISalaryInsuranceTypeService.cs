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
using CashCtrlApiNet.Abstractions.Models.Salary.InsuranceType;

namespace CashCtrlApiNet.Interfaces.Connectors.Salary;

/// <summary>
/// CashCtrl salary insurance type service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type">API Doc - Salary/Insurance type</a>
/// </summary>
public interface ISalaryInsuranceTypeService
{
    /// <summary>
    /// Read salary insurance type. Returns a single insurance type by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type/read.json">API Doc - Salary/Insurance type/Read</a>
    /// </summary>
    /// <param name="insuranceType">The entry containing the ID of the insurance type.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SalaryInsuranceType>>> Get(Entry insuranceType, CancellationToken cancellationToken = default);

    /// <summary>
    /// List salary insurance types. Returns a list of insurance types, optionally filtered and paginated.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type/list.json">API Doc - Salary/Insurance type/List</a>
    /// </summary>
    /// <param name="listParams">Optional filter and pagination parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SalaryInsuranceType>>> GetList(ListParams? listParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new salary insurance type. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type/create.json">API Doc - Salary/Insurance type/Create</a>
    /// </summary>
    /// <param name="insuranceType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SalaryInsuranceTypeCreate insuranceType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update salary insurance type. Updates an existing insurance type. Returns either a success or multiple error messages.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type/update.json">API Doc - Salary/Insurance type/Update</a>
    /// </summary>
    /// <param name="insuranceType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SalaryInsuranceTypeUpdate insuranceType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete salary insurance types. Deletes one or multiple insurance types. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/salary/insurance/type/delete.json">API Doc - Salary/Insurance type/Delete</a>
    /// </summary>
    /// <param name="insuranceTypes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries insuranceTypes, CancellationToken cancellationToken = default);
}
