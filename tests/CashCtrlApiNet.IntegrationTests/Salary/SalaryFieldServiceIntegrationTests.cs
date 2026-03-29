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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Salary.SalaryFieldService"/>
/// </summary>
public class SalaryFieldServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single salary field with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var field = SalaryFakers.Field.Generate();
        Server.StubGetJson("/api/v1/salary/field/read.json",
            CashCtrlResponseFactory.SingleResponse(field));

        // Act
        var result = await Client.Salary.Field.Get(new() { Id = field.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(field.Id);
        result.ResponseData.Data.Name.ShouldBe(field.Name);
        result.ResponseData.Data.VariableName.ShouldBe(field.VariableName);
    }

    /// <summary>
    /// Verify GetList returns a list of salary fields
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var fields = SalaryFakers.Field.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/salary/field/list.json",
            CashCtrlResponseFactory.ListResponse(fields));

        // Act
        var result = await Client.Salary.Field.GetList(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }
}
