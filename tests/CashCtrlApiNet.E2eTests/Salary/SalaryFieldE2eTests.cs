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

namespace CashCtrlApiNet.E2eTests.Salary;

/// <summary>
/// E2E tests for salary field service (read-only).
/// Covers all <see cref="CashCtrlApiNet.Interfaces.Connectors.Salary.ISalaryFieldService"/> operations.
/// </summary>
[Category("E2e")]
// ReSharper disable once InconsistentNaming
public class SalaryFieldE2eTests : CashCtrlE2eTestBase
{
    private int _salaryTypeId;
    private int _fieldId;

    /// <summary>
    /// Discovers a salary type and its fields for read-only testing
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Discover a salary type for field listing
        var typeResult = await CashCtrlApiClient.Salary.Type.GetList();
        if (typeResult.ResponseData?.Data is not { Length: > 0 } types)
        {
            Assert.Ignore("No salary types available for field testing");
            return;
        }

        _salaryTypeId = types.First().Id;

        // Discover fields for this type
        var fieldResult = await CashCtrlApiClient.Salary.Field.GetList(new() { Id = _salaryTypeId });
        if (fieldResult.ResponseData?.Data is not { Length: > 0 } fields)
        {
            Assert.Ignore("No salary fields available for the discovered salary type");
            return;
        }

        _fieldId = fields.First().Id;
    }

    /// <summary>
    /// Get a salary field by ID successfully
    /// </summary>
    [Test, Order(1)]
    public async Task Get_Success()
    {
        var res = await CashCtrlApiClient.Salary.Field.Get(new() { Id = _fieldId });
        var field = AssertSuccess(res);

        res.RequestsLeft.ShouldNotBeNull();
        res.RequestsLeft.Value.ShouldBeGreaterThan(0);
        res.CashCtrlHttpStatusCodeDescription.ShouldNotBeNullOrEmpty();

        field.Id.ShouldBeGreaterThan(0);
    }

    /// <summary>
    /// Get list of salary fields for a type successfully
    /// </summary>
    [Test, Order(2)]
    public async Task GetList_Success()
    {
        var res = await CashCtrlApiClient.Salary.Field.GetList(new() { Id = _salaryTypeId });
        var fields = AssertSuccess(res);

        fields.Length.ShouldBe(res.ResponseData!.Total);
        fields.ShouldContain(f => f.Id == _fieldId);
    }
}
