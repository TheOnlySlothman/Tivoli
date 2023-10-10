using Mapster;
using Microsoft.AspNetCore.Mvc;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.AdminApi.Controllers;

/// <summary>
///     BaseClass for all controllers that implement CRUD.
/// </summary>
/// <typeparam name="T">Entity Type used in controller.</typeparam>
/// <typeparam name="TDto">Dto for <typeparamref name="T"/> entity.</typeparam>
public abstract class BaseCrudController<T, TDto> : ControllerBase
    where T : class, IEntity, new() where TDto : class, IEntity, new()
{
    /// <summary>
    ///    UnitOfWork for the controller.
    /// </summary>
    private readonly UnitOfWork _unitOfWork;

    /// <summary>
    ///   Repo for the controller.
    /// </summary>
    private readonly BaseRepo<T> _repo;

    /// <summary>
    ///    Constructor for the BaseCrudController class.
    /// </summary>
    /// <param name="unitOfWork">Unit of work to use in controller.</param>
    /// <param name="repoFunc">Function pointing to repo in UnitOfWork.</param>
    protected BaseCrudController(UnitOfWork unitOfWork, Func<UnitOfWork, BaseRepo<T>> repoFunc)
    {
        _unitOfWork = unitOfWork;
        _repo = repoFunc(unitOfWork);
    }

    /// <summary>
    ///   Create a new entity.
    /// </summary>
    /// <param name="entityDto">Dto to create entity from.</param>
    /// <returns>OK response with entity as dto if successful.
    /// Otherwise if ModelState is invalid, returns <c>BadRequest</c> response.</returns>
    [HttpPost("create")]
    public Task<IActionResult> Create([FromBody] TDto entityDto)
    {
        if (!ModelState.IsValid) return Task.FromResult<IActionResult>(BadRequest(entityDto));

        while (entityDto.Id != Guid.Empty && _repo.Exists(entityDto.Id)) entityDto.Id = Guid.NewGuid();

        T entity = entityDto.Adapt<T>();
        _repo.Add(entity);
        _unitOfWork.SaveChanges();
        return Task.FromResult<IActionResult>(Ok(entity.Adapt<TDto>()));
    }

    /// <summary>
    ///     Read an entity.
    /// </summary>
    /// <param name="id">Id of entity.</param>
    /// <returns>OK response with entity as dto if successful.
    /// Otherwise if no entity with <paramref name="id"/> exists, returns <c>NotFound</c> response.</returns>
    [HttpGet("{id:guid}")]
    public Task<IActionResult> Read(Guid id)
    {
        if (!_repo.Exists(id))
            return Task.FromResult<IActionResult>(NotFound(id));

        T entity = _repo.Get(id);
        TDto entityDto = entity.Adapt<TDto>();
        return Task.FromResult<IActionResult>(Ok(entityDto));
    }

    /// <summary>
    ///    Read all entities.
    /// </summary>
    /// <returns>Ok response with entities as dto.</returns>
    [HttpGet]
    public Task<IActionResult> ReadAll()
    {
        IEnumerable<T> entities = _repo.GetAll();
        IEnumerable<TDto> dtoCollection = entities.Adapt<IEnumerable<TDto>>();
        return Task.FromResult<IActionResult>(Ok(dtoCollection));
    }

    /// <summary>
    ///    Update an entity.
    /// </summary>
    /// <param name="id">Id of entity to update.</param>
    /// <param name="entityDto">Dto to update entity with.</param>
    /// <returns>Returns <c>Ok</c> response with updated entity as dto.</returns>
    [HttpPut("{id:guid}")]
    public Task<IActionResult> Update(Guid id, [FromBody] TDto entityDto)
    {
        if (!id.Equals(entityDto.Id)) return Task.FromResult<IActionResult>(BadRequest(id));
        if (!ModelState.IsValid) return Task.FromResult<IActionResult>(BadRequest(entityDto));
        if (!_repo.Exists(entityDto.Id)) return Task.FromResult<IActionResult>(NotFound(entityDto.Id));

        T entity = _repo.Get(id);
        entityDto.Adapt(entity);

        _repo.Update(entity);
        _unitOfWork.SaveChanges();
        return Task.FromResult<IActionResult>(Ok(entityDto));
    }

    /// <summary>
    ///     Delete an entity.
    /// </summary>
    /// <param name="id">Id of entity to delete.</param>
    /// <returns><c>Ok</c> response if successful.
    /// Otherwise if no entity with <paramref name="id"/> exists, returns <c>NotFound</c>; response.</returns>
    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Delete(Guid id)
    {
        if (!_repo.Exists(id)) return Task.FromResult<IActionResult>(NotFound(id));
        _repo.Delete(id);
        _unitOfWork.SaveChanges();
        return Task.FromResult<IActionResult>(Ok());
    }
}