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
    private int _fileId;
    private int _importId;

    /// <summary>
    /// Uploads a CSV file via the File service (Prepare + Persist) for use in the import workflow
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Scavenge orphan articles that may have been created by previous import runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Inventory.Article.GetList(),
            a => a.Name,
            a => a.Id,
            ids => CashCtrlApiClient.Inventory.Article.Delete(ids));

        // Prepare a minimal CSV file for import
        var csvContent = "Nr;Name\nE2E-IMPORT-001;E2E-ImportTestArticle";
        var csvBytes = Encoding.UTF8.GetBytes(csvContent);

        using var content = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(csvBytes);
        fileContent.Headers.ContentType = new("text/csv");
        content.Add(fileContent, "file", "e2e-import-test.csv");

        var prepareResult = await CashCtrlApiClient.File.File.Prepare(content);
        _fileId = AssertCreated(prepareResult);

        RegisterCleanup(async () => await CashCtrlApiClient.File.File.Delete(new() { Ids = [_fileId] }));
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
            Mapping = "[{\"column\":\"Nr\",\"field\":\"nr\"},{\"column\":\"Name\",\"field\":\"name\"}]"
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
            Id = _importId
        });
        AssertSuccess(res);
    }
}
