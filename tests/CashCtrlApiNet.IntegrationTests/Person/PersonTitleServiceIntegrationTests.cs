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

namespace CashCtrlApiNet.IntegrationTests.Person;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Person.PersonTitleService"/>
/// </summary>
public class PersonTitleServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single person title with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var title = PersonFakers.PersonTitle.Generate();
        Server.StubGetJson("/api/v1/person/title/read.json",
            CashCtrlResponseFactory.SingleResponse(title));

        // Act
        var result = await Client.Person.Title.Get(new() { Id = title.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(title.Id);
        result.ResponseData.Data.Title.ShouldBe(title.Title);
    }

    /// <summary>
    /// Verify GetList returns a list of person titles
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var titles = PersonFakers.PersonTitle.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/person/title/list.json",
            CashCtrlResponseFactory.ListResponse(titles));

        // Act
        var result = await Client.Person.Title.GetList();

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
        var titleCreate = PersonFakers.PersonTitleCreate.Generate();
        Server.StubPostJson("/api/v1/person/title/create.json",
            CashCtrlResponseFactory.SuccessResponse("Title created", insertId: 15));

        // Act
        var result = await Client.Person.Title.Create(titleCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Title created");
        result.ResponseData.InsertId.ShouldBe(15);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var titleUpdate = PersonFakers.PersonTitleUpdate.Generate();
        Server.StubPostJson("/api/v1/person/title/update.json",
            CashCtrlResponseFactory.SuccessResponse("Title updated"));

        // Act
        var result = await Client.Person.Title.Update(titleUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Title updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/person/title/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Title deleted"));

        // Act
        var result = await Client.Person.Title.Delete(new() { Ids = [3, 4] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Title deleted");
    }
}
