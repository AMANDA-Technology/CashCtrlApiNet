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
using CashCtrlApiNet.Abstractions.Models.Common.Currency;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Common;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Common.CurrencyService"/>
/// </summary>
public class CurrencyServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single currency with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var currency = CommonFakers.Currency.Generate();
        Server.StubGetJson("/api/v1/currency/read.json",
            CashCtrlResponseFactory.SingleResponse(currency));

        // Act
        var result = await Client.Common.Currency.Get(new Entry { Id = currency.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(currency.Id);
        result.ResponseData.Data.Code.ShouldBe(currency.Code);
    }

    /// <summary>
    /// Verify GetList returns a list of currencies
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var currencies = CommonFakers.CurrencyListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/currency/list.json",
            CashCtrlResponseFactory.ListResponse(currencies));

        // Act
        var result = await Client.Common.Currency.GetList();

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
        var currencies = CommonFakers.CurrencyListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/currency/list.json",
            CashCtrlResponseFactory.ListResponse(currencies));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Common.Currency.GetList(listParams);

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
        var currencyCreate = CommonFakers.CurrencyCreate.Generate();
        Server.StubPostJson("/api/v1/currency/create.json",
            CashCtrlResponseFactory.SuccessResponse("Currency created", insertId: 42));

        // Act
        var result = await Client.Common.Currency.Create(currencyCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Currency created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var currencyUpdate = CommonFakers.CurrencyUpdate.Generate();
        Server.StubPostJson("/api/v1/currency/update.json",
            CashCtrlResponseFactory.SuccessResponse("Currency updated"));

        // Act
        var result = await Client.Common.Currency.Update(currencyUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Currency updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/currency/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Currency deleted"));

        // Act
        var result = await Client.Common.Currency.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Currency deleted");
    }

    /// <summary>
    /// Verify GetExchangeRate returns exchange rate data
    /// </summary>
    [Test]
    public async Task GetExchangeRate_ReturnsExpectedResult()
    {
        // Arrange
        var exchangeRate = CommonFakers.CurrencyExchangeRate.Generate();
        Server.StubGetJson("/api/v1/currency/exchangerate",
            CashCtrlResponseFactory.SingleResponse(exchangeRate));

        // Act
        var result = await Client.Common.Currency.GetExchangeRate(new CurrencyExchangeRateRequest
        {
            From = "CHF",
            To = "EUR"
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Rate.ShouldBe(exchangeRate.Rate);
    }
}
