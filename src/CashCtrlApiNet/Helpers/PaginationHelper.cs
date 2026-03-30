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

using System.Runtime.CompilerServices;

using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;

namespace CashCtrlApiNet.Helpers;

/// <summary>
/// Provides auto-pagination support for CashCtrl API list endpoints.
/// Iterates all pages and yields results as <see cref="IAsyncEnumerable{T}"/>.
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Default number of items to fetch per page when no page size is specified.
    /// </summary>
    public const int DefaultPageSize = 100;

    /// <summary>
    /// Automatically paginates through all pages of a CashCtrl list endpoint and yields each item as it is received.
    /// Accepts <see cref="ListParams"/> for filter/sort/query pass-through.
    /// </summary>
    /// <typeparam name="TItem">The model type returned by the list endpoint</typeparam>
    /// <param name="fetchPage">Delegate to the service's GetList method</param>
    /// <param name="listParams">Optional filter/sort/query parameters to pass through to each page request</param>
    /// <param name="pageSize">Number of items to fetch per page (default <see cref="DefaultPageSize"/>)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields each item from all pages</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="pageSize"/> is less than 1</exception>
    /// <exception cref="InvalidOperationException">Thrown when the API returns an HTTP error or null response data</exception>
    public static IAsyncEnumerable<TItem> ListAllAsync<TItem>(
        Func<ListParams?, CancellationToken, Task<ApiResult<ListResponse<TItem>>>> fetchPage,
        ListParams? listParams = null,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        where TItem : ModelBaseRecord
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        return ListAllAsyncCore(
            (p, ct) => fetchPage(p, ct),
            listParams ?? new ListParams(),
            pageSize,
            cancellationToken);
    }

    /// <summary>
    /// Automatically paginates through all pages of a CashCtrl list endpoint and yields each item as it is received.
    /// Accepts a derived <see cref="ListParams"/> type for endpoints with additional parameters (e.g., <c>CustomFieldListRequest</c>).
    /// </summary>
    /// <typeparam name="TItem">The model type returned by the list endpoint</typeparam>
    /// <typeparam name="TParams">The derived <see cref="ListParams"/> type with additional endpoint-specific parameters</typeparam>
    /// <param name="fetchPage">Delegate to the service's GetList method</param>
    /// <param name="listParams">The endpoint-specific parameters to pass through to each page request</param>
    /// <param name="pageSize">Number of items to fetch per page (default <see cref="DefaultPageSize"/>)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields each item from all pages</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="pageSize"/> is less than 1</exception>
    /// <exception cref="InvalidOperationException">Thrown when the API returns an HTTP error or null response data</exception>
    public static IAsyncEnumerable<TItem> ListAllAsync<TItem, TParams>(
        Func<TParams?, CancellationToken, Task<ApiResult<ListResponse<TItem>>>> fetchPage,
        TParams listParams,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
        where TItem : ModelBaseRecord
        where TParams : ListParams
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        return ListAllAsyncCore(
            (p, ct) => fetchPage((TParams?)p, ct),
            listParams,
            pageSize,
            cancellationToken);
    }

    /// <summary>
    /// Core pagination loop shared by both overloads.
    /// </summary>
    private static async IAsyncEnumerable<TItem> ListAllAsyncCore<TItem>(
        Func<ListParams?, CancellationToken, Task<ApiResult<ListResponse<TItem>>>> fetchPage,
        ListParams baseParams,
        int pageSize,
        [EnumeratorCancellation] CancellationToken cancellationToken)
        where TItem : ModelBaseRecord
    {
        var offset = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var pageParams = baseParams with { Start = offset, Limit = pageSize };
            var result = await fetchPage(pageParams, cancellationToken).ConfigureAwait(false);

            if (!result.IsHttpSuccess)
                throw new InvalidOperationException(
                    $"ListAllAsync pagination failed: HTTP {result.HttpStatusCode}");

            if (result.ResponseData is null)
                throw new InvalidOperationException(
                    "ListAllAsync pagination failed: response data is null.");

            var data = result.ResponseData.Data;

            foreach (var item in data)
                yield return item;

            offset += data.Length;

            if (data.Length == 0 || offset >= result.ResponseData.Total)
                yield break;
        }
    }
}
