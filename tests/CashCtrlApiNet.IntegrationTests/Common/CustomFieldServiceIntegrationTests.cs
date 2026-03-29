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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Common.CustomFieldService"/>
/// </summary>
public class CustomFieldServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single custom field with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var customField = CommonFakers.CustomField.Generate();
        Server.StubGetJson("/api/v1/customfield/read.json",
            CashCtrlResponseFactory.SingleResponse(customField));

        // Act
        var result = await Client.Common.CustomField.Get(new() { Id = customField.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(customField.Id);
        result.ResponseData.Data.RowLabel.ShouldBe(customField.RowLabel);
        result.ResponseData.Data.DataType.ShouldBe(customField.DataType);
    }

    /// <summary>
    /// Verify GetList returns a list of custom fields
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var customFields = CommonFakers.CustomFieldListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/customfield/list.json",
            CashCtrlResponseFactory.ListResponse(customFields));

        // Act
        var result = await Client.Common.CustomField.GetList(new()
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
        var customFieldCreate = CommonFakers.CustomFieldCreate.Generate();
        Server.StubPostJson("/api/v1/customfield/create.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field created", insertId: 42));

        // Act
        var result = await Client.Common.CustomField.Create(customFieldCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var customFieldUpdate = CommonFakers.CustomFieldUpdate.Generate();
        Server.StubPostJson("/api/v1/customfield/update.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field updated"));

        // Act
        var result = await Client.Common.CustomField.Update(customFieldUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/customfield/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Custom field deleted"));

        // Act
        var result = await Client.Common.CustomField.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom field deleted");
    }

    /// <summary>
    /// Verify Reorder sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Reorder_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/customfield/reorder.json",
            CashCtrlResponseFactory.SuccessResponse("Custom fields reordered"));

        // Act
        var result = await Client.Common.CustomField.Reorder(new()
        {
            Type = CustomFieldType.PERSON,
            Ids = [1, 2, 3],
            Target = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Custom fields reordered");
    }

    /// <summary>
    /// Verify GetTypes returns expected result
    /// </summary>
    [Test]
    public async Task GetTypes_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubGetJson("/api/v1/customfield/types.json",
            """{"success": true}""");

        // Act
        var result = await Client.Common.CustomField.GetTypes();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
    }
}
