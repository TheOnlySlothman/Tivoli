using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.Dal;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public abstract class BaseCrudControllerTests<T, TDto> : BaseControllerTests where T : class, IEntity, new()
{
    protected abstract string ControllerName { get; }

    protected UnitOfWork UnitOfWork { get; set; }

    // protected UnitOfWork UnitOfWork => new(Factory.Services.CreateScope().ServiceProvider.GetRequiredService<TivoliContext>());
    protected abstract BaseRepo<T> Repo { get; }

    protected BaseCrudControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) :
        base(factory, testOutputHelper)
    {
        UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();
    }

    protected abstract T ConstructModel();

    protected abstract TDto ConstructDto();
}