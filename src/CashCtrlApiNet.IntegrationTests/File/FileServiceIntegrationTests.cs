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

using System.Collections.Immutable;
using System.Net.Http;
using System.Text;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.File;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.File.FileService"/>
/// </summary>
public class FileServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify GetContent returns binary file content
    /// </summary>
    [Test]
    public async Task GetContent_ReturnsExpectedResult()
    {
        // Arrange
        var fileBytes = "fake-file-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/file/get", fileBytes, "application/octet-stream", "document.pdf");

        // Act
        var result = await Client.File.File.GetContent(new Entry { Id = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(fileBytes);
    }

    /// <summary>
    /// Verify Get returns a single file with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var file = FileFakers.File.Generate();
        Server.StubGetJson("/api/v1/file/read.json",
            CashCtrlResponseFactory.SingleResponse(file));

        // Act
        var result = await Client.File.File.Get(new Entry { Id = file.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(file.Id);
        result.ResponseData.Data.Name.ShouldBe(file.Name);
        result.ResponseData.Data.Description.ShouldBe(file.Description);
        result.ResponseData.Data.CategoryId.ShouldBe(file.CategoryId);
    }

    /// <summary>
    /// Verify GetList returns a list of files
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var files = FileFakers.File.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/file/list.json",
            CashCtrlResponseFactory.ListResponse(files));

        // Act
        var result = await Client.File.File.GetList();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify GetList with params returns filtered results
    /// </summary>
    [Test]
    public async Task GetList_WithParams_ReturnsExpectedResult()
    {
        // Arrange
        var files = FileFakers.File.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/file/list.json",
            CashCtrlResponseFactory.ListResponse(files));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.File.File.GetList(listParams);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(1);
    }

    /// <summary>
    /// Verify Prepare uploads multipart content and returns success
    /// </summary>
    [Test]
    public async Task Prepare_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/file/prepare.json",
            CashCtrlResponseFactory.SuccessResponse("File prepared", insertId: 99));
        using var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent("test-file-data"u8.ToArray()), "file", "test.txt");

        // Act
        var result = await Client.File.File.Prepare(content);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("File prepared");
        result.ResponseData.InsertId.ShouldBe(99);
    }

    /// <summary>
    /// Verify Persist sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Persist_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/file/persist.json",
            CashCtrlResponseFactory.SuccessResponse("File persisted"));

        // Act
        var result = await Client.File.File.Persist(new Entries { Ids = [99] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("File persisted");
    }

    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var fileCreate = FileFakers.FileCreate.Generate();
        Server.StubPostJson("/api/v1/file/create.json",
            CashCtrlResponseFactory.SuccessResponse("File created", insertId: 42));

        // Act
        var result = await Client.File.File.Create(fileCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("File created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var fileUpdate = FileFakers.FileUpdate.Generate();
        Server.StubPostJson("/api/v1/file/update.json",
            CashCtrlResponseFactory.SuccessResponse("File updated"));

        // Act
        var result = await Client.File.File.Update(fileUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("File updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/file/delete.json",
            CashCtrlResponseFactory.SuccessResponse("File deleted"));

        // Act
        var result = await Client.File.File.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("File deleted");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/file/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Files categorized"));

        // Act
        var result = await Client.File.File.Categorize(new EntriesCategorize { Ids = [1, 2], TargetCategoryId = 5 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Files categorized");
    }

    /// <summary>
    /// Verify EmptyArchive sends correct request and returns success
    /// </summary>
    [Test]
    public async Task EmptyArchive_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/file/empty_archive.json",
            CashCtrlResponseFactory.SuccessResponse("Archive emptied"));

        // Act
        var result = await Client.File.File.EmptyArchive();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Archive emptied");
    }

    /// <summary>
    /// Verify Restore sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Restore_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/file/restore.json",
            CashCtrlResponseFactory.SuccessResponse("Files restored"));

        // Act
        var result = await Client.File.File.Restore(new Entries { Ids = [3, 4] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Files restored");
    }

    /// <summary>
    /// Verify ExportExcel returns binary Excel data
    /// </summary>
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/file/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "files.xlsx");

        // Act
        var result = await Client.File.File.ExportExcel();

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
        var csvBytes = "id,name,description\n1,Test,A file"u8.ToArray();
        Server.StubGetBinary("/api/v1/file/list.csv", csvBytes, "text/csv", "files.csv");

        // Act
        var result = await Client.File.File.ExportCsv();

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
        Server.StubGetBinary("/api/v1/file/list.pdf", pdfBytes, "application/pdf", "files.pdf");

        // Act
        var result = await Client.File.File.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
