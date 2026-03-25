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
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Tests verifying the GetBinaryAsync methods exist on the connection handler interface and implementation
/// </summary>
public class ConnectionHandlerBinaryTests
{
    [Fact]
    public void Interface_ShouldDeclare_GetBinaryAsync_WithoutQueryParams()
    {
        var method = typeof(ICashCtrlConnectionHandler).GetMethod(
            nameof(ICashCtrlConnectionHandler.GetBinaryAsync),
            [typeof(string), typeof(CancellationToken)]);

        method.ShouldNotBeNull();
        method.ReturnType.ShouldBe(typeof(Task<ApiResult<BinaryResponse>>));
    }

    [Fact]
    public void Interface_ShouldDeclare_GetBinaryAsync_WithQueryParams()
    {
        var methods = typeof(ICashCtrlConnectionHandler).GetMethods()
            .Where(m => m.Name == nameof(ICashCtrlConnectionHandler.GetBinaryAsync) && m.IsGenericMethod)
            .ToArray();

        methods.Length.ShouldBe(1);
        methods[0].GetGenericArguments().Length.ShouldBe(1);
        methods[0].ReturnType.GetGenericTypeDefinition().ShouldBe(typeof(Task<>));
    }

    [Fact]
    public void Implementation_ShouldDeclare_GetBinaryAsync_WithoutQueryParams()
    {
        var method = typeof(CashCtrlConnectionHandler).GetMethod(
            nameof(ICashCtrlConnectionHandler.GetBinaryAsync),
            [typeof(string), typeof(CancellationToken)]);

        method.ShouldNotBeNull();
        method.ReturnType.ShouldBe(typeof(Task<ApiResult<BinaryResponse>>));
    }

    [Fact]
    public void Implementation_ShouldDeclare_GetBinaryAsync_WithQueryParams()
    {
        var methods = typeof(CashCtrlConnectionHandler).GetMethods()
            .Where(m => m.Name == nameof(ICashCtrlConnectionHandler.GetBinaryAsync) && m.IsGenericMethod)
            .ToArray();

        methods.Length.ShouldBe(1);
        methods[0].GetGenericArguments().Length.ShouldBe(1);
    }

    [Fact]
    public void BinaryResponse_ShouldHave_RequiredDataProperty()
    {
        var dataProperty = typeof(BinaryResponse).GetProperty(nameof(BinaryResponse.Data));

        dataProperty.ShouldNotBeNull();
        dataProperty.PropertyType.ShouldBe(typeof(byte[]));
        dataProperty.GetCustomAttribute<System.Runtime.CompilerServices.RequiredMemberAttribute>().ShouldNotBeNull();
    }

    [Fact]
    public void BinaryResponse_ShouldHave_ContentTypeProperty()
    {
        var contentTypeProperty = typeof(BinaryResponse).GetProperty(nameof(BinaryResponse.ContentType));

        contentTypeProperty.ShouldNotBeNull();
        contentTypeProperty.PropertyType.ShouldBe(typeof(string));
    }

    [Fact]
    public void BinaryResponse_ShouldHave_FileNameProperty()
    {
        var fileNameProperty = typeof(BinaryResponse).GetProperty(nameof(BinaryResponse.FileName));

        fileNameProperty.ShouldNotBeNull();
        fileNameProperty.PropertyType.ShouldBe(typeof(string));
    }

    [Fact]
    public void BinaryResponse_ShouldInheritFrom_ApiResponse()
    {
        typeof(BinaryResponse).BaseType.ShouldBe(typeof(Abstractions.Models.Api.Base.ApiResponse));
    }
}
