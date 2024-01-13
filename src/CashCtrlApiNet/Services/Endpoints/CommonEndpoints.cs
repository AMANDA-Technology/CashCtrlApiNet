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
/// Api endpoint definitions for common
/// </summary>
internal static class CommonEndpoints
{
    /// <summary>
    /// Api endpoint definitions for currency
    /// </summary>
    public static class Currency
    {
        /// <summary>
        /// Root path for currency
        /// </summary>
        private const string Root = $"{Api.V1}/currency";

        /// <summary>
        /// Endpoint to read a currency
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list currencies
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a currency
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a currency
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete currencies
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to get exchange rate between currencies
        /// </summary>
        public const string ExchangeRate = $"{Root}/exchangerate";
    }

    /// <summary>
    /// Api endpoint definitions for custom field
    /// </summary>
    public static class CustomField
    {
        /// <summary>
        /// Root path for custom field
        /// </summary>
        private const string Root = $"{Api.V1}/customfield";

        /// <summary>
        /// Endpoint to read a custom field
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list custom fields
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a custom field
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a custom field
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete custom fields
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to reorder custom fields
        /// </summary>
        public const string Reorder = $"{Root}/reorder.json";

        /// <summary>
        /// Endpoint to get custom field types
        /// </summary>
        public const string Types = $"{Root}/types.json";
    }

    /// <summary>
    /// Api endpoint definitions for custom field group
    /// </summary>
    public static class CustomFieldGroup
    {
        /// <summary>
        /// Root path for custom field group
        /// </summary>
        private const string Root = $"{Api.V1}/customfield/group";

        /// <summary>
        /// Endpoint to read a custom field group
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list custom field groups
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a custom field group
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a custom field group
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete custom field groups
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to reorder custom field groups
        /// </summary>
        public const string Reorder = $"{Root}/reorder.json";
    }

    /// <summary>
    /// Api endpoint definitions for rounding
    /// </summary>
    public static class Rounding
    {
        /// <summary>
        /// Root path for rounding
        /// </summary>
        private const string Root = $"{Api.V1}/rounding";

        /// <summary>
        /// Endpoint to read a rounding
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list roundings
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a rounding
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a rounding
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete roundings
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for sequence number
    /// </summary>
    public static class SequenceNumber
    {
        /// <summary>
        /// Root path for sequence number
        /// </summary>
        private const string Root = $"{Api.V1}/sequencenumber";

        /// <summary>
        /// Endpoint to read a sequence number
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list sequence numbers
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a sequence number
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a sequence number
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete sequence numbers
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for tax rate
    /// </summary>
    public static class TaxRate
    {
        /// <summary>
        /// Root path for tax rate
        /// </summary>
        private const string Root = $"{Api.V1}/tax";

        /// <summary>
        /// Endpoint to read a tax rate
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list tax rates
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a tax rate
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a tax rate
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete tax rates
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for text template
    /// </summary>
    public static class TextTemplate
    {
        /// <summary>
        /// Root path for text template
        /// </summary>
        private const string Root = $"{Api.V1}/text";

        /// <summary>
        /// Endpoint to read a text template
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list text templates
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a text template
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a text template
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete text templates
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }
}
