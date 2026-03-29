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

namespace CashCtrlApiNet.IntegrationTests.Salary;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryStatementService"/>
/// </summary>
public class SalaryStatementServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary statement with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var statement = SalaryFakers.Statement.Generate();
        Server.StubGetJson("/api/v1/salary/statement/read.json",
            CashCtrlResponseFactory.SingleResponse(statement));

        // Act
        var result = await Client.Salary.Statement.Get(new() { Id = statement.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(statement.Id);
        result.ResponseData.Data.PersonId.ShouldBe(statement.PersonId);
        result.ResponseData.Data.StatusId.ShouldBe(statement.StatusId);
        result.ResponseData.Data.TemplateId.ShouldBe(statement.TemplateId);
    }

    /// <summary>
    /// Verify GetList returns a list of salary statements
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var statements = SalaryFakers.Statement.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/statement/list.json",
            CashCtrlResponseFactory.ListResponse(statements));

        // Act
        var result = await Client.Salary.Statement.GetList();

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
        var statementCreate = SalaryFakers.StatementCreate.Generate();
        Server.StubPostJson("/api/v1/salary/statement/create.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statement created", insertId: 42));

        // Act
        var result = await Client.Salary.Statement.Create(statementCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statement created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var statementUpdate = SalaryFakers.StatementUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/statement/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statement updated"));

        // Act
        var result = await Client.Salary.Statement.Update(statementUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statement updated");
    }

    /// <summary>
    /// Verify UpdateMultiple sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateMultiple_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/statement/update_multiple.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statements updated"));

        // Act
        var result = await Client.Salary.Statement.UpdateMultiple(new()
        {
            Ids = "1,2,3",
            StatusId = 5,
            TemplateId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statements updated");
    }

    /// <summary>
    /// Verify UpdateStatus sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateStatus_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/statement/update_status.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statement status updated"));

        // Act
        var result = await Client.Salary.Statement.UpdateStatus(new()
        {
            Ids = "1,2",
            StatusId = 3
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statement status updated");
    }

    /// <summary>
    /// Verify UpdateRecurrence sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateRecurrence_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/statement/update_recurrence.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statement recurrence updated"));

        // Act
        var result = await Client.Salary.Statement.UpdateRecurrence(new()
        {
            Id = 1,
            Recurrence = "MONTHLY",
            StartDate = "2026-01-01",
            EndDate = "2026-12-31",
            DaysBefore = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statement recurrence updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/statement/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statement deleted"));

        // Act
        var result = await Client.Salary.Statement.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statement deleted");
    }

    /// <summary>
    /// Verify Calculate sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Calculate_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/statement/calculate.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statement calculated"));

        // Act
        var result = await Client.Salary.Statement.Calculate(new()
        {
            Id = 1,
            PersonId = 10,
            TemplateId = 5,
            Recalculate = true
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statement calculated");
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/statement/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Salary.Statement.UpdateAttachments(new()
        {
            Id = 1,
            AttachedFileIds = [10, 20]
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Attachments updated");
    }

    /// <summary>
    /// Verify ExportExcel returns binary Excel data
    /// </summary>
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/statement/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "statements.xlsx");

        // Act
        var result = await Client.Salary.Statement.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify ExportCsv returns binary CSV data
    /// </summary>
    [Test]
    public async Task ExportCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,date,personId\n1,2026-01-01,10"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/statement/list.csv", csvBytes,
            "text/csv", "statements.csv");

        // Act
        var result = await Client.Salary.Statement.ExportCsv();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify ExportPdf returns binary PDF data
    /// </summary>
    [Test]
    public async Task ExportPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/statement/list.pdf", pdfBytes,
            "application/pdf", "statements.pdf");

        // Act
        var result = await Client.Salary.Statement.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
