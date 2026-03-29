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

using CashCtrlApiNet.Abstractions.Models.Report.Set;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Report;

/// <summary>
/// E2E tests for report set service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Report.IReportSetService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class ReportSetE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupSetId;
    private int _secondSetId;
    private int _createdSetId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Creates the primary test report sets for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Create primary test report set
        var createResult = await CashCtrlApiClient.Report.Set.Create(new()
        {
            Name = _testId
        });
        _setupSetId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Report.Set.Delete(new() { Ids = [_setupSetId] }));

        // Create a second report set for reorder test
        var secondResult = await CashCtrlApiClient.Report.Set.Create(new()
        {
            Name = $"{_testId}-Second"
        });
        _secondSetId = AssertCreated(secondResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Report.Set.Delete(new() { Ids = [_secondSetId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a report set by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Report.Set.Get(new() { Id = _setupSetId });
        var set = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        set.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Create a report set successfully and store its ID for later tests
    /// </summary>
    [Test, Order(2)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Report.Set.Create(new()
        {
            Name = secondTestId
        });

        _createdSetId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Report.Set.Delete(new() { Ids = [_createdSetId] }));
    }

    /// <summary>
    /// Update a report set successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Report.Set.Get(new() { Id = _setupSetId });
        var set = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get report set for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Report.Set.Update((set as ReportSetUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Report.Set.Get(new() { Id = _setupSetId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Reorder report sets successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Reorder_Success()
    {
        var res = await CashCtrlApiClient.Report.Set.Reorder(new()
        {
            Ids = [_secondSetId],
            Target = _setupSetId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Get report set meta data successfully
    /// </summary>
    [Test, Order(5)]
    public async Task GetMeta_Success()
    {
        var res = await CashCtrlApiClient.Report.Set.GetMeta(new() { Id = _setupSetId });
        var meta = AssertSuccess(res);

        meta.ShouldNotBeNull();
    }

    /// <summary>
    /// Download report set as PDF successfully
    /// </summary>
    [Test, Order(6)]
    public async Task DownloadPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Set.DownloadPdf(new() { Id = _setupSetId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Download report set as CSV successfully
    /// </summary>
    [Test, Order(7)]
    public async Task DownloadCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Set.DownloadCsv(new() { Id = _setupSetId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Download report set as Excel successfully
    /// </summary>
    [Test, Order(8)]
    public async Task DownloadExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Set.DownloadExcel(new() { Id = _setupSetId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Download annual report as PDF successfully
    /// </summary>
    [Test, Order(9)]
    public async Task DownloadAnnualReport_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Set.DownloadAnnualReport(new() { Id = _setupSetId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the report set created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(10)]
    public async Task Delete_Success()
    {
        _createdSetId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Report.Set.Delete(new() { Ids = [_createdSetId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
