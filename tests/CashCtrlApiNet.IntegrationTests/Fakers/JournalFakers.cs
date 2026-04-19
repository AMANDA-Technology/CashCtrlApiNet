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
using CashCtrlApiNet.Abstractions.Models.Journal.Import;
using CashCtrlApiNet.Abstractions.Models.Journal.Import.Entry;
using JournalModel = CashCtrlApiNet.Abstractions.Models.Journal.Journal;
using JournalListedModel = CashCtrlApiNet.Abstractions.Models.Journal.JournalListed;
using JournalCreateModel = CashCtrlApiNet.Abstractions.Models.Journal.JournalCreate;
using JournalUpdateModel = CashCtrlApiNet.Abstractions.Models.Journal.JournalUpdate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Journal domain models
/// </summary>
public static class JournalFakers
{
    /// <summary>
    /// Faker for <see cref="JournalModel"/> (detail response)
    /// </summary>
    public static readonly Faker<JournalModel> Journal = new Faker<JournalModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            DateAdded = f.Date.Recent().ToString("yyyy-MM-dd"),
            Title = f.Lorem.Sentence(3),
            SequenceNumberId = f.Random.Int(1, 100),
            DebitId = f.Random.Int(1, 999),
            CreditId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="JournalListedModel"/> (list response)
    /// </summary>
    public static readonly Faker<JournalListedModel> JournalListed = new Faker<JournalListedModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            DateAdded = f.Date.Recent().ToString("yyyy-MM-dd"),
            Title = f.Lorem.Sentence(3),
            SequenceNumberId = f.Random.Int(1, 100),
            DebitId = f.Random.Int(1, 999),
            CreditId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="JournalCreateModel"/>
    /// </summary>
    public static readonly Faker<JournalCreateModel> JournalCreate = new Faker<JournalCreateModel>()
        .CustomInstantiator(f => new()
        {
            DateAdded = f.Date.Recent().ToString("yyyy-MM-dd"),
            Title = f.Lorem.Sentence(3),
            SequenceNumberId = f.Random.Int(1, 100),
            DebitId = f.Random.Int(1, 999),
            CreditId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000)
        });

    /// <summary>
    /// Faker for <see cref="JournalUpdateModel"/>
    /// </summary>
    public static readonly Faker<JournalUpdateModel> JournalUpdate = new Faker<JournalUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            DateAdded = f.Date.Recent().ToString("yyyy-MM-dd"),
            Title = f.Lorem.Sentence(3),
            SequenceNumberId = f.Random.Int(1, 100),
            DebitId = f.Random.Int(1, 999),
            CreditId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000)
        });

    /// <summary>
    /// Faker for <see cref="JournalImport"/> (read/list response)
    /// </summary>
    public static readonly Faker<JournalImport> Import = new Faker<JournalImport>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            FileId = f.Random.Int(1, 9999),
            Description = f.Lorem.Word(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="JournalImportCreate"/>
    /// </summary>
    public static readonly Faker<JournalImportCreate> ImportCreate = new Faker<JournalImportCreate>()
        .CustomInstantiator(f => new()
        {
            FileId = f.Random.Int(1, 9999),
            Mappings = """[{"columnAmount":"Amount","columnDate":"Date"}]"""
        });

    /// <summary>
    /// Faker for <see cref="JournalImportEntry"/> (detail response)
    /// </summary>
    public static readonly Faker<JournalImportEntry> ImportEntry = new Faker<JournalImportEntry>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="JournalImportEntryListed"/> (list response)
    /// </summary>
    public static readonly Faker<JournalImportEntryListed> ImportEntryListed = new Faker<JournalImportEntryListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="JournalImportEntryUpdate"/>
    /// </summary>
    public static readonly Faker<JournalImportEntryUpdate> ImportEntryUpdate = new Faker<JournalImportEntryUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999)
        });
}
