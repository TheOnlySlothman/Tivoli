using Mapster;
using Microsoft.AspNetCore.Mvc;
using Tivoli.Dal.Repo;
using Tivoli.Models;

namespace Tivoli.AdminApi.Controllers;

public abstract class BaseCrudController<T, TDto> : ControllerBase where T : class, IEntity, new() where TDto : class, IEntity, new()
{
    protected readonly UnitOfWork UnitOfWork;
    protected readonly BaseRepo<T> Repo;

    protected BaseCrudController(UnitOfWork unitOfWork, Func<UnitOfWork, BaseRepo<T>> repoFunc)
    {
        UnitOfWork = unitOfWork;
        Repo = repoFunc(unitOfWork);
    }
    
    [HttpPost("create")]
    public Task<IActionResult> Create([FromBody] TDto entityDto)
    {
        if (!ModelState.IsValid) return Task.FromResult<IActionResult>(BadRequest(entityDto));

        while (entityDto.Id != Guid.Empty && Repo.Exists(entityDto.Id)) entityDto.Id = Guid.NewGuid();
        
        T entity = entityDto.Adapt<T>();
        Repo.Add(entity);
        UnitOfWork.SaveChanges();
        return Task.FromResult<IActionResult>(Ok(entity.Adapt<TDto>()));
    }
    
    [HttpGet("{id:guid}")]
    public Task<IActionResult> Read(Guid id)
    {
        if (!Repo.Exists(id))
            return Task.FromResult<IActionResult>(NotFound(id));
        
        T entity = Repo.Get(id);
        TDto entityDto = entity.Adapt<TDto>();
        return Task.FromResult<IActionResult>(Ok(entityDto));
    }
    
    [HttpGet]
    public Task<IActionResult> ReadAll()
    {
        IEnumerable<T> entities = Repo.GetAll();
        IEnumerable<TDto> dtoCollection = entities.Adapt<IEnumerable<TDto>>();
        return Task.FromResult<IActionResult>(Ok(dtoCollection));
    }
    
    [HttpPut("{id:guid}")]
    public Task<IActionResult> Update(Guid id, [FromBody] TDto entityDto)
    {
        if (!id.Equals(entityDto.Id)) return Task.FromResult<IActionResult>(BadRequest(id));
        if (!ModelState.IsValid) return Task.FromResult<IActionResult>(BadRequest(entityDto));
        if (!Repo.Exists(entityDto.Id)) return Task.FromResult<IActionResult>(NotFound(entityDto.Id));
        
        T entity = Repo.Get(id);
        entityDto.Adapt(entity);
        
        Repo.Update(entity);
        UnitOfWork.SaveChanges();
        return Task.FromResult<IActionResult>(Ok(entityDto));
    }
    
    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Delete(Guid id)
    {
        if (!Repo.Exists(id)) return Task.FromResult<IActionResult>(NotFound(id));
        Repo.Delete(id);
        UnitOfWork.SaveChanges();
        return Task.FromResult<IActionResult>(Ok());
    }
}