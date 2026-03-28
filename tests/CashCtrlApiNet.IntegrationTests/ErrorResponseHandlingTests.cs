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

using System.Net;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Integration tests for error response handling across various HTTP status codes
/// </summary>
public class ErrorResponseHandlingTests : IntegrationTestBase
{
    /// <summary>
    /// Verify that a 404 response sets correct HTTP status and CashCtrl description
    /// </summary>
    [Test]
    public async Task Get_Returns404_SetsCorrectHttpStatus()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/read.json", "{}", 404);

        // Act
        var result = await ConnectionHandler.GetAsync("api/v1/inventory/article/read.json");

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.NotFound);
        result.CashCtrlHttpStatusCodeDescription.ShouldBe("The requested endpoint doesn't exist.");
    }

    /// <summary>
    /// Verify that a 404 response with a JSON body deserializes the error message
    /// </summary>
    [Test]
    public async Task Get_Returns404_WithJsonBody_DeserializesResponse()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/read.json",
            CashCtrlResponseFactory.SingleErrorResponse("Resource not found"), 404);

        // Act
        var result = await Client.Inventory.Article.Get(new Entry { Id = 42 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.NotFound);
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeFalse();
        result.ResponseData.ErrorMessage.ShouldBe("Resource not found");
    }

    /// <summary>
    /// Verify that a 422 response with validation errors deserializes the error fields
    /// </summary>
    [Test]
    public async Task Post_Returns422_WithValidationErrors_DeserializesErrorFields()
    {
        // Arrange
        var personCreate = PersonFakers.PersonCreate.Generate();
        Server.StubPostJson("/api/v1/person/create.json",
            CashCtrlResponseFactory.ErrorResponse(
                ("lastName", "Last name is required"),
                ("email", "Invalid email format")),
            422);

        // Act
        var result = await Client.Person.Person.Create(personCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        result.CashCtrlHttpStatusCodeDescription.ShouldBe("No description found for this http status code");
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeFalse();
        result.ResponseData.Errors.ShouldNotBeNull();
        result.ResponseData.Errors!.Value.Length.ShouldBe(2);
        result.ResponseData.Errors!.Value[0].Field.ShouldBe("lastName");
        result.ResponseData.Errors!.Value[0].Message.ShouldBe("Last name is required");
        result.ResponseData.Errors!.Value[1].Field.ShouldBe("email");
        result.ResponseData.Errors!.Value[1].Message.ShouldBe("Invalid email format");
    }

    /// <summary>
    /// Verify that a 200 response with success: false deserializes error fields (CashCtrl's actual pattern)
    /// </summary>
    [Test]
    public async Task Post_Returns200_WithValidationErrors_DeserializesErrorFields()
    {
        // Arrange
        var articleCreate = InventoryFakers.ArticleCreate.Generate();
        Server.StubPostJson("/api/v1/inventory/article/create.json",
            CashCtrlResponseFactory.ErrorResponse("name", "Name is required"));

        // Act
        var result = await Client.Inventory.Article.Create(articleCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeFalse();
        result.ResponseData.Errors.ShouldNotBeNull();
        result.ResponseData.Errors!.Value.Length.ShouldBe(1);
        result.ResponseData.Errors!.Value[0].Field.ShouldBe("name");
        result.ResponseData.Errors!.Value[0].Message.ShouldBe("Name is required");
    }

    /// <summary>
    /// Verify that a 401 response sets correct status and description
    /// </summary>
    [Test]
    public async Task Get_Returns401_SetsCorrectStatusAndDescription()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/read.json", "{}", 401);

        // Act
        var result = await ConnectionHandler.GetAsync("api/v1/inventory/article/read.json");

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        result.CashCtrlHttpStatusCodeDescription.ShouldBe("No valid API key provided.");
    }

    /// <summary>
    /// Verify that a 429 response sets correct status and rate-limiting description
    /// </summary>
    [Test]
    public async Task Get_Returns429_SetsCorrectStatusAndDescription()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/read.json", "{}", 429);

        // Act
        var result = await ConnectionHandler.GetAsync("api/v1/inventory/article/read.json");

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe((HttpStatusCode)429);
        result.CashCtrlHttpStatusCodeDescription.ShouldBe(
            "Too many requests hit the API too quickly. We recommend adding delays between your requests. For more information, see our 'https://cashctrl.com/en/about/terms'.");
    }

    /// <summary>
    /// Verify that a 500 response sets correct status and description
    /// </summary>
    [Test]
    public async Task Get_Returns500_SetsCorrectStatusAndDescription()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/read.json", "{}", 500);

        // Act
        var result = await ConnectionHandler.GetAsync("api/v1/inventory/article/read.json");

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.HttpStatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        result.CashCtrlHttpStatusCodeDescription.ShouldBe(
            "Something went wrong on our end - not your fault. Please contact support.");
    }

    /// <summary>
    /// Verify that the X-CashCtrl-Requests-Left header is preserved on GET error responses
    /// </summary>
    [Test]
    public async Task Get_ErrorResponse_PreservesRequestsLeftHeader()
    {
        // Arrange
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/read.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(404)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "42")
                .WithBody("{}"));

        // Act
        var result = await ConnectionHandler.GetAsync("api/v1/inventory/article/read.json");

        // Assert
        result.RequestsLeft.ShouldBe(42);
    }

    /// <summary>
    /// Verify that the X-CashCtrl-Requests-Left header is preserved on POST error responses
    /// </summary>
    [Test]
    public async Task Post_ErrorResponse_PreservesRequestsLeftHeader()
    {
        // Arrange
        var articleCreate = InventoryFakers.ArticleCreate.Generate();
        Server
            .Given(Request.Create().WithPath("/api/v1/inventory/article/create.json").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(422)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("X-CashCtrl-Requests-Left", "42")
                .WithBody(CashCtrlResponseFactory.ErrorResponse("name", "Name is required")));

        // Act
        var result = await Client.Inventory.Article.Create(articleCreate);

        // Assert
        result.RequestsLeft.ShouldBe(42);
    }
}
