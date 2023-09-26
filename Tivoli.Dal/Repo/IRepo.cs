using System.Linq.Expressions;
using Tivoli.Models;
// ReSharper disable UnusedMemberInSuper.Global

namespace Tivoli.Dal.Repo;

/// <summary>
/// Generic interface for all repositories.
/// </summary>
/// <typeparam name="T">Type to base repo on.</typeparam>
public interface IRepo<T> where T : class, IEntity
{
    /// <summary>
    ///     Get a single entity by id.
    /// </summary>
    /// <param name="id">Id of entity to get.</param>
    /// <returns>Entity with given id.</returns>
    public T Get(Guid id);
    
    /// <summary>
    ///     Get a single entity by predicate.
    /// </summary>
    /// <param name="predicate">Function to test each element for a condition.</param>
    /// <returns>The first entity that fulfill <paramref name="predicate"/>.</returns>
    public T? Get(Expression<Func<T, bool>> predicate);

    /// <summary>
    ///     Get a single entity by id with related entities.
    /// </summary>
    /// <param name="id">Id of entity to get.</param>
    /// <param name="relations">Relations to include.</param>
    /// <returns>Entity with given id.</returns>
    public T GetWithRelated(Guid id, params Expression<Func<T, object?>>[] relations);
    
    /// <summary>
    ///    Get a single entity by predicate with related entities.
    /// </summary>
    /// <param name="predicate">Function to test each element for a condition.</param>
    /// <param name="relations">Relations to include.</param>
    /// <returns>The first entity that fulfill <paramref name="predicate"/>.</returns>
    public T? GetWithRelated(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] relations);
    
    /// <summary>
    ///     Get a single entity by id without tracking.
    /// </summary>
    /// <param name="id">Id of entity to get.</param>
    /// <returns>Entity with given id.</returns>
    public T GetAsNoTracking(Guid id);
    
    /// <summary>
    ///     Get a single entity by predicate without tracking.
    /// </summary>
    /// <param name="predicate">Function to test each element for a condition.</param>
    /// <returns>The first entity that fulfill <paramref name="predicate"/>.</returns>
    public T GetAsNoTracking(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    ///     Get all entities.
    /// </summary>
    /// <returns>All entities.</returns>
    public IEnumerable<T> GetAll();
    
    /// <summary>
    ///     Get all entities by predicate.
    /// </summary>
    /// <param name="predicate">Function to test each element for a condition.</param>
    /// <returns>The entities that fulfill <paramref name="predicate"/>.</returns>
    public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    ///     Get all entities without tracking.
    /// </summary>
    /// <returns>All entities without tracking.</returns>
    public IEnumerable<T> GetAllAsNoTracking();
    
    /// <summary>
    ///     Get all entities by predicate without tracking.
    /// </summary>
    /// <param name="predicate">Function to test each element for a condition.</param>
    /// <returns>The entities that fulfill <paramref name="predicate"/>.</returns>
    public IEnumerable<T> GetAllAsNoTracking(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    ///     Check if entity with given id exists.
    /// </summary>
    /// <param name="id">Id of entity to check.</param>
    /// <returns><c>true</c> if entity with provided id exists; otherwise, <c>false</c>.</returns>
    public bool Exists(Guid id);
    
    /// <summary>
    ///     Add entity to database.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <returns>Added entity.</returns>
    public T Add(T entity);
    
    /// <summary>
    ///     Update entity in database.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <returns>Updated Entity.</returns>
    public T Update(T entity);
    
    /// <summary>
    ///     Delete entity from database.
    /// </summary>
    /// <param name="id">Id of entity to delete.</param>
    public void Delete(Guid id);
}