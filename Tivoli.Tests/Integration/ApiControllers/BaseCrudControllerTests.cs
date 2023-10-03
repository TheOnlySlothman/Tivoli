using Tivoli.Dal.Repo;
using Tivoli.Models;
using Xunit.Abstractions;

namespace Tivoli.AdminTests.Integration.ApiControllers;

public abstract class BaseCrudControllerTests<T, TDto> : BaseControllerTests where T : class, IEntity, new()
{
    protected abstract BaseRepo<T> Repo { get; }

    protected BaseCrudControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    protected abstract T ConstructModel();

    protected abstract TDto ConstructDto();
}