﻿/*
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

using System.Collections.Immutable;
using System.Text.Json.Serialization;
using CashCtrlApiNet.Abstractions.Models.Api.Base;

namespace CashCtrlApiNet.Abstractions.Models.Api;

/// <summary>
/// Api response. <a href="https://app.cashctrl.com/static/help/en/api/index.html#errors">API Doc - Error handling</a>
/// </summary>
/// <param name="Success"></param>
/// <param name="Errors"></param>
/// <param name="Message"></param>
/// <param name="InsertId"></param>
public record NoContentResponse(
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("errors")] ImmutableArray<ResponseError>? Errors,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("insertId")] int? InsertId)
    : ApiResponse;
