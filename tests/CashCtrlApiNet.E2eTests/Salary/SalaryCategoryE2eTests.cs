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

using CashCtrlApiNet.Abstractions.Models.Salary.Category;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary category service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryCategoryService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class SalaryCategoryE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupCategoryId;
    private int _createdCategoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test category for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan salary categories from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.Category.GetList(),
            c => c.Name,
            c => c.Id,
            ids => CashCtrlApiClient.Salary.Category.Delete(ids));

        // Create primary test category
        var createResult = await CashCtrlApiClient.Salary.Category.Create(new()
        {
            Name = _testId
        });
        _setupCategoryId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Category.Delete(new() { Ids = [_setupCategoryId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary category by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Category.Get(new() { Id = _setupCategoryId });
        var category = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        category.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of salary categories successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Category.GetList();
        var categories = AssertSuccess(res);

        categories.Length.ShouldBe(res.ResponseData!.Total);
        categories.ShouldContain(c => c.Id == _setupCategoryId);
    }

    /// <summary>
    /// Get salary category tree successfully
    /// </summary>
    [Test, Order(3)]
    public async Task GetTree_Success()
    {
        var res = await CashCtrlApiClient.Salary.Category.GetTree();
        AssertSuccess(res);
    }

    /// <summary>
    /// Create a salary category successfully and store its ID for later tests
    /// </summary>
    [Test, Order(4)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.Category.Create(new()
        {
            Name = secondTestId
        });

        _createdCategoryId = AssertCreated(res);
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.Category.Delete(new() { Ids = [_createdCategoryId] }));
    }

    /// <summary>
    /// Update a salary category successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Category.Get(new() { Id = _setupCategoryId });
        var category = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get salary category for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Salary.Category.Update((category as SalaryCategoryUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.Category.Get(new() { Id = _setupCategoryId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the salary category created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(6)]
    public async Task Delete_Success()
    {
        _createdCategoryId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.Category.Delete(new() { Ids = [_createdCategoryId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
