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

using CashCtrlApiNet.Interfaces.Connectors.Inventory;

namespace CashCtrlApiNet.Interfaces.Connectors;

/// <summary>
/// CashCtrl inventory service endpoint group. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory">API Doc - Inventory</see>
/// </summary>
public interface IInventoryConnector
{
    /// <summary>
    /// CashCtrl inventory article service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article">API Doc - Inventory/Article</see>
    /// </summary>
    public IArticleService Article { get; }

    /// <summary>
    /// CashCtrl inventory article category service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/category">API Doc - Inventory/Article category</see>
    /// </summary>
    public IArticleCategoryService ArticleCategory { get; }

    /// <summary>
    /// CashCtrl inventory fixed asset service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset">API Doc - Inventory/Fixed asset</see>
    /// </summary>
    public IFixedAssetService FixedAsset { get; }

    /// <summary>
    /// CashCtrl inventory fixed asset category service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/asset/category">API Doc - Inventory/Fixed asset category</see>
    /// </summary>
    public IFixedAssetCategoryService FixedAssetCategory { get; }

    /// <summary>
    /// CashCtrl inventory import service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/article/import">API Doc - Inventory/Import</see>
    /// </summary>
    public IInventoryImportService Import { get; }

    /// <summary>
    /// CashCtrl inventory unit service endpoint. <see href="https://app.cashctrl.com/static/help/en/api/index.html#/inventory/unit">API Doc - Inventory/Unit</see>
    /// </summary>
    public IUnitService Unit { get; }
}
