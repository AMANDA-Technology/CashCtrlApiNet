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

namespace CashCtrlApiNet.Services.Endpoints.Base;

/// <summary>
/// Defaults for endpoints
/// </summary>
internal static class Default
{
    /// <summary>
    /// Endpoints for read an entry
    /// </summary>
    public const string Read = "read.json";

    /// <summary>
    /// Endpoints for list entries
    /// </summary>
    public const string List = "list.json";

    /// <summary>
    /// Endpoints for create an entry
    /// </summary>
    public const string Create = "create.json";

    /// <summary>
    /// Endpoints for update an entry
    /// </summary>
    public const string Update = "update.json";

    /// <summary>
    /// Endpoints for delete entries
    /// </summary>
    public const string Delete = "delete.json";
}
