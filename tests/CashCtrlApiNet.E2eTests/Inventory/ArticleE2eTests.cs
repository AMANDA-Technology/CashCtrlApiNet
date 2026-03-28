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

using CashCtrlApiNet.Abstractions.Models.Inventory.Article;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Inventory;

/// <summary>
/// E2E tests for inventory article service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IArticleService"/> operations.
/// </summary>
[Category("E2e")]
public class ArticleE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupArticleId;
    private int _createdArticleId;
    private int _categoryId;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test article for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan articles from previous failed runs
        var listResult = await CashCtrlApiClient.Inventory.Article.GetList();
        if (listResult.ResponseData?.Data is { Length: > 0 } articles)
        {
            var orphanIds = articles
                .Where(a => a.Name.StartsWith("E2E-", StringComparison.Ordinal))
                .Select(a => a.Id)
                .ToArray();

            if (orphanIds.Length > 0)
                await CashCtrlApiClient.Inventory.Article.Delete(new() { Ids = [..orphanIds] });
        }

        // Discover a category ID for the categorize test
        var categoryResult = await CashCtrlApiClient.Inventory.ArticleCategory.GetList();
        _categoryId = categoryResult.ResponseData?.Data.FirstOrDefault()?.Id
                      ?? throw new InvalidOperationException("No article categories found for categorize test");

        // Create primary test article
        var createResult = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = _testId,
            Name = _testId
        });
        createResult.ResponseData.ShouldNotBeNull();
        createResult.ResponseData.Success.ShouldBeTrue();
        _setupArticleId = createResult.ResponseData.InsertId
                          ?? throw new InvalidOperationException("Failed to create setup article");

        RegisterCleanup(async () => await CashCtrlApiClient.Inventory.Article.Delete(new() { Ids = [_setupArticleId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get an article by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Get(new() { Id = _setupArticleId });
        res.IsHttpSuccess.ShouldBeTrue();

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.ShouldNotBeNull();
        res.ResponseData.Data.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of articles successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.GetList();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        res.ResponseData.Data.Length.ShouldBe(res.ResponseData.Total);
        res.ResponseData.Data.ShouldContain(a => a.Id == _setupArticleId);
    }

    /// <summary>
    /// Create an article successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = secondTestId,
            Name = secondTestId
        });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Success.ShouldBeTrue();
        res.ResponseData.Errors.ShouldBeNull();
        res.ResponseData.InsertId.ShouldNotBeNull();
        res.ResponseData.InsertId.Value.ShouldBeGreaterThan(0);
        res.ResponseData.Message.ShouldNotBeNullOrEmpty();

        _createdArticleId = res.ResponseData.InsertId.Value;
        RegisterCleanup(async () => await CashCtrlApiClient.Inventory.Article.Delete(new() { Ids = [_createdArticleId] }));
    }

    /// <summary>
    /// Try to create article with duplicate Nr and verify error response
    /// </summary>
    [Test, Order(4)]
    public async Task Create_DuplicateNrFail()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = _testId,
            Name = $"{_testId}-Duplicate"
        });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Success.ShouldBeFalse();
        res.ResponseData.InsertId.ShouldBeNull();

        res.ResponseData.Errors.ShouldNotBeNull();
        res.ResponseData.Errors.Value.ShouldContain(e => e.Field == "nr");
    }

    /// <summary>
    /// Update an article successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Inventory.Article.Get(new() { Id = _setupArticleId });
        var article = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get article for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Inventory.Article.Update((article as ArticleUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Inventory.Article.Get(new() { Id = _setupArticleId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Categorize an article successfully
    /// </summary>
    [Test, Order(6)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Categorize(new()
        {
            Ids = [_setupArticleId],
            TargetCategoryId = _categoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update article attachments successfully
    /// </summary>
    [Test, Order(7)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.UpdateAttachments(new()
        {
            Id = _setupArticleId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export articles as Excel successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportExcel_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.ExportExcel();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.FileName.ShouldNotBeNullOrEmpty();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);

        await DownloadFile(res.ResponseData.FileName, res.ResponseData.Data);
    }

    /// <summary>
    /// Export articles as CSV successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportCsv_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.ExportCsv();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.FileName.ShouldNotBeNullOrEmpty();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);

        await DownloadFile(res.ResponseData.FileName, res.ResponseData.Data);
    }

    /// <summary>
    /// Export articles as PDF successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportPdf_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.ExportPdf();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.FileName.ShouldNotBeNullOrEmpty();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);

        await DownloadFile(res.ResponseData.FileName, res.ResponseData.Data);
    }

    /// <summary>
    /// Delete the article created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(11)]
    public async Task Delete_Success()
    {
        _createdArticleId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Inventory.Article.Delete(new() { Ids = [_createdArticleId] });
        AssertSuccess(res);
    }
}
