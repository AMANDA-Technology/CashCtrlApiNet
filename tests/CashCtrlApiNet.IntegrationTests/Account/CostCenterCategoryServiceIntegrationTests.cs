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

namespace CashCtrlApiNet.IntegrationTests.Account;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Account.CostCenterCategoryService"/>
/// </summary>
public class CostCenterCategoryServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single cost center category with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var category = AccountFakers.CostCenterCategory.Generate();
        Server.StubGetJson("/api/v1/account/costcenter/category/read.json",
            CashCtrlResponseFactory.SingleResponse(category));

        // Act
        var result = await Client.Account.CostCenterCategory.Get(new() { Id = category.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(category.Id);
        result.ResponseData.Data.Name.ShouldBe(category.Name);
        result.ResponseData.Data.Path.ShouldBe(category.Path);
        result.ResponseData.Data.FullName.ShouldBe(category.FullName);
    }

    /// <summary>
    /// Verify GetList returns a list of cost center categories
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var categories = AccountFakers.CostCenterCategory.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/account/costcenter/category/list.json",
            CashCtrlResponseFactory.ListResponse(categories));

        // Act
        var result = await Client.Account.CostCenterCategory.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(2);
    }

    /// <summary>
    /// Verify GetTree returns the cost center category tree
    /// </summary>
    [Test]
    public async Task GetTree_ReturnsExpectedResult()
    {
        // Arrange
        var categories = AccountFakers.CostCenterCategory.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/account/costcenter/category/tree.json",
            CashCtrlResponseFactory.ListResponse(categories));

        // Act
        var result = await Client.Account.CostCenterCategory.GetTree();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var categoryCreate = AccountFakers.CostCenterCategoryCreate.Generate();
        Server.StubPostJson("/api/v1/account/costcenter/category/create.json",
            CashCtrlResponseFactory.SuccessResponse("Category created", insertId: 15));

        // Act
        var result = await Client.Account.CostCenterCategory.Create(categoryCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Category created");
        result.ResponseData.InsertId.ShouldBe(15);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var categoryUpdate = AccountFakers.CostCenterCategoryUpdate.Generate();
        Server.StubPostJson("/api/v1/account/costcenter/category/update.json",
            CashCtrlResponseFactory.SuccessResponse("Category updated"));

        // Act
        var result = await Client.Account.CostCenterCategory.Update(categoryUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Category updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/costcenter/category/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Category deleted"));

        // Act
        var result = await Client.Account.CostCenterCategory.Delete(new() { Ids = [7, 8] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Category deleted");
    }
}
