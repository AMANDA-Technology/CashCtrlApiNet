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

using CashCtrlApiNet.Services.Endpoints.Base;

namespace CashCtrlApiNet.Services.Endpoints;

/// <summary>
/// Api endpoint definitions for inventory
/// </summary>
internal static class InventoryEndpoints
{
    /// <summary>
    /// Root path for inventory
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/inventory";

    /// <summary>
    /// Api endpoint definitions for inventory article
    /// </summary>
    public static class Article
    {
        /// <summary>
        /// Root path for inventory article
        /// </summary>
        private const string Root = $"{GroupRoot}/article";

        /// <summary>
        /// Endpoint to read an inventory article
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list inventory articles
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an inventory article
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an inventory article
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete inventory articles
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to categorize inventory articles
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to update attachments of an inventory article
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";
    }

    /// <summary>
    /// Api endpoint definitions for inventory article categories
    /// </summary>
    public static class ArticleCategory
    {
        /// <summary>
        /// Root path for inventory article category
        /// </summary>
        private const string Root = $"{GroupRoot}/article/category";

        /// <summary>
        /// Endpoint to read an inventory article category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list inventory article categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of inventory article categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create an inventory article category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an inventory article category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete inventory article categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// pi endpoint definitions for inventory fixed asset
    /// </summary>
    public static class FixedAsset
    {
        /// <summary>
        /// Root path for inventory fixed asset
        /// </summary>
        private const string Root = $"{GroupRoot}/asset";

        /// <summary>
        /// Endpoint to read an inventory fixed asset
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list inventory fixed assets
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an inventory fixed asset
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an inventory fixed asset
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete inventory fixed assets
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to categorize inventory fixed assets
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to update attachments of an inventory fixed asset
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";
    }

    /// <summary>
    /// Api endpoint definitions for inventory fixed asset categories
    /// </summary>
    public static class FixedAssetCategory
    {
        /// <summary>
        /// Root path for inventory fixed asset category
        /// </summary>
        private const string Root = $"{GroupRoot}/asset/category";

        /// <summary>
        /// Endpoint to read an inventory fixed asset category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list inventory fixed asset categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of inventory fixed asset categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create an inventory fixed asset category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an inventory fixed asset category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete inventory fixed asset categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for inventory import
    /// </summary>
    public static class Import
    {
        /// <summary>
        /// Root path for inventory import
        /// </summary>
        private const string Root = $"{GroupRoot}/article/import";

        /// <summary>
        /// Endpoint to create an inventory import
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to define a mapping for an inventory import
        /// </summary>
        public const string Mapping = $"{Root}/mapping.json";

        /// <summary>
        /// Endpoint to list all available mapping fields for an inventory import
        /// </summary>
        public const string AvailableMappingFields = $"{Root}/mapping_combo.json";

        /// <summary>
        /// Endpoint to get preview of an inventory import
        /// </summary>
        public const string Preview = $"{Root}/preview.json";

        /// <summary>
        /// Endpoint to execute an inventory import
        /// </summary>
        public const string Execute = $"{Root}/execute.json";
    }

    /// <summary>
    /// Api endpoint definitions for inventory unit
    /// </summary>
    public static class Unit
    {
        /// <summary>
        /// Root path for inventory unit
        /// </summary>
        private const string Root = $"{GroupRoot}/unit";

        /// <summary>
        /// Endpoint to read an inventory unit
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list inventory units
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an inventory unit
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an inventory unit
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete inventory units
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }
}
