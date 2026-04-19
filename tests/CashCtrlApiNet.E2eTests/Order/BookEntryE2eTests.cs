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

using CashCtrlApiNet.Abstractions.Models.Order.BookEntry;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Order;

/// <summary>
/// E2E tests for order book entry service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Order.IBookEntryService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class BookEntryE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupBookEntryId;
    private int _createdBookEntryId;
    private int _orderId;
    private int _accountId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test book entry for the fixture
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

        // Discover an order category whose status list allows book entries (isBook=true on at
        // least one status — typically the Invoice/Rechnung category). Attempting this with the
        // default Offer category fails with "This document does not allow book entries."
        var categories = AssertSuccess(await CashCtrlApiClient.Order.Category.GetList());
        var bookableCategory = categories
            .Select(c => new
            {
                c.Id,
                c.AccountId,
                c.SequenceNrId,
                BookStatusId = TryFindBookStatusId(c)
            })
            .FirstOrDefault(c => c.BookStatusId is not null)
            ?? throw new InvalidOperationException("No order category with a status where isBook=true");

        _accountId = bookableCategory.AccountId;
        var sequenceNumberId = bookableCategory.SequenceNrId ?? throw new InvalidOperationException("Order category has no SequenceNrId");

        // Create an order as parent for book entries
        var orderResult = await CashCtrlApiClient.Order.Order.Create(new()
        {
            AccountId = _accountId,
            CategoryId = bookableCategory.Id,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            SequenceNumberId = sequenceNumberId,
            AssociateId = personId,
            Description = _testId
        });
        _orderId = AssertCreated(orderResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_orderId] }));

        // Move the order into a booked status so the API accepts book entries against it.
        AssertSuccess(await CashCtrlApiClient.Order.Order.UpdateStatus(new()
        {
            Ids = [_orderId],
            StatusId = bookableCategory.BookStatusId!.Value
        }));

        // Scavenge orphan book entries from previous failed runs (list requires the order id).
        await ScavengeOrphans(
            () => CashCtrlApiClient.Order.BookEntry.GetList(new() { OrderId = _orderId }),
            b => b.Description ?? "",
            b => b.Id,
            ids => CashCtrlApiClient.Order.BookEntry.Delete(ids));

        // Create primary test book entry
        var createResult = await CashCtrlApiClient.Order.BookEntry.Create(new()
        {
            OrderIds = [_orderId],
            AccountId = _accountId,
            Amount = 100.0,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            Description = _testId
        });
        _setupBookEntryId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Order.BookEntry.Delete(new() { Ids = [_setupBookEntryId] }));
    }

    /// <summary>
    /// Return the ID of the first status in the category whose <c>isBook</c> flag is true, or null
    /// if the category has no such status.
    /// </summary>
    private static int? TryFindBookStatusId(CashCtrlApiNet.Abstractions.Models.Order.Category.OrderCategory category)
    {
        if (category.Status is not { } statusArray)
            return null;
        foreach (var status in statusArray.EnumerateArray())
        {
            if (status.TryGetProperty("isBook", out var isBook) && isBook.GetBoolean())
                return status.GetProperty("id").GetInt32();
        }
        return null;
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a book entry by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Order.BookEntry.Get(new() { Id = _setupBookEntryId });
        var bookEntry = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        bookEntry.Description.ShouldBe(_testId);
        bookEntry.Amount.ShouldBe(100.0);
    }

    /// <summary>
    /// Get list of book entries successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Order.BookEntry.GetList(new() { OrderId = _orderId });
        var bookEntries = AssertSuccess(res);

        bookEntries.ShouldContain(b => b.Id == _setupBookEntryId);
    }

    /// <summary>
    /// Create a book entry successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Order.BookEntry.Create(new()
        {
            OrderIds = [_orderId],
            AccountId = _accountId,
            Amount = 50.0,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            Description = secondTestId
        });

        _createdBookEntryId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Order.BookEntry.Delete(new() { Ids = [_createdBookEntryId] }));
    }

    /// <summary>
    /// Update a book entry successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Order.BookEntry.Get(new() { Id = _setupBookEntryId });
        var bookEntry = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get book entry for update");

        var updatedDescription = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Order.BookEntry.Update((bookEntry as BookEntryUpdate) with
        {
            Description = updatedDescription
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Order.BookEntry.Get(new() { Id = _setupBookEntryId });
        verify.ResponseData?.Data?.Description.ShouldBe(updatedDescription);
    }

    /// <summary>
    /// Delete the book entry created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdBookEntryId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Order.BookEntry.Delete(new() { Ids = [_createdBookEntryId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
