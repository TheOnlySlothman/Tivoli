using Microsoft.EntityFrameworkCore;
using Tivoli.Dal.Entities;

namespace Tivoli.Dal.Repo;

/// <summary>
///    Unit of work for all repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    /// <summary>
    ///    Constructor.
    /// </summary>
    /// <param name="context">Context to apply to repos</param>
    public UnitOfWork(DbContext context)
    {
        _context = context;
        Customers = new BaseRepo<Customer>(context);
        Cards = new CardRepo(context);
    }

    /// <summary>
    ///     Gets the repository for customers.
    /// </summary>
    public BaseRepo<Customer> Customers { get; }

    /// <summary>
    ///    Gets the repository for cards.
    /// </summary>
    public CardRepo Cards { get; }

    /// <inheritdoc />
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public bool IsConnected()
    {
        return _context.Database.CanConnect();
    }
}

/// <summary>
///   Unit of work for all repositories.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///   Save changes to database.
    /// </summary>
    void SaveChanges();

    /// <summary>
    ///  Check if database is connected.
    /// </summary>
    /// <returns><c>true</c> if UnitOfWork can connect to database; otherwise false.</returns>
    bool IsConnected();
}