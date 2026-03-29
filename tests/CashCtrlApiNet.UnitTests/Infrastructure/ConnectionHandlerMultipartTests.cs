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

using CashCtrlApiNet.Abstractions.Models.Api.Base;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Services;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Tests verifying the PostMultipartAsync method exists on the connection handler interface and implementation
/// </summary>
public class ConnectionHandlerMultipartTests
{
    [Test]
    public void Interface_ShouldDeclare_PostMultipartAsync()
    {
        var methods = typeof(ICashCtrlConnectionHandler).GetMethods()
            .Where(m => m.Name == nameof(ICashCtrlConnectionHandler.PostMultipartAsync))
            .ToArray();

        methods.Length.ShouldBe(1);
        methods[0].IsGenericMethod.ShouldBeTrue();
        methods[0].GetGenericArguments().Length.ShouldBe(1);
    }

    [Test]
    public void Interface_PostMultipartAsync_ShouldAccept_MultipartFormDataContent()
    {
        var method = typeof(ICashCtrlConnectionHandler).GetMethods()
            .Single(m => m.Name == nameof(ICashCtrlConnectionHandler.PostMultipartAsync));

        var parameters = method.GetParameters();
        parameters.Length.ShouldBe(3);
        parameters[0].ParameterType.ShouldBe(typeof(string));
        parameters[1].ParameterType.ShouldBe(typeof(MultipartFormDataContent));
        parameters[2].ParameterType.ShouldBe(typeof(CancellationToken));
    }

    [Test]
    public void Interface_PostMultipartAsync_ShouldReturn_ApiResultOfTResult()
    {
        var method = typeof(ICashCtrlConnectionHandler).GetMethods()
            .Single(m => m.Name == nameof(ICashCtrlConnectionHandler.PostMultipartAsync));

        var returnType = method.ReturnType;
        returnType.GetGenericTypeDefinition().ShouldBe(typeof(Task<>));
    }

    [Test]
    public void Interface_PostMultipartAsync_GenericConstraint_ShouldRequire_ApiResponse()
    {
        var method = typeof(ICashCtrlConnectionHandler).GetMethods()
            .Single(m => m.Name == nameof(ICashCtrlConnectionHandler.PostMultipartAsync));

        var genericArg = method.GetGenericArguments()[0];
        genericArg.GetGenericParameterConstraints().ShouldContain(typeof(ApiResponse));
    }

    [Test]
    public void Implementation_ShouldDeclare_PostMultipartAsync()
    {
        var methods = typeof(CashCtrlConnectionHandler).GetMethods()
            .Where(m => m.Name == nameof(ICashCtrlConnectionHandler.PostMultipartAsync))
            .ToArray();

        methods.Length.ShouldBe(1);
        methods[0].IsGenericMethod.ShouldBeTrue();
    }

    [Test]
    public void Implementation_PostMultipartAsync_ShouldAccept_MultipartFormDataContent()
    {
        var method = typeof(CashCtrlConnectionHandler).GetMethods()
            .Single(m => m.Name == nameof(ICashCtrlConnectionHandler.PostMultipartAsync));

        var parameters = method.GetParameters();
        parameters[1].ParameterType.ShouldBe(typeof(MultipartFormDataContent));
    }

    [Test]
    public void ServiceTestBase_ShouldBeAbstractGenericClass()
    {
        var type = typeof(ServiceTestBase<>);

        type.IsAbstract.ShouldBeTrue();
        type.IsGenericType.ShouldBeTrue();
        type.GetGenericArguments().Length.ShouldBe(1);
    }

    [Test]
    public void ServiceTestBase_ShouldHave_ConnectionHandlerField()
    {
        var field = typeof(ServiceTestBase<>).GetField(
            "ConnectionHandler",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

        field.ShouldNotBeNull();
        field.FieldType.ShouldBe(typeof(ICashCtrlConnectionHandler));
    }

    [Test]
    public void ServiceTestBase_ShouldHave_ServiceField()
    {
        var type = typeof(ServiceTestBase<>);
        var field = type.GetField(
            "Service",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

        field.ShouldNotBeNull();
    }
}
