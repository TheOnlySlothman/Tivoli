using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.Dal;
using Tivoli.Dal.Repo;

namespace Tivoli.CustomerApi.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string databaseName = "InMemoryDbForTesting-" + Guid.NewGuid();

        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? dbContextOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<DbContext>));
            if (dbContextOptionsDescriptor != null) services.Remove(dbContextOptionsDescriptor);

            ServiceDescriptor? dbContextServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContext));
            if (dbContextServiceDescriptor != null) services.Remove(dbContextServiceDescriptor);

            ServiceDescriptor? unitOfWorkDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(UnitOfWork));
            if (unitOfWorkDescriptor != null) services.Remove(unitOfWorkDescriptor);

            // services.AddDbContext<DbContext, TivoliContext>(_ =>
            // {
            //     new DbContextOptionsBuilder()
            //         .UseInMemoryDatabase(databaseName);
            // });

            services.AddDbContext<DbContext, TivoliContext>(options => options.UseInMemoryDatabase(databaseName));

            // services.AddSingleton<DbContext, TivoliContext>(_ =>
            // {
            //     DbContextOptions options = new DbContextOptionsBuilder()
            //         .UseInMemoryDatabase(databaseName)
            //         .Options;
            //     return new TivoliContext(options);
            // });

            services.AddTransient<UnitOfWork>();

            ServiceProvider sp = services.BuildServiceProvider();

            using IServiceScope scope = sp.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            DbContext db = scopedServices.GetRequiredService<DbContext>();

            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }
}