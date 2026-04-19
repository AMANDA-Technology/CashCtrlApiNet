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

using Shouldly;

namespace CashCtrlApiNet.E2eTests.Order;

/// <summary>
/// E2E tests for order payment service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Order.IOrderPaymentService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class OrderPaymentE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _orderId;
    private int _paymentId;

    /// <summary>
    /// Scavenges orphan test data and creates the prerequisites for payment tests
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan persons from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Person.Person.GetList(),
            p => p.FirstName ?? "",
            p => p.Id,
            ids => CashCtrlApiClient.Person.Person.Delete(ids));

        // Scavenge orphan orders from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Order.Order.GetList(),
            o => o.Description ?? "",
            o => o.Id,
            ids => CashCtrlApiClient.Order.Order.Delete(ids));

        // Create a person as associate for the order
        var personResult = await CashCtrlApiClient.Person.Person.Create(new()
        {
            FirstName = _testId,
            LastName = "E2E"
        });
        var personId = AssertCreated(personResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [personId] }));

        // Discover an order category to get required IDs
        var categoryResult = await CashCtrlApiClient.Order.Category.GetList();
        var category = categoryResult.ResponseData?.Data.FirstOrDefault()
                       ?? throw new InvalidOperationException("No order categories found");

        var accountId = category.AccountId;
        var sequenceNumberId = category.SequenceNrId ?? throw new InvalidOperationException("Order category has no SequenceNrId");

        // Create an order for payment testing
        var orderResult = await CashCtrlApiClient.Order.Order.Create(new()
        {
            AccountId = accountId,
            CategoryId = category.Id,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            SequenceNumberId = sequenceNumberId,
            AssociateId = personId,
            Description = _testId
        });
        _orderId = AssertCreated(orderResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_orderId] }));

        // Book the order by updating its status
        var statusResult = await CashCtrlApiClient.Order.Category.GetStatus(new() { Id = category.Id });
        if (statusResult is { IsHttpSuccess: true, ResponseData.Data: not null })
        {
            await CashCtrlApiClient.Order.Order.UpdateStatus(new()
            {
                Id = _orderId,
                StatusId = statusResult.ResponseData.Data.Id
            });
        }
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Create a payment for an order successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Order.Payment.Create(new()
        {
            OrderId = _orderId
        });

        _paymentId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Download a payment file successfully
    /// </summary>
    [Test, Order(2)]
    public async Task Download_Success()
    {
        _paymentId.ShouldBeGreaterThan(0, "Create_Success must run before Download_Success");

        var res = await CashCtrlApiClient.Order.Payment.Download(new() { Id = _paymentId });
        var download = AssertSuccess(res);
        await DownloadFile(download.FileName!, download.Data);
    }
}
