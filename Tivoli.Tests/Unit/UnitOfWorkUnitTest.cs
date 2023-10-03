using Microsoft.EntityFrameworkCore;
using Tivoli.Dal;
using Tivoli.Dal.Repo;

namespace Tivoli.AdminTests.Unit;

public class UnitOfWorkUnitTest
{
    [Fact]
    public void ValidateModelTest()
    {
        // Arrange
        DbContextOptions<TivoliContext> options = new DbContextOptionsBuilder<TivoliContext>()
            .UseSqlServer($"Server=localhost;Database=Tivoli-{Guid.NewGuid()};Trusted_Connection=True;")
            .Options;
        TivoliContext sqlDbContext = new(options);
        // Act
        UnitOfWork unitOfWork = new(sqlDbContext);
        // Assert
        Assert.NotNull(unitOfWork);
    }

    [Fact]
    public void CanConnect()
    {
        // Arrange
        UnitOfWork unitOfWork = CreateUnitOfWork();

        // Act
        bool result = unitOfWork.IsConnected();

        // Assert
        Assert.True(result);
    }

    private static UnitOfWork CreateUnitOfWork()
    {
        DbContextOptions<TivoliContext> options = new DbContextOptionsBuilder<TivoliContext>()
            .UseInMemoryDatabase($"Tivoli-{Guid.NewGuid()}")
            .Options;
        TivoliContext sqlDbContext = new(options);
        return new UnitOfWork(sqlDbContext);
    }
}