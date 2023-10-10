using Microsoft.AspNetCore.Identity;

namespace Tivoli.Dal.Entities;

/// <summary>
///    Customer model.
/// </summary>
public class Customer : IdentityUser<Guid>, IEntity
{
    /// <inheritdoc cref="IEntity"/>
    public override Guid Id { get; set; }

    /// <summary>
    ///   Constructor for <see cref="Customer"/> class.
    /// </summary>
    public Customer()
    {
    }

    public List<Card> Cards { get; set; } = new();
}
