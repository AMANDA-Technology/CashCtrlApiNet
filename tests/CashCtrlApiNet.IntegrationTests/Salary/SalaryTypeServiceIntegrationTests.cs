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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryTypeService"/>
/// </summary>
public class SalaryTypeServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary type with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var salaryType = SalaryFakers.Type.Generate();
        Server.StubGetJson("/api/v1/salary/type/read.json",
            CashCtrlResponseFactory.SingleResponse(salaryType));

        // Act
        var result = await Client.Salary.Type.Get(new() { Id = salaryType.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(salaryType.Id);
        result.ResponseData.Data.Name.ShouldBe(salaryType.Name);
        result.ResponseData.Data.CategoryId.ShouldBe(salaryType.CategoryId);
        result.ResponseData.Data.Type.ShouldBe(salaryType.Type);
    }

    /// <summary>
    /// Verify GetList returns a list of salary types
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var salaryTypes = SalaryFakers.Type.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/type/list.json",
            CashCtrlResponseFactory.ListResponse(salaryTypes));

        // Act
        var result = await Client.Salary.Type.GetList();

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
        var typeCreate = SalaryFakers.TypeCreate.Generate();
        Server.StubPostJson("/api/v1/salary/type/create.json",
            CashCtrlResponseFactory.SuccessResponse("Salary type created", insertId: 42));

        // Act
        var result = await Client.Salary.Type.Create(typeCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary type created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var typeUpdate = SalaryFakers.TypeUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/type/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary type updated"));

        // Act
        var result = await Client.Salary.Type.Update(typeUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary type updated");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/type/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Salary types categorized"));

        // Act
        var result = await Client.Salary.Type.Categorize(new()
        {
            Ids = [1, 2],
            TargetCategoryId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary types categorized");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/type/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Salary type deleted"));

        // Act
        var result = await Client.Salary.Type.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary type deleted");
    }

    /// <summary>
    /// Verify ExportExcel returns binary Excel data
    /// </summary>
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/type/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "salary-types.xlsx");

        // Act
        var result = await Client.Salary.Type.ExportExcel();

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
        var csvBytes = "id,name,number,type\n1,Grundlohn,100,SALARY"u8.ToArray();
        Server.StubGetBinary("/api/v1/salary/type/list.csv", csvBytes,
            "text/csv", "salary-types.csv");

        // Act
        var result = await Client.Salary.Type.ExportCsv();

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
        Server.StubGetBinary("/api/v1/salary/type/list.pdf", pdfBytes,
            "application/pdf", "salary-types.pdf");

        // Act
        var result = await Client.Salary.Type.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
