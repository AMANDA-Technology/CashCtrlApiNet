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
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Common.TextTemplate;
using CashCtrlApiNet.Services.Connectors.Common;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Common;

/// <summary>
/// Unit tests for <see cref="TextTemplateService"/>
/// </summary>
public class TextTemplateServiceTests : ServiceTestBase<TextTemplateService>
{
    /// <inheritdoc />
    protected override TextTemplateService CreateService()
        => new(ConnectionHandler);

    [Test]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<TextTemplate>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<TextTemplate>>());

        var result = await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<TextTemplate>, Entry>(
                CommonEndpoints.TextTemplate.Read, entry, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<TextTemplateListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<TextTemplateListed>>());

        var result = await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<TextTemplateListed>>(
                CommonEndpoints.TextTemplate.List, (ListParams?)null, Arg.Any<CancellationToken>());
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<TextTemplateListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<TextTemplateListed>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<TextTemplateListed>>(
                CommonEndpoints.TextTemplate.List, listParams, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<TextTemplateListed>>();
        ConnectionHandler
            .GetAsync<ListResponse<TextTemplateListed>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }

    [Test]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var textTemplate = new TextTemplateCreate { Name = "Test Template" };
        ConnectionHandler
            .PostAsync<NoContentResponse, TextTemplateCreate>(Arg.Any<string>(), Arg.Any<TextTemplateCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(textTemplate);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, TextTemplateCreate>(
                CommonEndpoints.TextTemplate.Create, textTemplate, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var textTemplate = new TextTemplateUpdate { Id = 1, Name = "Test Template" };
        ConnectionHandler
            .PostAsync<NoContentResponse, TextTemplateUpdate>(Arg.Any<string>(), Arg.Any<TextTemplateUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(textTemplate);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, TextTemplateUpdate>(
                CommonEndpoints.TextTemplate.Update, textTemplate, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2, 3] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                CommonEndpoints.TextTemplate.Delete, entries, Arg.Any<CancellationToken>());
    }
}
