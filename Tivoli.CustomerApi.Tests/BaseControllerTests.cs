using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public abstract class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly CustomWebApplicationFactory<Program> Factory;

    protected BaseControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        Factory = factory;
        TestOutputHelper = testOutputHelper;
    }


    /// <summary>
    ///     Combines the parts into a url. If the first part ends with a slash, it is removed. If the second part starts with a slash, it is removed.
    /// </summary>
    /// <param name="parts">Parts to combine to a url.</param>
    /// <returns>The combined string.</returns>
    protected static string CombineUrl(params string[] parts)
    {
        string result = parts.Aggregate((a, b) =>
            $"{(a[^1..].StartsWith('/') ? a[..^1] : a)}/{(b.StartsWith('/') ? b[1..] : b)}");
        return result;
    }
}