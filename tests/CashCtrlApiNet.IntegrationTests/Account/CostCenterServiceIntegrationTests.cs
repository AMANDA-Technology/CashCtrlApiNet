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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Account.CostCenterService"/>
/// </summary>
public class CostCenterServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single cost center with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var costCenter = AccountFakers.CostCenter.Generate();
        Server.StubGetJson("/api/v1/account/costcenter/read.json",
            CashCtrlResponseFactory.SingleResponse(costCenter));

        // Act
        var result = await Client.Account.CostCenter.Get(new() { Id = costCenter.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(costCenter.Id);
        result.ResponseData.Data.Name.ShouldBe(costCenter.Name);
        result.ResponseData.Data.Number.ShouldBe(costCenter.Number);
    }

    /// <summary>
    /// Verify GetList returns a list of cost centers
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var costCenters = AccountFakers.CostCenterListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/account/costcenter/list.json",
            CashCtrlResponseFactory.ListResponse(costCenters));

        // Act
        var result = await Client.Account.CostCenter.GetList();

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
        var costCenters = AccountFakers.CostCenterListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/account/costcenter/list.json",
            CashCtrlResponseFactory.ListResponse(costCenters));
        var listParams = new ListParams { OnlyActive = true, Limit = 5 };

        // Act
        var result = await Client.Account.CostCenter.GetList(listParams);

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
        const decimal expectedBalance = 5678.90m;
        Server.StubGetPlainText("/api/v1/account/costcenter/balance",
            CashCtrlResponseFactory.DecimalResponse(expectedBalance));

        // Act
        var result = await Client.Account.CostCenter.GetBalance(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Value.ShouldBe(expectedBalance);
    }

    /// <summary>
    /// Verify GetBalance handles zero balance
    /// </summary>
    [Test]
    public async Task GetBalance_WithZeroBalance_ReturnsZero()
    {
        // Arrange
        Server.StubGetPlainText("/api/v1/account/costcenter/balance",
            CashCtrlResponseFactory.DecimalResponse(0m));

        // Act
        var result = await Client.Account.CostCenter.GetBalance(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Value.ShouldBe(0m);
    }

    /// <summary>
    /// Verify GetBalance returns failure result without throwing on HTTP error response
    /// </summary>
    [Test]
    public async Task GetBalance_WithHttpError_ReturnsFailureResult()
    {
        // Arrange
        const string errorBody = "{\"success\":false,\"errorMessage\":\"Not authorized\"}";
        Server.StubGetPlainText("/api/v1/account/costcenter/balance", errorBody, 401);

        // Act
        var result = await Client.Account.CostCenter.GetBalance(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeFalse();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Value.ShouldBe(0m);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var costCenterCreate = AccountFakers.CostCenterCreate.Generate();
        Server.StubPostJson("/api/v1/account/costcenter/create.json",
            CashCtrlResponseFactory.SuccessResponse("Cost center created", insertId: 77));

        // Act
        var result = await Client.Account.CostCenter.Create(costCenterCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Cost center created");
        result.ResponseData.InsertId.ShouldBe(77);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var costCenterUpdate = AccountFakers.CostCenterUpdate.Generate();
        Server.StubPostJson("/api/v1/account/costcenter/update.json",
            CashCtrlResponseFactory.SuccessResponse("Cost center updated"));

        // Act
        var result = await Client.Account.CostCenter.Update(costCenterUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Cost center updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/costcenter/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Cost center deleted"));

        // Act
        var result = await Client.Account.CostCenter.Delete(new() { Ids = [3, 4] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Cost center deleted");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/account/costcenter/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Cost centers categorized"));

        // Act
        var result = await Client.Account.CostCenter.Categorize(new()
        {
            Ids = [3, 4],
            TargetCategoryId = 20
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
        Server.StubPostJson("/api/v1/account/costcenter/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Account.CostCenter.UpdateAttachments(new()
        {
            Id = 3,
            AttachedFileIds = [30, 40]
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
        var excelBytes = "fake-excel-costcenter"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/costcenter/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "costcenters.xlsx");

        // Act
        var result = await Client.Account.CostCenter.ExportExcel();

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
        var csvBytes = "id,name,number\n1,IT,100"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/costcenter/list.csv", csvBytes, "text/csv", "costcenters.csv");

        // Act
        var result = await Client.Account.CostCenter.ExportCsv();

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
        var pdfBytes = "fake-pdf-costcenter"u8.ToArray();
        Server.StubGetBinary("/api/v1/account/costcenter/list.pdf", pdfBytes, "application/pdf", "costcenters.pdf");

        // Act
        var result = await Client.Account.CostCenter.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
