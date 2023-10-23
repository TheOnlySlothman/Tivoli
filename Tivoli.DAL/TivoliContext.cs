using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tivoli.Dal.Entities;

// ReSharper disable UnusedMember.Local

namespace Tivoli.Dal;

/// <summary>
///   Context for database.
/// </summary>
public class TivoliContext : IdentityDbContext<Customer, IdentityRole<Guid>, Guid>
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TivoliContext(DbContextOptions options) : base(options)
    {
    }

    // private DbSet<Customer> Customers { get; set; } = null!;
    private DbSet<Card> Cards { get; set; } = null!;

    /// <summary>
    ///    This method is called when the model for a derived context has been initialized.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("28f16864-f3ed-4c23-a0c2-ff952d884439"),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
        );
    }
}