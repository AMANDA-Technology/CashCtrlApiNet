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

namespace CashCtrlApiNet.IntegrationTests.Order;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Order.BookEntryService"/>
/// </summary>
public class BookEntryServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single book entry with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var bookEntry = OrderFakers.BookEntry.Generate();
        Server.StubGetJson("/api/v1/order/bookentry/read.json",
            CashCtrlResponseFactory.SingleResponse(bookEntry));

        // Act
        var result = await Client.Order.BookEntry.Get(new() { Id = bookEntry.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(bookEntry.Id);
        result.ResponseData.Data.OrderId.ShouldBe(bookEntry.OrderId);
        result.ResponseData.Data.AccountId.ShouldBe(bookEntry.AccountId);
        result.ResponseData.Data.Amount.ShouldBe(bookEntry.Amount);
    }

    /// <summary>
    /// Verify GetList returns a list of book entries
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var bookEntries = OrderFakers.BookEntry.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/order/bookentry/list.json",
            CashCtrlResponseFactory.ListResponse(bookEntries));

        // Act
        var result = await Client.Order.BookEntry.GetList(new() { OrderId = 1 });

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
        var bookEntryCreate = OrderFakers.BookEntryCreate.Generate();
        Server.StubPostJson("/api/v1/order/bookentry/create.json",
            CashCtrlResponseFactory.SuccessResponse("Book entry created", insertId: 42));

        // Act
        var result = await Client.Order.BookEntry.Create(bookEntryCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Book entry created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var bookEntryUpdate = OrderFakers.BookEntryUpdate.Generate();
        Server.StubPostJson("/api/v1/order/bookentry/update.json",
            CashCtrlResponseFactory.SuccessResponse("Book entry updated"));

        // Act
        var result = await Client.Order.BookEntry.Update(bookEntryUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Book entry updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/order/bookentry/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Book entry deleted"));

        // Act
        var result = await Client.Order.BookEntry.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Book entry deleted");
    }
}
