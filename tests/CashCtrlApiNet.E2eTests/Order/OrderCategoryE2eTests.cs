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

using System.Text.Json;
using CashCtrlApiNet.Abstractions.Models.Order.Category;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Order;

/// <summary>
/// E2E tests for order category service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Order.IOrderCategoryService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class OrderCategoryE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _accountId;
    private int _setupCategoryId;
    private int _createdCategoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test order category for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan order categories from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Order.Category.GetList(),
            c => c.Name ?? string.Empty,
            c => c.Id,
            ids => CashCtrlApiClient.Order.Category.Delete(ids));

        // Discover a usable account ID (needed for order category create)
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        _accountId = accountResult.ResponseData?.Data.FirstOrDefault()?.Id
                     ?? throw new InvalidOperationException("No accounts found");

        // Create primary test order category. Per API docs: accountId, nameSingular, namePlural,
        // and status are all mandatory — creating with just a Name fails with "This field cannot be empty"
        // (the Name property isn't even a real API parameter; it's a derived read-only field).
        var createResult = await CashCtrlApiClient.Order.Category.Create(BuildCreate(_testId, _accountId));
        _setupCategoryId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Order.Category.Delete(new() { Ids = [_setupCategoryId] }));
    }

    private static OrderCategoryCreate BuildCreate(string testId, int accountId) => new()
    {
        AccountId = accountId,
        NameSingular = testId,
        NamePlural = testId,
        // At least one status is mandatory. icon values: BLUE, GREEN, RED, YELLOW, ORANGE, BLACK, GRAY, BROWN, VIOLET, PINK.
        Status = JsonSerializer.Deserialize<JsonElement>("""[{"icon":"BLUE","name":"Draft"}]""")
    };

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get an order category by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Order.Category.Get(new() { Id = _setupCategoryId });
        var category = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        // Name is derived server-side; it's populated with localized XML that contains our testId.
        category.Name.ShouldNotBeNullOrEmpty();
        category.Name!.ShouldContain(_testId);
    }

    /// <summary>
    /// Get list of order categories successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Order.Category.GetList();
        var categories = AssertSuccess(res);

        categories.Length.ShouldBe(res.ResponseData!.Total);
        categories.ShouldContain(c => c.Id == _setupCategoryId);
    }

    /// <summary>
    /// Create an order category successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Order.Category.Create(BuildCreate(secondTestId, _accountId));

        _createdCategoryId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Order.Category.Delete(new() { Ids = [_createdCategoryId] }));
    }

    /// <summary>
    /// Update an order category successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Order.Category.Get(new() { Id = _setupCategoryId });
        var category = AssertSuccess(get);

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Order.Category.Update((category as OrderCategoryUpdate) with
        {
            NameSingular = updatedName,
            NamePlural = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted (Name is server-derived from NameSingular).
        var verify = await CashCtrlApiClient.Order.Category.Get(new() { Id = _setupCategoryId });
        verify.ResponseData?.Data?.Name!.ShouldContain(updatedName);
    }

    /// <summary>
    /// Reorder order categories successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Reorder_Success()
    {
        var res = await CashCtrlApiClient.Order.Category.Reorder(new()
        {
            Ids = [_setupCategoryId],
            Target = _setupCategoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Get a single status row from an order category successfully.
    /// The <c>read_status.json</c> endpoint takes a STATUS id (not a category id) — discover the
    /// real status id from the category's <c>status</c> array first.
    /// </summary>
    [Test, Order(6)]
    public async Task GetStatus_Success()
    {
        var category = AssertSuccess(await CashCtrlApiClient.Order.Category.Get(new() { Id = _setupCategoryId }));
        category.Status.ShouldNotBeNull();
        var firstStatusId = category.Status!.Value.EnumerateArray().First().GetProperty("id").GetInt32();

        var res = await CashCtrlApiClient.Order.Category.GetStatus(new() { Id = firstStatusId });
        var status = AssertSuccess(res);

        status.Id.ShouldBe(firstStatusId);
        status.CategoryId.ShouldBe(_setupCategoryId);
    }

    /// <summary>
    /// Delete the order category created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(7)]
    public async Task Delete_Success()
    {
        _createdCategoryId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Order.Category.Delete(new() { Ids = [_createdCategoryId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
