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

using CashCtrlApiNet.IntegrationTests.Fakers;
using CashCtrlApiNet.IntegrationTests.Helpers;
using Shouldly;

namespace CashCtrlApiNet.IntegrationTests.Order;

/// <summary>
/// Integration tests for <see cref="CashCtrlApiNet.Services.Connectors.Order.OrderPaymentService"/>
/// </summary>
public class OrderPaymentServiceIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Verify Create sends correct request and returns success
    /// </summary>
    [Test]
    public async Task Create_ReturnsExpectedResult()
    {
        // Arrange
        var paymentRequest = OrderFakers.PaymentRequest.Generate();
        Server.StubPostJson("/api/v1/order/payment/create.json",
            CashCtrlResponseFactory.SuccessResponse("Payment created", insertId: 42));

        // Act
        var result = await Client.Order.Payment.Create(paymentRequest);

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Success.ShouldBeTrue();
        result.ResponseData.Message.ShouldBe("Payment created");
        result.ResponseData.InsertId.ShouldBe(42);
    }

    /// <summary>
    /// Verify Download returns binary data
    /// </summary>
    [Test]
    public async Task Download_ReturnsExpectedResult()
    {
        // Arrange
        var paymentBytes = "fake-payment-file-content"u8.ToArray();
        Server.StubGetBinary("/api/v1/order/payment/download", paymentBytes,
            "application/octet-stream", "payment.xml");

        // Act
        var result = await Client.Order.Payment.Download(new() { Date = "2026-01-15", OrderIds = [1] });

        // Assert
        result.IsHttpSuccess.ShouldBeTrue();
        result.ResponseData.ShouldNotBeNull();
        result.ResponseData.Data.ShouldBe(paymentBytes);
    }
}
