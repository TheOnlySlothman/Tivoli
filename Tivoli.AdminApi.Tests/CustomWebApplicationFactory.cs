using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.Dal;

namespace Tivoli.AdminApi.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string databaseName = "InMemoryDbForTesting-" + Guid.NewGuid();

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<DbContext>(_ =>
            {
                DbContextOptions options = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase(databaseName)
                    .Options;
                return new TivoliContext(options);
            });

            ServiceProvider sp = services.BuildServiceProvider();

            using IServiceScope scope = sp.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            DbContext db = scopedServices.GetRequiredService<DbContext>();

            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }
}