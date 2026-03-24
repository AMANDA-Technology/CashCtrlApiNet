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

using CashCtrlApiNet.Abstractions.Models.Order.DocumentTemplate;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Services.Connectors.Order;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;

namespace CashCtrlApiNet.Tests.Order;

/// <summary>
/// Unit tests for <see cref="DocumentTemplateService"/>
/// </summary>
public class DocumentTemplateServiceTests : ServiceTestBase<DocumentTemplateService>
{
    /// <inheritdoc />
    protected override DocumentTemplateService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<DocumentTemplate>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<DocumentTemplate>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<DocumentTemplate>, Entry>(
                OrderEndpoints.DocumentTemplate.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<DocumentTemplate>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<DocumentTemplate>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<DocumentTemplate>>(
                OrderEndpoints.DocumentTemplate.List, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var template = new DocumentTemplateCreate { Name = "Test Template" };
        ConnectionHandler
            .PostAsync<NoContentResponse, DocumentTemplateCreate>(Arg.Any<string>(), Arg.Any<DocumentTemplateCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(template);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, DocumentTemplateCreate>(
                OrderEndpoints.DocumentTemplate.Create, template, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var template = new DocumentTemplateUpdate { Id = 1, Name = "Updated Template" };
        ConnectionHandler
            .PostAsync<NoContentResponse, DocumentTemplateUpdate>(Arg.Any<string>(), Arg.Any<DocumentTemplateUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(template);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, DocumentTemplateUpdate>(
                OrderEndpoints.DocumentTemplate.Update, template, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                OrderEndpoints.DocumentTemplate.Delete, entries, Arg.Any<CancellationToken>());
    }
}
