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
    /// Create a list response JSON with a custom total count (for pagination test scenarios)
    /// </summary>
    /// <typeparam name="T">The model type</typeparam>
    /// <param name="items">The items to include in the current page</param>
    /// <param name="total">The total number of items across all pages</param>
    /// <returns>JSON string matching CashCtrl ListResponse format with specified total</returns>
    public static string ListResponse<T>(T[] items, int total) where T : class
        => JsonSerializer.Serialize(new { total, data = items });

    /// <summary>
    /// Create a success response JSON (for create/update/delete endpoints)
    /// </summary>
    /// <param name="message">Success message</param>
    /// <param name="insertId">Optional insert ID for create operations</param>
    /// <returns>JSON string matching CashCtrl NoContentResponse format</returns>
    public static string SuccessResponse(string message = "Saved", int? insertId = null)
        => JsonSerializer.Serialize(new { success = true, message, insertId });

    /// <summary>
    /// Create a balance response body (raw decimal value as returned by CashCtrl balance endpoints)
    /// </summary>
    /// <param name="balance">The decimal balance value</param>
    /// <returns>String representation of the balance value</returns>
    public static string DecimalResponse(decimal balance)
        => balance.ToString(System.Globalization.CultureInfo.InvariantCulture);

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

    /// <summary>
    /// Create a multi-error response JSON (for write endpoints with multiple validation errors)
    /// </summary>
    /// <param name="errors">Tuples of (Field, Message) for each validation error</param>
    /// <returns>JSON string matching CashCtrl NoContentResponse error format</returns>
    public static string ErrorResponse(params (string Field, string Message)[] errors)
        => JsonSerializer.Serialize(new
        {
            success = false,
            errors = errors.Select(e => new { field = e.Field, message = e.Message }).ToArray()
        });

    /// <summary>
    /// Create a single error response JSON (for read endpoints returning SingleResponse format)
    /// </summary>
    /// <param name="errorMessage">The error message</param>
    /// <returns>JSON string matching CashCtrl SingleResponse error format</returns>
    public static string SingleErrorResponse(string errorMessage)
        => JsonSerializer.Serialize(new
        {
            success = false,
            errorMessage,
            data = (object?)null
        });
}
