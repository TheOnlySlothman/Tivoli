using Microsoft.VisualStudio.TestPlatform.TestHost;
using Tivoli.Models;
using Tivoli.Models.DTO;
using Xunit.Abstractions;

namespace Tivoli.AdminTests.Integration.ApiControllers;

public abstract class BaseCrudControllerTests<T> : BaseControllerTests where T : class, IEntity, new()
{
    protected BaseCrudControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }
    
    protected abstract T ConstructModel();

    protected abstract CardDto ConstructDto();
}