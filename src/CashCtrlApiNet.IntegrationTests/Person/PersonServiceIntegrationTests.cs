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
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Person;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Person.PersonService"/>
/// </summary>
public class PersonServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single person with correct deserialization
    /// </summary>
    [Fact]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var person = PersonFakers.Person.Generate();
        Server.StubGetJson("/api/v1/person/read.json",
            CashCtrlResponseFactory.SingleResponse(person));

        // Act
        var result = await Client.Person.Person.Get(new Entry { Id = person.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(person.Id);
        result.ResponseData.Data.FirstName.ShouldBe(person.FirstName);
        result.ResponseData.Data.LastName.ShouldBe(person.LastName);
        result.ResponseData.Data.Company.ShouldBe(person.Company);
    }

    /// <summary>
    /// Verify GetList returns a list of persons
    /// </summary>
    [Fact]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var persons = PersonFakers.PersonListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/person/list.json",
            CashCtrlResponseFactory.ListResponse(persons));

        // Act
        var result = await Client.Person.Person.GetList();

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
        var persons = PersonFakers.PersonListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/person/list.json",
            CashCtrlResponseFactory.ListResponse(persons));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.Person.Person.GetList(listParams);

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
        var personCreate = PersonFakers.PersonCreate.Generate();
        Server.StubPostJson("/api/v1/person/create.json",
            CashCtrlResponseFactory.SuccessResponse("Person created", insertId: 42));

        // Act
        var result = await Client.Person.Person.Create(personCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Person created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var personUpdate = PersonFakers.PersonUpdate.Generate();
        Server.StubPostJson("/api/v1/person/update.json",
            CashCtrlResponseFactory.SuccessResponse("Person updated"));

        // Act
        var result = await Client.Person.Person.Update(personUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Person updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/person/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Person deleted"));

        // Act
        var result = await Client.Person.Person.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Person deleted");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/person/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Persons categorized"));

        // Act
        var result = await Client.Person.Person.Categorize(new EntriesCategorize
        {
            Ids = [1, 2],
            TargetCategoryId = 10
        });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
    }

    /// <summary>
    /// Verify UpdateAttachments sends correct request and returns success
    /// </summary>
    [Fact]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/person/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Person.Person.UpdateAttachments(new EntryAttachments
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
    /// Verify ExportExcel returns binary data
    /// </summary>
    [Fact]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/person/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");

        // Act
        var result = await Client.Person.Person.ExportExcel();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(excelBytes);
    }

    /// <summary>
    /// Verify ExportCsv returns binary data
    /// </summary>
    [Fact]
    public async Task ExportCsv_ReturnsExpectedResult()
    {
        // Arrange
        var csvBytes = "id,firstName,lastName\n1,John,Doe"u8.ToArray();
        Server.StubGetBinary("/api/v1/person/list.csv", csvBytes, "text/csv", "persons.csv");

        // Act
        var result = await Client.Person.Person.ExportCsv();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(csvBytes);
    }

    /// <summary>
    /// Verify ExportPdf returns binary data
    /// </summary>
    [Fact]
    public async Task ExportPdf_ReturnsExpectedResult()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/person/list.pdf", pdfBytes, "application/pdf", "persons.pdf");

        // Act
        var result = await Client.Person.Person.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }

    /// <summary>
    /// Verify ExportVcard returns binary vCard data
    /// </summary>
    [Fact]
    public async Task ExportVcard_ReturnsExpectedResult()
    {
        // Arrange
        var vcardBytes = "BEGIN:VCARD\nVERSION:3.0\nN:Doe;John\nFN:John Doe\nEND:VCARD"u8.ToArray();
        Server.StubGetBinary("/api/v1/person/list.vcf", vcardBytes, "text/vcard", "persons.vcf");

        // Act
        var result = await Client.Person.Person.ExportVcard();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(vcardBytes);
    }
}
