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

using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Inventory.Article;
using CashCtrlApiNet.Helpers;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Integration tests for <see cref="PaginationHelper"/> using WireMock to simulate paginated CashCtrl API responses
/// </summary>
public class PaginationHelperIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify that ListAllAsync returns all items from a single page
    /// </summary>
    [Test]
    public async Task ListAllAsync_SinglePage_ReturnsAllItems()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        var items = new List<ArticleListed>();
        await foreach (var item in PaginationHelper.ListAllAsync<ArticleListed>(
            Client.Inventory.Article.GetList, pageSize: 10))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(3);
    }

    /// <summary>
    /// Verify that ListAllAsync iterates across multiple pages via WireMock
    /// </summary>
    [Test]
    public async Task ListAllAsync_MultiplePages_ReturnsAllItems()
    {
        // Arrange — 5 total articles, page size 2 → 3 page requests
        var allArticles = InventoryFakers.ArticleListed.Generate(5).ToArray();

        StubPaginatedEndpoint("/api/v1/inventory/article/list.json", allArticles, pageSize: 2);

        // Act
        var items = new List<ArticleListed>();
        await foreach (var item in PaginationHelper.ListAllAsync<ArticleListed>(
            Client.Inventory.Article.GetList, pageSize: 2))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(5);
        for (var i = 0; i < 5; i++)
            items[i].Id.ShouldBe(allArticles[i].Id);
    }

    /// <summary>
    /// Verify that ListAllAsync handles empty results correctly
    /// </summary>
    [Test]
    public async Task ListAllAsync_EmptyResult_YieldsNoItems()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(Array.Empty<ArticleListed>(), 0));

        // Act
        var items = new List<ArticleListed>();
        await foreach (var item in PaginationHelper.ListAllAsync<ArticleListed>(
            Client.Inventory.Article.GetList, pageSize: 10))
            items.Add(item);

        // Assert
        items.ShouldBeEmpty();
    }

    /// <summary>
    /// Verify that ListAllAsync passes ListParams (filter/sort) to the API
    /// </summary>
    [Test]
    public async Task ListAllAsync_WithListParams_PassesParametersToApi()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        var listParams = new ListParams { Query = "widget", Sort = "name", Dir = "ASC" };

        // Act
        var items = new List<ArticleListed>();
        await foreach (var item in PaginationHelper.ListAllAsync<ArticleListed>(
            Client.Inventory.Article.GetList, listParams, pageSize: 10))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(1);
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("query", "widget");
        request.ShouldHaveQueryParam("sort", "name");
        request.ShouldHaveQueryParam("dir", "ASC");
    }

    /// <summary>
    /// Verify that ListAllAsync handles exact page boundary without fetching an extra page
    /// </summary>
    [Test]
    public async Task ListAllAsync_ExactPageBoundary_DoesNotFetchExtraPage()
    {
        // Arrange — 4 total articles, page size 2 → exactly 2 pages
        var allArticles = InventoryFakers.ArticleListed.Generate(4).ToArray();

        StubPaginatedEndpoint("/api/v1/inventory/article/list.json", allArticles, pageSize: 2);

        // Act
        var items = new List<ArticleListed>();
        await foreach (var item in PaginationHelper.ListAllAsync<ArticleListed>(
            Client.Inventory.Article.GetList, pageSize: 2))
            items.Add(item);

        // Assert
        items.Count.ShouldBe(4);

        // Verify exactly 2 requests were made (not 3)
        var requests = Server.LogEntries
            .Where(e => e.RequestMessage?.Path == "/api/v1/inventory/article/list.json")
            .ToList();
        requests.Count.ShouldBe(2);
    }

    /// <summary>
    /// Verify that ListAllAsync throws on HTTP error from the API
    /// </summary>
    [Test]
    public async Task ListAllAsync_HttpError_ThrowsInvalidOperationException()
    {
        // Arrange — use a valid ListResponse body so the connection handler can deserialize it,
        // but with a 500 status code so IsHttpSuccess is false
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(Array.Empty<ArticleListed>(), 0), statusCode: 500);

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in PaginationHelper.ListAllAsync<ArticleListed>(
                Client.Inventory.Article.GetList, pageSize: 10))
            { }
        });
    }

    /// <summary>
    /// Stubs a paginated GET endpoint on WireMock by matching the <c>start</c> query parameter.
    /// Each page request is matched to the correct slice of items based on the offset.
    /// </summary>
    private void StubPaginatedEndpoint<T>(string path, T[] allItems, int pageSize) where T : class
    {
        var totalCount = allItems.Length;

        for (var offset = 0; offset < totalCount; offset += pageSize)
        {
            var page = allItems.Skip(offset).Take(pageSize).ToArray();
            var startValue = offset.ToString();

            Server
                .Given(Request.Create()
                    .WithPath(path)
                    .UsingGet()
                    .WithParam("start", new ExactMatcher(startValue)))
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-CashCtrl-Requests-Left", "100")
                    .WithBody(CashCtrlResponseFactory.ListResponse(page, totalCount)));
        }
    }
}
