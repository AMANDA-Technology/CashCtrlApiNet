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

using System.Diagnostics.CodeAnalysis;
using CashCtrlApiNet.Abstractions.Models.Inventory;

namespace CashCtrlApiNet.Tests.Inventory;

/// <summary>
/// Tests for inventory article service
/// </summary>
[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local", Justification = "This is the nature of Assert.Contains evaluations")]
public class ArticleTests : CashCtrlTestBase
{
    /// <summary>
    /// Get an article successfully
    /// </summary>
    [Fact]
    public async Task Test1_Get_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Get(new(){ Id = 1 });

        Assert.True(res.IsHttpSuccess);
        Assert.NotNull(res.RequestsLeft);
        Assert.NotNull(res.CashCtrlHttpStatusCodeDescription);

        Assert.NotNull(res.ResponseData?.Data);
        Assert.NotEmpty(res.ResponseData.Data.Name);
    }

    /// <summary>
    /// Get list of articles successfully
    /// </summary>
    [Fact]
    public async Task Test2_GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.GetList();

        Assert.NotNull(res.ResponseData);
        Assert.True(res.ResponseData.Data.Length.Equals(res.ResponseData.Total));
    }

    /// <summary>
    /// Try create article with duplicated Nr and fail
    /// </summary>
    [Fact]
    public async Task Test3_Create_DuplicateNrFail()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = "A-00001",
            Name = "Test"
        });

        Assert.NotNull(res.ResponseData);
        Assert.Multiple(() =>
        {
            Assert.False(res.ResponseData.Success);
            Assert.Null(res.ResponseData.InsertId);
            Assert.NotNull(res.ResponseData.Errors);
            Assert.Contains(res.ResponseData.Errors.Value, apiError
                => apiError.Field.Equals("nr")
                   && apiError.Message.Equals("This article no. is already used by another article."));
        });
    }

    /// <summary>
    /// Create an article successfully
    /// </summary>
    [Fact]
    public async Task Test4_Create_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = "A-00005",
            Name = "Test created"
        });

        Assert.NotNull(res.ResponseData);
        Assert.Multiple(() =>
        {
            Assert.True(res.ResponseData.Success);
            Assert.Null(res.ResponseData.Errors);
            Assert.NotNull(res.ResponseData.InsertId);
            Assert.NotNull(res.ResponseData.Message);
            Assert.Equal("Article saved", res.ResponseData.Message);
        });
    }

    /// <summary>
    /// Update an article successfully
    /// </summary>
    [Fact]
    public async Task? Test5_Update_Success()
    {
        var get = await CashCtrlApiClient.Inventory.Article.Get(new(){ Id = 1 });
        var article = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get article");

        var res = await CashCtrlApiClient.Inventory.Article.Update((article as ArticleUpdate) with
        {
            Name = "Test updated"
        });

        Assert.NotNull(res.ResponseData);
        Assert.Multiple(() =>
        {
            Assert.True(res.ResponseData.Success);
            Assert.Null(res.ResponseData.Errors);
            Assert.NotNull(res.ResponseData.InsertId);
            Assert.NotNull(res.ResponseData.Message);
            Assert.Equal("Article saved", res.ResponseData.Message);
        });
    }

    /// <summary>
    /// Delete an article successfully
    /// </summary>
    [Fact]
    public async Task Test6_Delete_Success()
    {
        // Wait until test article created
        Article? article = null;

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        while (article is null && !cts.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cts.Token);
            article = await Get(cts.Token);
        }

        Assert.NotNull(article);

        // Then delete it
        var res = await CashCtrlApiClient.Inventory.Article.Delete(new(){ Ids = [article.Id]}, cts.Token);

        Assert.NotNull(res.ResponseData);
        Assert.Multiple(() =>
        {
            Assert.True(res.ResponseData.Success);
            Assert.Null(res.ResponseData.Errors);
            Assert.NotNull(res.ResponseData.Message);
            Assert.Equal("1 article deleted", res.ResponseData.Message);
        });
        return;

        // Local function to get article
        async Task<Article?> Get(CancellationToken cancellationToken)
            => (await CashCtrlApiClient.Inventory.Article.GetList(cancellationToken))?.ResponseData?.Data.SingleOrDefault(a => a.Nr.Equals("A-00005"));
    }
}
