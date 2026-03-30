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

using System.Net.Http.Headers;
using System.Text;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors;
using CashCtrlApiNet.Services;
using CashCtrlApiNet.Services.Connectors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CashCtrlApiNet.AspNetCore;

/// <summary>
/// Extension methods for registering CashCtrl API services with the ASP.NET Core dependency injection container
/// </summary>
public static class CashCtrlServiceCollectionExtensions
{
    /// <summary>
    /// Adds CashCtrl API client services to the specified <see cref="IServiceCollection"/>.
    /// Registers <see cref="ICashCtrlApiClient"/>, <see cref="ICashCtrlConnectionHandler"/> (via <see cref="IHttpClientFactory"/>),
    /// <see cref="ICashCtrlConfiguration"/>, and all connector interfaces.
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="configureOptions">An action to configure the <see cref="CashCtrlOptions"/></param>
    /// <returns>The same service collection for fluent chaining</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configureOptions"/> is null</exception>
    public static IServiceCollection AddCashCtrl(this IServiceCollection services, Action<CashCtrlOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        // Register and validate options
        services.AddOptions<CashCtrlOptions>()
            .Configure(configureOptions)
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<CashCtrlOptions>, CashCtrlOptionsValidator>();

        // Register configuration adapter
        services.AddSingleton<ICashCtrlConfiguration, CashCtrlOptionsAdapter>();

        // Register typed HttpClient with BaseAddress and Authorization configured at registration time
        var options = new CashCtrlOptions();
        configureOptions(options);

        var httpClientBuilder = services.AddHttpClient<ICashCtrlConnectionHandler, CashCtrlConnectionHandler>(
            (sp, client) =>
            {
                var config = sp.GetRequiredService<ICashCtrlConfiguration>();

                var baseUri = config.BaseUri;
                if (!baseUri.EndsWith('/'))
                    baseUri += '/';

                client.BaseAddress = new(baseUri);
                client.DefaultRequestHeaders.Authorization = new("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ApiKey}:")));
            });

        if (options.EnableResilience)
        {
            httpClientBuilder.AddStandardResilienceHandler();
        }

        // Register all connectors
        services.AddScoped<IAccountConnector, AccountConnector>();
        services.AddScoped<ICommonConnector, CommonConnector>();
        services.AddScoped<IFileConnector, FileConnector>();
        services.AddScoped<IInventoryConnector, InventoryConnector>();
        services.AddScoped<IJournalConnector, JournalConnector>();
        services.AddScoped<IMetaConnector, MetaConnector>();
        services.AddScoped<IOrderConnector, OrderConnector>();
        services.AddScoped<IPersonConnector, PersonConnector>();
        services.AddScoped<IReportConnector, ReportConnector>();
        services.AddScoped<ISalaryConnector, SalaryConnector>();

        // Register API client
        services.AddScoped<ICashCtrlApiClient, CashCtrlApiClient>();

        return services;
    }
}
