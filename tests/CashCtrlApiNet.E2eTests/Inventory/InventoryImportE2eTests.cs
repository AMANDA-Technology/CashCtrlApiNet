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

using System.Collections.Immutable;
using System.Text;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Inventory;

/// <summary>
/// E2E tests for inventory import service with multi-step import workflow.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Inventory.IInventoryImportService"/> operations.
/// The import workflow requires: file upload (Prepare + Persist) -> Create -> Mapping -> GetMappingFields -> Preview -> Execute.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class InventoryImportE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _fileId;
    private int _importId;

    /// <summary>
    /// Uploads a CSV file via the File service (Prepare + Persist) for use in the import workflow
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan articles that may have been created by previous import runs.
        // Match by Name OR Nr because earlier broken-mapping runs could leave articles
        // with no "E2E-" name but an "E2E-" Nr (or vice versa), which a name-only filter misses.
        var listResult = await CashCtrlApiClient.Inventory.Article.GetList();
        if (listResult.ResponseData?.Data is { Length: > 0 } items)
        {
            var orphanIds = items
                .Where(a => a.Name.StartsWith("E2E-", StringComparison.Ordinal)
                         || a.Nr.StartsWith("E2E-", StringComparison.Ordinal))
                .Select(a => a.Id)
                .ToImmutableArray();

            if (orphanIds.Length > 0)
                await CashCtrlApiClient.Inventory.Article.Delete(new() { Ids = orphanIds });
        }

        // Upload a minimal CSV file with a unique Nr/Name so Execute can't collide with orphans from
        // prior runs whose name was never populated (broken mapping) and therefore slipped past scavenge.
        var csvContent = $"Nr;Name\n{_testId};{_testId}";
        _fileId = await UploadTestFile("e2e-import-test.csv", Encoding.UTF8.GetBytes(csvContent), "text/csv");
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Create a new inventory import with the uploaded file ID
    /// </summary>
    [Test, Order(1)]
    public async Task Create_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Import.Create(new()
        {
            FileId = _fileId
        });

        _importId = AssertCreated(res);
        _importId.ShouldBeGreaterThan(0);
    }

    /// <summary>
    /// Get available mapping fields for an inventory import
    /// </summary>
    [Test, Order(2)]
    public async Task GetMappingFields_Success()
    {
        var res = await CashCtrlApiClient.Inventory.Import.GetMappingFields();

        res.IsHttpSuccess.ShouldBeTrue();
    }

    /// <summary>
    /// Define the mapping for the inventory import
    /// </summary>
    [Test, Order(3)]
    public async Task Mapping_Success()
    {
        _importId.ShouldBeGreaterThan(0, "Create_Success must run before Mapping_Success");

        var res = await CashCtrlApiClient.Inventory.Import.Mapping(new()
        {
            Id = _importId,
            Mapping = "[{\"from\":\"Nr\",\"to\":\"NR\"},{\"from\":\"Name\",\"to\":\"NAME_DE\"}]"
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Get a preview of the inventory import
    /// </summary>
    [Test, Order(4)]
    public async Task Preview_Success()
    {
        _importId.ShouldBeGreaterThan(0, "Create_Success must run before Preview_Success");

        var res = await CashCtrlApiClient.Inventory.Import.Preview(new()
        {
            Id = _importId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Execute the inventory import
    /// </summary>
    [Test, Order(5)]
    public async Task Execute_Success()
    {
        _importId.ShouldBeGreaterThan(0, "Create_Success must run before Execute_Success");

        var res = await CashCtrlApiClient.Inventory.Import.Execute(new()
        {
            Id = _importId,
            Async = true
        });
        AssertSuccess(res);
    }
}
