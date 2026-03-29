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

using Bogus;
using ReportModel = CashCtrlApiNet.Abstractions.Models.Report.Report;
using ReportElementModel = CashCtrlApiNet.Abstractions.Models.Report.Element.ReportElement;
using ReportElementCreateModel = CashCtrlApiNet.Abstractions.Models.Report.Element.ReportElementCreate;
using ReportElementUpdateModel = CashCtrlApiNet.Abstractions.Models.Report.Element.ReportElementUpdate;
using ReportSetModel = CashCtrlApiNet.Abstractions.Models.Report.Set.ReportSet;
using ReportSetCreateModel = CashCtrlApiNet.Abstractions.Models.Report.Set.ReportSetCreate;
using ReportSetUpdateModel = CashCtrlApiNet.Abstractions.Models.Report.Set.ReportSetUpdate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Report domain models
/// </summary>
public static class ReportFakers
{
    /// <summary>
    /// Faker for <see cref="ReportModel"/> (tree node)
    /// </summary>
    public static readonly Faker<ReportModel> Report = new Faker<ReportModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999).ToString(),
            Name = f.Commerce.Department(),
            Type = f.PickRandom("BALANCE_SHEET", "INCOME_STATEMENT", "CASH_FLOW"),
            ParentId = f.Random.Int(1, 100).ToString()
        });

    /// <summary>
    /// Faker for <see cref="ReportElementModel"/> (detail response)
    /// </summary>
    public static readonly Faker<ReportElementModel> ReportElement = new Faker<ReportElementModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            ReportId = f.Random.Int(1, 100),
            AccountId = f.Random.Int(1, 100),
            CostCenterCategoryId = f.Random.Int(1, 100),
            CurrencyId = f.Random.Int(1, 10),
            IncludeTotal = f.Random.Bool(),
            NegateAmount = f.Random.Bool(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="ReportElementCreateModel"/>
    /// </summary>
    public static readonly Faker<ReportElementCreateModel> ReportElementCreate = new Faker<ReportElementCreateModel>()
        .CustomInstantiator(f => new()
        {
            ReportId = f.Random.Int(1, 100),
            AccountId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="ReportElementUpdateModel"/>
    /// </summary>
    public static readonly Faker<ReportElementUpdateModel> ReportElementUpdate = new Faker<ReportElementUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            ReportId = f.Random.Int(1, 100),
            AccountId = f.Random.Int(1, 100),
            IncludeTotal = f.Random.Bool(),
            NegateAmount = f.Random.Bool()
        });

    /// <summary>
    /// Faker for <see cref="ReportSetModel"/> (detail response)
    /// </summary>
    public static readonly Faker<ReportSetModel> ReportSet = new Faker<ReportSetModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Text = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="ReportSetCreateModel"/>
    /// </summary>
    public static readonly Faker<ReportSetCreateModel> ReportSetCreate = new Faker<ReportSetCreateModel>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.Department()
        });

    /// <summary>
    /// Faker for <see cref="ReportSetUpdateModel"/>
    /// </summary>
    public static readonly Faker<ReportSetUpdateModel> ReportSetUpdate = new Faker<ReportSetUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Text = f.Lorem.Sentence(3)
        });
}
