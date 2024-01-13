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

namespace CashCtrlApiNet.Tests.Inventory;

[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local", Justification = "This is the nature of Assert.Contains evaluations")]
public class ArticleTests : CashCtrlTestBase
{
    [Fact]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.GetList();

        Assert.NotNull(res.RequestsLeft);
        Assert.NotNull(res.CashCtrlHttpStatusCodeDescription);

        Assert.NotNull(res.ResponseData);
        Assert.Multiple(() =>
        {
            Assert.True(res.ResponseData.Data.Length.Equals(res.ResponseData.Total));
        });
    }

    [Fact]
    public async Task Create_DuplicateNrFail()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = "A-00001"
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

    [Fact]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Article.Create(new()
        {
            Nr = "A-00005",
            Name = "Test"
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
}
