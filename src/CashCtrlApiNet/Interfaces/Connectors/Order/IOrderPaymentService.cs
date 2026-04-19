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

namespace CashCtrlApiNet.Interfaces.Connectors.Order;

/// <summary>
/// CashCtrl order payment service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/payment">API Doc - Order/Payment</a>
/// </summary>
public interface IOrderPaymentService
{
    /// <summary>
    /// Creates a new payment for one or more orders. Must be called before Download so the server
    /// can match the payment against an imported camt.05x file later.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/payment/create.json">API Doc - Order/Payment/Create</a>
    /// </summary>
    /// <param name="payment">The payment request — date + order IDs are mandatory.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(OrderPaymentRequest payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download the pain.001 or PDF file for a payment. Call <see cref="Create"/> first with the
    /// same parameters, otherwise the server can't match the payment later.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/order/payment/download">API Doc - Order/Payment/Download</a>
    /// </summary>
    /// <param name="payment">The payment request — same shape as <see cref="Create"/>.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<BinaryResponse>> Download(OrderPaymentRequest payment, CancellationToken cancellationToken = default);
}
