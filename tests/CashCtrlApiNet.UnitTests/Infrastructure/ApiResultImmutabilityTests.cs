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

using System.Reflection;
using System.Runtime.CompilerServices;
using CashCtrlApiNet.Abstractions.Models.Api;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Tests for <see cref="ApiResult"/> immutability — all properties should use init-only setters
/// </summary>
public class ApiResultImmutabilityTests
{
    /// <summary>
    /// Verifies that RequestsLeft uses an init-only setter, not a mutable set accessor
    /// </summary>
    [Test]
    public void RequestsLeft_ShouldHaveInitOnlySetter()
    {
        var property = typeof(ApiResult).GetProperty(nameof(ApiResult.RequestsLeft));
        property.ShouldNotBeNull();

        var setter = property.SetMethod;
        setter.ShouldNotBeNull();

        // Init-only setters have the IsExternalInit modreq on their return parameter
        var modifiers = setter.ReturnParameter.GetRequiredCustomModifiers();
        modifiers.ShouldContain(typeof(IsExternalInit),
            "RequestsLeft should use { get; init; } not { get; set; }");
    }

    /// <summary>
    /// Verifies that all ApiResult properties use init-only setters for immutability
    /// </summary>
    [Test]
    public void AllProperties_ShouldHaveInitOnlySetters()
    {
        var properties = typeof(ApiResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var setter = property.SetMethod;
            if (setter is null) continue;

            var modifiers = setter.ReturnParameter.GetRequiredCustomModifiers();
            modifiers.ShouldContain(typeof(IsExternalInit),
                $"{property.Name} should use {{ get; init; }} not {{ get; set; }}");
        }
    }
}
