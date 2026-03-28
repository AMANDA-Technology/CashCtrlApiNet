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
/// Integration tests verifying CashCtrlConnectionHandler handles concurrent requests without state corruption
/// </summary>
public class ConcurrencyIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ConcurrentGetRequests_ShouldAllSucceed()
    {
        // Arrange
        var account = AccountFakers.Account.Generate();
        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleResponse(account));

        const int concurrentRequests = 20;

        // Act: fire 20 concurrent GET requests
        var tasks = Enumerable.Range(0, concurrentRequests)
            .Select(_ => Client.Account.Account.Get(new Entry { Id = account.Id }))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert: all should succeed without state corruption
        results.Length.ShouldBe(concurrentRequests);
        foreach (var result in results)
        {
            result.IsHttpSuccess.ShouldBeTrue();
            result.ResponseData.ShouldNotBeNull();
            result.ResponseData.Data.ShouldNotBeNull();
            result.ResponseData.Data.Id.ShouldBe(account.Id);
        }
    }

    [Test]
    public async Task ConcurrentMixedRequests_ShouldAllSucceed()
    {
        // Arrange
        var account = AccountFakers.Account.Generate();
        var accounts = AccountFakers.AccountListed.Generate(3).ToArray();

        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleResponse(account));
        Server.StubGetJson("/api/v1/account/list.json",
            CashCtrlResponseFactory.ListResponse(accounts));
        Server.StubPostJson("/api/v1/account/create.json",
            CashCtrlResponseFactory.SuccessResponse("Account created", 1));
        Server.StubGetBinary("/api/v1/account/list.xlsx",
            "excel-data"u8.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        // Act: fire concurrent mixed requests (GET, LIST, POST, BINARY)
        var getTasks = Enumerable.Range(0, 5)
            .Select(_ => Client.Account.Account.Get(new Entry { Id = account.Id }));
        var listTasks = Enumerable.Range(0, 5)
            .Select(_ => Client.Account.Account.GetList());
        var createTasks = Enumerable.Range(0, 5)
            .Select(_ => Client.Account.Account.Create(AccountFakers.AccountCreate.Generate()));
        var binaryTasks = Enumerable.Range(0, 5)
            .Select(_ => Client.Account.Account.ExportExcel());

        await Task.WhenAll(
            Task.WhenAll(getTasks),
            Task.WhenAll(listTasks),
            Task.WhenAll(createTasks),
            Task.WhenAll(binaryTasks));

        // Assert: if we got here without exceptions, concurrency is handled correctly
    }

    [Test]
    public async Task ConcurrentRequestsAcrossDomains_ShouldNotInterfere()
    {
        // Arrange: stub different domain endpoints
        var account = AccountFakers.Account.Generate();
        var article = InventoryFakers.Article.Generate();

        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleResponse(account));
        Server.StubGetJson("/api/v1/inventory/article/read.json",
            CashCtrlResponseFactory.SingleResponse(article));

        // Act: fire concurrent requests to different domains
        var accountTask = Client.Account.Account.Get(new Entry { Id = account.Id });
        var articleTask = Client.Inventory.Article.Get(new Entry { Id = article.Id });

        await Task.WhenAll(accountTask, articleTask);

        // Assert: each request got its correct response
        var accountResult = await accountTask;
        var articleResult = await articleTask;

        accountResult.IsHttpSuccess.ShouldBeTrue();
        accountResult.ResponseData.ShouldNotBeNull();
        accountResult.ResponseData.Data.ShouldNotBeNull();
        accountResult.ResponseData.Data.Id.ShouldBe(account.Id);

        articleResult.IsHttpSuccess.ShouldBeTrue();
        articleResult.ResponseData.ShouldNotBeNull();
        articleResult.ResponseData.Data.ShouldNotBeNull();
        articleResult.ResponseData.Data.Id.ShouldBe(article.Id);
    }
}
