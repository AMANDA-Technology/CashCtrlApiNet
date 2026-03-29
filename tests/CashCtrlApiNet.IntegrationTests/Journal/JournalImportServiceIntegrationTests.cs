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

namespace CashCtrlApiNet.IntegrationTests.Journal;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Journal.JournalImportService"/>
/// </summary>
public class JournalImportServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single journal import with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var import = JournalFakers.Import.Generate();
        Server.StubGetJson("/api/v1/journal/import/read.json",
            CashCtrlResponseFactory.SingleResponse(import));

        // Act
        var result = await Client.Journal.Import.Get(new() { Id = import.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(import.Id);
        result.ResponseData.Data.FileId.ShouldBe(import.FileId);
        result.ResponseData.Data.Name.ShouldBe(import.Name);
    }

    /// <summary>
    /// Verify GetList returns a list of journal imports
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var imports = new[] { JournalFakers.Import.Generate(), JournalFakers.Import.Generate(), JournalFakers.Import.Generate() };
        Server.StubGetJson("/api/v1/journal/import/list.json",
            CashCtrlResponseFactory.ListResponse(imports));

        // Act
        var result = await Client.Journal.Import.GetList();

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
        var imports = new[] { JournalFakers.Import.Generate() };
        Server.StubGetJson("/api/v1/journal/import/list.json",
            CashCtrlResponseFactory.ListResponse(imports));
        var listParams = new ListParams { Limit = 5 };

        // Act
        var result = await Client.Journal.Import.GetList(listParams);

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
        var importCreate = JournalFakers.ImportCreate.Generate();
        Server.StubPostJson("/api/v1/journal/import/create.json",
            CashCtrlResponseFactory.SuccessResponse("Import created", insertId: 77));

        // Act
        var result = await Client.Journal.Import.Create(importCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Import created");
        result.ResponseData.InsertId.ShouldBe(77);
    }

    /// <summary>
    /// Verify Execute sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Execute_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/import/execute.json",
            CashCtrlResponseFactory.SuccessResponse("Import executed"));

        // Act
        var result = await Client.Journal.Import.Execute(new() { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Import executed");
    }
}
