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

using CashCtrlApiNet.Interfaces;
using NSubstitute;

namespace CashCtrlApiNet.UnitTests;

/// <summary>
/// Base class for unit tests of connector services using a mocked <see cref="ICashCtrlConnectionHandler"/>
/// </summary>
/// <typeparam name="TService">The service type under test</typeparam>
public abstract class ServiceTestBase<TService> where TService : class
{
    /// <summary>
    /// Mocked connection handler for isolating service logic from HTTP calls
    /// </summary>
    protected readonly ICashCtrlConnectionHandler ConnectionHandler;

    /// <summary>
    /// The service instance under test
    /// </summary>
    protected readonly TService Service;

    /// <summary>
    /// Initializes the mocked connection handler and creates the service under test
    /// </summary>
    protected ServiceTestBase()
    {
        ConnectionHandler = Substitute.For<ICashCtrlConnectionHandler>();
        Service = CreateService();
    }

    /// <summary>
    /// Creates an instance of the service under test using the mocked connection handler
    /// </summary>
    /// <returns>A new instance of <typeparamref name="TService"/></returns>
    protected abstract TService CreateService();
}
