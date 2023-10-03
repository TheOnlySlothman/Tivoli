using Microsoft.EntityFrameworkCore;
using Tivoli.Models.Entity;

// ReSharper disable UnusedMember.Local

namespace Tivoli.Dal;

/// <summary>
///   Context for database.
/// </summary>
public class TivoliContext : DbContext
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TivoliContext(DbContextOptions options) : base(options)
    {
    }

    private DbSet<Customer> Customers { get; set; } = null!;
    private DbSet<Card> Cards { get; set; } = null!;

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // optionsBuilder.UseSqlServer("Server=localhost;Database=Tivoli;Trusted_Connection=True;");
    // }

    /// <summary>
    ///    This method is called when the model for a derived context has been initialized.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}