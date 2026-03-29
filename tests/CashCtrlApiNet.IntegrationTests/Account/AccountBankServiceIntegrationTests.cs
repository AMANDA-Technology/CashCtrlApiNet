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

namespace CashCtrlApiNet.IntegrationTests.Account;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Account.AccountBankService"/>
/// </summary>
public class AccountBankServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single bank account with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var bankAccount = AccountFakers.AccountBank.Generate();
        Server.StubGetJson("/api/v1/account/bank/read.json",
            CashCtrlResponseFactory.SingleResponse(bankAccount));

        // Act
        var result = await Client.Account.Bank.Get(new() { Id = bankAccount.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(bankAccount.Id);
        result.ResponseData.Data.Name.ShouldBe(bankAccount.Name);
        result.ResponseData.Data.Bic.ShouldBe(bankAccount.Bic);
        result.ResponseData.Data.Iban.ShouldBe(bankAccount.Iban);
        result.ResponseData.Data.Type.ShouldBe(bankAccount.Type);
    }

    /// <summary>
    /// Verify GetList returns a list of bank accounts
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var bankAccounts = AccountFakers.AccountBank.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/account/bank/list.json",
            CashCtrlResponseFactory.ListResponse(bankAccounts));

        // Act
        var result = await Client.Account.Bank.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(2);
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Test]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var bankAccounts = AccountFakers.AccountBank.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/account/bank/list.json",
            CashCtrlResponseFactory.ListResponse(bankAccounts));
        var listParams = new ListParams { OnlyActive = true };

        // Act
        var result = await Client.Account.Bank.GetList(listParams);

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
        var bankAccountCreate = AccountFakers.AccountBankCreate.Generate();
        Server.StubPostJson("/api/v1/account/bank/create.json",
            CashCtrlResponseFactory.SuccessResponse("Bank account created", insertId: 55));

        // Act
        var result = await Client.Account.Bank.Create(bankAccountCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Bank account created");
        result.ResponseData.InsertId.ShouldBe(55);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var bankAccountUpdate = AccountFakers.AccountBankUpdate.Generate();
        Server.StubPostJson("/api/v1/account/bank/update.json",
            CashCtrlResponseFactory.SuccessResponse("Bank account updated"));

        // Act
        var result = await Client.Account.Bank.Update(bankAccountUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Bank account updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/bank/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Bank account deleted"));

        // Act
        var result = await Client.Account.Bank.Delete(new() { Ids = [1] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Bank account deleted");
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/bank/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Account.Bank.UpdateAttachments(new()
        {
            Id = 5,
            AttachedFileIds = [100, 200]
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
        var excelBytes = "fake-excel-bank"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/bank/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "bank-accounts.xlsx");

        // Act
        var result = await Client.Account.Bank.ExportExcel();

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
        var csvBytes = "id,name,bic,iban\n1,Test,ABCD,CH00"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/bank/list.csv", csvBytes, "text/csv", "bank-accounts.csv");

        // Act
        var result = await Client.Account.Bank.ExportCsv();

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
        var pdfBytes = "fake-pdf-bank"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/bank/list.pdf", pdfBytes, "application/pdf", "bank-accounts.pdf");

        // Act
        var result = await Client.Account.Bank.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
