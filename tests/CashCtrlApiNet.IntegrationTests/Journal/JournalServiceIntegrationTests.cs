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
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Journal.JournalService"/>
/// </summary>
public class JournalServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single journal entry with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var journal = JournalFakers.Journal.Generate();
        Server.StubGetJson("/api/v1/journal/read.json",
            CashCtrlResponseFactory.SingleResponse(journal));

        // Act
        var result = await Client.Journal.Journal.Get(new() { Id = journal.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(journal.Id);
        result.ResponseData.Data.Title.ShouldBe(journal.Title);
        result.ResponseData.Data.DateAdded.ShouldBe(journal.DateAdded);
        result.ResponseData.Data.SequenceNumberId.ShouldBe(journal.SequenceNumberId);
    }

    /// <summary>
    /// Verify GetList returns a list of journal entries
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var journals = JournalFakers.JournalListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/journal/list.json",
            CashCtrlResponseFactory.ListResponse(journals));

        // Act
        var result = await Client.Journal.Journal.GetList();

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
        var journals = JournalFakers.JournalListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/journal/list.json",
            CashCtrlResponseFactory.ListResponse(journals));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.Journal.Journal.GetList(listParams);

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
        var journalCreate = JournalFakers.JournalCreate.Generate();
        Server.StubPostJson("/api/v1/journal/create.json",
            CashCtrlResponseFactory.SuccessResponse("Journal created", insertId: 42));

        // Act
        var result = await Client.Journal.Journal.Create(journalCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Journal created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var journalUpdate = JournalFakers.JournalUpdate.Generate();
        Server.StubPostJson("/api/v1/journal/update.json",
            CashCtrlResponseFactory.SuccessResponse("Journal updated"));

        // Act
        var result = await Client.Journal.Journal.Update(journalUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Journal updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Journal deleted"));

        // Act
        var result = await Client.Journal.Journal.Delete(new() { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Journal deleted");
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Journal.Journal.UpdateAttachments(new()
        {
            Id = 1,
            AttachedFileIds = [10, 20]
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify UpdateRecurrence sends correct request and returns success
    /// </summary>
    [Test]
    public async Task UpdateRecurrence_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/journal/update_recurrence.json",
            CashCtrlResponseFactory.SuccessResponse("Recurrence updated"));

        // Act
        var result = await Client.Journal.Journal.UpdateRecurrence(new()
        {
            Id = 1,
            Recurrence = "{\"type\":\"monthly\",\"interval\":1}"
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify ExportExcel returns binary data
    /// </summary>
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/journal/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "journal.xlsx");

        // Act
        var result = await Client.Journal.Journal.ExportExcel();

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
        var csvBytes = "id,title,amount\n1,Test,100.00"u8.ToArray();
        Server.StubGetBinary("/api/v1/journal/list.csv", csvBytes, "text/csv", "journal.csv");

        // Act
        var result = await Client.Journal.Journal.ExportCsv();

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
        Server.StubGetBinary("/api/v1/journal/list.pdf", pdfBytes, "application/pdf", "journal.pdf");

        // Act
        var result = await Client.Journal.Journal.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
