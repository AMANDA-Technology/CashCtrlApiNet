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
/// Api endpoint definitions for account
/// </summary>
internal static class AccountEndpoints
{
    /// <summary>
    /// Root path for account
    /// </summary>
    private const string GroupRoot = $"{Api.V1}/account";

    /// <summary>
    /// Api endpoint definitions for account
    /// </summary>
    public static class Account
    {
        /// <summary>
        /// Root path for account
        /// </summary>
        private const string Root = GroupRoot;

        /// <summary>
        /// Endpoint to read an account
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list accounts
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get balance of an account
        /// </summary>
        public const string Balance = $"{Root}/balance";

        /// <summary>
        /// Endpoint to create an account
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an account
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete accounts
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to categorize accounts
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to update attachments of an account
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";
    }

    /// <summary>
    /// Api endpoint definitions for account categories
    /// </summary>
    public static class AccountCategory
    {
        /// <summary>
        /// Root path for account category
        /// </summary>
        private const string Root = $"{GroupRoot}/category";

        /// <summary>
        /// Endpoint to read an account category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list account categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of account categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create an account category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an account category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete account categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for account cost center
    /// </summary>
    public static class CostCenter
    {
        /// <summary>
        /// Root path for account cost center
        /// </summary>
        private const string Root = $"{GroupRoot}/costcenter";

        /// <summary>
        /// Endpoint to read an account cost center
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list account cost centers
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get balance of an account cost center
        /// </summary>
        public const string Balance = $"{Root}/balance";

        /// <summary>
        /// Endpoint to create an account cost center
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an account cost center
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete account cost centers
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to categorize account cost centers
        /// </summary>
        public const string Categorize = $"{Root}/categorize.json";

        /// <summary>
        /// Endpoint to update attachments of an account cost center
        /// </summary>
        public const string UpdateAttachments = $"{Root}/update_attachments.json";
    }

    /// <summary>
    /// Api endpoint definitions for account cost center categories
    /// </summary>
    public static class CostCenterCategory
    {
        /// <summary>
        /// Root path for account cost center category
        /// </summary>
        private const string Root = $"{GroupRoot}/costcenter/category";

        /// <summary>
        /// Endpoint to read an account cost center category
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list account cost center categories
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to get tree of account cost center categories
        /// </summary>
        public const string Tree = $"{Root}/tree.json";

        /// <summary>
        /// Endpoint to create an account cost center category
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update an account cost center category
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete account cost center categories
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }
}
