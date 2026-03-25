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

namespace CashCtrlApiNet.IntegrationTests.Meta;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Meta.FiscalPeriodService"/>
/// </summary>
public class FiscalPeriodServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single fiscal period with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var fiscalPeriod = MetaFakers.FiscalPeriod.Generate();
        Server.StubGetJson("/api/v1/fiscalperiod/read.json",
            CashCtrlResponseFactory.SingleResponse(fiscalPeriod));

        // Act
        var result = await Client.Meta.FiscalPeriod.Get(new Entry { Id = fiscalPeriod.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(fiscalPeriod.Id);
        result.ResponseData.Data.StartDate.ShouldBe(fiscalPeriod.StartDate);
        result.ResponseData.Data.EndDate.ShouldBe(fiscalPeriod.EndDate);
        result.ResponseData.Data.Name.ShouldBe(fiscalPeriod.Name);
    }

    /// <summary>
    /// Verify GetList returns a list of fiscal periods
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var fiscalPeriods = MetaFakers.FiscalPeriodListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/fiscalperiod/list.json",
            CashCtrlResponseFactory.ListResponse(fiscalPeriods));

        // Act
        var result = await Client.Meta.FiscalPeriod.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify GetList with list params works correctly
    /// </summary>
    [Fact]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var fiscalPeriods = MetaFakers.FiscalPeriodListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/fiscalperiod/list.json",
            CashCtrlResponseFactory.ListResponse(fiscalPeriods));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Meta.FiscalPeriod.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var fiscalPeriodCreate = MetaFakers.FiscalPeriodCreate.Generate();
        Server.StubPostJson("/api/v1/fiscalperiod/create.json",
            CashCtrlResponseFactory.SuccessResponse("Fiscal period created", insertId: 42));

        // Act
        var result = await Client.Meta.FiscalPeriod.Create(fiscalPeriodCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fiscal period created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var fiscalPeriodUpdate = MetaFakers.FiscalPeriodUpdate.Generate();
        Server.StubPostJson("/api/v1/fiscalperiod/update.json",
            CashCtrlResponseFactory.SuccessResponse("Fiscal period updated"));

        // Act
        var result = await Client.Meta.FiscalPeriod.Update(fiscalPeriodUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fiscal period updated");
    }

    /// <summary>
    /// Verify Switch sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Switch_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/switch.json",
            CashCtrlResponseFactory.SuccessResponse("Fiscal period switched"));

        // Act
        var result = await Client.Meta.FiscalPeriod.Switch(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fiscal period switched");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Fiscal period deleted"));

        // Act
        var result = await Client.Meta.FiscalPeriod.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fiscal period deleted");
    }

    /// <summary>
    /// Verify GetResult returns a single fiscal period result
    /// </summary>
    [Fact]
    public async Task GetResult_ReturnsExpectedResult()
    {
        // Arrange
        var fiscalPeriod = MetaFakers.FiscalPeriod.Generate();
        Server.StubGetJson("/api/v1/fiscalperiod/result",
            CashCtrlResponseFactory.SingleResponse(fiscalPeriod));

        // Act
        var result = await Client.Meta.FiscalPeriod.GetResult(new Entry { Id = fiscalPeriod.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(fiscalPeriod.Id);
    }

    /// <summary>
    /// Verify GetDepreciations returns a list of depreciations
    /// </summary>
    [Fact]
    public async Task GetDepreciations_ReturnsExpectedResult()
    {
        // Arrange
        var depreciations = MetaFakers.FiscalPeriodListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/fiscalperiod/depreciations.json",
            CashCtrlResponseFactory.ListResponse(depreciations));

        // Act
        var result = await Client.Meta.FiscalPeriod.GetDepreciations(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(2);
    }

    /// <summary>
    /// Verify BookDepreciations sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task BookDepreciations_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/bookdepreciations.json",
            CashCtrlResponseFactory.SuccessResponse("Depreciations booked"));

        // Act
        var result = await Client.Meta.FiscalPeriod.BookDepreciations(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Depreciations booked");
    }

    /// <summary>
    /// Verify GetExchangeDiff returns a list of exchange differences
    /// </summary>
    [Fact]
    public async Task GetExchangeDiff_ReturnsExpectedResult()
    {
        // Arrange
        var exchangeDiffs = MetaFakers.FiscalPeriodListed.Generate(2).ToArray();
        Server.StubGetJson("/api/v1/fiscalperiod/exchangediff.json",
            CashCtrlResponseFactory.ListResponse(exchangeDiffs));

        // Act
        var result = await Client.Meta.FiscalPeriod.GetExchangeDiff(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(2);
    }

    /// <summary>
    /// Verify BookExchangeDiff sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task BookExchangeDiff_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/bookexchangediff.json",
            CashCtrlResponseFactory.SuccessResponse("Exchange differences booked"));

        // Act
        var result = await Client.Meta.FiscalPeriod.BookExchangeDiff(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Exchange differences booked");
    }

    /// <summary>
    /// Verify Complete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Complete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/complete.json",
            CashCtrlResponseFactory.SuccessResponse("Fiscal period completed"));

        // Act
        var result = await Client.Meta.FiscalPeriod.Complete(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fiscal period completed");
    }

    /// <summary>
    /// Verify Reopen sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Reopen_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/reopen.json",
            CashCtrlResponseFactory.SuccessResponse("Fiscal period reopened"));

        // Act
        var result = await Client.Meta.FiscalPeriod.Reopen(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Fiscal period reopened");
    }

    /// <summary>
    /// Verify CompleteMonths sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task CompleteMonths_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/complete_months.json",
            CashCtrlResponseFactory.SuccessResponse("Months completed"));

        // Act
        var result = await Client.Meta.FiscalPeriod.CompleteMonths(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Months completed");
    }

    /// <summary>
    /// Verify ReopenMonths sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task ReopenMonths_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/fiscalperiod/reopen_months.json",
            CashCtrlResponseFactory.SuccessResponse("Months reopened"));

        // Act
        var result = await Client.Meta.FiscalPeriod.ReopenMonths(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Months reopened");
    }
}
