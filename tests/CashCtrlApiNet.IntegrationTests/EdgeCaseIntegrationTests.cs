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

using System.Text.Json;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Integration tests for edge cases: timeout, malformed responses, and boundary values
/// </summary>
public class EdgeCaseIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Get_WithTimeout_ShouldThrowWhenCancelled()
    {
        // Arrange: WireMock responds with a 5-second delay
        Server
            .Given(Request.Create().WithPath("/api/v1/account/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "100")
                .WithBody(CashCtrlResponseFactory.SingleResponse(AccountFakers.Account.Generate()))
                .WithDelay(TimeSpan.FromSeconds(5)));

        // Use a short timeout to simulate network timeout
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        // Act & Assert
        await Should.ThrowAsync<TaskCanceledException>(
            () => Client.Account.Account.Get(new Entry { Id = 1 }, cts.Token));
    }

    [Test]
    public async Task Get_WithMalformedJson_ShouldThrowJsonException()
    {
        // Arrange: respond with invalid JSON
        Server.StubGetJson("/api/v1/account/read.json", "{ this is not valid json }}}");

        // Act & Assert
        await Should.ThrowAsync<JsonException>(
            () => Client.Account.Account.Get(new Entry { Id = 1 }));
    }

    [Test]
    public async Task GetList_WithMalformedJson_ShouldThrowJsonException()
    {
        // Arrange: respond with invalid JSON
        Server.StubGetJson("/api/v1/account/list.json", "<<<not json at all>>>");

        // Act & Assert
        await Should.ThrowAsync<JsonException>(
            () => Client.Account.Account.GetList());
    }

    [Test]
    public async Task Get_WithEmptyJsonResponse_ShouldReturnNullData()
    {
        // Arrange: respond with valid JSON but null data
        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleErrorResponse("Not found"));

        // Act
        var result = await Client.Account.Account.Get(new Entry { Id = 999 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeFalse();
        result.ResponseData.ErrorMessage.ShouldBe("Not found");
        result.ResponseData.Data.ShouldBeNull();
    }

    [Test]
    public async Task Get_WithHttp404_ShouldReturnNotSuccess()
    {
        // Arrange: return 404
        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleErrorResponse("Account not found"), statusCode: 404);

        // Act
        var result = await Client.Account.Account.Get(new Entry { Id = 999 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_WithHttp500_ShouldReturnNotSuccess()
    {
        // Arrange: return 500
        Server.StubGetJson("/api/v1/account/read.json",
            "{\"success\":false,\"errorMessage\":\"Internal server error\"}", statusCode: 500);

        // Act
        var result = await Client.Account.Account.Get(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);
    }

    [Test]
    public async Task Create_WithValidationError_ShouldReturnErrorDetails()
    {
        // Arrange: return a validation error
        Server.StubPostJson("/api/v1/account/create.json",
            CashCtrlResponseFactory.ErrorResponse("name", "Name is required"));

        // Act
        var result = await Client.Account.Account.Create(AccountFakers.AccountCreate.Generate());

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeFalse();
        result.ResponseData.Errors.ShouldNotBeNull();
        result.ResponseData.Errors!.Value.Length.ShouldBe(1);
        result.ResponseData.Errors.Value[0].Field.ShouldBe("name");
        result.ResponseData.Errors.Value[0].Message.ShouldBe("Name is required");
    }

    [Test]
    public async Task Create_WithMultipleValidationErrors_ShouldReturnAllErrors()
    {
        // Arrange: return multiple validation errors
        Server.StubPostJson("/api/v1/account/create.json",
            CashCtrlResponseFactory.ErrorResponse(
                ("name", "Name is required"),
                ("number", "Number must be unique")));

        // Act
        var result = await Client.Account.Account.Create(AccountFakers.AccountCreate.Generate());

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeFalse();
        result.ResponseData.Errors.ShouldNotBeNull();
        result.ResponseData.Errors!.Value.Length.ShouldBe(2);
    }

    [Test]
    public async Task GetList_WithEmptyList_ShouldReturnEmptyData()
    {
        // Arrange: return empty list
        Server.StubGetJson("/api/v1/account/list.json",
            "{\"total\":0,\"data\":[]}");

        // Act
        var result = await Client.Account.Account.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Total.ShouldBe(0);
        result.ResponseData.Data.Length.ShouldBe(0);
    }

    [Test]
    public async Task GetBinary_WithEmptyContent_ShouldReturnEmptyData()
    {
        // Arrange: return empty binary response
        Server.StubGetBinary("/api/v1/account/list.xlsx", [], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        // Act
        var result = await Client.Account.Account.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(0);
    }

    [Test]
    public async Task Get_WithMissingRequestsLeftHeader_ShouldReturnNullRequestsLeft()
    {
        // Arrange: respond without the X-CashCtrl-Requests-Left header
        Server
            .Given(Request.Create().WithPath("/api/v1/account/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(CashCtrlResponseFactory.SingleResponse(AccountFakers.Account.Generate())));

        // Act
        var result = await Client.Account.Account.Get(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.RequestsLeft.ShouldBeNull();
    }
}
