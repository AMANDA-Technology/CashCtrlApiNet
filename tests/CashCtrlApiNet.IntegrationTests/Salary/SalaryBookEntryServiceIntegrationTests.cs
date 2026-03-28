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

namespace CashCtrlApiNet.IntegrationTests.Salary;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryBookEntryService"/>
/// </summary>
public class SalaryBookEntryServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary book entry with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var bookEntry = SalaryFakers.BookEntry.Generate();
        Server.StubGetJson("/api/v1/salary/bookentry/read.json",
            CashCtrlResponseFactory.SingleResponse(bookEntry));

        // Act
        var result = await Client.Salary.BookEntry.Get(new Entry { Id = bookEntry.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(bookEntry.Id);
        result.ResponseData.Data.CreditId.ShouldBe(bookEntry.CreditId);
        result.ResponseData.Data.DebitId.ShouldBe(bookEntry.DebitId);
        result.ResponseData.Data.Amount.ShouldBe(bookEntry.Amount);
    }

    /// <summary>
    /// Verify GetList returns a list of salary book entries
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var bookEntries = SalaryFakers.BookEntry.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/bookentry/list.json",
            CashCtrlResponseFactory.ListResponse(bookEntries));

        // Act
        var result = await Client.Salary.BookEntry.GetList(new Entry { Id = 1 });

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
        var bookEntryCreate = SalaryFakers.BookEntryCreate.Generate();
        Server.StubPostJson("/api/v1/salary/bookentry/create.json",
            CashCtrlResponseFactory.SuccessResponse("Salary book entry created", insertId: 42));

        // Act
        var result = await Client.Salary.BookEntry.Create(bookEntryCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary book entry created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var bookEntryUpdate = SalaryFakers.BookEntryUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/bookentry/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary book entry updated"));

        // Act
        var result = await Client.Salary.BookEntry.Update(bookEntryUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary book entry updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/bookentry/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Salary book entry deleted"));

        // Act
        var result = await Client.Salary.BookEntry.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary book entry deleted");
    }
}
