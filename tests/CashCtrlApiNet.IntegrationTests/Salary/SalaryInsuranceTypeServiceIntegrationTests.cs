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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryInsuranceTypeService"/>
/// </summary>
public class SalaryInsuranceTypeServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary insurance type with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var insuranceType = SalaryFakers.InsuranceType.Generate();
        Server.StubGetJson("/api/v1/salary/insurance/type/read.json",
            CashCtrlResponseFactory.SingleResponse(insuranceType));

        // Act
        var result = await Client.Salary.InsuranceType.Get(new() { Id = insuranceType.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(insuranceType.Id);
        result.ResponseData.Data.Name.ShouldBe(insuranceType.Name);
        result.ResponseData.Data.Description.ShouldBe(insuranceType.Description);
    }

    /// <summary>
    /// Verify GetList returns a list of salary insurance types
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var insuranceTypes = SalaryFakers.InsuranceType.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/insurance/type/list.json",
            CashCtrlResponseFactory.ListResponse(insuranceTypes));

        // Act
        var result = await Client.Salary.InsuranceType.GetList();

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
        var insuranceTypeCreate = SalaryFakers.InsuranceTypeCreate.Generate();
        Server.StubPostJson("/api/v1/salary/insurance/type/create.json",
            CashCtrlResponseFactory.SuccessResponse("Salary insurance type created", insertId: 42));

        // Act
        var result = await Client.Salary.InsuranceType.Create(insuranceTypeCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary insurance type created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var insuranceTypeUpdate = SalaryFakers.InsuranceTypeUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/insurance/type/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary insurance type updated"));

        // Act
        var result = await Client.Salary.InsuranceType.Update(insuranceTypeUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary insurance type updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/insurance/type/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Salary insurance type deleted"));

        // Act
        var result = await Client.Salary.InsuranceType.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary insurance type deleted");
    }
}
