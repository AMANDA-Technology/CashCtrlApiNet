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

using CashCtrlApiNet.Abstractions.Models.Common.SequenceNumber;
using Shouldly;

namespace CashCtrlApiNet.E2eTests.Common;

/// <summary>
/// E2E tests for common sequence number service with full lifecycle management.
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Common.ISequenceNumberService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class SequenceNumberE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _setupSequenceNumberId;
    private int _createdSequenceNumberId;
    private Action _cancelCreatedCleanup = null!;

    /// <summary>
    /// Scavenges orphan test data and creates the primary test sequence number for the fixture
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan sequence numbers from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Common.SequenceNumber.GetList(),
            s => s.Name,
            s => s.Id,
            ids => CashCtrlApiClient.Common.SequenceNumber.Delete(ids));

        // Create primary test sequence number
        var createResult = await CashCtrlApiClient.Common.SequenceNumber.Create(new()
        {
            Name = _testId,
            Pattern = "E2E-{0000}"
        });
        _setupSequenceNumberId = AssertCreated(createResult);

        RegisterCleanup(async () => await CashCtrlApiClient.Common.SequenceNumber.Delete(new() { Ids = [_setupSequenceNumberId] }));
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a sequence number by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Common.SequenceNumber.Get(new() { Id = _setupSequenceNumberId });
        var sequenceNumber = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        sequenceNumber.Name.ShouldBe(_testId);
    }

    /// <summary>
    /// Get list of sequence numbers successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Common.SequenceNumber.GetList();
        var sequenceNumbers = AssertSuccess(res);

        sequenceNumbers.Length.ShouldBe(res.ResponseData!.Total);
        sequenceNumbers.ShouldContain(s => s.Id == _setupSequenceNumberId);
    }

    /// <summary>
    /// Create a sequence number successfully and store its ID for later tests
    /// </summary>
    [Test, Order(3)]
    public async Task Create_Success()
    {
        var secondTestId = GenerateTestId();
        var res = await CashCtrlApiClient.Common.SequenceNumber.Create(new()
        {
            Name = secondTestId,
            Pattern = "E2E-{0000}"
        });

        _createdSequenceNumberId = AssertCreated(res);
        res.ResponseData!.Message.ShouldNotBeNullOrEmpty();
        _cancelCreatedCleanup = RegisterCleanup(async () => await CashCtrlApiClient.Common.SequenceNumber.Delete(new() { Ids = [_createdSequenceNumberId] }));
    }

    /// <summary>
    /// Update a sequence number successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var get = await CashCtrlApiClient.Common.SequenceNumber.Get(new() { Id = _setupSequenceNumberId });
        var sequenceNumber = get.ResponseData?.Data ?? throw new InvalidOperationException("Failed to get sequence number for update");

        var updatedName = $"{_testId}-Updated";
        var res = await CashCtrlApiClient.Common.SequenceNumber.Update((sequenceNumber as SequenceNumberUpdate) with
        {
            Name = updatedName
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Common.SequenceNumber.Get(new() { Id = _setupSequenceNumberId });
        verify.ResponseData?.Data?.Name.ShouldBe(updatedName);
    }

    /// <summary>
    /// Get generated number for a sequence number successfully
    /// </summary>
    [Test, Order(5)]
    public async Task GetGeneratedNumber_Success()
    {
        var res = await CashCtrlApiClient.Common.SequenceNumber.GetGeneratedNumber(new() { Id = _setupSequenceNumberId });
        AssertSuccess(res);
    }

    /// <summary>
    /// Delete the sequence number created in <see cref="Create_Success"/>
    /// </summary>
    [Test, Order(6)]
    public async Task Delete_Success()
    {
        _createdSequenceNumberId.ShouldBeGreaterThan(0, "Create_Success must run before Delete_Success");

        var res = await CashCtrlApiClient.Common.SequenceNumber.Delete(new() { Ids = [_createdSequenceNumberId] });
        AssertSuccess(res);

        _cancelCreatedCleanup();
    }
}
