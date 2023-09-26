using Microsoft.AspNetCore.Identity;
using Tivoli.Dal;
using Tivoli.Models.Entity;

namespace Tivoli.AdminApi;

/// <summary>
///   This is a static class for configuring services.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///    Configures Identity for the application.
    /// </summary>
    /// <param name="services">Service to configure.</param>
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<Customer>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<TivoliContext>()
            .AddDefaultTokenProviders();
    }
}