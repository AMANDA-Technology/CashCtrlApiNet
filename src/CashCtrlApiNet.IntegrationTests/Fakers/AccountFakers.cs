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
using CashCtrlApiNet.Abstractions.Enums.Account;
using CashCtrlApiNet.Abstractions.Models.Account.Bank;
using CashCtrlApiNet.Abstractions.Models.Account.Category;
using CashCtrlApiNet.Abstractions.Models.Account.CostCenter;
using CashCtrlApiNet.Abstractions.Models.Account.CostCenterCategory;
using AccountModel = CashCtrlApiNet.Abstractions.Models.Account.Account;
using AccountListedModel = CashCtrlApiNet.Abstractions.Models.Account.AccountListed;
using AccountCreateModel = CashCtrlApiNet.Abstractions.Models.Account.AccountCreate;
using AccountUpdateModel = CashCtrlApiNet.Abstractions.Models.Account.AccountUpdate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Account domain models
/// </summary>
public static class AccountFakers
{
    /// <summary>
    /// Faker for <see cref="AccountModel"/> (detail response)
    /// </summary>
    public static readonly Faker<AccountModel> Account = new Faker<AccountModel>()
        .CustomInstantiator(f => new AccountModel
        {
            Id = f.Random.Int(1, 9999),
            CategoryId = f.Random.Int(1, 100),
            Name = f.Finance.AccountName(),
            Number = f.Random.Int(1000, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="AccountListedModel"/> (list response)
    /// </summary>
    public static readonly Faker<AccountListedModel> AccountListed = new Faker<AccountListedModel>()
        .CustomInstantiator(f => new AccountListedModel
        {
            Id = f.Random.Int(1, 9999),
            CategoryId = f.Random.Int(1, 100),
            Name = f.Finance.AccountName(),
            Number = f.Random.Int(1000, 9999),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="AccountCreateModel"/>
    /// </summary>
    public static readonly Faker<AccountCreateModel> AccountCreate = new Faker<AccountCreateModel>()
        .CustomInstantiator(f => new AccountCreateModel
        {
            CategoryId = f.Random.Int(1, 100),
            Name = f.Finance.AccountName(),
            Number = f.Random.Int(1000, 9999)
        });

    /// <summary>
    /// Faker for <see cref="AccountUpdateModel"/>
    /// </summary>
    public static readonly Faker<AccountUpdateModel> AccountUpdate = new Faker<AccountUpdateModel>()
        .CustomInstantiator(f => new AccountUpdateModel
        {
            Id = f.Random.Int(1, 9999),
            CategoryId = f.Random.Int(1, 100),
            Name = f.Finance.AccountName(),
            Number = f.Random.Int(1000, 9999)
        });

    /// <summary>
    /// Faker for <see cref="AccountBank"/> (detail response)
    /// </summary>
    public static readonly Faker<AccountBank> AccountBank = new Faker<AccountBank>()
        .CustomInstantiator(f => new AccountBank
        {
            Id = f.Random.Int(1, 9999),
            Bic = f.Finance.Bic(),
            Iban = f.Finance.Iban(),
            Name = f.Company.CompanyName() + " Bank",
            Type = f.PickRandom<BankAccountType>(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="AccountBankCreate"/>
    /// </summary>
    public static readonly Faker<AccountBankCreate> AccountBankCreate = new Faker<AccountBankCreate>()
        .CustomInstantiator(f => new AccountBankCreate
        {
            Bic = f.Finance.Bic(),
            Iban = f.Finance.Iban(),
            Name = f.Company.CompanyName() + " Bank",
            Type = f.PickRandom<BankAccountType>()
        });

    /// <summary>
    /// Faker for <see cref="AccountBankUpdate"/>
    /// </summary>
    public static readonly Faker<AccountBankUpdate> AccountBankUpdate = new Faker<AccountBankUpdate>()
        .CustomInstantiator(f => new AccountBankUpdate
        {
            Id = f.Random.Int(1, 9999),
            Bic = f.Finance.Bic(),
            Iban = f.Finance.Iban(),
            Name = f.Company.CompanyName() + " Bank",
            Type = f.PickRandom<BankAccountType>()
        });

    /// <summary>
    /// Faker for <see cref="AccountCategory"/> (detail response)
    /// </summary>
    public static readonly Faker<AccountCategory> AccountCategory = new Faker<AccountCategory>()
        .CustomInstantiator(f => new AccountCategory
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Path = "/" + f.Commerce.Categories(1)[0],
            FullName = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="AccountCategoryCreate"/>
    /// </summary>
    public static readonly Faker<AccountCategoryCreate> AccountCategoryCreate = new Faker<AccountCategoryCreate>()
        .CustomInstantiator(f => new AccountCategoryCreate
        {
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="AccountCategoryUpdate"/>
    /// </summary>
    public static readonly Faker<AccountCategoryUpdate> AccountCategoryUpdate = new Faker<AccountCategoryUpdate>()
        .CustomInstantiator(f => new AccountCategoryUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="CostCenter"/> (detail response)
    /// </summary>
    public static readonly Faker<CostCenter> CostCenter = new Faker<CostCenter>()
        .CustomInstantiator(f => new CostCenter
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CostCenterListed"/> (list response)
    /// </summary>
    public static readonly Faker<CostCenterListed> CostCenterListed = new Faker<CostCenterListed>()
        .CustomInstantiator(f => new CostCenterListed
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department(),
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="CostCenterCreate"/>
    /// </summary>
    public static readonly Faker<CostCenterCreate> CostCenterCreate = new Faker<CostCenterCreate>()
        .CustomInstantiator(f => new CostCenterCreate
        {
            Name = f.Commerce.Department()
        });

    /// <summary>
    /// Faker for <see cref="CostCenterUpdate"/>
    /// </summary>
    public static readonly Faker<CostCenterUpdate> CostCenterUpdate = new Faker<CostCenterUpdate>()
        .CustomInstantiator(f => new CostCenterUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department()
        });

    /// <summary>
    /// Faker for <see cref="CostCenterCategory"/> (detail response)
    /// </summary>
    public static readonly Faker<CostCenterCategory> CostCenterCategory = new Faker<CostCenterCategory>()
        .CustomInstantiator(f => new CostCenterCategory
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department() + " Category",
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Path = "/" + f.Commerce.Department(),
            FullName = f.Commerce.Department() + " Category"
        });

    /// <summary>
    /// Faker for <see cref="CostCenterCategoryCreate"/>
    /// </summary>
    public static readonly Faker<CostCenterCategoryCreate> CostCenterCategoryCreate = new Faker<CostCenterCategoryCreate>()
        .CustomInstantiator(f => new CostCenterCategoryCreate
        {
            Name = f.Commerce.Department() + " Category"
        });

    /// <summary>
    /// Faker for <see cref="CostCenterCategoryUpdate"/>
    /// </summary>
    public static readonly Faker<CostCenterCategoryUpdate> CostCenterCategoryUpdate = new Faker<CostCenterCategoryUpdate>()
        .CustomInstantiator(f => new CostCenterCategoryUpdate
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Department() + " Category"
        });
}
