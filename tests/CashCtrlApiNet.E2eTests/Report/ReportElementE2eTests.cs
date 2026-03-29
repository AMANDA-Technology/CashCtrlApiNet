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

using CashCtrlApiNet.Abstractions.Models.Report.Element;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Report;

/// <summary>
/// E2E tests for report element service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Report.IReportElementService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class ReportElementE2eTests : CashCtrlE2eTestBase
{
    private int _reportId;
    private int _accountId;
    private int _setupSetId;
    private int _setupElementId;
    private int _secondElementId;
    private int _createdElementId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Discovers prerequisite IDs and creates the primary test report element for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Discover a report ID from the report tree
        var treeResult = await CashCtrlApiClient.Report.Report.GetTree();
        treeResult.IsHttpSuccess.ShouldBeTrue();
        treeResult.ResponseData.ShouldNotBeNull();
        treeResult.ResponseData.Data.Length.ShouldBeGreaterThan(0);
        _reportId = treeResult.ResponseData.Data.First().Id;

        // Discover an account ID for creating report elements
        var accountResult = await CashCtrlApiClient.Account.Account.GetList();
        accountResult.IsHttpSuccess.ShouldBeTrue();
        _accountId = accountResult.ResponseData?.Data.FirstOrDefault()?.Id
                     ?? throw new InvalidOperationException("No accounts found for report element tests");

        // Create a prerequisite report set for element tests
        var setName = GenerateTestId();
        var setResult = await CashCtrlApiClient.Report.Set.Create(new()
        {
            Name = setName
        });
        _setupSetId = AssertCreated(setResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Report.Set.Delete(new() { Ids = [_setupSetId] }));

        // Create primary test report element
        var createResult = await CashCtrlApiClient.Report.Element.Create(new()
        {
            ReportId = _reportId,
            AccountId = _accountId
        });
        _setupElementId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Report.Element.Delete(new() { Ids = [_setupElementId] }));

        // Create a second element for reorder test
        var secondResult = await CashCtrlApiClient.Report.Element.Create(new()
        {
            ReportId = _reportId,
            AccountId = _accountId
        });
        _secondElementId = AssertCreated(secondResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Report.Element.Delete(new() { Ids = [_secondElementId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a report element by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Report.Element.Get(new() { Id = _setupElementId });
        var element = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        element.ReportId.ShouldBe(_reportId);
        element.AccountId.ShouldBe(_accountId);
    }

    /// <summary>
    /// Create a report element successfully and store its ID for later tests
    /// </summary>
    [Test, Order(2)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Report.Element.Create(new()
        {
            ReportId = _reportId,
            AccountId = _accountId
        });

        _createdElementId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Report.Element.Delete(new() { Ids = [_createdElementId] }));
    }

    /// <summary>
    /// Update a report element successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Report.Element.Get(new() { Id = _setupElementId });
        var element = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get report element for update");

        var res = await CashCtrlApiClient.Report.Element.Update((element as ReportElementUpdate) with
        {
            NegateAmount = true
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Report.Element.Get(new() { Id = _setupElementId });
        verify.ResponseData?.Data?.NegateAmount.ShouldBe(true);
    }

    /// <summary>
    /// Reorder report elements successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Reorder_Success()
    {
        var res = await CashCtrlApiClient.Report.Element.Reorder(new()
        {
            Ids = [_secondElementId],
            Target = _setupElementId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Get report element data as JSON successfully
    /// </summary>
    [Test, Order(5)]
    public async Task GetData_Success()
    {
        var res = await CashCtrlApiClient.Report.Element.GetData(new() { Id = _setupElementId });
        var data = AssertSuccess(res);

        data.ShouldNotBeNull();
    }

    /// <summary>
    /// Get report element data as HTML successfully
    /// </summary>
    [Test, Order(6)]
    public async Task GetDataHtml_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Element.GetDataHtml(new() { Id = _setupElementId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Get report element meta data successfully
    /// </summary>
    [Test, Order(7)]
    public async Task GetMeta_Success()
    {
        var res = await CashCtrlApiClient.Report.Element.GetMeta(new() { Id = _setupElementId });
        var meta = AssertSuccess(res);

        meta.ShouldNotBeNull();
    }

    /// <summary>
    /// Download report element as PDF successfully
    /// </summary>
    [Test, Order(8)]
    public async Task DownloadPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Element.DownloadPdf(new() { Id = _setupElementId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Download report element as CSV successfully
    /// </summary>
    [Test, Order(9)]
    public async Task DownloadCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Element.DownloadCsv(new() { Id = _setupElementId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Download report element as Excel successfully
    /// </summary>
    [Test, Order(10)]
    public async Task DownloadExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Report.Element.DownloadExcel(new() { Id = _setupElementId }));
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Delete the report element created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(11)]
    public async Task Delete_Success()
    {
        _createdElementId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Report.Element.Delete(new() { Ids = [_createdElementId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
