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

using System.Collections.Immutable;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Helpers;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Unit tests for <see cref="PaginationHelper"/>
/// </summary>
[TestFixture]
public class PaginationHelperTests
{
    /// <summary>
    /// Test record to use as a list item in pagination tests
    /// </summary>
    private record TestItem : ModelBaseRecord
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    [Test]
    public async Task ListAllAsync_EmptyResult_YieldsNoItems()
    {
        // Arrange
        var fetchCalled = 0;

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            fetchCalled++;
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });
        }

        // Act
        var items = new List<TestItem>();
        await foreach (var item in PaginationHelper.ListAllAsync<TestItem>(FetchPage))
            items.Add(item);

        // Assert
        items.ShouldBeEmpty();
        fetchCalled.ShouldBe(1);
    }

    [Test]
    public async Task ListAllAsync_SinglePage_YieldsAllItems()
    {
        // Arrange
        var testItems = ImmutableArray.Create(
            new TestItem { Id = 1, Name = "A" },
            new TestItem { Id = 2, Name = "B" });

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
            => Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 2, Data = testItems }
            });

        // Act
        var items = new List<TestItem>();
        await foreach (var item in PaginationHelper.ListAllAsync<TestItem>(FetchPage))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(2);
        items[0].Id.ShouldBe(1);
        items[1].Id.ShouldBe(2);
    }

    [Test]
    public async Task ListAllAsync_MultiplePages_YieldsAllItems()
    {
        // Arrange — 5 total items, page size 2 → 3 pages (2, 2, 1)
        var allItems = Enumerable.Range(1, 5)
            .Select(i => new TestItem { Id = i, Name = $"Item{i}" })
            .ToArray();

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            var start = p?.Start ?? 0;
            var limit = p?.Limit ?? 2;
            var page = allItems.Skip(start).Take(limit).ToImmutableArray();
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 5, Data = page }
            });
        }

        // Act
        var items = new List<TestItem>();
        await foreach (var item in PaginationHelper.ListAllAsync<TestItem>(FetchPage, pageSize: 2))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(5);
        items.Select(i => i.Id).ShouldBe([1, 2, 3, 4, 5]);
    }

    [Test]
    public async Task ListAllAsync_ExactPageBoundary_DoesNotFetchExtraPage()
    {
        // Arrange — 4 total items, page size 2 → exactly 2 pages
        var allItems = Enumerable.Range(1, 4)
            .Select(i => new TestItem { Id = i, Name = $"Item{i}" })
            .ToArray();
        var fetchCount = 0;

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            fetchCount++;
            var start = p?.Start ?? 0;
            var limit = p?.Limit ?? 2;
            var page = allItems.Skip(start).Take(limit).ToImmutableArray();
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 4, Data = page }
            });
        }

        // Act
        var items = new List<TestItem>();
        await foreach (var item in PaginationHelper.ListAllAsync<TestItem>(FetchPage, pageSize: 2))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(4);
        fetchCount.ShouldBe(2);
    }

    [Test]
    public async Task ListAllAsync_PassesListParamsToDelegate()
    {
        // Arrange
        var capturedParams = new List<ListParams?>();

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            capturedParams.Add(p);
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });
        }

        var listParams = new ListParams { Query = "search", Sort = "name", Dir = "ASC", CategoryId = 42 };

        // Act
        await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage, listParams, pageSize: 50))
        { }

        // Assert
        capturedParams.Count.ShouldBe(1);
        var p = capturedParams[0];
        p.ShouldNotBeNull();
        p.Query.ShouldBe("search");
        p.Sort.ShouldBe("name");
        p.Dir.ShouldBe("ASC");
        p.CategoryId.ShouldBe(42);
        p.Limit.ShouldBe(50);
        p.Start.ShouldBe(0);
    }

    [Test]
    public async Task ListAllAsync_PreservesListParamsAcrossPages()
    {
        // Arrange
        var capturedParams = new List<ListParams?>();
        var allItems = Enumerable.Range(1, 3)
            .Select(i => new TestItem { Id = i, Name = $"Item{i}" })
            .ToArray();

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            capturedParams.Add(p);
            var start = p?.Start ?? 0;
            var limit = p?.Limit ?? 2;
            var page = allItems.Skip(start).Take(limit).ToImmutableArray();
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 3, Data = page }
            });
        }

        var listParams = new ListParams { Query = "test", Sort = "id" };

        // Act
        await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage, listParams, pageSize: 2))
        { }

        // Assert
        capturedParams.Count.ShouldBe(2);
        capturedParams[0]!.Query.ShouldBe("test");
        capturedParams[0]!.Sort.ShouldBe("id");
        capturedParams[0]!.Start.ShouldBe(0);
        capturedParams[0]!.Limit.ShouldBe(2);
        capturedParams[1]!.Query.ShouldBe("test");
        capturedParams[1]!.Sort.ShouldBe("id");
        capturedParams[1]!.Start.ShouldBe(2);
        capturedParams[1]!.Limit.ShouldBe(2);
    }

    [Test]
    public async Task ListAllAsync_DefaultPageSize_Is100()
    {
        // Arrange
        ListParams? capturedParams = null;

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            capturedParams = p;
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });
        }

        // Act
        await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage))
        { }

        // Assert
        capturedParams.ShouldNotBeNull();
        capturedParams.Limit.ShouldBe(100);
    }

    [Test]
    public void ListAllAsync_HttpFailure_ThrowsInvalidOperationException()
    {
        // Arrange
        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
            => Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = false,
                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
            });

        // Act & Assert
        Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage))
            { }
        });
    }

    [Test]
    public void ListAllAsync_NullResponseData_ThrowsInvalidOperationException()
    {
        // Arrange
        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
            => Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = null
            });

        // Act & Assert
        Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage))
            { }
        });
    }

    [Test]
    public async Task ListAllAsync_CancellationToken_IsPassedToDelegate()
    {
        // Arrange
        CancellationToken capturedToken = default;
        using var cts = new CancellationTokenSource();

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            capturedToken = ct;
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });
        }

        // Act
        await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage, cancellationToken: cts.Token))
        { }

        // Assert
        capturedToken.ShouldBe(cts.Token);
    }

    [Test]
    public async Task ListAllAsync_NullListParams_UsesDefaultParams()
    {
        // Arrange
        ListParams? capturedParams = null;

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            capturedParams = p;
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });
        }

        // Act
        await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage, listParams: null))
        { }

        // Assert
        capturedParams.ShouldNotBeNull();
        capturedParams.Limit.ShouldBe(100);
        capturedParams.Start.ShouldBe(0);
        capturedParams.Query.ShouldBeNull();
    }

    [Test]
    public async Task ListAllAsync_GenericOverload_WithDerivedParams_PreservesType()
    {
        // Arrange — use a derived ListParams type to verify the generic overload preserves it
        var capturedParams = new List<ListParams?>();
        var allItems = Enumerable.Range(1, 3)
            .Select(i => new TestItem { Id = i, Name = $"Item{i}" })
            .ToArray();

        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
        {
            capturedParams.Add(p);
            var start = p?.Start ?? 0;
            var limit = p?.Limit ?? 2;
            var page = allItems.Skip(start).Take(limit).ToImmutableArray();
            return Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 3, Data = page }
            });
        }

        var derivedParams = new DerivedListParams { Query = "search", CustomField = "extra" };

        // Act
        await foreach (var _ in PaginationHelper.ListAllAsync<TestItem, DerivedListParams>(FetchPage, derivedParams, pageSize: 2))
        { }

        // Assert
        capturedParams.Count.ShouldBe(2);
        var first = capturedParams[0] as DerivedListParams;
        first.ShouldNotBeNull();
        first.CustomField.ShouldBe("extra");
        first.Start.ShouldBe(0);
        first.Limit.ShouldBe(2);

        var second = capturedParams[1] as DerivedListParams;
        second.ShouldNotBeNull();
        second.CustomField.ShouldBe("extra");
        second.Start.ShouldBe(2);
        second.Limit.ShouldBe(2);
    }

    [Test]
    public void ListAllAsync_PageSizeZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
            => Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });

        // Act & Assert
        Should.ThrowAsync<ArgumentOutOfRangeException>(async () =>
        {
            await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage, pageSize: 0))
            { }
        });
    }

    [Test]
    public void ListAllAsync_NegativePageSize_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Task<ApiResult<ListResponse<TestItem>>> FetchPage(ListParams? p, CancellationToken ct)
            => Task.FromResult(new ApiResult<ListResponse<TestItem>>
            {
                IsHttpSuccess = true,
                ResponseData = new ListResponse<TestItem> { Total = 0, Data = [] }
            });

        // Act & Assert
        Should.ThrowAsync<ArgumentOutOfRangeException>(async () =>
        {
            await foreach (var _ in PaginationHelper.ListAllAsync<TestItem>(FetchPage, pageSize: -1))
            { }
        });
    }

    /// <summary>
    /// Derived ListParams type for testing the generic overload
    /// </summary>
    private record DerivedListParams : ListParams
    {
        public string? CustomField { get; init; }
    }
}
