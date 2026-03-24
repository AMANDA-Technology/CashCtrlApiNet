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

using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests;

/// <summary>
/// Smoke tests to verify the integration test infrastructure works correctly
/// </summary>
public class SmokeTests : IntegrationTestBase
{
    /// <summary>
    /// Verify that the WireMock server starts and the client can reach it
    /// </summary>
    [Fact]
    public async Task WireMockServer_StartsAndClientCanConnect()
    {
        // Arrange
        Server.StubGetJson("/api/v1/inventory/article/read.json", """
            {
                "success": true,
                "data": {
                    "id": 1,
                    "nr": "A-00001",
                    "name": "Test Article",
                    "created": "2024-01-01 00:00:00.0",
                    "createdBy": "test"
                }
            }
            """);

        // Act
        var result = await Client.Inventory.Article.Get(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.RequestsLeft.ShouldBe(100);
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Name.ShouldBe("Test Article");
    }

    /// <summary>
    /// Verify that the response factory creates valid JSON that deserializes correctly
    /// </summary>
    [Fact]
    public async Task CashCtrlResponseFactory_SuccessResponse_DeserializesCorrectly()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/article/create.json",
            CashCtrlResponseFactory.SuccessResponse("Article saved", insertId: 42));

        // Act
        var result = await Client.Inventory.Article.Create(new() { Name = "Test" });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Article saved");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify that binary response stubbing works
    /// </summary>
    [Fact]
    public async Task WireMockServer_BinaryResponseWorks()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/inventory/article/list.xlsx", pdfBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "articles.xlsx");

        // Act
        var result = await Client.Inventory.Article.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
