using Microsoft.AspNetCore.Identity;

namespace Tivoli.Models.Entity;

/// <summary>
///    Customer model.
/// </summary>
public class Customer : IdentityUser<Guid>, IEntity
{
    /// <inheritdoc cref="IEntity"/>
    public override Guid Id { get; set; }
}