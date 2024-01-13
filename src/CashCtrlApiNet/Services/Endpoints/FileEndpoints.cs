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
/// Api endpoint definitions for file
/// </summary>
internal static class FileEndpoints
{
    /// <summary>
    /// Root path for file
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/file";

    /// <summary>
    /// Api endpoint definitions for file
    /// </summary>
    public static class File
    {
        /// <summary>
        /// Root path for file
        /// </summary>
        private const string Root = GroupRoot;

        /// <summary>
        /// Endpoint to get content of a file
        /// </summary>
        public const string GetContent = $"{Root}/get";

        /// <summary>
        /// Endpoint to read a file
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list files
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to prepare files
        /// </summary>
        public const string Prepare = $"{Root}/prepare.json";

        /// <summary>
        /// Endpoint to persist files
        /// </summary>
        public const string Persist = $"{Root}/persist.json";

        /// <summary>
        /// Endpoint to create a file
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a file
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete files
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to categorize files
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to delete all archived files
        /// </summary>
        public const string EmptyArchive = $"{Root}/empty_archive.json";

        /// <summary>
        /// Endpoint to restore files
        /// </summary>
        public const string Restore = $"{Root}/restore.json";
    }

    /// <summary>
    /// Api endpoint definitions for file categories
    /// </summary>
    public static class ArticleCategory
    {
        /// <summary>
        /// Root path for file category
        /// </summary>
        private const string Root = $"{GroupRoot}/category";

        /// <summary>
        /// Endpoint to read an file category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list file categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of file categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create an file category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a file category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete file categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }
}
