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

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary payment service.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryPaymentService"/> operations.
/// </summary>
[Category("E2e")]
[Ignore("Group 7 (Salary) not yet verified against live API — expect model/parameter discrepancies similar to Groups 1-6. See doc/analysis/2026-03-29-e2e-test-verification.md. Remove this attribute when the fixture is verified.")]
// ReSharper disable once InconsistentNaming
public class SalaryPaymentE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _personId;
    private int _statementId;

    /// <summary>
    /// Creates prerequisite test data: a person and a salary statement for payment operations
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

        // Discover prerequisite IDs
        var statusResult = await CashCtrlApiClient.Salary.Status.GetList();
        var statusId = statusResult.ResponseData?.Data.FirstOrDefault()?.Id
                       ?? throw new InvalidOperationException("No salary statuses found");

        var templateResult = await CashCtrlApiClient.Salary.Template.GetList();
        var templateId = templateResult.ResponseData?.Data.FirstOrDefault()?.Id
                         ?? throw new InvalidOperationException("No salary templates found");

        // Create a salary statement as prerequisite for payments
        var statementResult = await CashCtrlApiClient.Salary.Statement.Create(new()
        {
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            DatePayment = DateTime.UtcNow.AddDays(30).ToString("yyyy-MM-dd"),
            PersonId = _personId,
            StatusId = statusId,
            TemplateId = templateId,
            Nr = _testId
        });
        _statementId = AssertCreated(statementResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Salary.Statement.Delete(new() { Ids = [_statementId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Create a salary payment successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Salary.Payment.Create(new()
        {
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            StatementIds = _statementId.ToString()
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Download a salary payment file successfully
    /// </summary>
    [Test, Order(2)]
    public async Task Download_Success()
    {
        var res = await CashCtrlApiClient.Salary.Payment.Download(new()
        {
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            StatementIds = _statementId.ToString()
        });
        var download = AssertSuccess(res);
        await DownloadFile(download.FileName!, download.Data);
    }
}
