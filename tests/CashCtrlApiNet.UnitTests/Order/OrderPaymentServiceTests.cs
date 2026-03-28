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

using CashCtrlApiNet.Abstractions.Models.Order.Payment;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Order;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Order;

/// <summary>
/// Unit tests for <see cref="OrderPaymentService"/>
/// </summary>
public class OrderPaymentServiceTests : ServiceTestBase<OrderPaymentService>
{
    /// <inheritdoc />
    protected override OrderPaymentService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var payment = new OrderPaymentCreate { OrderId = 42 };
        ConnectionHandler
            .PostAsync<NoContentResponse, OrderPaymentCreate>(Arg.Any<string>(), Arg.Any<OrderPaymentCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(payment);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, OrderPaymentCreate>(
                OrderEndpoints.Payment.Create, payment, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Download_ShouldCallGetBinaryAsync()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetBinaryAsync<Entry>(Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<BinaryResponse> { ResponseData = new BinaryResponse { Data = [1, 2, 3] } });

        var result = await Service.Download(entry);

        await ConnectionHandler.Received(1)
            .GetBinaryAsync<Entry>(OrderEndpoints.Payment.Download, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }
}
