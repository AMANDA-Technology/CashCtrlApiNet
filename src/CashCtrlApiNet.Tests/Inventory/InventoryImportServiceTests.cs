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
using CashCtrlApiNet.Abstractions.Models.Inventory.Import;
using CashCtrlApiNet.Services.Connectors.Inventory;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;

namespace CashCtrlApiNet.Tests.Inventory;

/// <summary>
/// Unit tests for <see cref="InventoryImportService"/>
/// </summary>
public class InventoryImportServiceTests : ServiceTestBase<InventoryImportService>
{
    /// <inheritdoc />
    protected override InventoryImportService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var importCreate = new InventoryImportCreate { FileId = 100 };
        ConnectionHandler
            .PostAsync<NoContentResponse, InventoryImportCreate>(Arg.Any<string>(), Arg.Any<InventoryImportCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(importCreate);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, InventoryImportCreate>(
                InventoryEndpoints.Import.Create, importCreate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Mapping_ShouldPostToCorrectEndpoint()
    {
        var importMapping = new InventoryImportMapping { Id = 1, Mapping = "{\"field1\":\"value1\"}" };
        ConnectionHandler
            .PostAsync<NoContentResponse, InventoryImportMapping>(Arg.Any<string>(), Arg.Any<InventoryImportMapping>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Mapping(importMapping);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, InventoryImportMapping>(
                InventoryEndpoints.Import.Mapping, importMapping, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetMappingFields_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult());

        await Service.GetMappingFields();

        await ConnectionHandler.Received(1)
            .GetAsync(InventoryEndpoints.Import.AvailableMappingFields, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Preview_ShouldPostToCorrectEndpoint()
    {
        var importPreview = new InventoryImportPreview { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, InventoryImportPreview>(Arg.Any<string>(), Arg.Any<InventoryImportPreview>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Preview(importPreview);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, InventoryImportPreview>(
                InventoryEndpoints.Import.Preview, importPreview, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Execute_ShouldPostToCorrectEndpoint()
    {
        var importExecute = new InventoryImportExecute { Id = 1 };
        ConnectionHandler
            .PostAsync<NoContentResponse, InventoryImportExecute>(Arg.Any<string>(), Arg.Any<InventoryImportExecute>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Execute(importExecute);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, InventoryImportExecute>(
                InventoryEndpoints.Import.Execute, importExecute, Arg.Any<CancellationToken>());
    }
}
