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
/// Api endpoint definitions for person
/// </summary>
internal static class PersonEndpoints
{
    /// <summary>
    /// Root path for person
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/person";

    /// <summary>
    /// Api endpoint definitions for person
    /// </summary>
    public static class Order
    {
        /// <summary>
        /// Root path for person
        /// </summary>
        private const string Root = GroupRoot;

        /// <summary>
        /// Endpoint to read a person
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list persons
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a person
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a person
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete persons
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to categorize persons
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to update attachments of a person
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";
    }

    /// <summary>
    /// Api endpoint definitions for person category
    /// </summary>
    public static class Category
    {
        /// <summary>
        /// Root path for person category
        /// </summary>
        private const string Root = $"{GroupRoot}/category";

        /// <summary>
        /// Endpoint to read a person category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list person categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of person categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create a person category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a person category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete person categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for person import
    /// </summary>
    public static class Import
    {
        /// <summary>
        /// Root path for person import
        /// </summary>
        private const string Root = $"{GroupRoot}/import";

        /// <summary>
        /// Endpoint to create a person import
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to define a mapping for a person import
        /// </summary>
        public const string Mapping = $"{Root}/mapping.json";

        /// <summary>
        /// Endpoint to list all available mapping fields for a person import
        /// </summary>
        public const string AvailableMappingFields = $"{Root}/mapping_combo.json";

        /// <summary>
        /// Endpoint to get preview of a person import
        /// </summary>
        public const string Preview = $"{Root}/preview.json";

        /// <summary>
        /// Endpoint to execute a person import
        /// </summary>
        public const string Execute = $"{Root}/execute.json";
    }

    /// <summary>
    /// Api endpoint definitions for person title
    /// </summary>
    public static class Title
    {
        /// <summary>
        /// Root path for person title
        /// </summary>
        private const string Root = $"{GroupRoot}/title";

        /// <summary>
        /// Endpoint to read a person title
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list person titles
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a person title
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a person title
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete person titles
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }
}
