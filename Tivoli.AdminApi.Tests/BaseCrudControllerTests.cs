using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.AdminApi.Tests;

public abstract class BaseCrudControllerTests<T, TDto> : BaseControllerTests where T : class, IEntity, new()
{
    protected abstract string ControllerName { get; }
    protected abstract BaseRepo<T> Repo { get; }

    protected BaseCrudControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    protected abstract T ConstructModel();

    protected abstract TDto ConstructDto();
}