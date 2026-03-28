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
/// Integration tests validating that request parameters are correctly serialized
/// and response fields are correctly deserialized
/// </summary>
public class RequestParameterValidationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get Article sends correct id query parameter and lang parameter, and deserializes response fields
    /// </summary>
    [Test]
    public async Task Get_Article_SendsCorrectIdQueryParameter()
    {
        // Arrange
        var article = InventoryFakers.Article.Generate();
        Server.StubGetJson("/api/v1/inventory/article/read.json",
            CashCtrlResponseFactory.SingleResponse(article));

        // Act
        var result = await Client.Inventory.Article.Get(new Entry { Id = 42 });

        // Assert - request parameters
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/read.json", "GET");
        request.ShouldHaveQueryParam("id", "42");
        request.ShouldHaveQueryParam("lang", "de");

        // Assert - response deserialization
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Name.ShouldBe(article.Name);
        result.ResponseData.Data.Nr.ShouldBe(article.Nr);
        result.ResponseData.Data.CategoryId.ShouldBe(article.CategoryId);
        result.ResponseData.Data.SalesPrice.ShouldBe(article.SalesPrice);
        result.ResponseData.Data.CreatedBy.ShouldBe(article.CreatedBy);
    }

    /// <summary>
    /// Verify Get Person sends correct id query parameter and deserializes response fields
    /// </summary>
    [Test]
    public async Task Get_Person_SendsCorrectIdQueryParameter()
    {
        // Arrange
        var person = PersonFakers.Person.Generate();
        Server.StubGetJson("/api/v1/person/read.json",
            CashCtrlResponseFactory.SingleResponse(person));

        // Act
        var result = await Client.Person.Person.Get(new Entry { Id = 7 });

        // Assert - request parameters
        var request = Server.ShouldHaveReceivedRequest("/api/v1/person/read.json", "GET");
        request.ShouldHaveQueryParam("id", "7");

        // Assert - response deserialization
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.FirstName.ShouldBe(person.FirstName);
        result.ResponseData.Data.LastName.ShouldBe(person.LastName);
        result.ResponseData.Data.Company.ShouldBe(person.Company);
        result.ResponseData.Data.Email.ShouldBe(person.Email);
        result.ResponseData.Data.Phone.ShouldBe(person.Phone);
        result.ResponseData.Data.City.ShouldBe(person.City);
        result.ResponseData.Data.Country.ShouldBe(person.Country);
    }

    /// <summary>
    /// Verify Create Article sends correct form parameters and deserializes success response
    /// </summary>
    [Test]
    public async Task Create_Article_SendsCorrectFormParameters()
    {
        // Arrange
        var articleCreate = InventoryFakers.ArticleCreate.Generate();
        Server.StubPostJson("/api/v1/inventory/article/create.json",
            CashCtrlResponseFactory.SuccessResponse("Article created", insertId: 99));

        // Act
        var result = await Client.Inventory.Article.Create(articleCreate);

        // Assert - request parameters
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/create.json", "POST");
        request.ShouldHaveFormParam("name", articleCreate.Name);
        request.ShouldHaveFormParam("categoryId", articleCreate.CategoryId?.ToString()!);
        request.ShouldHaveFormParam("salesPrice", articleCreate.SalesPrice?.ToString()!);

        // Assert - response deserialization
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Article created");
        result.ResponseData.InsertId.ShouldBe(99);
    }

    /// <summary>
    /// Verify Create Person sends correct form parameters
    /// </summary>
    [Test]
    public async Task Create_Person_SendsCorrectFormParameters()
    {
        // Arrange
        var personCreate = PersonFakers.PersonCreate.Generate();
        Server.StubPostJson("/api/v1/person/create.json",
            CashCtrlResponseFactory.SuccessResponse("Person created", insertId: 55));

        // Act
        var result = await Client.Person.Person.Create(personCreate);

        // Assert - request parameters
        var request = Server.ShouldHaveReceivedRequest("/api/v1/person/create.json", "POST");
        request.ShouldHaveFormParam("firstName", personCreate.FirstName!);
        request.ShouldHaveFormParam("lastName", personCreate.LastName!);
        request.ShouldHaveFormParam("company", personCreate.Company!);
        request.ShouldHaveFormParam("email", personCreate.Email!);

        // Assert - response deserialization
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify Update Article sends correct form parameters including id
    /// </summary>
    [Test]
    public async Task Update_Article_SendsCorrectFormParameters()
    {
        // Arrange
        var articleUpdate = InventoryFakers.ArticleUpdate.Generate();
        Server.StubPostJson("/api/v1/inventory/article/update.json",
            CashCtrlResponseFactory.SuccessResponse("Article updated"));

        // Act
        var result = await Client.Inventory.Article.Update(articleUpdate);

        // Assert - request parameters
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/update.json", "POST");
        request.ShouldHaveFormParam("id", articleUpdate.Id.ToString());
        request.ShouldHaveFormParam("name", articleUpdate.Name);
        request.ShouldHaveFormParam("categoryId", articleUpdate.CategoryId?.ToString()!);

        // Assert - response deserialization
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify Delete Article sends correct form parameters with CSV-encoded ids
    /// </summary>
    [Test]
    public async Task Delete_Article_SendsCorrectFormParameters()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/article/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Article deleted"));

        // Act
        var result = await Client.Inventory.Article.Delete(new Entries { Ids = [10, 20, 30] });

        // Assert - request parameters
        var request = Server.ShouldHaveReceivedRequest("/api/v1/inventory/article/delete.json", "POST");
        request.ShouldHaveFormParam("ids", "10,20,30");

        // Assert - response deserialization
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify GetList Account deserializes all fields from a list response
    /// </summary>
    [Test]
    public async Task GetList_Account_DeserializesAllFields()
    {
        // Arrange
        var accounts = AccountFakers.AccountListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/account/list.json",
            CashCtrlResponseFactory.ListResponse(accounts));

        // Act
        var result = await Client.Account.Account.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);

        for (var i = 0; i < accounts.Length; i++)
        {
            result.ResponseData.Data[i].Id.ShouldBe(accounts[i].Id);
            result.ResponseData.Data[i].Name.ShouldBe(accounts[i].Name);
            result.ResponseData.Data[i].Number.ShouldBe(accounts[i].Number);
            result.ResponseData.Data[i].CategoryId.ShouldBe(accounts[i].CategoryId);
            result.ResponseData.Data[i].CreatedBy.ShouldBe(accounts[i].CreatedBy);
        }
    }

    /// <summary>
    /// Verify GetList Order deserializes all fields from a list response
    /// </summary>
    [Test]
    public async Task GetList_Order_DeserializesAllFields()
    {
        // Arrange
        var orders = OrderFakers.OrderListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/order/list.json",
            CashCtrlResponseFactory.ListResponse(orders));

        // Act
        var result = await Client.Order.Order.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);

        for (var i = 0; i < orders.Length; i++)
        {
            result.ResponseData.Data[i].Id.ShouldBe(orders[i].Id);
            result.ResponseData.Data[i].AccountId.ShouldBe(orders[i].AccountId);
            result.ResponseData.Data[i].CategoryId.ShouldBe(orders[i].CategoryId);
            result.ResponseData.Data[i].Date.ShouldBe(orders[i].Date);
            result.ResponseData.Data[i].Description.ShouldBe(orders[i].Description);
            result.ResponseData.Data[i].CreatedBy.ShouldBe(orders[i].CreatedBy);
        }
    }
}
