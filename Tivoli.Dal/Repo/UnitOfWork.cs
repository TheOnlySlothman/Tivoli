using Microsoft.EntityFrameworkCore;
using Tivoli.Models;
using Tivoli.Models.Entity;

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
        Cards = new BaseRepo<Card>(context);
    }

    /// <summary>
    ///     Repository for customers.
    /// </summary>
    public BaseRepo<Customer> Customers { get; }

    /// <summary>
    ///    Repository for cards.
    /// </summary>
    public BaseRepo<Card> Cards { get; }
    
    /// <summary>
    ///   Save changes to database.
    /// </summary>
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    
    /// <summary>
    ///  Check if database is connected.
    /// </summary>
    /// <returns><c>true</c> if UnitOfWork can connect to database; otherwise false.</returns>
    public bool IsConnected()
    {
        return _context.Database.CanConnect();
    }
}

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