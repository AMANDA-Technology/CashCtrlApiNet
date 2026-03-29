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
using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.Abstractions.Models.Common.Currency;
using CashCtrlApiNet.Abstractions.Models.Common.CustomField;
using CashCtrlApiNet.Abstractions.Models.Common.CustomFieldGroup;
using CashCtrlApiNet.Abstractions.Models.Common.History;
using CashCtrlApiNet.Abstractions.Models.Common.Rounding;
using CashCtrlApiNet.Abstractions.Models.Common.SequenceNumber;
using CashCtrlApiNet.Abstractions.Models.Common.TaxRate;
using CashCtrlApiNet.Abstractions.Models.Common.TextTemplate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Common domain models
/// </summary>
public static class CommonFakers
{
    // ── Currency ────────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="Currency"/> (detail response)
    /// </summary>
    public static readonly Faker<Currency> Currency = new Faker<Currency>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Code = f.Finance.Currency().Code,
            Description = f.Finance.Currency().Description,
            IsDefault = f.Random.Bool(),
            Rate = f.Random.Double(0.5, 2.0),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CurrencyListed"/> (list response)
    /// </summary>
    public static readonly Faker<CurrencyListed> CurrencyListed = new Faker<CurrencyListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Code = f.Finance.Currency().Code,
            Description = f.Finance.Currency().Description,
            IsDefault = f.Random.Bool(),
            Rate = f.Random.Double(0.5, 2.0),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CurrencyCreate"/>
    /// </summary>
    public static readonly Faker<CurrencyCreate> CurrencyCreate = new Faker<CurrencyCreate>()
        .CustomInstantiator(f => new()
        {
            Code = f.Finance.Currency().Code
        });

