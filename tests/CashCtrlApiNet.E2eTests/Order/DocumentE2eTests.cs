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

using Shouldly;

namespace CashCtrlApiNet.E2eTests.Order;

/// <summary>
/// E2E tests for order document service with full lifecycle management.
/// Covers <see cref="CashCtrlApiNet.Interfaces.Connectors.Order.IDocumentService"/> operations (excludes SendMail).
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class DocumentE2eTests : CashCtrlE2eTestBase
{
    private string _testId = null!;
    private int _orderId;
    private int _documentId;

    /// <summary>
    /// Creates the prerequisites for document tests: a person, an order, and discovers its document
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = GenerateTestId();

        // Scavenge orphan persons from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Person.Person.GetList(),
            p => p.FirstName ?? "",
            p => p.Id,
            ids => CashCtrlApiClient.Person.Person.Delete(ids));

        // Scavenge orphan orders from previous failed runs
        await ScavengeOrphans(
            () => CashCtrlApiClient.Order.Order.GetList(),
            o => o.Description ?? "",
            o => o.Id,
            ids => CashCtrlApiClient.Order.Order.Delete(ids));

        // Create a person as associate for the order
        var personResult = await CashCtrlApiClient.Person.Person.Create(new()
        {
            FirstName = _testId,
            LastName = "E2E"
        });
        var personId = AssertCreated(personResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Person.Person.Delete(new() { Ids = [personId] }));

        // Discover an order category to get required IDs
        var categoryResult = await CashCtrlApiClient.Order.Category.GetList();
        var category = categoryResult.ResponseData?.Data.FirstOrDefault()
                       ?? throw new InvalidOperationException("No order categories found");

        var accountId = category.AccountId ?? throw new InvalidOperationException("Order category has no AccountId");
        var sequenceNumberId = category.SequenceNumberId ?? throw new InvalidOperationException("Order category has no SequenceNumberId");

        // Create an order that should have an associated document
        var orderResult = await CashCtrlApiClient.Order.Order.Create(new()
        {
            AccountId = accountId,
            CategoryId = category.Id,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            SequenceNumberId = sequenceNumberId,
            AssociateId = personId,
            Description = _testId
        });
        _orderId = AssertCreated(orderResult);
        RegisterCleanup(async () => await CashCtrlApiClient.Order.Order.Delete(new() { Ids = [_orderId] }));

        // The document ID matches the order ID in CashCtrl
        _documentId = _orderId;
    }

    /// <summary>
    /// Cleans up all test data created during the fixture
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await RunCleanup();

    /// <summary>
    /// Get a document by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Order.Document.Get(new() { Id = _documentId });
        var document = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        document.Id.ShouldBe(_documentId);
    }

    /// <summary>
    /// Download document as PDF successfully
    /// </summary>
    [Test, Order(2)]
    public async Task DownloadPdf_Success()
    {
        var res = await CashCtrlApiClient.Order.Document.DownloadPdf(new() { Id = _documentId });
        var pdf = AssertSuccess(res);
        await DownloadFile(pdf.FileName!, pdf.Data);
    }

    /// <summary>
    /// Download document as ZIP successfully
    /// </summary>
    [Test, Order(3)]
    public async Task DownloadZip_Success()
    {
        var res = await CashCtrlApiClient.Order.Document.DownloadZip(new() { Id = _documentId });
        var zip = AssertSuccess(res);
        await DownloadFile(zip.FileName!, zip.Data);
    }

    /// <summary>
    /// Update a document successfully
    /// </summary>
    [Test, Order(4)]
    public async Task Update_Success()
    {
        var updatedText = $"{_testId}-UpdatedText";
        var res = await CashCtrlApiClient.Order.Document.Update(new()
        {
            Id = _documentId,
            Text = updatedText
        });
        AssertSuccess(res);

        // Verify the update persisted
        var verify = await CashCtrlApiClient.Order.Document.Get(new() { Id = _documentId });
        verify.ResponseData?.Data?.Text.ShouldBe(updatedText);
    }
}
