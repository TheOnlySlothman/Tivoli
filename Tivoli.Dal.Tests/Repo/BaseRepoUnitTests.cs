﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.Dal.Tests.Repo;

public abstract class BaseRepoUnitTests<T> : IRepoUnitTests where T : class, IEntity, new()
{
    private readonly TivoliContext _context;
    protected readonly UnitOfWork UnitOfWork;
    protected abstract BaseRepo<T> Repo { get; }

    private const int TestDataCount = 10;

    protected BaseRepoUnitTests()
    {
        DbContextOptions options = new DbContextOptionsBuilder<TivoliContext>()
            .UseInMemoryDatabase($"Tivoli-{Guid.NewGuid()}")
            .Options;
        _context = new TivoliContext(options);
        UnitOfWork = new UnitOfWork(_context);
    }

    protected abstract T CreateModel();
    protected abstract T UpdateModel(T model);

    protected abstract Expression<Func<T, object?>>[] GetRelations();

    private void UseDatabase(Action callback)
    {
        try
        {
            callback();
        }
        finally
        {
            // Cleanup
            _context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public void Get_ExistingEntityById_ReturnsEntity()
    {
        UseDatabase(() =>
            {
                // Arrange
                T model = CreateModel();
                Repo.Add(model);
                UnitOfWork.SaveChanges();
                // Act
                T result = Repo.Get(model.Id);

                // Assert
                Assert.Same(model, result);
            }
        );
    }

    [Fact]
    public void Get_NonExistingEntityById_ThrowsKeyNotFoundException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Func<Guid, T> act = Repo.Get;

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => act.Invoke(id));
        });
    }

    [Fact]
    public void Get_EmptyGuid_ThrowsArgumentException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.Empty;
            Func<Guid, T> act = Repo.Get;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => act.Invoke(id));
        });
    }

    [Fact]
    public void Get_ExistingEntityByPredicate_ReturnsEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();
            // Act
            T? result = Repo.Get(x => x.Id == model.Id);

            // Assert
            Assert.Same(model, result);
        });
    }

    [Fact]
    public void Get_NonExistingEntityByPredicate_ReturnsNull()
    {
        UseDatabase(() =>
        {
            // Act
            T? result = Repo.Get(x => x.Id == Guid.NewGuid());
            // Assert
            Assert.Null(result);
        });
    }

    [Fact]
    public void GetWithRelated_ExistingEntityById_ReturnsEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();

            Expression<Func<T, object?>>[] relations = GetRelations();

            // Act
            T result = Repo.GetWithRelated(model.Id, relations);
            // Assert
            Assert.Same(model, result);

            foreach (Expression<Func<T, object?>> relation in relations)
                Assert.NotNull(relation.Compile().Invoke(result));
        });
    }

    [Fact]
    public void GetWithRelated_NonExistingEntityById_ThrowsKeyNotFoundException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Func<Guid, Expression<Func<T, object?>>[], T> act = Repo.GetWithRelated;

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => act.Invoke(id, GetRelations()));
        });
    }

    [Fact]
    public void GetWithRelated_EmptyGuid_ThrowsArgumentException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.Empty;
            Func<Guid, Expression<Func<T, object?>>[], T> act = Repo.GetWithRelated;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => act.Invoke(id, GetRelations()));
        });
    }

    [Fact]
    public void GetWithRelated_ExistingEntityByPredicate_ReturnsEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();

            Expression<Func<T, object?>>[] relations = GetRelations();

            // Act
            T? result = Repo.GetWithRelated(x => x.Id == model.Id, relations);
            // Assert
            Assert.Same(model, result);
            Assert.NotNull(result);

            foreach (Expression<Func<T, object?>> relation in relations)
                Assert.NotNull(relation.Compile().Invoke(result));
        });
    }

    [Fact]
    public void GetWithRelated_NonExistingEntityByPredicate_ReturnsNull()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            T? result = Repo.GetWithRelated(x => x.Id == id, GetRelations());

            // Assert
            Assert.Null(result);
        });
    }

    [Fact]
    public void GetAsNoTracking_ExistingEntity_ReturnsEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();
            // Act
            T result = Repo.GetAsNoTracking(model.Id);

            // Assert
            Assert.Equal(model.Id, result.Id);
            Assert.NotSame(model, result);
        });
    }

    [Fact]
    public void GetAsNoTracking_NonExistingEntity_ThrowsKeyNotFoundException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Func<Guid, T> act = Repo.GetAsNoTracking;

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => act.Invoke(id));
        });
    }

    [Fact]
    public void GetAsNoTracking_EmptyGuid_ThrowsArgumentException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.Empty;
            Func<Guid, T> act = Repo.GetAsNoTracking;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => act.Invoke(id));
        });
    }

    [Fact]
    public void GetAll_ReturnsAllEntities()
    {
        UseDatabase(() =>
        {
            // Arrange
            List<T> models = new byte[TestDataCount].Select(_ => CreateModel()).ToList();
            models.ForEach(x => Repo.Add(x));
            UnitOfWork.SaveChanges();
            // Act
            IEnumerable<T> result = Repo.GetAll().ToList();

            // Assert
            Assert.Equal(models.Count, result.Count());
            foreach (T model in models)
                Assert.Contains(model, result);
        });
    }

    [Fact]
    public void GetAllAsNoTracking_ReturnsAllEntities()
    {
        UseDatabase(() =>
        {
            // Arrange
            List<T> models = new byte[TestDataCount].Select(_ => CreateModel()).ToList();
            models.ForEach(x => Repo.Add(x));
            UnitOfWork.SaveChanges();
            // Act
            IEnumerable<T> result = Repo.GetAllAsNoTracking().ToList();

            // Assert
            Assert.Equal(models.Count, result.Count());
            Assert.Contains(result, entity => models.Any(model => model.Id == entity.Id));
        });
    }

    [Fact]
    public void Exists_ExistingEntity_ReturnsTrue()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();
            // Act
            bool result = Repo.Exists(model.Id);

            // Assert
            Assert.True(result);
        });
    }

    [Fact]
    public void Exists_NonExistingEntity_ReturnsFalse()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.NewGuid();
            // Act
            bool result = Repo.Exists(id);

            // Assert
            Assert.False(result);
        });
    }

    [Fact]
    public void Exists_EmptyGuid_ThrowsArgumentException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.Empty;
            Func<Guid, bool> act = Repo.Exists;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => act.Invoke(id));
        });
    }

    [Fact]
    public void Add_ValidEntity_AddsEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            // Act
            T result = Repo.Add(model);
            UnitOfWork.SaveChanges();

            // Assert
            Assert.Same(model, result);
        });
    }

    [Fact]
    public void Add_NullEntity_ThrowsArgumentNullException()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = null!;
            Func<T, T> act = Repo.Add;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => act.Invoke(model));
        });
    }

    [Fact]
    public void Add_EntityWithExistingId_ThrowsArgumentException()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();
            Func<T, T> act = Repo.Add;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => act.Invoke(model));
        });
    }


    [Fact]
    public void Update_ValidEntity_UpdatesEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();
            _ = UpdateModel(model);
            // Act
            T result = Repo.Update(model);
            UnitOfWork.SaveChanges();

            // Assert
            Assert.Same(model, result);
        });
    }


    [Fact]
    public void Update_NullEntity_ThrowsArgumentNullException()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = null!;
            Func<T, T> act = Repo.Update;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => act.Invoke(model));
        });
    }

    [Fact]
    public void Update_EntityWithEmptyGuid_ThrowsKeyNotFoundException()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            model.Id = Guid.Empty;
            Func<T, T> act = Repo.Update;

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => act.Invoke(model));
        });
    }

    [Fact]
    public void Update_EntityWithNonExistingId_ThrowsKeyNotFoundException()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            model.Id = Guid.NewGuid();
            Func<T, T> act = Repo.Update;

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => act.Invoke(model));
        });
    }

    [Fact]
    public void Delete_ExistingEntity_DeletesEntity()
    {
        UseDatabase(() =>
        {
            // Arrange
            T model = CreateModel();
            Repo.Add(model);
            UnitOfWork.SaveChanges();
            // Act
            Repo.Delete(model.Id);
            UnitOfWork.SaveChanges();

            // Assert
            Assert.False(Repo.Exists(model.Id));
        });
    }

    [Fact]
    public void Delete_NonExistingEntity_ThrowsKeyNotFoundException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Action<Guid> act = Repo.Delete;

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => act.Invoke(id));
        });
    }

    [Fact]
    public void Delete_EmptyGuid_ThrowsArgumentException()
    {
        UseDatabase(() =>
        {
            // Arrange
            Guid id = Guid.Empty;
            Action<Guid> act = Repo.Delete;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => act.Invoke(id));
        });
    }
}