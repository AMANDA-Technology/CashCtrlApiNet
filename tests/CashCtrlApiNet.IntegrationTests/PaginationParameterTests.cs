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
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Integration tests validating that pagination parameters and ListParams fields
/// are correctly passed as query parameters to list endpoints
/// </summary>
public class PaginationParameterTests : IntegrationTestBase
{
    /// <summary>
    /// Verify GetList sends both start and limit pagination query parameters
    /// </summary>
    [Test]
    public async Task GetList_WithStartAndLimit_SendsPaginationQueryParams()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        await Client.Inventory.Article.GetList(new ListParams { Start = 20, Limit = 10 });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("start", "20");
        request.ShouldHaveQueryParam("limit", "10");
    }

    /// <summary>
    /// Verify GetList with only Limit sends limit query parameter with correct value
    /// </summary>
    [Test]
    public async Task GetList_WithOnlyLimit_SendsLimitQueryParam()
    {
        // Arrange
        var persons = PersonFakers.PersonListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/person/list.json",
            CashCtrlResponseFactory.ListResponse(persons));

        // Act
        await Client.Person.Person.GetList(new ListParams { Limit = 50 });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/person/list.json", "GET");
        request.ShouldHaveQueryParam("limit", "50");
        request.ShouldHaveQueryParam("lang", "de");
    }

    /// <summary>
    /// Verify GetList with only Start sends start query parameter with correct value
    /// </summary>
    [Test]
    public async Task GetList_WithOnlyStart_SendsStartQueryParam()
    {
        // Arrange
        var accounts = AccountFakers.AccountListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/account/list.json",
            CashCtrlResponseFactory.ListResponse(accounts));

        // Act
        await Client.Account.Account.GetList(new ListParams { Start = 0 });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/account/list.json", "GET");
        request.ShouldHaveQueryParam("start", "0");
        request.ShouldHaveQueryParam("lang", "de");
    }

    /// <summary>
    /// Verify GetList with all ListParams fields sends all query parameters with correct values
    /// </summary>
    [Test]
    public async Task GetList_WithAllListParams_SendsAllQueryParams()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        await Client.Inventory.Article.GetList(new ListParams
        {
            Start = 10,
            Limit = 25,
            CategoryId = 5,
            Dir = "ASC",
            Sort = "name",
            Filter = "test-filter",
            Query = "search-text",
            FiscalPeriodId = 3,
            OnlyActive = true
        });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("start", "10");
        request.ShouldHaveQueryParam("limit", "25");
        request.ShouldHaveQueryParam("categoryId", "5");
        request.ShouldHaveQueryParam("dir", "ASC");
        request.ShouldHaveQueryParam("sort", "name");
        request.ShouldHaveQueryParam("filter", "test-filter");
        request.ShouldHaveQueryParam("query", "search-text");
        request.ShouldHaveQueryParam("fiscalPeriodId", "3");
        request.ShouldHaveQueryParam("onlyActive", "True");
    }

    /// <summary>
    /// Verify GetList with no ListParams sends only the default lang query parameter
    /// </summary>
    [Test]
    public async Task GetList_WithNullListParams_SendsNoExtraQueryParams()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        await Client.Inventory.Article.GetList();

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("lang", "de");
        request.ShouldNotHaveQueryParam("start");
        request.ShouldNotHaveQueryParam("limit");
        request.ShouldNotHaveQueryParam("categoryId");
        request.ShouldNotHaveQueryParam("filter");
        request.ShouldNotHaveQueryParam("query");
        request.ShouldNotHaveQueryParam("sort");
        request.ShouldNotHaveQueryParam("dir");
        request.ShouldNotHaveQueryParam("fiscalPeriodId");
        request.ShouldNotHaveQueryParam("onlyActive");
    }

    /// <summary>
    /// Verify GetList with Start=0 sends start=0 (not omitted because value is 0, not null)
    /// </summary>
    [Test]
    public async Task GetList_WithZeroStart_SendsStartAsZero()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        await Client.Inventory.Article.GetList(new ListParams { Start = 0 });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("start", "0");
    }

    /// <summary>
    /// Verify GetList with large pagination values serializes correctly
    /// </summary>
    [Test]
    public async Task GetList_WithLargeValues_SendsCorrectValues()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        await Client.Inventory.Article.GetList(new ListParams { Start = 100000, Limit = 5000 });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("start", "100000");
        request.ShouldHaveQueryParam("limit", "5000");
    }

    /// <summary>
    /// Verify GetList with boolean params sends correct boolean string format (True/False)
    /// </summary>
    [Test]
    public async Task GetList_WithBooleanParams_SendsCorrectValues()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        await Client.Inventory.Article.GetList(new ListParams
        {
            OnlyActive = true,
            OnlyCostCenters = false,
            OnlyNotes = true
        });

        // Assert
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/list.json", "GET");
        request.ShouldHaveQueryParam("onlyActive", "True");
        request.ShouldHaveQueryParam("onlyCostCenters", "False");
        request.ShouldHaveQueryParam("onlyNotes", "True");
    }

    /// <summary>
    /// Verify pagination parameters work consistently across multiple domain endpoints
    /// </summary>
    [Test]
    public async Task GetList_Pagination_AcrossMultipleDomains()
    {
        // Arrange
        var orders = OrderFakers.OrderListed.Generate(2).ToArray();
        var journals = JournalFakers.JournalListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/order/list.json",
            CashCtrlResponseFactory.ListResponse(orders));
        Server.StubGetJson("/api/v1/journal/list.json",
            CashCtrlResponseFactory.ListResponse(journals));

        var listParams = new ListParams { Start = 5, Limit = 15 };

        // Act
        await Client.Order.Order.GetList(listParams);
        await Client.Journal.Journal.GetList(listParams);

        // Assert - Order request
        var orderRequest = Server.ShouldHaveReceivedRequest("/api/v1/order/list.json", "GET");
        orderRequest.ShouldHaveQueryParam("start", "5");
        orderRequest.ShouldHaveQueryParam("limit", "15");

        // Assert - Journal request
        var journalRequest = Server.ShouldHaveReceivedRequest("/api/v1/journal/list.json", "GET");
        journalRequest.ShouldHaveQueryParam("start", "5");
        journalRequest.ShouldHaveQueryParam("limit", "15");
    }
}
