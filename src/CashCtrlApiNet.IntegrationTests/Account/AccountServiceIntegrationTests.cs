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

using System.Collections.Immutable;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Account;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Account.AccountService"/>
/// </summary>
public class AccountServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single account with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var account = AccountFakers.Account.Generate();
        Server.StubGetJson("/api/v1/account/read.json",
            CashCtrlResponseFactory.SingleResponse(account));

        // Act
        var result = await Client.Account.Account.Get(new Entry { Id = account.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(account.Id);
        result.ResponseData.Data.Name.ShouldBe(account.Name);
        result.ResponseData.Data.Number.ShouldBe(account.Number);
        result.ResponseData.Data.CategoryId.ShouldBe(account.CategoryId);
    }

    /// <summary>
    /// Verify GetList returns a list of accounts
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
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
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Test]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var accounts = AccountFakers.AccountListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/account/list.json",
            CashCtrlResponseFactory.ListResponse(accounts));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.Account.Account.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify GetBalance returns a decimal balance value
    /// </summary>
    [Test]
    public async Task GetBalance_ReturnsExpectedResult()
    {
        // Arrange
        const decimal expectedBalance = 1234.56m;
        Server.StubGetPlainText("/api/v1/account/balance",
            CashCtrlResponseFactory.BalanceResponse(expectedBalance));

        // Act
        var result = await Client.Account.Account.GetBalance(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Balance.ShouldBe(expectedBalance);
    }

    /// <summary>
    /// Verify GetBalance handles zero balance
    /// </summary>
    [Test]
    public async Task GetBalance_WithZeroBalance_ReturnsZero()
    {
        // Arrange
        Server.StubGetPlainText("/api/v1/account/balance",
            CashCtrlResponseFactory.BalanceResponse(0m));

        // Act
        var result = await Client.Account.Account.GetBalance(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Balance.ShouldBe(0m);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var accountCreate = AccountFakers.AccountCreate.Generate();
        Server.StubPostJson("/api/v1/account/create.json",
            CashCtrlResponseFactory.SuccessResponse("Account created", insertId: 42));

        // Act
        var result = await Client.Account.Account.Create(accountCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Account created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var accountUpdate = AccountFakers.AccountUpdate.Generate();
        Server.StubPostJson("/api/v1/account/update.json",
            CashCtrlResponseFactory.SuccessResponse("Account updated"));

        // Act
        var result = await Client.Account.Account.Update(accountUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Account updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Account deleted"));

        // Act
        var result = await Client.Account.Account.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Account deleted");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Accounts categorized"));

        // Act
        var result = await Client.Account.Account.Categorize(new EntriesCategorize
        {
            Ids = [1, 2],
            TargetCategoryId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Account.Account.UpdateAttachments(new EntryAttachments
        {
            Id = 1,
            AttachedFileIds = [10, 20]
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify ExportExcel returns binary data
    /// </summary>
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "accounts.xlsx");

        // Act
        var result = await Client.Account.Account.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify ExportCsv returns binary data
    /// </summary>
    [Test]
    public async Task ExportCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,name,number\n1,Test,1000"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/list.csv", csvBytes, "text/csv", "accounts.csv");

        // Act
        var result = await Client.Account.Account.ExportCsv();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify ExportPdf returns binary data
    /// </summary>
    [Test]
    public async Task ExportPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/list.pdf", pdfBytes, "application/pdf", "accounts.pdf");

        // Act
        var result = await Client.Account.Account.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
