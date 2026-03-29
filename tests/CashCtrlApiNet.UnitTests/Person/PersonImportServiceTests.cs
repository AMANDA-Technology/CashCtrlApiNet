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

using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Person.Import;
using CashCtrlApiNet.Services.Connectors.Person;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;

namespace CashCtrlApiNet.UnitTests.Person;

/// <summary>
/// Unit tests for <see cref="PersonImportService"/>
/// </summary>
public class PersonImportServiceTests : ServiceTestBase<PersonImportService>
{
    /// <inheritdoc />
    protected override PersonImportService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var importCreate = new PersonImportCreate { FileId = 100 };
        ConnectionHandler
            .PostAsync<NoContentResponse, PersonImportCreate>(Arg.Any<string>(), Arg.Any<PersonImportCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(importCreate);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, PersonImportCreate>(
                PersonEndpoints.Import.Create, importCreate, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Mapping_ShouldPostToCorrectEndpoint()
    {
        var importMapping = new PersonImportMapping { Id = 1, Mapping = "{\"field1\":\"value1\"}" };
        ConnectionHandler
            .PostAsync<NoContentResponse, PersonImportMapping>(Arg.Any<string>(), Arg.Any<PersonImportMapping>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Mapping(importMapping);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, PersonImportMapping>(
                PersonEndpoints.Import.Mapping, importMapping, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetMappingFields_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult());

        await Service.GetMappingFields();

        await ConnectionHandler.Received(1)
            .GetAsync(PersonEndpoints.Import.AvailableMappingFields, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Preview_ShouldPostToCorrectEndpoint()
    {
        var importPreview = new PersonImportPreview { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, PersonImportPreview>(Arg.Any<string>(), Arg.Any<PersonImportPreview>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Preview(importPreview);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, PersonImportPreview>(
                PersonEndpoints.Import.Preview, importPreview, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Execute_ShouldPostToCorrectEndpoint()
    {
        var importExecute = new PersonImportExecute { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, PersonImportExecute>(Arg.Any<string>(), Arg.Any<PersonImportExecute>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Execute(importExecute);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, PersonImportExecute>(
                PersonEndpoints.Import.Execute, importExecute, Arg.Any<CancellationToken>());
    }
}
