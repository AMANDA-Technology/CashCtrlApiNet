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

using CashCtrlApiNet.Abstractions.Models.Order;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Order;

/// <summary>
/// E2E tests for order service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Order.IOrderService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class OrderE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupOrderId;
    private int _createdOrderId;
    private int _secondOrderId;
    private int _personId;
    private int _categoryId;
    private int _accountId;
    private int _sequenceNumberId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test order for the fixture
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
        _personId = AssertCreated(personResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [_personId] }));

        // Discover an order category to get required IDs
        var categoryResult = await CashCtrlApiClient.Order.Category.GetList();
        var category = categoryResult.ResponseData?.Data.FirstOrDefault()
                       ?? throw new InvalidOperationException("No order categories found");
        _categoryId = category.Id;
        _accountId = category.AccountId;
        _sequenceNumberId = category.SequenceNrId ?? throw new InvalidOperationException("Order category has no SequenceNrId");

        // Create primary test order
        var createResult = await CashCtrlApiClient.Order.Order.Create(new()
        {
            AccountId = _accountId,
            CategoryId = _categoryId,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            SequenceNumberId = _sequenceNumberId,
            AssociateId = _personId,
            Description = _testId
        });
        _setupOrderId = AssertCreated(createResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_setupOrderId] }));

        // Create a second order for dossier tests
        var secondResult = await CashCtrlApiClient.Order.Order.Create(new()
        {
            AccountId = _accountId,
            CategoryId = _categoryId,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            SequenceNumberId = _sequenceNumberId,
            AssociateId = _personId,
            Description = $"{_testId}-Dossier"
        });
        _secondOrderId = AssertCreated(secondResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_secondOrderId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get an order by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Order.Order.Get(new() { Id = _setupOrderId });
        var order = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        order.Description.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of orders successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Order.Order.GetList();
        var orders = AssertSuccess(res);

        orders.Length.ShouldBe(res.ResponseData!.Total);
        orders.ShouldContain(o => o.Id == _setupOrderId);
    }

    /// <summary>
    /// Create an order successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Order.Order.Create(new()
        {
            AccountId = _accountId,
            CategoryId = _categoryId,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            SequenceNumberId = _sequenceNumberId,
            AssociateId = _personId,
            Description = GenerateTestId()
        });

        _createdOrderId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_createdOrderId] }));
    }

    /// <summary>
    /// Update an order successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Order.Order.Get(new() { Id = _setupOrderId });
        var order = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get order for update");

        var updatedDescription = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Order.Order.Update((order as OrderUpdate) with
        {
            Description = updatedDescription
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Order.Order.Get(new() { Id = _setupOrderId });
        verify.ResponseData?.Data?.Description.ShouldBe(updatedDescription);
    }

    /// <summary>
    /// Update order status successfully. Discover a real status id from the category's status
    /// array — Category.GetStatus takes a status id, not a category id.
    /// </summary>
    [Test, Order(5)]
    public async Task UpdateStatus_Success()
    {
        var category = AssertSuccess(await CashCtrlApiClient.Order.Category.Get(new() { Id = _categoryId }));
        category.Status.ShouldNotBeNull();
        var firstStatusId = category.Status!.Value.EnumerateArray().First().GetProperty("id").GetInt32();

        var res = await CashCtrlApiClient.Order.Order.UpdateStatus(new()
        {
            Ids = [_setupOrderId],
            StatusId = firstStatusId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update order recurrence successfully
    /// </summary>
    [Test, Order(6)]
    public async Task UpdateRecurrence_Success()
    {
        var res = await CashCtrlApiClient.Order.Order.UpdateRecurrence(new()
        {
            Id = _setupOrderId,
            Recurrence = "MONTHLY",
            // startDate is documented as optional but is required in practice whenever recurrence is set.
            StartDate = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd")
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Continue the setup order as a new order in a different target category
    /// (e.g. turning an offer into an invoice).
    /// </summary>
    [Test, Order(7)]
    public async Task Continue_Success()
    {
        // Continue requires a target categoryId different from the source. Discover one.
        var categoriesRes = await CashCtrlApiClient.Order.Category.GetList();
        var categories = AssertSuccess(categoriesRes);
        var targetCategoryId = categories.First(c => c.Id != _categoryId).Id;

        var res = await CashCtrlApiClient.Order.Order.Continue(new()
        {
            CategoryId = targetCategoryId,
            Ids = [_setupOrderId]
        });
        AssertSuccess(res);

        // Continue creates a new order in the target category, sharing the setup order's dossier.
        // Find and register cleanup so we don't leak the continued order.
        var dossier = AssertSuccess(await CashCtrlApiClient.Order.Order.GetDossier(new() { Id = _setupOrderId }));
        var continuedOrderId = dossier.Items
            .Where(o => o.Id != _setupOrderId && o.Id != _secondOrderId && o.CategoryId == targetCategoryId)
            .Select(o => o.Id)
            .FirstOrDefault();
        if (continuedOrderId > 0)
            RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [continuedOrderId] }));
    }

    /// <summary>
    /// Get the dossier (group of related orders) the setup order belongs to.
    /// </summary>
    [Test, Order(8)]
    public async Task GetDossier_Success()
    {
        var res = await CashCtrlApiClient.Order.Order.GetDossier(new() { Id = _setupOrderId });
        var dossier = AssertSuccess(res);
        dossier.Items.ShouldContain(o => o.Id == _setupOrderId);
    }

    /// <summary>
    /// Add an order to an existing dossier successfully. <c>GroupId</c> comes from the setup
    /// order's own <c>groupId</c> — <c>ids</c> is the list of orders to add to that group.
    /// </summary>
    [Test, Order(9)]
    public async Task DossierAdd_Success()
    {
        var setupOrder = AssertSuccess(await CashCtrlApiClient.Order.Order.Get(new() { Id = _setupOrderId }));
        var groupId = setupOrder.GroupId ?? throw new InvalidOperationException("Setup order has no GroupId");

        var res = await CashCtrlApiClient.Order.Order.DossierAdd(new()
        {
            GroupId = groupId,
            Ids = [_secondOrderId]
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Remove an order from a dossier successfully. Counterpart to <see cref="DossierAdd_Success"/>.
    /// </summary>
    [Test, Order(10)]
    public async Task DossierRemove_Success()
    {
        var setupOrder = AssertSuccess(await CashCtrlApiClient.Order.Order.Get(new() { Id = _setupOrderId }));
        var groupId = setupOrder.GroupId ?? throw new InvalidOperationException("Setup order has no GroupId");

        var res = await CashCtrlApiClient.Order.Order.DossierRemove(new()
        {
            GroupId = groupId,
            Ids = [_secondOrderId]
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update order attachments successfully
    /// </summary>
    [Test, Order(11)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Order.Order.UpdateAttachments(new()
        {
            Id = _setupOrderId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export orders as Excel successfully
    /// </summary>
    [Test, Order(12)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Order.Order.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export orders as CSV successfully
    /// </summary>
    [Test, Order(13)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Order.Order.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export orders as PDF successfully
    /// </summary>
    [Test, Order(14)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Order.Order.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the order created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(15)]
    public async Task Delete_Success()
    {
        _createdOrderId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_createdOrderId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
