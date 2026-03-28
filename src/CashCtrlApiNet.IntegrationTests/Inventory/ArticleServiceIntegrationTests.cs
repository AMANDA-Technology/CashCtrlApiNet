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

namespace CashCtrlApiNet.IntegrationTests.Inventory;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Inventory.ArticleService"/>
/// </summary>
public class ArticleServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Get returns a single article with correct deserialization
    /// </summary>
    [Test]
    public async Task Get_ReturnsExpectedResult()
    {
        // Arrange
        var article = InventoryFakers.Article.Generate();
        Server.StubGetJson("/api/v1/inventory/article/read.json",
            CashCtrlResponseFactory.SingleResponse(article));

        // Act
        var result = await Client.Inventory.Article.Get(new Entry { Id = article.Id });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldNotBeNull();
        result.ResponseData.Data.Id.ShouldBe(article.Id);
        result.ResponseData.Data.Name.ShouldBe(article.Name);
        result.ResponseData.Data.Nr.ShouldBe(article.Nr);
        result.ResponseData.Data.CategoryId.ShouldBe(article.CategoryId);
    }

    /// <summary>
    /// Verify GetList returns a list of articles
    /// </summary>
    [Test]
    public async Task GetList_ReturnsExpectedResult()
    {
        // Arrange
        var articles = InventoryFakers.ArticleListed.Generate(3).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));

        // Act
        var result = await Client.Inventory.Article.GetList();

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
        var articles = InventoryFakers.ArticleListed.Generate(1).ToArray();
        Server.StubGetJson("/api/v1/inventory/article/list.json",
            CashCtrlResponseFactory.ListResponse(articles));
        var listParams = new ListParams { CategoryId = 1, Limit = 10 };

        // Act
        var result = await Client.Inventory.Article.GetList(listParams);

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
        var articleCreate = InventoryFakers.ArticleCreate.Generate();
        Server.StubPostJson("/api/v1/inventory/article/create.json",
            CashCtrlResponseFactory.SuccessResponse("Article created", insertId: 42));

        // Act
        var result = await Client.Inventory.Article.Create(articleCreate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Article created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Update sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Update_ReturnsExpectedResult()
    {
        // Arrange
        var articleUpdate = InventoryFakers.ArticleUpdate.Generate();
        Server.StubPostJson("/api/v1/inventory/article/update.json",
            CashCtrlResponseFactory.SuccessResponse("Article updated"));

        // Act
        var result = await Client.Inventory.Article.Update(articleUpdate);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Article updated");
    }

    /// <summary>
    /// Verify Delete sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Delete_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/article/delete.json",
            CashCtrlResponseFactory.SuccessResponse("Article deleted"));

        // Act
        var result = await Client.Inventory.Article.Delete(new Entries { Ids = [1, 2] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Article deleted");
    }

    /// <summary>
    /// Verify Categorize sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Categorize_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/article/categorize.json",
            CashCtrlResponseFactory.SuccessResponse("Articles categorized"));

        // Act
        var result = await Client.Inventory.Article.Categorize(new EntriesCategorize
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
    [Test]
    public async Task UpdateAttachments_ReturnsExpectedResult()
    {
        // Arrange
        Server.StubPostJson("/api/v1/inventory/article/update_attachments.json",
            CashCtrlResponseFactory.SuccessResponse("Attachments updated"));

        // Act
        var result = await Client.Inventory.Article.UpdateAttachments(new EntryAttachments
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
    [Test]
    public async Task ExportExcel_ReturnsExpectedResult()
    {
        // Arrange
        var excelBytes = "fake-excel-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/inventory/article/list.xlsx", excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "articles.xlsx");

        // Act
        var result = await Client.Inventory.Article.ExportExcel();

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
        var csvBytes = "id,name,nr\n1,Test,A001"u8.ToArray();
        Server.StubGetBinary("/api/v1/inventory/article/list.csv", csvBytes, "text/csv", "articles.csv");

        // Act
        var result = await Client.Inventory.Article.ExportCsv();

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
        Server.StubGetBinary("/api/v1/inventory/article/list.pdf", pdfBytes, "application/pdf", "articles.pdf");

        // Act
        var result = await Client.Inventory.Article.ExportPdf();

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(pdfBytes);
    }
}
