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

namespace CashCtrlApiNet.IntegrationTests.Common;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Common.RoundingService"/>
/// </summary>
public class RoundingServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single rounding with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var rounding = CommonFakers.Rounding.Generate();
        Server.StubGetJson("/api/v1/rounding/read.json",
            CashCtrlResponseFactory.SingleResponse(rounding));

        // Act
        var result = await Client.Common.Rounding.Get(new Entry { Id = rounding.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(rounding.Id);
        result.ResponseData.Data.Name.ShouldBe(rounding.Name);
        result.ResponseData.Data.AccountId.ShouldBe(rounding.AccountId);
    }

    /// <summary>
    /// Verify GetList returns a list of roundings
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var roundings = CommonFakers.RoundingListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/rounding/list.json",
            CashCtrlResponseFactory.ListResponse(roundings));

        // Act
        var result = await Client.Common.Rounding.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Test]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var roundings = CommonFakers.RoundingListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/rounding/list.json",
            CashCtrlResponseFactory.ListResponse(roundings));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Common.Rounding.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var roundingCreate = CommonFakers.RoundingCreate.Generate();
        Server.StubPostJson("/api/v1/rounding/create.json",
            CashCtrlResponseFactory.SuccessResponse("Rounding created", insertId: 42));

        // Act
        var result = await Client.Common.Rounding.Create(roundingCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Rounding created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var roundingUpdate = CommonFakers.RoundingUpdate.Generate();
        Server.StubPostJson("/api/v1/rounding/update.json",
            CashCtrlResponseFactory.SuccessResponse("Rounding updated"));

        // Act
        var result = await Client.Common.Rounding.Update(roundingUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Rounding updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/rounding/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Rounding deleted"));

        // Act
        var result = await Client.Common.Rounding.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Rounding deleted");
    }
}
