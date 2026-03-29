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

using CashCtrlApiNet.Abstractions.Models.Person;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Person;

/// <summary>
/// E2E tests for person service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Person.IPersonService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class PersonE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupPersonId;
    private int _createdPersonId;
    private int _categoryId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test person for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan persons from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Person.Person.GetList(),
            p => p.LastName ?? string.Empty,
            p => p.Id,
            ids => CashCtrlApiClient.Person.Person.Delete(ids));

        // Discover a category ID for the categorize test
        var categoryResult = await CashCtrlApiClient.Person.Category.GetList();
        _categoryId = categoryResult.ResponseData?.Data.FirstOrDefault()?.Id
                      ?? throw new InvalidOperationException("No person categories found for categorize test");

        // Create primary test person
        var createResult = await CashCtrlApiClient.Person.Person.Create(new()
        {
            FirstName = "E2E-First",
            LastName = _testId
        });
        _setupPersonId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [_setupPersonId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a person by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Person.Person.Get(new() { Id = _setupPersonId });
        var person = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        person.LastName.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of persons successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Person.Person.GetList();
        var persons = AssertSuccess(res);

        persons.Length.ShouldBe(res.ResponseData!.Total);
        persons.ShouldContain(p => p.Id == _setupPersonId);
    }

    /// <summary>
    /// Create a person successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Person.Person.Create(new()
        {
            FirstName = "E2E-Second",
            LastName = secondTestId
        });

        _createdPersonId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [_createdPersonId] }));
    }

    /// <summary>
    /// Update a person successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Person.Person.Get(new() { Id = _setupPersonId });
        var person = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get person for update");

        var updatedLastName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Person.Person.Update((person as PersonUpdate) with
        {
            LastName = updatedLastName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Person.Person.Get(new() { Id = _setupPersonId });
        verify.ResponseData?.Data?.LastName.ShouldBe(updatedLastName);
    }

    /// <summary>
    /// Categorize a person successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Categorize_Success()
    {
        var res = await CashCtrlApiClient.Person.Person.Categorize(new()
        {
            Ids = [_setupPersonId],
            TargetCategoryId = _categoryId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update person attachments successfully
    /// </summary>
    [Test, Order(6)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Person.Person.UpdateAttachments(new()
        {
            Id = _setupPersonId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export persons as Excel successfully
    /// </summary>
    [Test, Order(7)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Person.Person.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export persons as CSV successfully
    /// </summary>
    [Test, Order(8)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Person.Person.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export persons as PDF successfully
    /// </summary>
    [Test, Order(9)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Person.Person.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export persons as vCard successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportVcard_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Person.Person.ExportVcard());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the person created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(11)]
    public async Task Delete_Success()
    {
        _createdPersonId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [_createdPersonId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
