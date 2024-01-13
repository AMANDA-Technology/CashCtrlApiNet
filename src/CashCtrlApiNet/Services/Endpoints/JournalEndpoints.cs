/*
MIT License

Copyright (c) 2022 Philip Näf <philip.naef@amanda-technology.ch>
Copyright (c) 2022 Manuel Gysin <manuel.gysin@amanda-technology.ch>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation journals (the "Software"), to deal
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
/// Api endpoint definitions for journal
/// </summary>
internal static class JournalEndpoints
{
    /// <summary>
    /// Root path for journal
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/journal";

    /// <summary>
    /// Api endpoint definitions for journal
    /// </summary>
    public static class Journal
    {
        /// <summary>
        /// Root path for journal entry
        /// </summary>
        private const string Root = GroupRoot;

        /// <summary>
        /// Endpoint to read a journal entry
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list journal entries
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a journal entry
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a journal entry
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete journal entries
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to update attachments of a journal entry
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";

        /// <summary>
        /// Endpoint to update recurrence of a journal entry
        /// </summary>
        public const string UpdateRecurrence = $"{Root}/update_recurrence.json";
    }

    /// <summary>
    /// Api endpoint definitions for journal import
    /// </summary>
    public static class Import
    {
        /// <summary>
        /// Root path for journal import
        /// </summary>
        private const string Root = $"{GroupRoot}/import";

        /// <summary>
        /// Endpoint to read a journal import
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list journal imports
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create an journal import
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to execute an journal import
        /// </summary>
        public const string Execute = $"{Root}/execute.json";
    }

    /// <summary>
    /// Api endpoint definitions for journal import entry
    /// </summary>
    public static class ImportEntry
    {
        /// <summary>
        /// Root path for journal import
        /// </summary>
        private const string Root = $"{GroupRoot}/import/entry";

        /// <summary>
        /// Endpoint to read a journal import entry
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list journal import entries
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to update a journal import entry
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to ignore journal import entries
        /// </summary>
        public const string Ignore = $"{Root}/delete.json";

        /// <summary>
        /// Endpoint to restore journal import entries
        /// </summary>
        public const string Restore = $"{Root}/restore.json";

        /// <summary>
        /// Endpoint to confirm journal import entries
        /// </summary>
        public const string Confirm = $"{Root}/confirm.json";

        /// <summary>
        /// Endpoint to unconfirm journal import entries
        /// </summary>
        public const string Unconfirm = $"{Root}/unconfirm.json";
    }
}
