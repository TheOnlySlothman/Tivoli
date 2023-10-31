using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tivoli.Dal;
using Tivoli.Dal.Entities;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public abstract class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly TivoliContext DbContext;
    public WebApplicationFactory<Program> Factory { get; set; }


    protected BaseControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        Factory = factory;
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        TestOutputHelper = testOutputHelper;
        DbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<DbContext>() as TivoliContext ??
                    throw new InvalidOperationException();

        // Client = factory.WithWebHostBuilder(builder => builder.ConfigureServices(
        //     services =>
        //     {
        //         ServiceDescriptor? authenticationServiceDescriptor = services.SingleOrDefault(
        //             d => d.ServiceType ==
        //                  typeof(IAuthenticationService));
        //         if (authenticationServiceDescriptor != null)
        //             services.Remove(authenticationServiceDescriptor);
        //
        //         services.AddScoped<IAuthenticationService, TestAuthenticationService>();
        //
        //         ServiceDescriptor? authorizationServiceDescriptor = services.SingleOrDefault(
        //             d => d.ServiceType ==
        //                  typeof(IAuthorizationService));
        //         if (authorizationServiceDescriptor != null)
        //             services.Remove(authorizationServiceDescriptor);
        //
        //         services.AddScoped<IAuthorizationService, TestDefaultAuthorizationService>();
        //     })).CreateClient();
    }


    private class TestAuthenticationService : AuthenticationService
    {
        public TestAuthenticationService(IAuthenticationSchemeProvider schemes, IAuthenticationHandlerProvider handlers,
            IClaimsTransformation transform, IOptions<AuthenticationOptions> options) : base(schemes, handlers,
            transform, options)
        {
        }

        public override Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
        {
            Claim[] claims = { new(ClaimTypes.Name, "Test user") };
            ClaimsIdentity identity = new(claims, "Test");
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, "TestScheme");

            AuthenticateResult result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    private class TestDefaultAuthorizationService : DefaultAuthorizationService
    {
        public TestDefaultAuthorizationService(IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers, ILogger<DefaultAuthorizationService> logger,
            IAuthorizationHandlerContextFactory contextFactory, IAuthorizationEvaluator evaluator,
            IOptions<AuthorizationOptions> options) : base(policyProvider, handlers, logger, contextFactory, evaluator,
            options)
        {
        }

        public override Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource,
            IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }
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

    protected async Task UseDatabase(Func<Task> callback)
    {
        try
        {
            await callback();
        }
        finally
        {
            // Cleanup

            // using IServiceScope scope = Factory.Services.CreateScope();
            // DbContext db = scope.ServiceProvider.GetRequiredService<DbContext>();
            // await db.Database.EnsureDeletedAsync();

            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.Database.EnsureCreatedAsync();
        }
    }

    protected async Task<Customer> AddUserAsync(string username = "Test@Test.dk", string password = "Test123.")
    {
        Customer customer = DbContext.Users.Add(new Customer
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = username,
            NormalizedEmail = username.ToUpper(),
            NormalizedUserName = username.ToUpper()
        }).Entity;

        DbContext.UserRoles.Add(new IdentityUserRole<Guid>
        {
            UserId = customer.Id,
            RoleId = DbContext.Roles.Single(r => r.Name == "Customer").Id
        });

        customer.PasswordHash = new PasswordHasher<Customer>().HashPassword(customer, password);
        await DbContext.SaveChangesAsync();
        return customer;
    }
}