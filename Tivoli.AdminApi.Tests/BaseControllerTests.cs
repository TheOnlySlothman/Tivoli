using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.AdminApi.Tests;

public abstract class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly UnitOfWork UnitOfWork;
    protected readonly DbContext Context;
    protected readonly HttpClient Client;
    protected readonly ITestOutputHelper TestOutputHelper;

    protected BaseControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        using (IServiceScope scope = factory.Services.CreateScope())
        {
            UnitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
            Context = scope.ServiceProvider.GetRequiredService<DbContext>();
        }

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