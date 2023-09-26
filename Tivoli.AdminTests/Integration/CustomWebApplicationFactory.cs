using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.Dal;

namespace Tivoli.AdminTests.Integration;

// ReSharper disable once ClassNeverInstantiated.Global
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        const string databaseName = "InMemoryDbForTesting";
        
        builder.ConfigureServices(services =>
        {
            
            #region Doesn't work
            // ServiceDescriptor? contextOptionsDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbContextOptions<TivoliContext>));
            //
            // if (contextOptionsDescriptor is not null) services.Remove(contextOptionsDescriptor);
            //
            // ServiceDescriptor? contextDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(TivoliContext));
            // if (contextDescriptor is not null) services.Remove(contextDescriptor);
            //
            // ServiceDescriptor? unitOfWorkDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(UnitOfWork));
            // if (unitOfWorkDescriptor is not null) services.Remove(unitOfWorkDescriptor);
            //
            //
            //
            //
            // services.AddDbContext<DbContext, TivoliContext>(options => 
            //     options.UseInMemoryDatabase(databaseName));
            //
            // services.AddSingleton<DbContextOptions>(new DbContextOptionsBuilder()
            //     .UseInMemoryDatabase(databaseName)
            //     .Options);
            //
            // services.AddSingleton<TivoliContext>(x => 
            // {
            //     DbContextOptions options = x.GetRequiredService<DbContextOptions>();
            //     return new TivoliContext(options);
            // });
            //
            // services.AddSingleton<DbContext>(x => 
            // {
            //     DbContextOptions options = x.GetRequiredService<DbContextOptions>();
            //     return new DbContext(options);
            // });
            
            // services.AddSingleton<UnitOfWork>(x => 
            // {
            //     DbContextOptions options = x.GetRequiredService<DbContextOptions>();
            //     TivoliContext context = new(options);
            //     return new UnitOfWork(context);
            // });
            //
            // services.AddSingleton<UnitOfWork>(x =>
            // {
            //     DbContext context = x.GetRequiredService<DbContext>();
            //     return new UnitOfWork(context);
            // });
            //
            // services.AddSingleton<UnitOfWork>(x => 
            // {
            //     DbContextOptions options = new DbContextOptionsBuilder()
            //         .UseInMemoryDatabase(databaseName)
            //         .Options;
            //     TivoliContext context = new(options);
            //     return new UnitOfWork(context);
            // });

            // services.AddTransient<UnitOfWork>();
            #endregion
            
            services.AddSingleton<DbContext>(x =>
            {
                DbContextOptions options = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase(databaseName)
                    .Options;
                return new TivoliContext(options);
            });
            
            // services.AddSingleton<UnitOfWork>(x => 
            // {
            //     DbContextOptions options = new DbContextOptionsBuilder()
            //         .UseInMemoryDatabase(databaseName)
            //         .Options;
            //     TivoliContext context = new(options);
            //     return new UnitOfWork(context);
            // });
            // services.AddSingleton<UnitOfWork>();
            
            ServiceProvider sp = services.BuildServiceProvider();

            using IServiceScope scope = sp.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            DbContext db = scopedServices.GetRequiredService<DbContext>();

            db.Database.EnsureCreated();
        });
        
        builder.UseEnvironment("Development");
    }
}