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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Common.SequenceNumberService"/>
/// </summary>
public class SequenceNumberServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single sequence number with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var sequenceNumber = CommonFakers.SequenceNumber.Generate();
        Server.StubGetJson("/api/v1/sequencenumber/read.json",
            CashCtrlResponseFactory.SingleResponse(sequenceNumber));

        // Act
        var result = await Client.Common.SequenceNumber.Get(new() { Id = sequenceNumber.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(sequenceNumber.Id);
        result.ResponseData.Data.Name.ShouldBe(sequenceNumber.Name);
        result.ResponseData.Data.Pattern.ShouldBe(sequenceNumber.Pattern);
    }

    /// <summary>
    /// Verify GetList returns a list of sequence numbers
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var sequenceNumbers = CommonFakers.SequenceNumberListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/sequencenumber/list.json",
            CashCtrlResponseFactory.ListResponse(sequenceNumbers));

        // Act
        var result = await Client.Common.SequenceNumber.GetList();

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
        var sequenceNumbers = CommonFakers.SequenceNumberListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/sequencenumber/list.json",
            CashCtrlResponseFactory.ListResponse(sequenceNumbers));
        var listParams = new ListParams { Limit = 10 };

        // Act
        var result = await Client.Common.SequenceNumber.GetList(listParams);

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
        var sequenceNumberCreate = CommonFakers.SequenceNumberCreate.Generate();
        Server.StubPostJson("/api/v1/sequencenumber/create.json",
            CashCtrlResponseFactory.SuccessResponse("Sequence number created", insertId: 42));

        // Act
        var result = await Client.Common.SequenceNumber.Create(sequenceNumberCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Sequence number created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var sequenceNumberUpdate = CommonFakers.SequenceNumberUpdate.Generate();
        Server.StubPostJson("/api/v1/sequencenumber/update.json",
            CashCtrlResponseFactory.SuccessResponse("Sequence number updated"));

        // Act
        var result = await Client.Common.SequenceNumber.Update(sequenceNumberUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Sequence number updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/sequencenumber/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Sequence number deleted"));

        // Act
        var result = await Client.Common.SequenceNumber.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Sequence number deleted");
    }

    /// <summary>
    /// Verify GetGeneratedNumber returns a generated sequence number
    /// </summary>
    [Test]
    public async Task GetGeneratedNumber_ReturnsExpectedResult()
    {
        // Arrange
        var sequenceNumber = CommonFakers.SequenceNumber.Generate();
        Server.StubGetJson("/api/v1/sequencenumber/get",
            CashCtrlResponseFactory.SingleResponse(sequenceNumber));

        // Act
        var result = await Client.Common.SequenceNumber.GetGeneratedNumber(new() { Id = sequenceNumber.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(sequenceNumber.Id);
        result.ResponseData.Data.Name.ShouldBe(sequenceNumber.Name);
    }
}
