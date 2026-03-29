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

using CashCtrlApiNet.Abstractions.Models.Salary.Certificate;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary certificate service.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryCertificateService"/> operations.
/// Certificates are system-generated and cannot be created directly.
/// </summary>
[Category("E2e")]
public class SalaryCertificateE2eTests : CashCtrlE2eTestBase
{
    private int _certificateId;

    /// <summary>
    /// Discovers an existing salary certificate for use in tests
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Certificates are system-generated; discover an existing one
        var listResult = await CashCtrlApiClient.Salary.Certificate.GetList();
        _certificateId = listResult.ResponseData?.Data.FirstOrDefault()?.Id ?? 0;

        if (_certificateId == 0)
            Assert.Ignore("No salary certificates available in the test environment");
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a salary certificate by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Certificate.Get(new() { Id = _certificateId });
        var certificate = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        certificate.Id.ShouldBe(_certificateId);
    }

    /// <summary>
    /// Get list of salary certificates successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Certificate.GetList();
        var certificates = AssertSuccess(res);

        certificates.Length.ShouldBe(res.ResponseData!.Total);
        certificates.ShouldContain(c => c.Id == _certificateId);
    }

    /// <summary>
    /// Update a salary certificate successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Salary.Certificate.Get(new() { Id = _certificateId });
        var certificate = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get certificate for update");

        var res = await CashCtrlApiClient.Salary.Certificate.Update((certificate as SalaryCertificateUpdate) with
        {
            Notes = $"E2E test update {DateTime.UtcNow:yyyy-MM-dd}"
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Export salary certificates as Excel successfully
    /// </summary>
    [Test, Order(4)]
    public async Task ExportExcel_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Certificate.ExportExcel());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export salary certificates as CSV successfully
    /// </summary>
    [Test, Order(5)]
    public async Task ExportCsv_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Certificate.ExportCsv());
        await DownloadFile(export.FileName!, export.Data);
    }

    /// <summary>
    /// Export salary certificates as PDF successfully
    /// </summary>
    [Test, Order(6)]
    public async Task ExportPdf_Success()
    {
        var export = AssertSuccess(await CashCtrlApiClient.Salary.Certificate.ExportPdf());
        await DownloadFile(export.FileName!, export.Data);
    }
}
