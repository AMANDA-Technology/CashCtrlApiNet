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

using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.AspNetCore;
using CashCtrlApiNet.Interfaces;
using CashCtrlApiNet.Interfaces.Connectors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Unit tests for ASP.NET Core dependency injection registration via <see cref="CashCtrlServiceCollectionExtensions"/>
/// </summary>
public class AspNetCoreDependencyInjectionTests
{
    /// <summary>
    /// Helper to build a service collection with valid options
    /// </summary>
    private static ServiceCollection CreateServiceCollectionWithValidOptions(
        string baseUri = "https://test.cashctrl.com/",
        string apiKey = "test-api-key",
        Language language = Language.De)
    {
        var services = new ServiceCollection();
        services.AddCashCtrl(options =>
        {
            options.BaseUri = baseUri;
            options.ApiKey = apiKey;
            options.Language = language;
        });
        return services;
    }

    /// <summary>
    /// Verifies that <see cref="ICashCtrlApiClient"/> is registered and resolvable
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldRegister_ICashCtrlApiClient()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        var client = provider.GetService<ICashCtrlApiClient>();

        client.ShouldNotBeNull();
    }

    /// <summary>
    /// Verifies that <see cref="ICashCtrlConnectionHandler"/> is registered and resolvable
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldRegister_ICashCtrlConnectionHandler()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        var handler = provider.GetService<ICashCtrlConnectionHandler>();

        handler.ShouldNotBeNull();
    }

    /// <summary>
    /// Verifies that <see cref="ICashCtrlConfiguration"/> is registered and resolvable
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldRegister_ICashCtrlConfiguration()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        var config = provider.GetService<ICashCtrlConfiguration>();

        config.ShouldNotBeNull();
    }

    /// <summary>
    /// Verifies that all 10 connector interfaces are registered and resolvable
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldRegister_AllConnectors()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        provider.GetService<IAccountConnector>().ShouldNotBeNull();
        provider.GetService<ICommonConnector>().ShouldNotBeNull();
        provider.GetService<IFileConnector>().ShouldNotBeNull();
        provider.GetService<IInventoryConnector>().ShouldNotBeNull();
        provider.GetService<IJournalConnector>().ShouldNotBeNull();
        provider.GetService<IMetaConnector>().ShouldNotBeNull();
        provider.GetService<IOrderConnector>().ShouldNotBeNull();
        provider.GetService<IPersonConnector>().ShouldNotBeNull();
        provider.GetService<IReportConnector>().ShouldNotBeNull();
        provider.GetService<ISalaryConnector>().ShouldNotBeNull();
    }

    /// <summary>
    /// Verifies that <see cref="CashCtrlServiceCollectionExtensions.AddCashCtrl"/> returns the same service collection for fluent chaining
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldReturnSameServiceCollection()
    {
        var services = new ServiceCollection();

        var result = services.AddCashCtrl(options =>
        {
            options.BaseUri = "https://test.cashctrl.com/";
            options.ApiKey = "test-api-key";
        });

        result.ShouldBeSameAs(services);
    }

    /// <summary>
    /// Verifies that the BaseUri option is correctly mapped to <see cref="ICashCtrlConfiguration.BaseUri"/>
    /// </summary>
    [Test]
    public void AddCashCtrl_Configuration_ShouldMapBaseUri()
    {
        var services = CreateServiceCollectionWithValidOptions(baseUri: "https://myorg.cashctrl.com/");
        using var provider = services.BuildServiceProvider();

        var config = provider.GetRequiredService<ICashCtrlConfiguration>();

        config.BaseUri.ShouldBe("https://myorg.cashctrl.com/");
    }

    /// <summary>
    /// Verifies that the ApiKey option is correctly mapped to <see cref="ICashCtrlConfiguration.ApiKey"/>
    /// </summary>
    [Test]
    public void AddCashCtrl_Configuration_ShouldMapApiKey()
    {
        var services = CreateServiceCollectionWithValidOptions(apiKey: "my-secret-key");
        using var provider = services.BuildServiceProvider();

        var config = provider.GetRequiredService<ICashCtrlConfiguration>();

        config.ApiKey.ShouldBe("my-secret-key");
    }

    /// <summary>
    /// Verifies that the Language option is correctly mapped to <see cref="ICashCtrlConfiguration.DefaultLanguage"/>
    /// </summary>
    [Test]
    public void AddCashCtrl_Configuration_ShouldMapLanguage()
    {
        var services = CreateServiceCollectionWithValidOptions(language: Language.En);
        using var provider = services.BuildServiceProvider();

        var config = provider.GetRequiredService<ICashCtrlConfiguration>();

        config.DefaultLanguage.ShouldBe("en");
    }

    /// <summary>
    /// Verifies that Language defaults to <see cref="Language.De"/> when not explicitly set
    /// </summary>
    [Test]
    public void AddCashCtrl_Configuration_ShouldDefaultLanguageToDe()
    {
        var services = new ServiceCollection();
        services.AddCashCtrl(options =>
        {
            options.BaseUri = "https://test.cashctrl.com/";
            options.ApiKey = "test-api-key";
        });
        using var provider = services.BuildServiceProvider();

        var config = provider.GetRequiredService<ICashCtrlConfiguration>();

        config.DefaultLanguage.ShouldBe("de");
    }

    /// <summary>
    /// Verifies that <see cref="CashCtrlServiceCollectionExtensions.AddCashCtrl"/> throws when services parameter is null
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldThrow_WhenServicesIsNull()
    {
        IServiceCollection services = null!;

        Should.Throw<ArgumentNullException>(() => services.AddCashCtrl(options =>
        {
            options.BaseUri = "https://test.cashctrl.com/";
            options.ApiKey = "test-api-key";
        }));
    }

    /// <summary>
    /// Verifies that <see cref="CashCtrlServiceCollectionExtensions.AddCashCtrl"/> throws when configureOptions parameter is null
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldThrow_WhenConfigureOptionsIsNull()
    {
        var services = new ServiceCollection();

        Should.Throw<ArgumentNullException>(() => services.AddCashCtrl(null!));
    }

    /// <summary>
    /// Verifies that options validation fails when BaseUri is not set
    /// </summary>
    [Test]
    public void AddCashCtrl_Validation_ShouldFail_WhenBaseUriIsMissing()
    {
        var services = new ServiceCollection();
        services.AddCashCtrl(options =>
        {
            options.ApiKey = "test-api-key";
        });
        using var provider = services.BuildServiceProvider();

        Should.Throw<OptionsValidationException>(() => provider.GetRequiredService<ICashCtrlConfiguration>());
    }

    /// <summary>
    /// Verifies that options validation fails when ApiKey is not set
    /// </summary>
    [Test]
    public void AddCashCtrl_Validation_ShouldFail_WhenApiKeyIsMissing()
    {
        var services = new ServiceCollection();
        services.AddCashCtrl(options =>
        {
            options.BaseUri = "https://test.cashctrl.com/";
        });
        using var provider = services.BuildServiceProvider();

        Should.Throw<OptionsValidationException>(() => provider.GetRequiredService<ICashCtrlConfiguration>());
    }

    /// <summary>
    /// Verifies that options validation fails when BaseUri is an empty string
    /// </summary>
    [Test]
    public void AddCashCtrl_Validation_ShouldFail_WhenBaseUriIsEmpty()
    {
        var services = new ServiceCollection();
        services.AddCashCtrl(options =>
        {
            options.BaseUri = "";
            options.ApiKey = "test-api-key";
        });
        using var provider = services.BuildServiceProvider();

        Should.Throw<OptionsValidationException>(() => provider.GetRequiredService<ICashCtrlConfiguration>());
    }

    /// <summary>
    /// Verifies that options validation fails when ApiKey is whitespace only
    /// </summary>
    [Test]
    public void AddCashCtrl_Validation_ShouldFail_WhenApiKeyIsWhitespace()
    {
        var services = new ServiceCollection();
        services.AddCashCtrl(options =>
        {
            options.BaseUri = "https://test.cashctrl.com/";
            options.ApiKey = "   ";
        });
        using var provider = services.BuildServiceProvider();

        Should.Throw<OptionsValidationException>(() => provider.GetRequiredService<ICashCtrlConfiguration>());
    }

    /// <summary>
    /// Verifies that IHttpClientFactory is registered by AddCashCtrl
    /// </summary>
    [Test]
    public void AddCashCtrl_ShouldRegister_IHttpClientFactory()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        var factory = provider.GetService<IHttpClientFactory>();

        factory.ShouldNotBeNull();
    }

    /// <summary>
    /// Verifies that scoped services return the same instance within a single scope
    /// </summary>
    [Test]
    public void AddCashCtrl_ScopedServices_ShouldReturnSameInstanceWithinScope()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var client1 = scope.ServiceProvider.GetRequiredService<ICashCtrlApiClient>();
        var client2 = scope.ServiceProvider.GetRequiredService<ICashCtrlApiClient>();

        client1.ShouldBeSameAs(client2);
    }

    /// <summary>
    /// Verifies that scoped services return different instances across different scopes
    /// </summary>
    [Test]
    public void AddCashCtrl_ScopedServices_ShouldReturnDifferentInstancesAcrossScopes()
    {
        var services = CreateServiceCollectionWithValidOptions();
        using var provider = services.BuildServiceProvider();

        ICashCtrlApiClient client1;
        ICashCtrlApiClient client2;

        using (var scope1 = provider.CreateScope())
        {
            client1 = scope1.ServiceProvider.GetRequiredService<ICashCtrlApiClient>();
        }

        using (var scope2 = provider.CreateScope())
        {
            client2 = scope2.ServiceProvider.GetRequiredService<ICashCtrlApiClient>();
        }

        client1.ShouldNotBeSameAs(client2);
    }
}
