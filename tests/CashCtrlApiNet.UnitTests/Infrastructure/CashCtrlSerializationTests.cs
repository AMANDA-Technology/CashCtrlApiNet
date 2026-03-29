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

using System.Diagnostics.CodeAnalysis;
using CashCtrlApiNet.Abstractions.Helpers;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Regression tests for <see cref="CashCtrlSerialization"/>
/// </summary>
public class CashCtrlSerializationTests
{
    /// <summary>
    /// Test record with a DateTime property for serialization verification
    /// </summary>
    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
    private record TestModel(DateTime CreatedAt, string? Name);

    [Test]
    public void Serialize_ShouldUseCashCtrlDateTimeFormat()
    {
        // Arrange
        var date = new DateTime(2024, 1, 7, 20, 21, 38, 100);
        var model = new TestModel(date, "Test");

        // Act
        var result = CashCtrlSerialization.Serialize(model);

        // Assert — must use CashCtrl format (yyyy-MM-dd HH:mm:ss.f), not ISO 8601
        result.ShouldContain("2024-01-07 20:21:38.1");
        result.ShouldNotContain("2024-01-07T");
    }

    [Test]
    public void Serialize_ShouldOmitNullProperties()
    {
        // Arrange
        var model = new TestModel(DateTime.Now, null);

        // Act
        var result = CashCtrlSerialization.Serialize(model);

        // Assert — WhenWritingNull should exclude null fields
        result.ShouldNotContain("Name");
    }
}
