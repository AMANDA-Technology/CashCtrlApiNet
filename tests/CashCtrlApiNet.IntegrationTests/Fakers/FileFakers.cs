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
using CashCtrlApiNet.Abstractions.Models.File;
using CashCtrlApiNet.Abstractions.Models.File.Category;
using FileModel = CashCtrlApiNet.Abstractions.Models.File.File;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for File domain models
/// </summary>
public static class FileFakers
{
    /// <summary>
    /// Faker for <see cref="FileModel"/> (detail/list response)
    /// </summary>
    public static readonly Faker<FileModel> File = new Faker<FileModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.System.FileName(),
            Description = f.Lorem.Sentence(),
            CategoryId = f.Random.Int(1, 100),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName
        });

    /// <summary>
    /// Faker for <see cref="FileCreate"/>
    /// </summary>
    public static readonly Faker<FileCreate> FileCreate = new Faker<FileCreate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.System.FileName(),
            Description = f.Lorem.Sentence(),
            CategoryId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="FileUpdate"/>
    /// </summary>
    public static readonly Faker<FileUpdate> FileUpdate = new Faker<FileUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.System.FileName(),
            Description = f.Lorem.Sentence(),
            CategoryId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="FileCategory"/> (detail response)
    /// </summary>
    public static readonly Faker<FileCategory> FileCategory = new Faker<FileCategory>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Path = "/" + f.Commerce.Categories(1)[0],
            FullName = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="FileCategoryCreate"/>
    /// </summary>
    public static readonly Faker<FileCategoryCreate> FileCategoryCreate = new Faker<FileCategoryCreate>()
        .CustomInstantiator(f => new()
        {
            Name = f.Commerce.Categories(1)[0]
        });

    /// <summary>
    /// Faker for <see cref="FileCategoryUpdate"/>
    /// </summary>
    public static readonly Faker<FileCategoryUpdate> FileCategoryUpdate = new Faker<FileCategoryUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0]
        });
}
