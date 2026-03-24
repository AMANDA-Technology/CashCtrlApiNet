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
using CashCtrlApiNet.Abstractions.Models.Person.Import;
using PersonModel = CashCtrlApiNet.Abstractions.Models.Person.Person;
using PersonListedModel = CashCtrlApiNet.Abstractions.Models.Person.PersonListed;
using PersonCreateModel = CashCtrlApiNet.Abstractions.Models.Person.PersonCreate;
using PersonUpdateModel = CashCtrlApiNet.Abstractions.Models.Person.PersonUpdate;
using PersonCategoryModel = CashCtrlApiNet.Abstractions.Models.Person.Category.PersonCategory;
using PersonCategoryCreateModel = CashCtrlApiNet.Abstractions.Models.Person.Category.PersonCategoryCreate;
using PersonCategoryUpdateModel = CashCtrlApiNet.Abstractions.Models.Person.Category.PersonCategoryUpdate;
using PersonTitleModel = CashCtrlApiNet.Abstractions.Models.Person.Title.PersonTitle;
using PersonTitleCreateModel = CashCtrlApiNet.Abstractions.Models.Person.Title.PersonTitleCreate;
using PersonTitleUpdateModel = CashCtrlApiNet.Abstractions.Models.Person.Title.PersonTitleUpdate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Person domain models
/// </summary>
public static class PersonFakers
{
    /// <summary>
    /// Faker for <see cref="PersonModel"/> (detail response)
    /// </summary>
    public static readonly Faker<PersonModel> Person = new Faker<PersonModel>()
        .CustomInstantiator(f => new PersonModel
        {
            Id = f.Random.Int(1, 9999),
            FirstName = f.Person.FirstName,
            LastName = f.Person.LastName,
            Company = f.Company.CompanyName(),
            Email = f.Internet.Email(),
            Phone = f.Phone.PhoneNumber(),
            CategoryId = f.Random.Int(1, 100),
            City = f.Address.City(),
            Country = f.Address.Country(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="PersonListedModel"/> (list response)
    /// </summary>
    public static readonly Faker<PersonListedModel> PersonListed = new Faker<PersonListedModel>()
        .CustomInstantiator(f => new PersonListedModel
        {
            Id = f.Random.Int(1, 9999),
            FirstName = f.Person.FirstName,
            LastName = f.Person.LastName,
            Company = f.Company.CompanyName(),
            Email = f.Internet.Email(),
            Phone = f.Phone.PhoneNumber(),
            CategoryId = f.Random.Int(1, 100),
            City = f.Address.City(),
            Country = f.Address.Country(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="PersonCreateModel"/>
    /// </summary>
    public static readonly Faker<PersonCreateModel> PersonCreate = new Faker<PersonCreateModel>()
        .CustomInstantiator(f => new PersonCreateModel
        {
            FirstName = f.Person.FirstName,
            LastName = f.Person.LastName,
            Company = f.Company.CompanyName(),
            Email = f.Internet.Email()
        });

    /// <summary>
    /// Faker for <see cref="PersonUpdateModel"/>
    /// </summary>
    public static readonly Faker<PersonUpdateModel> PersonUpdate = new Faker<PersonUpdateModel>()
        .CustomInstantiator(f => new PersonUpdateModel
        {
            Id = f.Random.Int(1, 9999),
            FirstName = f.Person.FirstName,
            LastName = f.Person.LastName,
            Company = f.Company.CompanyName(),
            Email = f.Internet.Email(),
            CategoryId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="PersonCategoryModel"/> (detail response)
    /// </summary>
    public static readonly Faker<PersonCategoryModel> PersonCategory = new Faker<PersonCategoryModel>()
        .CustomInstantiator(f => new PersonCategoryModel
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            Number = f.Random.Int(100, 999).ToString(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow,
            Path = "/" + f.Commerce.Categories(1)[0],
            FullName = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="PersonCategoryCreateModel"/>
    /// </summary>
    public static readonly Faker<PersonCategoryCreateModel> PersonCategoryCreate = new Faker<PersonCategoryCreateModel>()
        .CustomInstantiator(f => new PersonCategoryCreateModel
        {
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="PersonCategoryUpdateModel"/>
    /// </summary>
    public static readonly Faker<PersonCategoryUpdateModel> PersonCategoryUpdate = new Faker<PersonCategoryUpdateModel>()
        .CustomInstantiator(f => new PersonCategoryUpdateModel
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="PersonTitleModel"/> (detail response)
    /// </summary>
    public static readonly Faker<PersonTitleModel> PersonTitle = new Faker<PersonTitleModel>()
        .CustomInstantiator(f => new PersonTitleModel
        {
            Id = f.Random.Int(1, 9999),
            Title = f.PickRandom("Mr.", "Mrs.", "Ms.", "Dr.", "Prof."),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="PersonTitleCreateModel"/>
    /// </summary>
    public static readonly Faker<PersonTitleCreateModel> PersonTitleCreate = new Faker<PersonTitleCreateModel>()
        .CustomInstantiator(f => new PersonTitleCreateModel
        {
            Title = f.PickRandom("Mr.", "Mrs.", "Ms.", "Dr.", "Prof.")
        });

    /// <summary>
    /// Faker for <see cref="PersonTitleUpdateModel"/>
    /// </summary>
    public static readonly Faker<PersonTitleUpdateModel> PersonTitleUpdate = new Faker<PersonTitleUpdateModel>()
        .CustomInstantiator(f => new PersonTitleUpdateModel
        {
            Id = f.Random.Int(1, 9999),
            Title = f.PickRandom("Mr.", "Mrs.", "Ms.", "Dr.", "Prof.")
        });

    /// <summary>
    /// Faker for <see cref="PersonImportCreate"/>
    /// </summary>
    public static readonly Faker<PersonImportCreate> ImportCreate = new Faker<PersonImportCreate>()
        .CustomInstantiator(f => new PersonImportCreate
        {
            FileId = f.Random.Int(1, 9999)
        });

    /// <summary>
    /// Faker for <see cref="PersonImportMapping"/>
    /// </summary>
    public static readonly Faker<PersonImportMapping> ImportMapping = new Faker<PersonImportMapping>()
        .CustomInstantiator(f => new PersonImportMapping
        {
            Id = f.Random.Int(1, 9999),
            Mapping = "{\"firstName\":\"First Name\",\"lastName\":\"Last Name\"}"
        });

    /// <summary>
    /// Faker for <see cref="PersonImportPreview"/>
    /// </summary>
    public static readonly Faker<PersonImportPreview> ImportPreview = new Faker<PersonImportPreview>()
        .CustomInstantiator(f => new PersonImportPreview
        {
            Id = f.Random.Int(1, 9999)
        });

    /// <summary>
    /// Faker for <see cref="PersonImportExecute"/>
    /// </summary>
    public static readonly Faker<PersonImportExecute> ImportExecute = new Faker<PersonImportExecute>()
        .CustomInstantiator(f => new PersonImportExecute
        {
            Id = f.Random.Int(1, 9999)
        });
}
