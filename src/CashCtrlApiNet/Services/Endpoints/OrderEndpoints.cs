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
/// Api endpoint definitions for order
/// </summary>
internal static class OrderEndpoints
{
    /// <summary>
    /// Root path for order
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/order";

    /// <summary>
    /// Api endpoint definitions for order
    /// </summary>
    public static class Order
    {
        /// <summary>
        /// Root path for order
        /// </summary>
        private const string Root = GroupRoot;

        /// <summary>
        /// Endpoint to read an order
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list orders
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an order
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an order
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete orders
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to read status of an order
        /// </summary>
        public const string ReadStatus = $"{Root}/status_info.json";

        /// <summary>
        /// Endpoint to update status of an order
        /// </summary>
        public const string UpdateStatus = $"{Root}/update_status.json";

        /// <summary>
        /// Endpoint to update recurrence of an order
        /// </summary>
        public const string UpdateRecurrence = $"{Root}/update_recurrence.json";

        /// <summary>
        /// Endpoint to continue an order
        /// </summary>
        public const string Continue = $"{Root}/continue.json";

        /// <summary>
        /// Endpoint to read dossier of orders
        /// </summary>
        public const string ReadDossier = $"{Root}/dossier.json";

        /// <summary>
        /// Endpoint to add orders to a dossier
        /// </summary>
        public const string DossierAdd = $"{Root}/dossier_add.json";

        /// <summary>
        /// Endpoint to remove orders to a dossier
        /// </summary>
        public const string DossierRemove = $"{Root}/dossier_remove.json";

        /// <summary>
        /// Endpoint to update attachments of a dossier
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";
    }

    /// <summary>
    /// Api endpoint definitions for order book entry
    /// </summary>
    public static class BookEntry
    {
        /// <summary>
        /// Root path for order book entry
        /// </summary>
        private const string Root = $"{GroupRoot}/bookentry";

        /// <summary>
        /// Endpoint to read an order book entry
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list order book entries
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an order book entry
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an order book entry
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete order book entries
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for order category
    /// </summary>
    public static class Category
    {
        /// <summary>
        /// Root path for order category
        /// </summary>
        private const string Root = $"{GroupRoot}/category";

        /// <summary>
        /// Endpoint to read an order category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list order categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an order category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an order category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete order categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to reorder order categories
        /// </summary>
        public const string Reorder = $"{Root}/reorder.json";

        /// <summary>
        /// Endpoint to get status of an order category
        /// </summary>
        public const string ReadStatus = $"{Root}/read_status.json";
    }

    /// <summary>
    /// Api endpoint definitions for order document
    /// </summary>
    public static class Document
    {
        /// <summary>
        /// Root path for order document
        /// </summary>
        private const string Root = $"{GroupRoot}/document";

        /// <summary>
        /// Endpoint to read an order document
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to send an order document per mail
        /// </summary>
        public const string SendMail = $"{Root}/mail.json";

        /// <summary>
        /// Endpoint to update an order document
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";
    }

    /// <summary>
    /// Api endpoint definitions for order document template
    /// </summary>
    public static class DocumentTemplate
    {
        /// <summary>
        /// Root path for order document
        /// </summary>
        private const string Root = $"{GroupRoot}/template";

        /// <summary>
        /// Endpoint to read an order document
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list order documents
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an order document
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an order document
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete order documents
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }
}
