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
using CashCtrlApiNet.Abstractions.Enums.Salary;
using CashCtrlApiNet.Abstractions.Models.Salary.BookEntry;
using CashCtrlApiNet.Abstractions.Models.Salary.Category;
using CashCtrlApiNet.Abstractions.Models.Salary.Certificate;
using CashCtrlApiNet.Abstractions.Models.Salary.CertificateDocument;
using CashCtrlApiNet.Abstractions.Models.Salary.CertificateTemplate;
using CashCtrlApiNet.Abstractions.Models.Salary.Document;
using CashCtrlApiNet.Abstractions.Models.Salary.Field;
using CashCtrlApiNet.Abstractions.Models.Salary.InsuranceType;
using CashCtrlApiNet.Abstractions.Models.Salary.Layout;
using CashCtrlApiNet.Abstractions.Models.Salary.Payment;
using CashCtrlApiNet.Abstractions.Models.Salary.Setting;
using CashCtrlApiNet.Abstractions.Models.Salary.Statement;
using CashCtrlApiNet.Abstractions.Models.Salary.Status;
using CashCtrlApiNet.Abstractions.Models.Salary.Sum;
using CashCtrlApiNet.Abstractions.Models.Salary.Template;
using CashCtrlApiNet.Abstractions.Models.Salary.Type;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Salary domain models
/// </summary>
public static class SalaryFakers
{
    // ── BookEntry ──────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryBookEntry"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryBookEntry> BookEntry = new Faker<SalaryBookEntry>()
        .CustomInstantiator(f => new SalaryBookEntry
        {
            Id = f.Random.Int(1, 9999),
            CreditId = f.Random.Int(1, 999),
            DebitId = f.Random.Int(1, 999),
            StatementIds = f.Random.Int(1, 9999).ToString(),
            Amount = f.Random.Decimal(1, 10000),
            Description = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryBookEntryCreate"/>
    /// </summary>
    public static readonly Faker<SalaryBookEntryCreate> BookEntryCreate = new Faker<SalaryBookEntryCreate>()
        .CustomInstantiator(f => new SalaryBookEntryCreate
        {
            CreditId = f.Random.Int(1, 999),
            DebitId = f.Random.Int(1, 999),
            StatementIds = f.Random.Int(1, 9999).ToString(),
            Amount = f.Random.Decimal(1, 10000),
            Description = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="SalaryBookEntryUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryBookEntryUpdate> BookEntryUpdate = new Faker<SalaryBookEntryUpdate>()
        .CustomInstantiator(f => new SalaryBookEntryUpdate
        {
            Id = f.Random.Int(1, 9999),
            CreditId = f.Random.Int(1, 999),
            DebitId = f.Random.Int(1, 999),
            StatementIds = f.Random.Int(1, 9999).ToString(),
            Amount = f.Random.Decimal(1, 10000),
            Description = f.Lorem.Sentence(3)
        });

    // ── Category ───────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryCategory"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryCategory> Category = new Faker<SalaryCategory>()
        .CustomInstantiator(f => new SalaryCategory
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryCategoryCreate"/>
    /// </summary>
    public static readonly Faker<SalaryCategoryCreate> CategoryCreate = new Faker<SalaryCategoryCreate>()
        .CustomInstantiator(f => new SalaryCategoryCreate
        {
            Name = f.Commerce.Categories(1)[0],
            Number = f.Random.Int(100, 999).ToString()
        });

    /// <summary>
    /// Faker for <see cref="SalaryCategoryUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryCategoryUpdate> CategoryUpdate = new Faker<SalaryCategoryUpdate>()
        .CustomInstantiator(f => new SalaryCategoryUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            Number = f.Random.Int(100, 999).ToString()
        });

    // ── Certificate ────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryCertificate"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryCertificate> Certificate = new Faker<SalaryCertificate>()
        .CustomInstantiator(f => new SalaryCertificate
        {
            Id = f.Random.Int(1, 9999),
            Notes = f.Lorem.Sentence(5),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryCertificateUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryCertificateUpdate> CertificateUpdate = new Faker<SalaryCertificateUpdate>()
        .CustomInstantiator(f => new SalaryCertificateUpdate
        {
            Id = f.Random.Int(1, 9999),
            Notes = f.Lorem.Sentence(5)
        });

    // ── CertificateDocument ────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryCertificateDocument"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryCertificateDocument> CertificateDocument = new Faker<SalaryCertificateDocument>()
        .CustomInstantiator(f => new SalaryCertificateDocument
        {
            Id = f.Random.Int(1, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    // ── CertificateTemplate ────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryCertificateTemplate"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryCertificateTemplate> CertificateTemplate = new Faker<SalaryCertificateTemplate>()
        .CustomInstantiator(f => new SalaryCertificateTemplate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryCertificateTemplateCreate"/>
    /// </summary>
    public static readonly Faker<SalaryCertificateTemplateCreate> CertificateTemplateCreate = new Faker<SalaryCertificateTemplateCreate>()
        .CustomInstantiator(f => new SalaryCertificateTemplateCreate
        {
            Name = f.Lorem.Word()
        });

    /// <summary>
    /// Faker for <see cref="SalaryCertificateTemplateUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryCertificateTemplateUpdate> CertificateTemplateUpdate = new Faker<SalaryCertificateTemplateUpdate>()
        .CustomInstantiator(f => new SalaryCertificateTemplateUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word()
        });

    // ── Document ───────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryDocument"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryDocument> Document = new Faker<SalaryDocument>()
        .CustomInstantiator(f => new SalaryDocument
        {
            Id = f.Random.Int(1, 9999),
            Header = f.Lorem.Sentence(3),
            Footer = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryDocumentUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryDocumentUpdate> DocumentUpdate = new Faker<SalaryDocumentUpdate>()
        .CustomInstantiator(f => new SalaryDocumentUpdate
        {
            Id = f.Random.Int(1, 9999),
            Header = f.Lorem.Sentence(3),
            Footer = f.Lorem.Sentence(3)
        });

    // ── Field ──────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryField"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryField> Field = new Faker<SalaryField>()
        .CustomInstantiator(f => new SalaryField
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    // ── InsuranceType ──────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryInsuranceType"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryInsuranceType> InsuranceType = new Faker<SalaryInsuranceType>()
        .CustomInstantiator(f => new SalaryInsuranceType
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Company.CompanyName(),
            Description = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryInsuranceTypeCreate"/>
    /// </summary>
    public static readonly Faker<SalaryInsuranceTypeCreate> InsuranceTypeCreate = new Faker<SalaryInsuranceTypeCreate>()
        .CustomInstantiator(f => new SalaryInsuranceTypeCreate
        {
            Name = f.Company.CompanyName(),
            Description = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="SalaryInsuranceTypeUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryInsuranceTypeUpdate> InsuranceTypeUpdate = new Faker<SalaryInsuranceTypeUpdate>()
        .CustomInstantiator(f => new SalaryInsuranceTypeUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Company.CompanyName(),
            Description = f.Lorem.Sentence(3)
        });

    // ── Layout ─────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryLayout"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryLayout> Layout = new Faker<SalaryLayout>()
        .CustomInstantiator(f => new SalaryLayout
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryLayoutCreate"/>
    /// </summary>
    public static readonly Faker<SalaryLayoutCreate> LayoutCreate = new Faker<SalaryLayoutCreate>()
        .CustomInstantiator(f => new SalaryLayoutCreate
        {
            Name = f.Lorem.Word()
        });

    /// <summary>
    /// Faker for <see cref="SalaryLayoutUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryLayoutUpdate> LayoutUpdate = new Faker<SalaryLayoutUpdate>()
        .CustomInstantiator(f => new SalaryLayoutUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word()
        });

    // ── Payment ────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryPaymentCreate"/>
    /// </summary>
    public static readonly Faker<SalaryPaymentCreate> PaymentCreate = new Faker<SalaryPaymentCreate>()
        .CustomInstantiator(f => new SalaryPaymentCreate
        {
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            StatementIds = f.Random.Int(1, 9999).ToString()
        });

    // ── Setting ────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalarySetting"/> (detail response)
    /// </summary>
    public static readonly Faker<SalarySetting> Setting = new Faker<SalarySetting>()
        .CustomInstantiator(f => new SalarySetting
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            Type = f.PickRandom<SalarySettingType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalarySettingCreate"/>
    /// </summary>
    public static readonly Faker<SalarySettingCreate> SettingCreate = new Faker<SalarySettingCreate>()
        .CustomInstantiator(f => new SalarySettingCreate
        {
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            Type = f.PickRandom<SalarySettingType>()
        });

    /// <summary>
    /// Faker for <see cref="SalarySettingUpdate"/>
    /// </summary>
    public static readonly Faker<SalarySettingUpdate> SettingUpdate = new Faker<SalarySettingUpdate>()
        .CustomInstantiator(f => new SalarySettingUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            Type = f.PickRandom<SalarySettingType>()
        });

    // ── Statement ──────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryStatement"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryStatement> Statement = new Faker<SalaryStatement>()
        .CustomInstantiator(f => new SalaryStatement
        {
            Id = f.Random.Int(1, 9999),
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            DatePayment = f.Date.Recent().ToString("yyyy-MM-dd"),
            PersonId = f.Random.Int(1, 999),
            StatusId = f.Random.Int(1, 10),
            TemplateId = f.Random.Int(1, 100),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryStatementCreate"/>
    /// </summary>
    public static readonly Faker<SalaryStatementCreate> StatementCreate = new Faker<SalaryStatementCreate>()
        .CustomInstantiator(f => new SalaryStatementCreate
        {
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            DatePayment = f.Date.Recent().ToString("yyyy-MM-dd"),
            PersonId = f.Random.Int(1, 999),
            StatusId = f.Random.Int(1, 10),
            TemplateId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="SalaryStatementUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryStatementUpdate> StatementUpdate = new Faker<SalaryStatementUpdate>()
        .CustomInstantiator(f => new SalaryStatementUpdate
        {
            Id = f.Random.Int(1, 9999),
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            DatePayment = f.Date.Recent().ToString("yyyy-MM-dd"),
            PersonId = f.Random.Int(1, 999),
            StatusId = f.Random.Int(1, 10),
            TemplateId = f.Random.Int(1, 100)
        });

    // ── Status ─────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryStatus"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryStatus> Status = new Faker<SalaryStatus>()
        .CustomInstantiator(f => new SalaryStatus
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            Icon = f.PickRandom<SalaryStatusIcon>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryStatusCreate"/>
    /// </summary>
    public static readonly Faker<SalaryStatusCreate> StatusCreate = new Faker<SalaryStatusCreate>()
        .CustomInstantiator(f => new SalaryStatusCreate
        {
            Name = f.Lorem.Word(),
            Icon = f.PickRandom<SalaryStatusIcon>()
        });

    /// <summary>
    /// Faker for <see cref="SalaryStatusUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryStatusUpdate> StatusUpdate = new Faker<SalaryStatusUpdate>()
        .CustomInstantiator(f => new SalaryStatusUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            Icon = f.PickRandom<SalaryStatusIcon>()
        });

    // ── Sum ────────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalarySum"/> (detail response)
    /// </summary>
    public static readonly Faker<SalarySum> Sum = new Faker<SalarySum>()
        .CustomInstantiator(f => new SalarySum
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalarySumCreate"/>
    /// </summary>
    public static readonly Faker<SalarySumCreate> SumCreate = new Faker<SalarySumCreate>()
        .CustomInstantiator(f => new SalarySumCreate
        {
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            Number = f.Random.Int(100, 999).ToString()
        });

    /// <summary>
    /// Faker for <see cref="SalarySumUpdate"/>
    /// </summary>
    public static readonly Faker<SalarySumUpdate> SumUpdate = new Faker<SalarySumUpdate>()
        .CustomInstantiator(f => new SalarySumUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            VariableName = f.Lorem.Word(),
            Number = f.Random.Int(100, 999).ToString()
        });

    // ── Template ───────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryTemplate"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryTemplate> Template = new Faker<SalaryTemplate>()
        .CustomInstantiator(f => new SalaryTemplate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryTemplateCreate"/>
    /// </summary>
    public static readonly Faker<SalaryTemplateCreate> TemplateCreate = new Faker<SalaryTemplateCreate>()
        .CustomInstantiator(f => new SalaryTemplateCreate
        {
            Name = f.Lorem.Word()
        });

    /// <summary>
    /// Faker for <see cref="SalaryTemplateUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryTemplateUpdate> TemplateUpdate = new Faker<SalaryTemplateUpdate>()
        .CustomInstantiator(f => new SalaryTemplateUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word()
        });

    // ── Type ───────────────────────────────────────────────────────────

    /// <summary>
    /// Faker for <see cref="SalaryType"/> (detail response)
    /// </summary>
    public static readonly Faker<SalaryType> Type = new Faker<SalaryType>()
        .CustomInstantiator(f => new SalaryType
        {
            Id = f.Random.Int(1, 9999),
            CategoryId = f.Random.Int(1, 100),
            Name = f.Lorem.Word(),
            Number = f.Random.Int(100, 999).ToString(),
            Type = f.PickRandom<SalaryTypeKind>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="SalaryTypeCreate"/>
    /// </summary>
    public static readonly Faker<SalaryTypeCreate> TypeCreate = new Faker<SalaryTypeCreate>()
        .CustomInstantiator(f => new SalaryTypeCreate
        {
            CategoryId = f.Random.Int(1, 100),
            Name = f.Lorem.Word(),
            Number = f.Random.Int(100, 999).ToString(),
            Type = f.PickRandom<SalaryTypeKind>()
        });

    /// <summary>
    /// Faker for <see cref="SalaryTypeUpdate"/>
    /// </summary>
    public static readonly Faker<SalaryTypeUpdate> TypeUpdate = new Faker<SalaryTypeUpdate>()
        .CustomInstantiator(f => new SalaryTypeUpdate
        {
            Id = f.Random.Int(1, 9999),
            CategoryId = f.Random.Int(1, 100),
            Name = f.Lorem.Word(),
            Number = f.Random.Int(100, 999).ToString(),
            Type = f.PickRandom<SalaryTypeKind>()
        });
}
