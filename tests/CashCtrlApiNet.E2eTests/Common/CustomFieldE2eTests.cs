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
using CashCtrlApiNet.Abstractions.Models.Common.CustomField;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common custom field service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.ICustomFieldService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class CustomFieldE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupFieldId;
    private int _createdFieldId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test custom field for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan custom fields from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.CustomField.GetList(new() { Type = CustomFieldType.PERSON }),
            f => f.RowLabel,
            f => f.Id,
            ids => CashCtrlApiClient.Common.CustomField.Delete(ids));

        // Create primary test custom field
        var createResult = await CashCtrlApiClient.Common.CustomField.Create(new()
        {
            DataType = CustomFieldDataType.TEXT,
            RowLabel = _testId,
            Type = CustomFieldType.PERSON
        });
        _setupFieldId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.CustomField.Delete(new() { Ids = [_setupFieldId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a custom field by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.CustomField.Get(new() { Id = _setupFieldId });
        var field = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        field.RowLabel.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of custom fields successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.CustomField.GetList(new() { Type = CustomFieldType.PERSON });
        var fields = AssertSuccess(res);

        fields.Length.ShouldBe(res.ResponseData!.Total);
        fields.ShouldContain(f => f.Id == _setupFieldId);
    }

    /// <summary>
    /// Create a custom field successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.CustomField.Create(new()
        {
            DataType = CustomFieldDataType.TEXT,
            RowLabel = secondTestId,
            Type = CustomFieldType.PERSON
        });

        _createdFieldId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.CustomField.Delete(new() { Ids = [_createdFieldId] }));
    }

    /// <summary>
    /// Update a custom field successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.CustomField.Get(new() { Id = _setupFieldId });
        var field = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get custom field for update");

        var updatedLabel = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Common.CustomField.Update(new()
        {
            Id = field.Id,
            DataType = field.DataType,
            RowLabel = updatedLabel,
            Type = field.Type
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.CustomField.Get(new() { Id = _setupFieldId });
        verify.ResponseData?.Data?.RowLabel.ShouldBe(updatedLabel);
    }

    /// <summary>
    /// Reorder custom fields successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Reorder_Success()
    {
        _createdFieldId.ShouldBeGreaterThan(0, "Create_Success must run before Reorder_Success");

        var res = await CashCtrlApiClient.Common.CustomField.Reorder(new()
        {
            Type = CustomFieldType.PERSON,
            Ids = [_createdFieldId],
            Target = _setupFieldId,
            Before = true
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Get custom field types successfully
    /// </summary>
    [Test, Order(6)]
    public async Task GetTypes_Success()
    {
        var res = await CashCtrlApiClient.Common.CustomField.GetTypes();
        res.IsHttpSuccess.ShouldBeTrue();
    }

    /// <summary>
    /// Delete the custom field created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(7)]
    public async Task Delete_Success()
    {
        _createdFieldId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.CustomField.Delete(new() { Ids = [_createdFieldId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