    /// <summary>
    /// Faker for <see cref="CurrencyUpdate"/>
    /// </summary>
    public static readonly Faker<CurrencyUpdate> CurrencyUpdate = new Faker<CurrencyUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Code = f.Finance.Currency().Code
        });

    /// <summary>
    /// Faker for <see cref="CurrencyExchangeRate"/>
    /// </summary>
    public static readonly Faker<CurrencyExchangeRate> CurrencyExchangeRate = new Faker<CurrencyExchangeRate>()
        .CustomInstantiator(f => new()
        {
            Rate = f.Random.Double(0.5, 2.0)
        });

    // ── CustomField ─────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="CustomField"/> (detail response)
    /// </summary>
    public static readonly Faker<CustomField> CustomField = new Faker<CustomField>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            DataType = f.PickRandom<CustomFieldDataType>(),
            RowLabel = f.Lorem.Word(),
            Type = f.PickRandom<CustomFieldType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CustomFieldListed"/> (list response)
    /// </summary>
    public static readonly Faker<CustomFieldListed> CustomFieldListed = new Faker<CustomFieldListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            DataType = f.PickRandom<CustomFieldDataType>(),
            RowLabel = f.Lorem.Word(),
            Type = f.PickRandom<CustomFieldType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CustomFieldCreate"/>
    /// </summary>
    public static readonly Faker<CustomFieldCreate> CustomFieldCreate = new Faker<CustomFieldCreate>()
        .CustomInstantiator(f => new()
        {
            DataType = f.PickRandom<CustomFieldDataType>(),
            RowLabel = f.Lorem.Word(),
            Type = f.PickRandom<CustomFieldType>()
        });

    /// <summary>
    /// Faker for <see cref="CustomFieldUpdate"/>
    /// </summary>
    public static readonly Faker<CustomFieldUpdate> CustomFieldUpdate = new Faker<CustomFieldUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            DataType = f.PickRandom<CustomFieldDataType>(),
            RowLabel = f.Lorem.Word(),
            Type = f.PickRandom<CustomFieldType>()
        });

    // ── CustomFieldGroup ────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="CustomFieldGroup"/> (detail response)
    /// </summary>
    public static readonly Faker<CustomFieldGroup> CustomFieldGroup = new Faker<CustomFieldGroup>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Type = f.PickRandom<CustomFieldType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CustomFieldGroupListed"/> (list response)
    /// </summary>
    public static readonly Faker<CustomFieldGroupListed> CustomFieldGroupListed = new Faker<CustomFieldGroupListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Type = f.PickRandom<CustomFieldType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CustomFieldGroupCreate"/>
    /// </summary>
    public static readonly Faker<CustomFieldGroupCreate> CustomFieldGroupCreate = new Faker<CustomFieldGroupCreate>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.Department(),
            Type = f.PickRandom<CustomFieldType>()
        });

    /// <summary>
    /// Faker for <see cref="CustomFieldGroupUpdate"/>
    /// </summary>
    public static readonly Faker<CustomFieldGroupUpdate> CustomFieldGroupUpdate = new Faker<CustomFieldGroupUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Type = f.PickRandom<CustomFieldType>()
        });

    // ── History ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="HistoryEntry"/>
    /// </summary>
    public static readonly Faker<HistoryEntry> HistoryEntry = new Faker<HistoryEntry>()
        .CustomInstantiator(f => new()
        {
            ActionType = f.PickRandom("CREATE", "UPDATE", "DELETE"),
            UserId = f.Random.Int(1, 100),
            UserName = f.Person.UserName,
            Text = f.Lorem.Sentence()
        });

    // ── Rounding ────────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="Rounding"/> (detail response)
    /// </summary>
    public static readonly Faker<Rounding> Rounding = new Faker<Rounding>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 100),
            Name = f.Commerce.ProductName() + " Rounding",
            Mode = f.PickRandom<RoundingMode>(),
            Value = f.Random.Double(0.01),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="RoundingListed"/> (list response)
    /// </summary>
    public static readonly Faker<RoundingListed> RoundingListed = new Faker<RoundingListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 100),
            Name = f.Commerce.ProductName() + " Rounding",
            Mode = f.PickRandom<RoundingMode>(),
            Value = f.Random.Double(0.01),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="RoundingCreate"/>
    /// </summary>
    public static readonly Faker<RoundingCreate> RoundingCreate = new Faker<RoundingCreate>()
        .CustomInstantiator(f => new()
        {
            AccountId = f.Random.Int(1, 100),
            Name = f.Commerce.ProductName() + " Rounding"
        });

    /// <summary>
    /// Faker for <see cref="RoundingUpdate"/>
    /// </summary>
    public static readonly Faker<RoundingUpdate> RoundingUpdate = new Faker<RoundingUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 100),
            Name = f.Commerce.ProductName() + " Rounding"
        });

    // ── SequenceNumber ──────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SequenceNumber"/> (detail response)
    /// </summary>
    public static readonly Faker<SequenceNumber> SequenceNumber = new Faker<SequenceNumber>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName() + " Sequence",
            Pattern = "{YYYY}-{####}",
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="SequenceNumberListed"/> (list response)
    /// </summary>
    public static readonly Faker<SequenceNumberListed> SequenceNumberListed = new Faker<SequenceNumberListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName() + " Sequence",
            Pattern = "{YYYY}-{####}",
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="SequenceNumberCreate"/>
    /// </summary>
    public static readonly Faker<SequenceNumberCreate> SequenceNumberCreate = new Faker<SequenceNumberCreate>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.ProductName() + " Sequence",
            Pattern = "{YYYY}-{####}"
        });

    /// <summary>
    /// Faker for <see cref="SequenceNumberUpdate"/>
    /// </summary>
    public static readonly Faker<SequenceNumberUpdate> SequenceNumberUpdate = new Faker<SequenceNumberUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName() + " Sequence",
            Pattern = "{YYYY}-{####}"
        });

    // ── TaxRate ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="TaxRate"/> (detail response)
    /// </summary>
    public static readonly Faker<TaxRate> TaxRate = new Faker<TaxRate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Code = f.Random.AlphaNumeric(6).ToUpperInvariant(),
            Description = f.Commerce.ProductName() + " Tax",
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="TaxRateListed"/> (list response)
    /// </summary>
    public static readonly Faker<TaxRateListed> TaxRateListed = new Faker<TaxRateListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Code = f.Random.AlphaNumeric(6).ToUpperInvariant(),
            Description = f.Commerce.ProductName() + " Tax",
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="TaxRateCreate"/>
    /// </summary>
    public static readonly Faker<TaxRateCreate> TaxRateCreate = new Faker<TaxRateCreate>()
        .CustomInstantiator(f => new()
        {
            Code = f.Random.AlphaNumeric(6).ToUpperInvariant(),
            Description = f.Commerce.ProductName() + " Tax"
        });

    /// <summary>
    /// Faker for <see cref="TaxRateUpdate"/>
    /// </summary>
    public static readonly Faker<TaxRateUpdate> TaxRateUpdate = new Faker<TaxRateUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Code = f.Random.AlphaNumeric(6).ToUpperInvariant(),
            Description = f.Commerce.ProductName() + " Tax"
        });

    // ── TextTemplate ────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="TextTemplate"/> (detail response)
    /// </summary>
    public static readonly Faker<TextTemplate> TextTemplate = new Faker<TextTemplate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName() + " Template",
            Text = f.Lorem.Paragraph(),
            Type = f.PickRandom<TextTemplateType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="TextTemplateListed"/> (list response)
    /// </summary>
    public static readonly Faker<TextTemplateListed> TextTemplateListed = new Faker<TextTemplateListed>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName() + " Template",
            Text = f.Lorem.Paragraph(),
            Type = f.PickRandom<TextTemplateType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="TextTemplateCreate"/>
    /// </summary>
    public static readonly Faker<TextTemplateCreate> TextTemplateCreate = new Faker<TextTemplateCreate>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.ProductName() + " Template"
        });

    /// <summary>
    /// Faker for <see cref="TextTemplateUpdate"/>
    /// </summary>
    public static readonly Faker<TextTemplateUpdate> TextTemplateUpdate = new Faker<TextTemplateUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.ProductName() + " Template"
        });
}
