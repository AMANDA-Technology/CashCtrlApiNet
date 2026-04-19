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

namespace CashCtrlApiNet.IntegrationTests.Journal;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Journal.JournalImportEntryService"/>
/// </summary>
public class JournalImportEntryServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single journal import entry with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var entry = JournalFakers.ImportEntry.Generate();
        Server.StubGetJson("/api/v1/journal/import/entry/read.json",
            CashCtrlResponseFactory.SingleResponse(entry));

        // Act
        var result = await Client.Journal.ImportEntry.Get(new() { Id = entry.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(entry.Id);
    }

    /// <summary>
    /// Verify GetList returns a list of journal import entries
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var entries = JournalFakers.ImportEntryListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/journal/import/entry/list.json",
            CashCtrlResponseFactory.ListResponse(entries));

        // Act
        var result = await Client.Journal.ImportEntry.GetList(new() { ImportId = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.Length.ShouldBe(3);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var entryUpdate = JournalFakers.ImportEntryUpdate.Generate();
        Server.StubPostJson("/api/v1/journal/import/entry/update.json",
            CashCtrlResponseFactory.SuccessResponse("Entry updated"));

        // Act
        var result = await Client.Journal.ImportEntry.Update(entryUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Entry updated");
    }

    /// <summary>
    /// Verify Ignore sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Ignore_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/import/entry/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Entries ignored"));

        // Act
        var result = await Client.Journal.ImportEntry.Ignore(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Entries ignored");
    }

    /// <summary>
    /// Verify Restore sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Restore_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/import/entry/restore.json",
            CashCtrlResponseFactory.SuccessResponse("Entries restored"));

        // Act
        var result = await Client.Journal.ImportEntry.Restore(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Entries restored");
    }

    /// <summary>
    /// Verify Confirm sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Confirm_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/import/entry/confirm.json",
            CashCtrlResponseFactory.SuccessResponse("Entries confirmed"));

        // Act
        var result = await Client.Journal.ImportEntry.Confirm(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Entries confirmed");
    }

    /// <summary>
    /// Verify Unconfirm sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Unconfirm_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/import/entry/unconfirm.json",
            CashCtrlResponseFactory.SuccessResponse("Entries unconfirmed"));

        // Act
        var result = await Client.Journal.ImportEntry.Unconfirm(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Entries unconfirmed");
    }

    /// <summary>
    /// Verify ExportExcel returns binary data
    /// </summary>
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/journal/import/entry/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "import-entries.xlsx");

        // Act
        var result = await Client.Journal.ImportEntry.ExportExcel(new() { ImportId = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify ExportCsv returns binary data
    /// </summary>
    [Test]
    public async Task ExportCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,status\n1,confirmed"u8.ToArray();
        Server.StubGetBinary("/api/v1/journal/import/entry/list.csv", csvBytes, "text/csv", "import-entries.csv");

        // Act
        var result = await Client.Journal.ImportEntry.ExportCsv(new() { ImportId = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify ExportPdf returns binary data
    /// </summary>
    [Test]
    public async Task ExportPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/journal/import/entry/list.pdf", pdfBytes, "application/pdf", "import-entries.pdf");

        // Act
        var result = await Client.Journal.ImportEntry.ExportPdf(new() { ImportId = 1 });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
