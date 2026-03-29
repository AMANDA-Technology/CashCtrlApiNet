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
using CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod;
using CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod.Task;
using CashCtrlApiNet.Abstractions.Models.Meta.Location;
using CashCtrlApiNet.Abstractions.Models.Meta.Settings;
using FiscalPeriodModel = CashCtrlApiNet.Abstractions.Models.Meta.FiscalPeriod.FiscalPeriod;
using LocationModel = CashCtrlApiNet.Abstractions.Models.Meta.Location.Location;
using SettingsModel = CashCtrlApiNet.Abstractions.Models.Meta.Settings.Settings;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Meta domain models
/// </summary>
public static class MetaFakers
{
    /// <summary>
    /// Faker for <see cref="FiscalPeriodModel"/> (detail response)
    /// </summary>
    public static readonly Faker<FiscalPeriodModel> FiscalPeriod = new Faker<FiscalPeriodModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            StartDate = f.Date.Recent().ToString("yyyy-MM-dd"),
            EndDate = f.Date.Soon().ToString("yyyy-MM-dd"),
            IsComplete = f.Random.Bool(),
            IsCurrent = f.Random.Bool(),
            Name = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="FiscalPeriodListed"/> (list response)
    /// </summary>
    public static readonly Faker<FiscalPeriodListed> FiscalPeriodListed = new Faker<FiscalPeriodListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            StartDate = f.Date.Recent().ToString("yyyy-MM-dd"),
            EndDate = f.Date.Soon().ToString("yyyy-MM-dd"),
            IsComplete = f.Random.Bool(),
            IsCurrent = f.Random.Bool(),
            Name = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="FiscalPeriodCreate"/>
    /// </summary>
    public static readonly Faker<FiscalPeriodCreate> FiscalPeriodCreate = new Faker<FiscalPeriodCreate>()
        .CustomInstantiator(f => new()
        {
            StartDate = f.Date.Recent().ToString("yyyy-MM-dd"),
            EndDate = f.Date.Soon().ToString("yyyy-MM-dd")
        });

    /// <summary>
    /// Faker for <see cref="FiscalPeriodUpdate"/>
    /// </summary>
    public static readonly Faker<FiscalPeriodUpdate> FiscalPeriodUpdate = new Faker<FiscalPeriodUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            StartDate = f.Date.Recent().ToString("yyyy-MM-dd"),
            EndDate = f.Date.Soon().ToString("yyyy-MM-dd")
        });

    /// <summary>
    /// Faker for <see cref="FiscalPeriodTask"/> (list response)
    /// </summary>
    public static readonly Faker<FiscalPeriodTask> FiscalPeriodTask = new Faker<FiscalPeriodTask>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            FiscalPeriodId = f.Random.Int(1, 9999),
            Name = f.Lorem.Sentence(3),
            IsDone = f.Random.Bool(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="FiscalPeriodTaskCreate"/>
    /// </summary>
    public static readonly Faker<FiscalPeriodTaskCreate> FiscalPeriodTaskCreate = new Faker<FiscalPeriodTaskCreate>()
        .CustomInstantiator(f => new()
        {
            FiscalPeriodId = f.Random.Int(1, 9999),
            Name = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="LocationModel"/> (detail response)
    /// </summary>
    public static readonly Faker<LocationModel> Location = new Faker<LocationModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Company.CompanyName(),
            OrgName = f.Company.CompanyName(),
            LogoFileId = f.Random.Int(1, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="LocationListed"/> (list response)
    /// </summary>
    public static readonly Faker<LocationListed> LocationListed = new Faker<LocationListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Company.CompanyName(),
            OrgName = f.Company.CompanyName(),
            LogoFileId = f.Random.Int(1, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="LocationCreate"/>
    /// </summary>
    public static readonly Faker<LocationCreate> LocationCreate = new Faker<LocationCreate>()
        .CustomInstantiator(f => new()
        {
            Name = f.Company.CompanyName(),
            OrgName = f.Company.CompanyName(),
            LogoFileId = f.Random.Int(1, 9999)
        });

    /// <summary>
    /// Faker for <see cref="LocationUpdate"/>
    /// </summary>
    public static readonly Faker<LocationUpdate> LocationUpdate = new Faker<LocationUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Company.CompanyName(),
            OrgName = f.Company.CompanyName(),
            LogoFileId = f.Random.Int(1, 9999)
        });

    /// <summary>
    /// Faker for <see cref="SettingsModel"/> (read/get response)
    /// </summary>
    public static readonly Faker<SettingsModel> Settings = new Faker<SettingsModel>()
        .CustomInstantiator(f => new()
        {
            DefaultCurrencyId = f.Random.Int(1, 999),
            DefaultOpeningAccountId = f.Random.Int(1, 999),
            DefaultTaxId = f.Random.Int(1, 999),
            DefaultInventoryAssetAccountId = f.Random.Int(1, 999),
            DefaultInventoryDepreciationAccountId = f.Random.Int(1, 999),
            CsvDelimiter = f.PickRandom(",", ";", "\t"),
            FirstDayOfWeek = f.Random.Int(0, 6),
            IsProMode = f.Random.Bool()
        });

    /// <summary>
    /// Faker for <see cref="SettingsUpdate"/>
    /// </summary>
    public static readonly Faker<SettingsUpdate> SettingsUpdate = new Faker<SettingsUpdate>()
        .CustomInstantiator(f => new()
        {
            DefaultCurrencyId = f.Random.Int(1, 999),
            DefaultOpeningAccountId = f.Random.Int(1, 999),
            DefaultTaxId = f.Random.Int(1, 999),
            CsvDelimiter = f.PickRandom(",", ";"),
            FirstDayOfWeek = f.Random.Int(0, 6),
            IsProMode = f.Random.Bool()
        });
}
