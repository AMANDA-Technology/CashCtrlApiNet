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

using System.Text.Json;

namespace CashCtrlApiNet.IntegrationTests.Helpers;

/// <summary>
/// Factory for building CashCtrl API JSON response payloads
/// </summary>
public static class CashCtrlResponseFactory
{
    /// <summary>
    /// Create a single-item response JSON (for read endpoints)
    /// </summary>
    /// <typeparam name="T">The model type</typeparam>
    /// <param name="data">The data object to serialize</param>
    /// <returns>JSON string matching CashCtrl SingleResponse format</returns>
    public static string SingleResponse<T>(T data) where T : class
        => JsonSerializer.Serialize(new { success = true, data });

    /// <summary>
    /// Create a list response JSON (for list endpoints)
    /// </summary>
    /// <typeparam name="T">The model type</typeparam>
    /// <param name="items">The items to include in the list</param>
    /// <returns>JSON string matching CashCtrl ListResponse format</returns>
    public static string ListResponse<T>(params T[] items) where T : class
        => JsonSerializer.Serialize(new { total = items.Length, data = items });

    /// <summary>
    /// Create a success response JSON (for create/update/delete endpoints)
    /// </summary>
    /// <param name="message">Success message</param>
    /// <param name="insertId">Optional insert ID for create operations</param>
    /// <returns>JSON string matching CashCtrl NoContentResponse format</returns>
    public static string SuccessResponse(string message = "Saved", int? insertId = null)
        => JsonSerializer.Serialize(new { success = true, message, insertId });

    /// <summary>
    /// Create an error response JSON
    /// </summary>
    /// <param name="field">The field with the error</param>
    /// <param name="message">The error message</param>
    /// <returns>JSON string matching CashCtrl error response format</returns>
    public static string ErrorResponse(string field, string message)
        => JsonSerializer.Serialize(new
        {
            success = false,
            errors = new[] { new { field, message } }
        });
}
