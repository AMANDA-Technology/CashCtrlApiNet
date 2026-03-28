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

using CashCtrlApiNet.Abstractions.Models.Inventory.ArticleCategory;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Inventory;

/// <summary>
/// E2E tests for inventory article category service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IArticleCategoryService"/> operations.
/// </summary>
[Category("E2e")]
public class ArticleCategoryE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupCategoryId;
    private int _createdCategoryId;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test category for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan article categories from previous failed runs
        var listResult = await CashCtrlApiClient.Inventory.ArticleCategory.GetList();
        if (listResult.ResponseData?.Data is { Length: > 0 } categories)
        {
            var orphanIds = categories
                .Where(c => c.Name.StartsWith("E2E-", StringComparison.Ordinal))
                .Select(c => c.Id)
                .ToArray();

            if (orphanIds.Length > 0)
                await CashCtrlApiClient.Inventory.ArticleCategory.Delete(new() { Ids = [..orphanIds] });
        }

        // Create primary test category
        var createResult = await CashCtrlApiClient.Inventory.ArticleCategory.Create(new()
        {
            Name = _testId
        });
        createResult.ResponseData.ShouldNotBeNull();
        createResult.ResponseData.Success.ShouldBeTrue();
        _setupCategoryId = createResult.ResponseData.InsertId
                           ?? throw new InvalidOperationException("Failed to create setup article category");

        RegisterCleanup(async () => await CashCtrlApiClient.Inventory.ArticleCategory.Delete(new() { Ids = [_setupCategoryId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get an article category by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Get(new() { Id = _setupCategoryId });
        res.IsHttpSuccess.ShouldBeTrue();

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.ShouldNotBeNull();
        res.ResponseData.Data.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of article categories successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.GetList();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        res.ResponseData.Data.Length.ShouldBe(res.ResponseData.Total);
        res.ResponseData.Data.ShouldContain(c => c.Id == _setupCategoryId);
    }

    /// <summary>
    /// Get article category tree successfully
    /// </summary>
    [Test, Order(3)]
    public async Task GetTree_Success()
    {
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.GetTree();
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Data.Length.ShouldBeGreaterThan(0);
    }

    /// <summary>
    /// Create an article category successfully and store its ID for later tests
    /// </summary>
    [Test, Order(4)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Create(new()
        {
            Name = secondTestId
        });
        res.IsHttpSuccess.ShouldBeTrue();

        res.ResponseData.ShouldNotBeNull();
        res.ResponseData.Success.ShouldBeTrue();
        res.ResponseData.Errors.ShouldBeNull();
        res.ResponseData.InsertId.ShouldNotBeNull();
        res.ResponseData.InsertId.Value.ShouldBeGreaterThan(0);
        res.ResponseData.Message.ShouldNotBeNullOrEmpty();

        _createdCategoryId = res.ResponseData.InsertId.Value;
        RegisterCleanup(async () => await CashCtrlApiClient.Inventory.ArticleCategory.Delete(new() { Ids = [_createdCategoryId] }));
    }

    /// <summary>
    /// Update an article category successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Inventory.ArticleCategory.Get(new() { Id = _setupCategoryId });
        var category = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get article category for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Update((category as ArticleCategoryUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Inventory.ArticleCategory.Get(new() { Id = _setupCategoryId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the article category created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(6)]
    public async Task Delete_Success()
    {
        _createdCategoryId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Delete(new() { Ids = [_createdCategoryId] });
        AssertSuccess(res);
    }
}
