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

namespace CashCtrlApiNet.IntegrationTests.Common;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Common.TaxRateService"/>
/// </summary>
public class TaxRateServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single tax rate with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var taxRate = CommonFakers.TaxRate.Generate();
        Server.StubGetJson("/api/v1/tax/read.json",
            CashCtrlResponseFactory.SingleResponse(taxRate));

        // Act
        var result = await Client.Common.TaxRate.Get(new() { Id = taxRate.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(taxRate.Id);
        result.ResponseData.Data.Description.ShouldBe(taxRate.Description);
        result.ResponseData.Data.Code.ShouldBe(taxRate.Code);
    }

    /// <summary>
    /// Verify GetList returns a list of tax rates
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var taxRates = CommonFakers.TaxRateListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/tax/list.json",
            CashCtrlResponseFactory.ListResponse(taxRates));

        // Act
        var result = await Client.Common.TaxRate.GetList();

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
        var taxRates = CommonFakers.TaxRateListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/tax/list.json",
            CashCtrlResponseFactory.ListResponse(taxRates));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Common.TaxRate.GetList(listParams);

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
        var taxRateCreate = CommonFakers.TaxRateCreate.Generate();
        Server.StubPostJson("/api/v1/tax/create.json",
            CashCtrlResponseFactory.SuccessResponse("Tax rate created", insertId: 42));

        // Act
        var result = await Client.Common.TaxRate.Create(taxRateCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Tax rate created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var taxRateUpdate = CommonFakers.TaxRateUpdate.Generate();
        Server.StubPostJson("/api/v1/tax/update.json",
            CashCtrlResponseFactory.SuccessResponse("Tax rate updated"));

        // Act
        var result = await Client.Common.TaxRate.Update(taxRateUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Tax rate updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/tax/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Tax rate deleted"));

        // Act
        var result = await Client.Common.TaxRate.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Tax rate deleted");
    }
}
