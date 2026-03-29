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

using Bogus;
using CashCtrlApiNet.Abstractions.Models.Inventory.Import;
using ArticleModel = CashCtrlApiNet.Abstractions.Models.Inventory.Article.Article;
using ArticleListedModel = CashCtrlApiNet.Abstractions.Models.Inventory.Article.ArticleListed;
using ArticleCreateModel = CashCtrlApiNet.Abstractions.Models.Inventory.Article.ArticleCreate;
using ArticleUpdateModel = CashCtrlApiNet.Abstractions.Models.Inventory.Article.ArticleUpdate;
using ArticleCategoryModel = CashCtrlApiNet.Abstractions.Models.Inventory.ArticleCategory.ArticleCategory;
using ArticleCategoryCreateModel = CashCtrlApiNet.Abstractions.Models.Inventory.ArticleCategory.ArticleCategoryCreate;
using ArticleCategoryUpdateModel = CashCtrlApiNet.Abstractions.Models.Inventory.ArticleCategory.ArticleCategoryUpdate;
using FixedAssetModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset.FixedAsset;
using FixedAssetListedModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset.FixedAssetListed;
using FixedAssetCreateModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset.FixedAssetCreate;
using FixedAssetUpdateModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAsset.FixedAssetUpdate;
using FixedAssetCategoryModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAssetCategory.FixedAssetCategory;
using FixedAssetCategoryCreateModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAssetCategory.FixedAssetCategoryCreate;
using FixedAssetCategoryUpdateModel = CashCtrlApiNet.Abstractions.Models.Inventory.FixedAssetCategory.FixedAssetCategoryUpdate;
using UnitModel = CashCtrlApiNet.Abstractions.Models.Inventory.Unit.Unit;
using UnitCreateModel = CashCtrlApiNet.Abstractions.Models.Inventory.Unit.UnitCreate;
using UnitUpdateModel = CashCtrlApiNet.Abstractions.Models.Inventory.Unit.UnitUpdate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Inventory domain models
/// </summary>
public static class InventoryFakers
{
    /// <summary>
    /// Faker for <see cref="ArticleModel"/> (detail response)
    /// </summary>
    public static readonly Faker<ArticleModel> Article = new Faker<ArticleModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.AlphaNumeric(10),
            CategoryId = f.Random.Int(1, 100),
            SalesPrice = f.Random.Double(1, 1000),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="ArticleListedModel"/> (list response)
    /// </summary>
    public static readonly Faker<ArticleListedModel> ArticleListed = new Faker<ArticleListedModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.AlphaNumeric(10),
            CategoryId = f.Random.Int(1, 100),
            SalesPrice = f.Random.Double(1, 1000),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="ArticleCreateModel"/>
    /// </summary>
    public static readonly Faker<ArticleCreateModel> ArticleCreate = new Faker<ArticleCreateModel>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.ProductName(),
            CategoryId = f.Random.Int(1, 100),
            SalesPrice = f.Random.Double(1, 1000)
        });

    /// <summary>
    /// Faker for <see cref="ArticleUpdateModel"/>
    /// </summary>
    public static readonly Faker<ArticleUpdateModel> ArticleUpdate = new Faker<ArticleUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.AlphaNumeric(10),
            CategoryId = f.Random.Int(1, 100),
            SalesPrice = f.Random.Double(1, 1000)
        });

    /// <summary>
    /// Faker for <see cref="ArticleCategoryModel"/> (detail response)
    /// </summary>
    public static readonly Faker<ArticleCategoryModel> ArticleCategory = new Faker<ArticleCategoryModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="ArticleCategoryCreateModel"/>
    /// </summary>
    public static readonly Faker<ArticleCategoryCreateModel> ArticleCategoryCreate = new Faker<ArticleCategoryCreateModel>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="ArticleCategoryUpdateModel"/>
    /// </summary>
    public static readonly Faker<ArticleCategoryUpdateModel> ArticleCategoryUpdate = new Faker<ArticleCategoryUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetModel"/> (detail response)
    /// </summary>
    public static readonly Faker<FixedAssetModel> FixedAsset = new Faker<FixedAssetModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.AlphaNumeric(10),
            CategoryId = f.Random.Int(1, 100),
            PurchasePrice = f.Random.Double(100, 50000),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetListedModel"/> (list response)
    /// </summary>
    public static readonly Faker<FixedAssetListedModel> FixedAssetListed = new Faker<FixedAssetListedModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.AlphaNumeric(10),
            CategoryId = f.Random.Int(1, 100),
            PurchasePrice = f.Random.Double(100, 50000),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetCreateModel"/>
    /// </summary>
    public static readonly Faker<FixedAssetCreateModel> FixedAssetCreate = new Faker<FixedAssetCreateModel>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.ProductName(),
            CategoryId = f.Random.Int(1, 100),
            PurchasePrice = f.Random.Double(100, 50000)
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetUpdateModel"/>
    /// </summary>
    public static readonly Faker<FixedAssetUpdateModel> FixedAssetUpdate = new Faker<FixedAssetUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName(),
            Nr = f.Random.AlphaNumeric(10),
            CategoryId = f.Random.Int(1, 100),
            PurchasePrice = f.Random.Double(100, 50000)
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetCategoryModel"/> (detail response)
    /// </summary>
    public static readonly Faker<FixedAssetCategoryModel> FixedAssetCategory = new Faker<FixedAssetCategoryModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Path = "/" + f.Commerce.Categories(1)[0],
            FullName = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetCategoryCreateModel"/>
    /// </summary>
    public static readonly Faker<FixedAssetCategoryCreateModel> FixedAssetCategoryCreate = new Faker<FixedAssetCategoryCreateModel>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="FixedAssetCategoryUpdateModel"/>
    /// </summary>
    public static readonly Faker<FixedAssetCategoryUpdateModel> FixedAssetCategoryUpdate = new Faker<FixedAssetCategoryUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="UnitModel"/> (detail response)
    /// </summary>
    public static readonly Faker<UnitModel> Unit = new Faker<UnitModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.PickRandom("pcs.", "kg", "m", "l", "h"),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="UnitCreateModel"/>
    /// </summary>
    public static readonly Faker<UnitCreateModel> UnitCreate = new Faker<UnitCreateModel>()
        .CustomInstantiator(f => new()
        {
            Name = f.PickRandom("pcs.", "kg", "m", "l", "h")
        });

    /// <summary>
    /// Faker for <see cref="UnitUpdateModel"/>
    /// </summary>
    public static readonly Faker<UnitUpdateModel> UnitUpdate = new Faker<UnitUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.PickRandom("pcs.", "kg", "m", "l", "h")
        });

    /// <summary>
    /// Faker for <see cref="InventoryImportCreate"/>
    /// </summary>
    public static readonly Faker<InventoryImportCreate> ImportCreate = new Faker<InventoryImportCreate>()
        .CustomInstantiator(f => new()
        {
            FileId = f.Random.Int(1, 9999)
        });

    /// <summary>
    /// Faker for <see cref="InventoryImportMapping"/>
    /// </summary>
    public static readonly Faker<InventoryImportMapping> ImportMapping = new Faker<InventoryImportMapping>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Mapping = "{\"name\":\"Name\",\"nr\":\"Number\"}"
        });

    /// <summary>
    /// Faker for <see cref="InventoryImportPreview"/>
    /// </summary>
    public static readonly Faker<InventoryImportPreview> ImportPreview = new Faker<InventoryImportPreview>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999)
        });

    /// <summary>
    /// Faker for <see cref="InventoryImportExecute"/>
    /// </summary>
    public static readonly Faker<InventoryImportExecute> ImportExecute = new Faker<InventoryImportExecute>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999)
        });
}
