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

using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Common;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Common.CustomFieldGroupService"/>
/// </summary>
public class CustomFieldGroupServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single custom field group with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var group = CommonFakers.CustomFieldGroup.Generate();
        Server.StubGetJson("/api/v1/customfield/group/read.json",
            CashCtrlResponseFactory.SingleResponse(group));

        // Act
        var result = await Client.Common.CustomFieldGroup.Get(new() { Id = group.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(group.Id);
        result.ResponseData.Data.Name.ShouldBe(group.Name);
        result.ResponseData.Data.Type.ShouldBe(group.Type);
    }

    /// <summary>
    /// Verify GetList returns a list of custom field groups
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var groups = CommonFakers.CustomFieldGroupListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/customfield/group/list.json",
            CashCtrlResponseFactory.ListResponse(groups));

        // Act
        var result = await Client.Common.CustomFieldGroup.GetList(new()
        {
            Type = CustomFieldType.JOURNAL
        });

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
        var groupCreate = CommonFakers.CustomFieldGroupCreate.Generate();
        Server.StubPostJson("/api/v1/customfield/group/create.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field group created", insertId: 42));

        // Act
        var result = await Client.Common.CustomFieldGroup.Create(groupCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field group created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var groupUpdate = CommonFakers.CustomFieldGroupUpdate.Generate();
        Server.StubPostJson("/api/v1/customfield/group/update.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field group updated"));

        // Act
        var result = await Client.Common.CustomFieldGroup.Update(groupUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field group updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/customfield/group/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field group deleted"));

        // Act
        var result = await Client.Common.CustomFieldGroup.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field group deleted");
    }

    /// <summary>
    /// Verify Reorder sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Reorder_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/customfield/group/reorder.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field groups reordered"));

        // Act
        var result = await Client.Common.CustomFieldGroup.Reorder(new()
        {
            Type = CustomFieldType.PERSON,
            Ids = [1, 2, 3],
            Target = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field groups reordered");
    }
}
