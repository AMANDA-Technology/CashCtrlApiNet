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
using CashCtrlApiNet.Abstractions.Models.Salary.Template;
using CashCtrlApiNet.Services.Connectors.Salary;
using CashCtrlApiNet.Services.Endpoints;
using NSubstitute;
using Shouldly;

namespace CashCtrlApiNet.Tests.Salary;

/// <summary>
/// Unit tests for <see cref="SalaryTemplateService"/>
/// </summary>
public class SalaryTemplateServiceTests : ServiceTestBase<SalaryTemplateService>
{
    /// <inheritdoc />
    protected override SalaryTemplateService CreateService()
        => new(ConnectionHandler);

    [Fact]
    public async Task Get_ShouldCallCorrectEndpoint_WithEntryParameter()
    {
        var entry = new Entry { Id = 42 };
        ConnectionHandler
            .GetAsync<SingleResponse<SalaryTemplate>, Entry>(
                Arg.Any<string>(), Arg.Any<Entry>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<SingleResponse<SalaryTemplate>>());

        await Service.Get(entry);

        await ConnectionHandler.Received(1)
            .GetAsync<SingleResponse<SalaryTemplate>, Entry>(
                SalaryEndpoints.Template.Read, entry, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<SalaryTemplate>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryTemplate>>());

        await Service.GetList();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryTemplate>>(
                SalaryEndpoints.Template.List, (ListParams?)null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTree_ShouldCallCorrectEndpoint()
    {
        ConnectionHandler
            .GetAsync<ListResponse<SalaryTemplate>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryTemplate>>());

        await Service.GetTree();

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryTemplate>>(
                SalaryEndpoints.Template.Tree, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldPostToCorrectEndpoint()
    {
        var template = new SalaryTemplateCreate { Name = "Test" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryTemplateCreate>(Arg.Any<string>(), Arg.Any<SalaryTemplateCreate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Create(template);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryTemplateCreate>(
                SalaryEndpoints.Template.Create, template, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_ShouldPostToCorrectEndpoint()
    {
        var template = new SalaryTemplateUpdate { Id = 1, Name = "Test" };
        ConnectionHandler
            .PostAsync<NoContentResponse, SalaryTemplateUpdate>(Arg.Any<string>(), Arg.Any<SalaryTemplateUpdate>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Update(template);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, SalaryTemplateUpdate>(
                SalaryEndpoints.Template.Update, template, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_ShouldPostToCorrectEndpoint()
    {
        var entries = new Entries { Ids = [1, 2, 3] };
        ConnectionHandler
            .PostAsync<NoContentResponse, Entries>(Arg.Any<string>(), Arg.Any<Entries>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<NoContentResponse>());

        await Service.Delete(entries);

        await ConnectionHandler.Received(1)
            .PostAsync<NoContentResponse, Entries>(
                SalaryEndpoints.Template.Delete, entries, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldCallCorrectEndpoint()
    {
        var listParams = new ListParams { Query = "test", OnlyActive = true };
        ConnectionHandler
            .GetAsync<ListResponse<SalaryTemplate>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(new ApiResult<ListResponse<SalaryTemplate>>());

        await Service.GetList(listParams);

        await ConnectionHandler.Received(1)
            .GetAsync<ListResponse<SalaryTemplate>>(
                SalaryEndpoints.Template.List, listParams, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetList_WithListParams_ShouldReturnResult()
    {
        var listParams = new ListParams { Query = "test" };
        var expected = new ApiResult<ListResponse<SalaryTemplate>>();
        ConnectionHandler
            .GetAsync<ListResponse<SalaryTemplate>>(Arg.Any<string>(), Arg.Any<ListParams?>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await Service.GetList(listParams);

        result.ShouldBe(expected);
    }
}
