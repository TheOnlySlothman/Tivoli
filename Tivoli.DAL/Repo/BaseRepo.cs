using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tivoli.Dal.Entities;

namespace Tivoli.Dal.Repo;

/// <summary>
///     Generic class for all repositories.
/// </summary>
/// <typeparam name="T">Type to base repo on.</typeparam>
public class BaseRepo<T> : IRepo<T> where T : class, IEntity, new()
{
    /// <summary>
    ///    Constructor.
    /// </summary>
    /// <param name="context">Context to apply repo</param>
    public BaseRepo(DbContext context)
    {
        DbSet = context.Set<T>();
    }

    protected readonly DbSet<T> DbSet;

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Entity was not found.</exception>
    public T Get(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        T? entity = DbSet.Find(id);
        if (entity is null) throw new KeyNotFoundException($"No {typeof(T).Name} with id {id} found");

        return entity;
    }

    /// <inheritdoc />
    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return DbSet.FirstOrDefault(predicate);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Entity was not found.</exception>
    public T GetWithRelated(Guid id, params Expression<Func<T, object?>>[] relations)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        if (!Exists(id)) throw new KeyNotFoundException($"No {typeof(T).Name} with id {id} found");

        IQueryable<T> query =
            relations.Aggregate(DbSet.Where(x => x.Id == id), (current, relation) => current.Include(relation));

        return query.First();
    }

    /// <inheritdoc />
    public T? GetWithRelated(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] relations)
    {
        IQueryable<T> query =
            relations.Aggregate(DbSet.Where(predicate), (current, relation) => current.Include(relation));

        return query.FirstOrDefault();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Id is empty.</exception>
    public T GetAsNoTracking(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        T? entity = DbSet.Where(x => x.Id == id).AsNoTracking().FirstOrDefault();
        if (entity is null) throw new KeyNotFoundException($"No {typeof(T).Name} with id {id} found");

        return entity;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Id is empty.</exception>
    public T GetAsNoTracking(Expression<Func<T, bool>> predicate)
    {
        T? entity = DbSet.Where(predicate).AsNoTracking().FirstOrDefault();
        if (entity is null)
            throw new ArgumentException("No entity found.", nameof(predicate));
        return entity;
    }

    /// <inheritdoc />
    public IEnumerable<T> GetAll()
    {
        return DbSet;
    }

    /// <inheritdoc />
    public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }

    /// <inheritdoc />
    public IEnumerable<T> GetAllAsNoTracking()
    {
        return DbSet.AsNoTracking();
    }

    /// <inheritdoc />
    public IEnumerable<T> GetAllAsNoTracking(Expression<Func<T, bool>> predicate)
    {
        return DbSet.Where(predicate).AsNoTracking();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Id is empty.</exception>
    public bool Exists(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        return DbSet.Any(x => x.Id == id);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">Entity is null.</exception>
    /// <exception cref="ArgumentException">Entity with id already exists.</exception>
    public T Add(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        if (entity.Id != Guid.Empty && Exists(entity.Id)) throw new ArgumentException($"Entity with id {entity.Id} already exists");
        return DbSet.Add(entity).Entity;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">Entity is null.</exception>
    /// <exception cref="KeyNotFoundException">Entity was not found.</exception>
    public T Update(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        if (entity.Id == Guid.Empty || !Exists(entity.Id)) throw new KeyNotFoundException($"No {typeof(T).Name} with id {entity.Id} found");
        return DbSet.Update(entity).Entity;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Entity was not found.</exception>
    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (!Exists(id)) throw new KeyNotFoundException($"No {typeof(T).Name} with id {id} found");
        DbSet.Remove(Get(id));
    }
}