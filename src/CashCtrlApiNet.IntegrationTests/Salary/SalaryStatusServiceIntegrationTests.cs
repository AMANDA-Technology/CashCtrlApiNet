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
using CashCtrlApiNet.Abstractions.Models.Salary.Status;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Salary;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryStatusService"/>
/// </summary>
public class SalaryStatusServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary status with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var status = SalaryFakers.Status.Generate();
        Server.StubGetJson("/api/v1/salary/status/read.json",
            CashCtrlResponseFactory.SingleResponse(status));

        // Act
        var result = await Client.Salary.Status.Get(new Entry { Id = status.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(status.Id);
        result.ResponseData.Data.Name.ShouldBe(status.Name);
        result.ResponseData.Data.Icon.ShouldBe(status.Icon);
    }

    /// <summary>
    /// Verify GetList returns a list of salary statuses
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var statuses = SalaryFakers.Status.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/status/list.json",
            CashCtrlResponseFactory.ListResponse(statuses));

        // Act
        var result = await Client.Salary.Status.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var statusCreate = SalaryFakers.StatusCreate.Generate();
        Server.StubPostJson("/api/v1/salary/status/create.json",
            CashCtrlResponseFactory.SuccessResponse("Salary status created", insertId: 42));

        // Act
        var result = await Client.Salary.Status.Create(statusCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary status created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var statusUpdate = SalaryFakers.StatusUpdate.Generate();
        Server.StubPostJson("/api/v1/salary/status/update.json",
            CashCtrlResponseFactory.SuccessResponse("Salary status updated"));

        // Act
        var result = await Client.Salary.Status.Update(statusUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary status updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/status/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Salary status deleted"));

        // Act
        var result = await Client.Salary.Status.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary status deleted");
    }

    /// <summary>
    /// Verify Reorder sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Reorder_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/salary/status/reorder.json",
            CashCtrlResponseFactory.SuccessResponse("Salary statuses reordered"));

        // Act
        var result = await Client.Salary.Status.Reorder(new SalaryStatusReorder
        {
            Ids = "1,2,3",
            Target = 5
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Salary statuses reordered");
    }
}
