using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tivoli.Dal;

/// <summary>
///    This class is used to create a <c>TivoliContext</c> for the <c>dotnet ef</c> commands.
/// </summary>
// ReSharper disable once UnusedType.Global
public class TivoliContextFactory : IDesignTimeDbContextFactory<TivoliContext>
{
    /// <summary>
    ///   Creates a <c>TivoliContext</c> for the <c>dotnet ef</c> commands.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public TivoliContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string? connectionString = configuration
            .GetConnectionString("sqlConnection");

        DbContextOptionsBuilder<TivoliContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(connectionString);

        return new TivoliContext(optionsBuilder.Options);
    }
}