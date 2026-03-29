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

using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.Abstractions.Models.Common.CustomFieldGroup;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common custom field group service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.ICustomFieldGroupService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class CustomFieldGroupE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupGroupId;
    private int _createdGroupId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test custom field group for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan custom field groups from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.CustomFieldGroup.GetList(new() { Type = CustomFieldType.PERSON }),
            g => g.Name,
            g => g.Id,
            ids => CashCtrlApiClient.Common.CustomFieldGroup.Delete(ids));

        // Create primary test custom field group
        var createResult = await CashCtrlApiClient.Common.CustomFieldGroup.Create(new()
        {
            Name = _testId,
            Type = CustomFieldType.PERSON
        });
        _setupGroupId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.CustomFieldGroup.Delete(new() { Ids = [_setupGroupId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a custom field group by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.CustomFieldGroup.Get(new() { Id = _setupGroupId });
        var group = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        group.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of custom field groups successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.CustomFieldGroup.GetList(new() { Type = CustomFieldType.PERSON });
        var groups = AssertSuccess(res);

        groups.Length.ShouldBe(res.ResponseData!.Total);
        groups.ShouldContain(g => g.Id == _setupGroupId);
    }

    /// <summary>
    /// Create a custom field group successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.CustomFieldGroup.Create(new()
        {
            Name = secondTestId,
            Type = CustomFieldType.PERSON
        });

        _createdGroupId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.CustomFieldGroup.Delete(new() { Ids = [_createdGroupId] }));
    }

    /// <summary>
    /// Update a custom field group successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.CustomFieldGroup.Get(new() { Id = _setupGroupId });
        var group = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get custom field group for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Common.CustomFieldGroup.Update((group as CustomFieldGroupUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.CustomFieldGroup.Get(new() { Id = _setupGroupId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Reorder custom field groups successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Reorder_Success()
    {
        _createdGroupId.ShouldBeGreaterThan(0, "Create_Success must run before Reorder_Success");

        var res = await CashCtrlApiClient.Common.CustomFieldGroup.Reorder(new()
        {
            Ids = [_createdGroupId],
            Target = _setupGroupId,
            Before = true
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Delete the custom field group created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(6)]
    public async Task Delete_Success()
    {
        _createdGroupId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.CustomFieldGroup.Delete(new() { Ids = [_createdGroupId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
