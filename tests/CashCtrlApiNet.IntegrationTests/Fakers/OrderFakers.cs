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

using System.Text.Json;
using Bogus;
using CashCtrlApiNet.Abstractions.Models.Order.BookEntry;
using CashCtrlApiNet.Abstractions.Models.Order.Category;
using CashCtrlApiNet.Abstractions.Models.Order.Document;
using CashCtrlApiNet.Abstractions.Models.Order.Layout;
using CashCtrlApiNet.Abstractions.Models.Order.Payment;
using OrderModel = CashCtrlApiNet.Abstractions.Models.Order.Order;
using OrderListedModel = CashCtrlApiNet.Abstractions.Models.Order.OrderListed;
using OrderCreateModel = CashCtrlApiNet.Abstractions.Models.Order.OrderCreate;
using OrderUpdateModel = CashCtrlApiNet.Abstractions.Models.Order.OrderUpdate;

namespace CashCtrlApiNet.IntegrationTests.Fakers;

/// <summary>
/// Bogus fakers for Order domain models
/// </summary>
public static class OrderFakers
{
    /// <summary>
    /// Faker for <see cref="OrderModel"/> (detail response)
    /// </summary>
    public static readonly Faker<OrderModel> Order = new Faker<OrderModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 999),
            CategoryId = f.Random.Int(1, 100),
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            SequenceNumberId = f.Random.Int(1, 100),
            Description = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="OrderListedModel"/> (list response)
    /// </summary>
    public static readonly Faker<OrderListedModel> OrderListed = new Faker<OrderListedModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 999),
            CategoryId = f.Random.Int(1, 100),
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            SequenceNumberId = f.Random.Int(1, 100),
            Description = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="OrderCreateModel"/>
    /// </summary>
    public static readonly Faker<OrderCreateModel> OrderCreate = new Faker<OrderCreateModel>()
        .CustomInstantiator(f => new()
        {
            AccountId = f.Random.Int(1, 999),
            CategoryId = f.Random.Int(1, 100),
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            SequenceNumberId = f.Random.Int(1, 100),
            Description = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="OrderUpdateModel"/>
    /// </summary>
    public static readonly Faker<OrderUpdateModel> OrderUpdate = new Faker<OrderUpdateModel>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 999),
            CategoryId = f.Random.Int(1, 100),
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            SequenceNumberId = f.Random.Int(1, 100),
            Description = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="OrderCategory"/> (detail response)
    /// </summary>
    public static readonly Faker<OrderCategory> Category = new Faker<OrderCategory>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Commerce.Categories(1)[0],
            NameSingular = f.Commerce.Categories(1)[0],
            NamePlural = f.Commerce.Categories(1)[0],
            AccountId = f.Random.Int(1, 999),
            SequenceNrId = f.Random.Int(1, 100),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="OrderCategoryCreate"/>
    /// </summary>
    public static readonly Faker<OrderCategoryCreate> CategoryCreate = new Faker<OrderCategoryCreate>()
        .CustomInstantiator(f => new()
        {
            NameSingular = f.Commerce.Categories(1)[0],
            NamePlural = f.Commerce.Categories(1)[0],
            AccountId = f.Random.Int(1, 999),
            Status = JsonSerializer.Deserialize<JsonElement>("""[{"icon":"BLUE","name":"Draft"}]"""),
            SequenceNrId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="OrderCategoryUpdate"/>
    /// </summary>
    public static readonly Faker<OrderCategoryUpdate> CategoryUpdate = new Faker<OrderCategoryUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            NameSingular = f.Commerce.Categories(1)[0],
            NamePlural = f.Commerce.Categories(1)[0],
            AccountId = f.Random.Int(1, 999),
            Status = JsonSerializer.Deserialize<JsonElement>("""[{"icon":"BLUE","name":"Draft"}]"""),
            SequenceNrId = f.Random.Int(1, 100)
        });

    /// <summary>
    /// Faker for <see cref="OrderLayout"/> (detail response)
    /// </summary>
    public static readonly Faker<OrderLayout> Layout = new Faker<OrderLayout>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="OrderLayoutCreate"/>
    /// </summary>
    public static readonly Faker<OrderLayoutCreate> LayoutCreate = new Faker<OrderLayoutCreate>()
        .CustomInstantiator(f => new()
        {
            Name = f.Lorem.Word()
        });

    /// <summary>
    /// Faker for <see cref="OrderLayoutUpdate"/>
    /// </summary>
    public static readonly Faker<OrderLayoutUpdate> LayoutUpdate = new Faker<OrderLayoutUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            Name = f.Lorem.Word()
        });

    /// <summary>
    /// Faker for <see cref="BookEntry"/> (detail response)
    /// </summary>
    public static readonly Faker<BookEntry> BookEntry = new Faker<BookEntry>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            OrderId = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000),
            Description = f.Lorem.Sentence(3),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="BookEntryCreate"/>
    /// </summary>
    public static readonly Faker<BookEntryCreate> BookEntryCreate = new Faker<BookEntryCreate>()
        .CustomInstantiator(f => new()
        {
            OrderIds = [f.Random.Int(1, 9999)],
            AccountId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000),
            Description = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="BookEntryUpdate"/>
    /// </summary>
    public static readonly Faker<BookEntryUpdate> BookEntryUpdate = new Faker<BookEntryUpdate>()
        .CustomInstantiator(f => new()
        {
            Id = f.Random.Int(1, 9999),
            AccountId = f.Random.Int(1, 999),
            Amount = f.Random.Double(1, 10000),
            Description = f.Lorem.Sentence(3)
        });

    /// <summary>
    /// Faker for <see cref="Document"/> (detail response)
    /// </summary>
    public static readonly Faker<Document> Document = new Faker<Document>()
        .CustomInstantiator(f => new()
        {
            OrderId = f.Random.Int(1, 9999),
            Header = f.Lorem.Sentence(),
            Footer = f.Lorem.Sentence(),
            CreatedBy = f.Person.UserName,
            LastUpdatedBy = f.Person.UserName,
            Created = DateTime.UtcNow
        });

    /// <summary>
    /// Faker for <see cref="OrderPaymentRequest"/>
    /// </summary>
    public static readonly Faker<OrderPaymentRequest> PaymentRequest = new Faker<OrderPaymentRequest>()
        .CustomInstantiator(f => new()
        {
            Date = f.Date.Recent().ToString("yyyy-MM-dd"),
            OrderIds = [f.Random.Int(1, 9999)]
        });
}
