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

using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Integration tests verifying CancellationToken propagation through the full request pipeline
/// </summary>
public class CancellationTokenIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Get_WithCancelledToken_ShouldThrowTaskCanceledException()
    {
        // Arrange: stub a delayed response so cancellation has time to fire
        Server
            .Given(Request.Create().WithPath("/api/v1/account/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(CashCtrlResponseFactory.SingleResponse(AccountFakers.Account.Generate()))
                .WithDelay(TimeSpan.FromSeconds(5)));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Should.ThrowAsync<TaskCanceledException>(
            () => Client.Account.Account.Get(new() { Id = 1 }, cts.Token));
    }

    [Test]
    public async Task GetList_WithCancelledToken_ShouldThrowTaskCanceledException()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/account/list.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(CashCtrlResponseFactory.ListResponse(AccountFakers.AccountListed.Generate()))
                .WithDelay(TimeSpan.FromSeconds(5)));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Should.ThrowAsync<TaskCanceledException>(
            () => Client.Account.Account.GetList(cancellationToken: cts.Token));
    }

    [Test]
    public async Task Post_WithCancelledToken_ShouldThrowTaskCanceledException()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/account/create.json").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(CashCtrlResponseFactory.SuccessResponse("Account created", 1))
                .WithDelay(TimeSpan.FromSeconds(5)));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Should.ThrowAsync<TaskCanceledException>(
            () => Client.Account.Account.Create(AccountFakers.AccountCreate.Generate(), cts.Token));
    }

    [Test]
    public async Task GetBinary_WithCancelledToken_ShouldThrowTaskCanceledException()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/account/list.xlsx").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody("fake-excel"u8.ToArray())
                .WithDelay(TimeSpan.FromSeconds(5)));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Should.ThrowAsync<TaskCanceledException>(
            () => Client.Account.Account.ExportExcel(cts.Token));
    }

    [Test]
    public async Task Get_WithLiveToken_ShouldSucceed()
    {
        // Arrange
        var account = AccountFakers.Account.Generate();
        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleResponse(account));

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        // Act
        var result = await Client.Account.Account.Get(new() { Id = account.Id }, cts.Token);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(account.Id);
    }
}
