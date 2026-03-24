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
/// Api endpoint definitions for salary
/// </summary>
internal static class SalaryEndpoints
{
    /// <summary>
    /// Root path for salary
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/salary";

    /// <summary>
    /// Api endpoint definitions for salary book entry
    /// </summary>
    public static class BookEntry
    {
        /// <summary>
        /// Root path for salary book entry
        /// </summary>
        private const string Root = $"{GroupRoot}/bookentry";

        /// <summary>
        /// Endpoint to read a salary book entry
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary book entries
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary book entry
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary book entry
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary book entries
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary category
    /// </summary>
    public static class Category
    {
        /// <summary>
        /// Root path for salary category
        /// </summary>
        private const string Root = $"{GroupRoot}/category";

        /// <summary>
        /// Endpoint to read a salary category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of salary categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create a salary category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary certificate
    /// </summary>
    public static class Certificate
    {
        /// <summary>
        /// Root path for salary certificate
        /// </summary>
        private const string Root = $"{GroupRoot}/certificate";

        /// <summary>
        /// Endpoint to read a salary certificate
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary certificates
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to update a salary certificate
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to export salary certificates as Excel
        /// </summary>
        public const string ListXlsx = $"{Root}/list.xlsx";

        /// <summary>
        /// Endpoint to export salary certificates as CSV
        /// </summary>
        public const string ListCsv = $"{Root}/list.csv";

        /// <summary>
        /// Endpoint to export salary certificates as PDF
        /// </summary>
        public const string ListPdf = $"{Root}/list.pdf";
    }

    /// <summary>
    /// Api endpoint definitions for salary certificate document
    /// </summary>
    public static class CertificateDocument
    {
        /// <summary>
        /// Root path for salary certificate document
        /// </summary>
        private const string Root = $"{GroupRoot}/certificate/document";

        /// <summary>
        /// Endpoint to read a salary certificate document
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to read a salary certificate document as PDF
        /// </summary>
        public const string ReadPdf = $"{Root}/read.pdf";

        /// <summary>
        /// Endpoint to read salary certificate documents as ZIP
        /// </summary>
        public const string ReadZip = $"{Root}/read.zip";

        /// <summary>
        /// Endpoint to mail a salary certificate document
        /// </summary>
        public const string Mail = $"{Root}/mail.json";
    }

    /// <summary>
    /// Api endpoint definitions for salary certificate template
    /// </summary>
    public static class CertificateTemplate
    {
        /// <summary>
        /// Root path for salary certificate template
        /// </summary>
        private const string Root = $"{GroupRoot}/certificate/template";

        /// <summary>
        /// Endpoint to read a salary certificate template
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary certificate templates
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of salary certificate templates
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create a salary certificate template
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary certificate template
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary certificate templates
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary document
    /// </summary>
    public static class Document
    {
        /// <summary>
        /// Root path for salary document
        /// </summary>
        private const string Root = $"{GroupRoot}/document";

        /// <summary>
        /// Endpoint to read a salary document
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to read a salary document as PDF
        /// </summary>
        public const string ReadPdf = $"{Root}/read.pdf";

        /// <summary>
        /// Endpoint to read salary documents as ZIP
        /// </summary>
        public const string ReadZip = $"{Root}/read.zip";

        /// <summary>
        /// Endpoint to mail a salary document
        /// </summary>
        public const string Mail = $"{Root}/mail.json";

        /// <summary>
        /// Endpoint to update a salary document
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";
    }

    /// <summary>
    /// Api endpoint definitions for salary field
    /// </summary>
    public static class Field
    {
        /// <summary>
        /// Root path for salary field
        /// </summary>
        private const string Root = $"{GroupRoot}/field";

        /// <summary>
        /// Endpoint to read a salary field
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary fields
        /// </summary>
        public const string List = $"{Root}/{Default.List}";
    }

    /// <summary>
    /// Api endpoint definitions for salary insurance type
    /// </summary>
    public static class InsuranceType
    {
        /// <summary>
        /// Root path for salary insurance type
        /// </summary>
        private const string Root = $"{GroupRoot}/insurance/type";

