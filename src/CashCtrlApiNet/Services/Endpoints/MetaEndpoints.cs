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
/// Api endpoint definitions for meta
/// </summary>
internal static class MetaEndpoints
{
    /// <summary>
    /// Api endpoint definitions for fiscal period
    /// </summary>
    public static class FiscalPeriod
    {
        /// <summary>
        /// Root path for fiscal period
        /// </summary>
        private const string Root = $"{Api.V1}/fiscalperiod";

        /// <summary>
        /// Endpoint to read a fiscal period
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list fiscal periods
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a fiscal period
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a fiscal period
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to switch a fiscal period
        /// </summary>
        public const string Switch = $"{Root}/switch.json";

        /// <summary>
        /// Endpoint to delete a fiscal periods
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";

        /// <summary>
        /// Endpoint to get result of a fiscal period
        /// </summary>
        public const string Result = $"{Root}/result";

        /// <summary>
        /// Endpoint to list deprecations of a fiscal period
        /// </summary>
        public const string Deprecations = $"{Root}/depreciations.json";

        /// <summary>
        /// Endpoint to book deprecations of a fiscal period
        /// </summary>
        public const string BookDeprecations = $"{Root}/bookdepreciations.json";

        /// <summary>
        /// Endpoint to list exchange differences of a fiscal period
        /// </summary>
        public const string ExchangeDifferences = $"{Root}/exchangediff.json";

        /// <summary>
        /// Endpoint to book exchange differences of a fiscal period
        /// </summary>
        public const string BookExchangeDifferences = $"{Root}/bookexchangediff.json";

        /// <summary>
        /// Endpoint to complete a fiscal period
        /// </summary>
        public const string Complete = $"{Root}/complete.json";

        /// <summary>
        /// Endpoint to reopen a fiscal period
        /// </summary>
        public const string Reopen = $"{Root}/reopen.json";
    }

    /// <summary>
    /// Api endpoint definitions for fiscal period task
    /// </summary>
    public static class FiscalPeriodTask
    {
        /// <summary>
        /// Root path for fiscal period task
        /// </summary>
        private const string Root = $"{Api.V1}/fiscalperiod/task";

        /// <summary>
        /// Endpoint to list fiscal period tasks
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a fiscal period task
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to delete a fiscal period tasks
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for location
    /// </summary>
    public static class Location
    {
        /// <summary>
        /// Root path for location
        /// </summary>
        private const string Root = $"{Api.V1}/location";

        /// <summary>
        /// Endpoint to read a location
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to list locations
        /// </summary>
        public const string List = $"{Root}/{Default.List}";

        /// <summary>
        /// Endpoint to create a location
        /// </summary>
        public const string Create = $"{Root}/{Default.Create}";

        /// <summary>
        /// Endpoint to update a location
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";

        /// <summary>
        /// Endpoint to delete locations
        /// </summary>
        public const string Delete = $"{Root}/{Default.Delete}";
    }

    /// <summary>
    /// Api endpoint definitions for organisation
    /// </summary>
    public static class Organisation
    {
        /// <summary>
        /// Endpoint to get organisation log
        /// </summary>
        public const string Logo = $"{Api.V1}/domain/current/logo";
    }

    /// <summary>
    /// Api endpoint definitions for settings
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Root path for settings
        /// </summary>
        private const string Root = $"{Api.V1}/setting";

        /// <summary>
        /// Endpoint to read settings
        /// </summary>
        public const string Read = $"{Root}/{Default.Read}";

        /// <summary>
        /// Endpoint to get a value from settings
        /// </summary>
        public const string Get = $"{Root}/get";

        /// <summary>
        /// Endpoint to update settings
        /// </summary>
        public const string Update = $"{Root}/{Default.Update}";
    }
}
