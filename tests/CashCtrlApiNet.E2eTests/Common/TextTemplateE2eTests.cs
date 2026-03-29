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

using CashCtrlApiNet.Abstractions.Models.Common.TextTemplate;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common text template service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.ITextTemplateService"/> operations.
/// </summary>
[Category("E2e")]
public class TextTemplateE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupTemplateId;
    private int _createdTemplateId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test text template for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan text templates from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.TextTemplate.GetList(),
            t => t.Name,
            t => t.Id,
            ids => CashCtrlApiClient.Common.TextTemplate.Delete(ids));

        // Create primary test text template
        var createResult = await CashCtrlApiClient.Common.TextTemplate.Create(new()
        {
            Name = _testId,
            Text = "<p>E2E test template content</p>"
        });
        _setupTemplateId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.TextTemplate.Delete(new() { Ids = [_setupTemplateId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a text template by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.TextTemplate.Get(new() { Id = _setupTemplateId });
        var template = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        template.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of text templates successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.TextTemplate.GetList();
        var templates = AssertSuccess(res);

        templates.Length.ShouldBe(res.ResponseData!.Total);
        templates.ShouldContain(t => t.Id == _setupTemplateId);
    }

    /// <summary>
    /// Create a text template successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.TextTemplate.Create(new()
        {
            Name = secondTestId,
            Text = "<p>Second E2E test template</p>"
        });

        _createdTemplateId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.TextTemplate.Delete(new() { Ids = [_createdTemplateId] }));
    }

    /// <summary>
    /// Update a text template successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.TextTemplate.Get(new() { Id = _setupTemplateId });
        var template = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get text template for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Common.TextTemplate.Update((template as TextTemplateUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.TextTemplate.Get(new() { Id = _setupTemplateId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Delete the text template created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdTemplateId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.TextTemplate.Delete(new() { Ids = [_createdTemplateId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