        /// <summary>
        /// Endpoint to read a salary insurance type
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary insurance types
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary insurance type
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary insurance type
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary insurance types
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary layout
    /// </summary>
    public static class Layout
    {
        /// <summary>
        /// Root path for salary layout
        /// </summary>
        private const string Root = $"{GroupRoot}/layout";

        /// <summary>
        /// Endpoint to read a salary layout
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary layouts
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary layout
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary layout
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary layouts
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary payment
    /// </summary>
    public static class Payment
    {
        /// <summary>
        /// Root path for salary payment
        /// </summary>
        private const string Root = $"{GroupRoot}/payment";

        /// <summary>
        /// Endpoint to create a salary payment
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to download a salary payment
        /// </summary>
        public const string Download = $"{Root}/download";
    }

    /// <summary>
    /// Api endpoint definitions for salary setting
    /// </summary>
    public static class Setting
    {
        /// <summary>
        /// Root path for salary setting
        /// </summary>
        private const string Root = $"{GroupRoot}/setting";

        /// <summary>
        /// Endpoint to read a salary setting
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary settings
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary setting
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary setting
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary settings
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary statement
    /// </summary>
    public static class Statement
    {
        /// <summary>
        /// Root path for salary statement
        /// </summary>
        private const string Root = $"{GroupRoot}/statement";

        /// <summary>
        /// Endpoint to read a salary statement
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary statements
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary statement
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary statement
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to update multiple salary statements
        /// </summary>
        public const string UpdateMultiple = $"{Root}/update_multiple.json";

        /// <summary>
        /// Endpoint to update status of salary statements
        /// </summary>
        public const string UpdateStatus = $"{Root}/update_status.json";

        /// <summary>
        /// Endpoint to update recurrence of a salary statement
        /// </summary>
        public const string UpdateRecurrence = $"{Root}/update_recurrence.json";

        /// <summary>
        /// Endpoint to delete salary statements
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to calculate a salary statement
        /// </summary>
        public const string Calculate = $"{Root}/calculate.json";

        /// <summary>
        /// Endpoint to update attachments of a salary statement
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";

        /// <summary>
        /// Endpoint to export salary statements as Excel
        /// </summary>
        public const string ListXlsx = $"{Root}/list.xlsx";

        /// <summary>
        /// Endpoint to export salary statements as CSV
        /// </summary>
        public const string ListCsv = $"{Root}/list.csv";

        /// <summary>
        /// Endpoint to export salary statements as PDF
        /// </summary>
        public const string ListPdf = $"{Root}/list.pdf";
    }

    /// <summary>
    /// Api endpoint definitions for salary status
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// Root path for salary status
        /// </summary>
        private const string Root = $"{GroupRoot}/status";

        /// <summary>
        /// Endpoint to read a salary status
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary statuses
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary status
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary status
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary statuses
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to reorder salary statuses
        /// </summary>
        public const string Reorder = $"{Root}/reorder.json";
    }

    /// <summary>
    /// Api endpoint definitions for salary sum
    /// </summary>
    public static class Sum
    {
        /// <summary>
        /// Root path for salary sum
        /// </summary>
        private const string Root = $"{GroupRoot}/sum";

        /// <summary>
        /// Endpoint to read a salary sum
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary sums
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary sum
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary sum
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary sums
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary template
    /// </summary>
    public static class Template
    {
        /// <summary>
        /// Root path for salary template
        /// </summary>
        private const string Root = $"{GroupRoot}/template";

        /// <summary>
        /// Endpoint to read a salary template
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary templates
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of salary templates
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create a salary template
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary template
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete salary templates
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for salary type
    /// </summary>
    public static class Type
    {
        /// <summary>
        /// Root path for salary type
        /// </summary>
        private const string Root = $"{GroupRoot}/type";

        /// <summary>
        /// Endpoint to read a salary type
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list salary types
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a salary type
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a salary type
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to categorize salary types
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to delete salary types
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to export salary types as Excel
        /// </summary>
        public const string ListXlsx = $"{Root}/list.xlsx";

        /// <summary>
        /// Endpoint to export salary types as CSV
        /// </summary>
        public const string ListCsv = $"{Root}/list.csv";

        /// <summary>
        /// Endpoint to export salary types as PDF
        /// </summary>
        public const string ListPdf = $"{Root}/list.pdf";
    }
}
