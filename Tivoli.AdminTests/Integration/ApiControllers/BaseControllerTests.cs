using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Tivoli.Dal.Repo;
using Xunit;
using Xunit.Abstractions;

namespace Tivoli.AdminTests.Integration.ApiControllers;

public abstract class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly UnitOfWork UnitOfWork;
    protected readonly HttpClient Client;
    protected abstract string ControllerName { get; }
    private readonly CustomWebApplicationFactory<Program> _factory;
    protected readonly ITestOutputHelper TestOutputHelper;

    protected BaseControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        
        try
        {
            UnitOfWork = _factory.Services.GetService(typeof(UnitOfWork)) as UnitOfWork ??
                         throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        TestOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Combines the parts into a url. If the first part ends with a slash, it is removed. If the second part starts with a slash, it is removed.
    /// </summary>
    /// <param name="parts">TODO</param>
    /// <returns>The combined string.</returns>
    protected static string CombineUrl(params string[] parts)
    {
        string result = parts.Aggregate((a, b) =>
            $"{(a[^1..].StartsWith('/') ? a[..^1] : a)}/{(b.StartsWith('/') ? b[1..] : b)}");
        return result;
    }
}