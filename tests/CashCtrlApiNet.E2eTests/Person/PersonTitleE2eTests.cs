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

using CashCtrlApiNet.Abstractions.Models.Person.Title;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Person;

/// <summary>
/// E2E tests for person title service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Person.IPersonTitleService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class PersonTitleE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupTitleId;
    private int _createdTitleId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test title for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan person titles from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Person.Title.GetList(),
            t => t.Title,
            t => t.Id,
            ids => CashCtrlApiClient.Person.Title.Delete(ids));

        // Create primary test title
        var createResult = await CashCtrlApiClient.Person.Title.Create(new()
        {
            Title = _testId
        });
        _setupTitleId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Person.Title.Delete(new() { Ids = [_setupTitleId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a person title by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Person.Title.Get(new() { Id = _setupTitleId });
        var title = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        title.Title.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of person titles successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Person.Title.GetList();
        var titles = AssertSuccess(res);

        titles.Length.ShouldBe(res.ResponseData!.Total);
        titles.ShouldContain(t => t.Id == _setupTitleId);
    }

    /// <summary>
    /// Create a person title successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Person.Title.Create(new()
        {
            Title = secondTestId
        });

        _createdTitleId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Person.Title.Delete(new() { Ids = [_createdTitleId] }));
    }

    /// <summary>
    /// Update a person title successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Person.Title.Get(new() { Id = _setupTitleId });
        var title = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get person title for update");

        var updatedTitle = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Person.Title.Update((title as PersonTitleUpdate) with
        {
            Title = updatedTitle
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Person.Title.Get(new() { Id = _setupTitleId });
        verify.ResponseData?.Data?.Title.ShouldBe(updatedTitle);
    }

    /// <summary>
    /// Delete the person title created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(5)]
    public async Task Delete_Success()
    {
        _createdTitleId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Person.Title.Delete(new() { Ids = [_createdTitleId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
