/*
MIT License

Copyright (c) 2022 Philip Näf <philip.naef@amanda-technology.ch>
Copyright (c) 2022 Manuel Gysin <manuel.gysin@amanda-technology.ch>

Permission is hereby granted, free of charge, to any report element obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit report elements to whom the Software is
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
/// Api endpoint definitions for report
/// </summary>
internal static class ReportEndpoints
{
    /// <summary>
    /// Root path for report
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/report";

    /// <summary>
    /// Api endpoint definitions for report
    /// </summary>
    public static class Report
    {
        /// <summary>
        /// Root path for report
        /// </summary>
        private const string Root = GroupRoot;

        /// <summary>
        /// Endpoint to get tree of a report
        /// </summary>
        public const string Tree = $"{Root}/tree.json";
    }

    /// <summary>
    /// Api endpoint definitions for report element
    /// </summary>
    public static class Element
    {
        /// <summary>
        /// Root path for report element
        /// </summary>
        private const string Root = $"{GroupRoot}/element";

        /// <summary>
        /// Endpoint to read a report element
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to create a report element
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a report element
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete report elements
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to reorder report elements
        /// </summary>
        public const string Reorder = $"{Root}/reorder.json";

        /// <summary>
        /// Endpoint to read json data of a report element
        /// </summary>
        public const string ReadJson = $"{Root}/data.json";

        /// <summary>
        /// Endpoint to read html data of a report element
        /// </summary>
        public const string ReadHtml = $"{Root}/data.html";

        /// <summary>
        /// Endpoint to read meta data of a report element
        /// </summary>
        public const string ReadMeta = $"{Root}/meta.json";
    }

    /// <summary>
    /// Api endpoint definitions for report set
    /// </summary>
    public static class Set
    {
        /// <summary>
        /// Root path for report set
        /// </summary>
        private const string Root = $"{GroupRoot}/set";

        /// <summary>
        /// Endpoint to read a report set
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to create a report set
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a report set
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete report sets
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to reorder report sets
        /// </summary>
        public const string Reorder = $"{Root}/reorder.json";

        /// <summary>
        /// Endpoint to read meta data of a report set
        /// </summary>
        public const string ReadMeta = $"{Root}/meta.json";
    }
}
