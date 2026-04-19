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

using CashCtrlApiNet.Abstractions.Models.Salary.Document;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary document service.
/// Covers <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryDocumentService"/> operations
/// (excludes SendMail). Salary documents are system-generated and cannot be created directly.
/// </summary>
[Category("E2e")]
[Ignore("Group 7 (Salary) not yet verified against live API — expect model/parameter discrepancies similar to Groups 1-6. See doc/analysis/2026-03-29-e2e-test-verification.md. Remove this attribute when the fixture is verified.")]
// ReSharper disable once InconsistentNaming
public class SalaryDocumentE2eTests : CashCtrlE2eTestBase
{
    private int _documentId;

    /// <summary>
    /// Discovers an existing salary document for use in tests
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Salary documents are system-generated from statements; discover an existing one
        var statementListResult = await CashCtrlApiClient.Salary.Statement.GetList();
        if (statementListResult.ResponseData?.Data is not { Length: > 0 } statements)
        {
            Assert.Ignore("No salary statements available in the test environment");
            return;
        }

        // Try to find a salary document by iterating through statements
        foreach (var stmt in statements)
        {
            try
            {
                var docResult = await CashCtrlApiClient.Salary.Document.Get(new() { Id = stmt.Id });
                if (docResult is { IsHttpSuccess: true, ResponseData.Data: not null })
                {
                    _documentId = docResult.ResponseData.Data.Id;
                    break;
                }
            }
            catch
            {
                // Continue trying other statements
            }
        }

        if (_documentId == 0)
            Assert.Ignore("No salary documents available in the test environment");
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary document by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Document.Get(new() { Id = _documentId });
        var document = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        document.Id.ShouldBe(_documentId);
    }

    /// <summary>
    /// Download salary document as PDF successfully
    /// </summary>
    [Test, Order(2)]
    public async Task DownloadPdf_Success()
    {
        var res = await CashCtrlApiClient.Salary.Document.DownloadPdf(new() { Ids = [_documentId] });
        var download = AssertSuccess(res);
        await DownloadFile(download.FileName!, download.Data);
    }

    /// <summary>
    /// Download salary documents as ZIP successfully
    /// </summary>
    [Test, Order(3)]
    public async Task DownloadZip_Success()
    {
        var res = await CashCtrlApiClient.Salary.Document.DownloadZip(new() { Ids = [_documentId] });
        var download = AssertSuccess(res);
        await DownloadFile(download.FileName!, download.Data);
    }

    /// <summary>
    /// Update a salary document successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Document.Get(new() { Id = _documentId });
        var document = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get document for update");

        var res = await CashCtrlApiClient.Salary.Document.Update((document as SalaryDocumentUpdate) with
        {
            Footer = $"E2E test update {DateTime.UtcNow:yyyy-MM-dd}"
        });
        AssertSuccess(res);
    }
}
