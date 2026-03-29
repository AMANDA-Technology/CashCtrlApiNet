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

namespace CashCtrlApiNet.E2eTests.Person;

/// <summary>
/// E2E tests for person import service with full import workflow management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Person.IPersonImportService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class PersonImportE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _fileId;
    private int _importId;

    /// <summary>
    /// Scavenges orphan test data and uploads a VCF file for import tests
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan persons from previous failed import runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Person.Person.GetList(),
            p => p.LastName ?? string.Empty,
            p => p.Id,
            ids => CashCtrlApiClient.Person.Person.Delete(ids));

        // Upload a minimal VCF file for import tests
        var vcfContent = $"BEGIN:VCARD\nVERSION:3.0\nN:{_testId};E2E-First\nFN:E2E-First {_testId}\nEND:VCARD";
        using var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(vcfContent)), "file", "e2e-test.vcf");

        var prepareResult = await CashCtrlApiClient.File.File.Prepare(content);
        _fileId = AssertCreated(prepareResult);

        // Register cleanup to delete any imported persons after test
        RegisterCleanup(async () =>
        {
            var listResult = await CashCtrlApiClient.Person.Person.GetList();
            if (listResult.ResponseData?.Data is not { Length: > 0 } items)
                return;

            var importedIds = items
                .Where(p => (p.LastName ?? string.Empty).StartsWith("E2E-", StringComparison.Ordinal))
                .Select(p => p.Id)
                .ToArray();

            if (importedIds.Length > 0)
                await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [..importedIds] });
        });
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get available mapping fields for person import successfully
    /// </summary>
    [Test, Order(1)]
    public async Task GetMappingFields_Success()
    {
        var res = await CashCtrlApiClient.Person.Import.GetMappingFields();
        res.IsHttpSuccess.ShouldBeTrue();
    }

    /// <summary>
    /// Create a person import from uploaded VCF file successfully
    /// </summary>
    [Test, Order(2)]
    public async Task Create_Success()
    {
        _fileId.ShouldBeGreaterThan(0, "File upload must succeed before Create");

        var res = await CashCtrlApiClient.Person.Import.Create(new()
        {
            FileId = _fileId
        });

        _importId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
    }

    /// <summary>
    /// Define mapping for a person import successfully
    /// </summary>
    [Test, Order(3)]
    public async Task Mapping_Success()
    {
        _importId.ShouldBeGreaterThan(0, "Create must run before Mapping");

        var res = await CashCtrlApiClient.Person.Import.Mapping(new()
        {
            Id = _importId,
            Mapping = "{\"lastName\":\"lastName\",\"firstName\":\"firstName\"}"
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Get a preview of a person import successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Preview_Success()
    {
        _importId.ShouldBeGreaterThan(0, "Create must run before Preview");

        var res = await CashCtrlApiClient.Person.Import.Preview(new()
        {
            Id = _importId
        });
        AssertSuccess(res);
    }

    /// <summary>
    /// Execute a person import successfully
    /// </summary>
    [Test, Order(5)]
    public async Task Execute_Success()
    {
        _importId.ShouldBeGreaterThan(0, "Create must run before Execute");

        var res = await CashCtrlApiClient.Person.Import.Execute(new()
        {
            Id = _importId
        });
        AssertSuccess(res);
    }
}
