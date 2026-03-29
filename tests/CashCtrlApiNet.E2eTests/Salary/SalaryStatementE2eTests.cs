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

using CashCtrlApiNet.Abstractions.Models.Salary.Statement;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary statement service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryStatementService"/> operations.
/// </summary>
[Category("E2e")]
public class SalaryStatementE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _personId;
    private int _statusId;
    private int _templateId;
    private int _setupStatementId;
    private int _createdStatementId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test statement for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan statements from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Salary.Statement.GetList(),
            s => s.Nr ?? string.Empty,
            s => s.Id,
            ids => CashCtrlApiClient.Salary.Statement.Delete(ids));

        // Scavenge orphan persons from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Person.Person.GetList(),
            p => p.LastName ?? string.Empty,
            p => p.Id,
            ids => CashCtrlApiClient.Person.Person.Delete(ids));

        // Create a person for salary statements
        var personResult = await CashCtrlApiClient.Person.Person.Create(new()
        {
            FirstName = "Test",
            LastName = _testId
        });
        _personId = AssertCreated(personResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [_personId] }));

        // Discover a salary status ID
        var statusResult = await CashCtrlApiClient.Salary.Status.GetList();
        _statusId = statusResult.ResponseData?.Data.FirstOrDefault()?.Id
                    ?? throw new InvalidOperationException("No salary statuses found");

        // Discover a salary template ID
        var templateResult = await CashCtrlApiClient.Salary.Template.GetList();
        _templateId = templateResult.ResponseData?.Data.FirstOrDefault()?.Id
                      ?? throw new InvalidOperationException("No salary templates found");

        // Create primary test statement
        var createResult = await CashCtrlApiClient.Salary.Statement.Create(new()
        {
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            DatePayment = DateTime.UtcNow.AddDays(30).ToString("yyyy-MM-dd"),
            PersonId = _personId,
            StatusId = _statusId,
            TemplateId = _templateId,
            Nr = _testId
        });
        _setupStatementId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Statement.Delete(new() { Ids = [_setupStatementId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary statement by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Statement.Get(new() { Id = _setupStatementId });
        var statement = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        statement.Nr.ShouldBe(_testId);
        statement.PersonId.ShouldBe(_personId);
    }

    /// <summary>
    /// Get list of salary statements successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Statement.GetList();
        var statements = AssertSuccess(res);

        statements.Length.ShouldBe(res.ResponseData!.Total);
        statements.ShouldContain(s => s.Id == _setupStatementId);
    }

    /// <summary>
    /// Create a salary statement successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Salary.Statement.Create(new()
        {
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            DatePayment = DateTime.UtcNow.AddDays(30).ToString("yyyy-MM-dd"),
            PersonId = _personId,
            StatusId = _statusId,
            TemplateId = _templateId,
            Nr = secondTestId
        });

        _createdStatementId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Salary.Statement.Delete(new() { Ids = [_createdStatementId] }));
    }

    /// <summary>
    /// Update a salary statement successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Statement.Get(new() { Id = _setupStatementId });
        var statement = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get statement for update");

        var updatedNotes = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Salary.Statement.Update((statement as SalaryStatementUpdate) with
        {
            Notes = updatedNotes
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Salary.Statement.Get(new() { Id = _setupStatementId });
        verify.ResponseData?.Data?.Notes.ShouldBe(updatedNotes);
    }

    /// <summary>
    /// Update multiple salary statements successfully
    /// </summary>
    [Test, Order(5)]
    public async Task UpdateMultiple_Success()
    {
        _createdStatementId.ShouldBeGreaterThan(0, "Create_Success must run before UpdateMultiple_Success");

        var updatedNotes = $"{_testId}-MultiUpdated";
        var res = await CashCtrlApiClient.Salary.Statement.UpdateMultiple(new()
        {
            Ids = $"{_setupStatementId},{_createdStatementId}",
            Notes = updatedNotes
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update salary statement status successfully
    /// </summary>
    [Test, Order(6)]
    public async Task UpdateStatus_Success()
    {
        var res = await CashCtrlApiClient.Salary.Statement.UpdateStatus(new()
        {
            Ids = _setupStatementId.ToString(),
            StatusId = _statusId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update salary statement recurrence successfully
    /// </summary>
    [Test, Order(7)]
    public async Task UpdateRecurrence_Success()
    {
        var res = await CashCtrlApiClient.Salary.Statement.UpdateRecurrence(new()
        {
            Id = _setupStatementId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Calculate salary statement successfully
    /// </summary>
    [Test, Order(8)]
    public async Task Calculate_Success()
    {
        var res = await CashCtrlApiClient.Salary.Statement.Calculate(new()
        {
            Id = _setupStatementId,
            Recalculate = true
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Update salary statement attachments successfully
    /// </summary>
    [Test, Order(9)]
    public async Task UpdateAttachments_Success()
    {
        var res = await CashCtrlApiClient.Salary.Statement.UpdateAttachments(new()
        {
            Id = _setupStatementId,
            AttachedFileIds = []
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export salary statements as Excel successfully
    /// </summary>
    [Test, Order(10)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Statement.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export salary statements as CSV successfully
    /// </summary>
    [Test, Order(11)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Statement.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export salary statements as PDF successfully
    /// </summary>
    [Test, Order(12)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Statement.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the salary statement created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(13)]
    public async Task Delete_Success()
    {
        _createdStatementId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Salary.Statement.Delete(new() { Ids = [_createdStatementId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
