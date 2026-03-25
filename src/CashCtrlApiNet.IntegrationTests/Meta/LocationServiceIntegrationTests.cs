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

namespace CashCtrlApiNet.IntegrationTests.Meta;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Meta.LocationService"/>
/// </summary>
public class LocationServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single location with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var location = MetaFakers.Location.Generate();
        Server.StubGetJson("/api/v1/location/read.json",
            CashCtrlResponseFactory.SingleResponse(location));

        // Act
        var result = await Client.Meta.Location.Get(new Entry { Id = location.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(location.Id);
        result.ResponseData.Data.Name.ShouldBe(location.Name);
        result.ResponseData.Data.OrgName.ShouldBe(location.OrgName);
    }

    /// <summary>
    /// Verify GetList returns a list of locations
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var locations = MetaFakers.LocationListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/location/list.json",
            CashCtrlResponseFactory.ListResponse(locations));

        // Act
        var result = await Client.Meta.Location.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Fact]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var locations = MetaFakers.LocationListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/location/list.json",
            CashCtrlResponseFactory.ListResponse(locations));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Meta.Location.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var locationCreate = MetaFakers.LocationCreate.Generate();
        Server.StubPostJson("/api/v1/location/create.json",
            CashCtrlResponseFactory.SuccessResponse("Location created", insertId: 42));

        // Act
        var result = await Client.Meta.Location.Create(locationCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Location created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var locationUpdate = MetaFakers.LocationUpdate.Generate();
        Server.StubPostJson("/api/v1/location/update.json",
            CashCtrlResponseFactory.SuccessResponse("Location updated"));

        // Act
        var result = await Client.Meta.Location.Update(locationUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Location updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/location/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Location deleted"));

        // Act
        var result = await Client.Meta.Location.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Location deleted");
    }
}
