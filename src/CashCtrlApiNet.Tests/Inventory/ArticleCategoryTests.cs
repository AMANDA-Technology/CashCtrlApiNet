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
using FluentAssertions;

namespace CashCtrlApiNet.Tests.Inventory;

/// <summary>
/// Tests for inventory article category service
/// </summary>
[TestCaseOrderer("CashCtrlApiNet.Tests.AlphabeticalOrderer", "CashCtrlApiNet.Tests")]
public class ArticleCategoryTests : CashCtrlTestBase
{
    /// <summary>
    /// Get an article successfully
    /// </summary>
    [Fact]
    public async Task Test1_Get_Success()
    {
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Get(new(){ Id = 1 });
        res.IsHttpSuccess.Should().BeTrue();

        res.RequestsLeft.Should().NotBeNull().And.BePositive();
        res.CashCtrlHttpStatusCodeDescription.Should().NotBeNullOrEmpty();

        Assert.NotNull(res.ResponseData?.Data);
        res.ResponseData.Data.Name.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Get list of articles successfully
    /// </summary>
    [Fact]
    public async Task Test2_GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.GetList();
        res.IsHttpSuccess.Should().BeTrue();

        Assert.NotNull(res.ResponseData);
        res.ResponseData.Data.Length.Should().BePositive().And.Be(res.ResponseData.Total);
    }

    /// <summary>
    /// Create an article successfully
    /// </summary>
    [Fact]
    public async Task Test4_Create_Success()
    {
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Create(new()
        {
            Name = "Test created"
        });
        res.IsHttpSuccess.Should().BeTrue();

        Assert.NotNull(res.ResponseData);
        res.ResponseData.Success.Should().BeTrue();
        res.ResponseData.Errors.Should().BeNull();
        res.ResponseData.InsertId.Should().NotBeNull().And.BePositive();
        res.ResponseData.Message.Should().NotBeNullOrEmpty().And.Be("Category saved");
    }

    /// <summary>
    /// Update an article successfully
    /// </summary>
    [Fact]
    public async Task? Test5_Update_Success()
    {
        var get = await CashCtrlApiClient.Inventory.ArticleCategory.Get(new(){ Id = 6 });
        var articleCategory = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get article category");

        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Update((articleCategory as ArticleCategoryUpdate) with
        {
            Name = "Test updated"
        });
        res.IsHttpSuccess.Should().BeTrue();

        Assert.NotNull(res.ResponseData);
        res.ResponseData.Success.Should().BeTrue();
        res.ResponseData.Errors.Should().BeNull();
        res.ResponseData.InsertId.Should().NotBeNull().And.BePositive();
        res.ResponseData.Message.Should().NotBeNullOrEmpty().And.Be("Category saved");
    }

    /// <summary>
    /// Delete an article successfully
    /// </summary>
    [Fact]
    public async Task Test6_Delete_Success()
    {
        // Wait until test article created
        ArticleCategory? articleCategory = null;

        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20)))
        {
            while (articleCategory is null && !cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cts.Token);
                articleCategory = await Get(cts.Token);
            }
        }

        Assert.NotNull(articleCategory);

        // Then delete it
        var res = await CashCtrlApiClient.Inventory.ArticleCategory.Delete(new(){ Ids = [articleCategory.Id]});
        res.IsHttpSuccess.Should().BeTrue();

        Assert.NotNull(res.ResponseData);
        res.ResponseData.Success.Should().BeTrue();
        res.ResponseData.Errors.Should().BeNull();
        res.ResponseData.Message.Should().NotBeNullOrEmpty().And.Be("1 category deleted");
        return;

        // Local function to get article
        async Task<ArticleCategory?> Get(CancellationToken cancellationToken)
            => (await CashCtrlApiClient.Inventory.ArticleCategory.GetList(cancellationToken)).ResponseData?.Data.SingleOrDefault(a => a.Name.Equals("Test created"));
    }
}
