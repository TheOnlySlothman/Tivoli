
// ReSharper disable UnusedMemberInSuper.Global

namespace Tivoli.AdminTests.Unit.Repo;

public interface IRepoUnitTests
{
    public void Get_ExistingEntityById_ReturnsEntity();
    public void Get_NonExistingEntityById_ThrowsKeyNotFoundException();
    public void Get_EmptyGuid_ThrowsArgumentException();
    public void Get_ExistingEntityByPredicate_ReturnsEntity();
    public void Get_NonExistingEntityByPredicate_ReturnsNull();
    public void GetWithRelated_ExistingEntityById_ReturnsEntity();
    public void GetWithRelated_NonExistingEntityById_ThrowsKeyNotFoundException();
    public void GetWithRelated_EmptyGuid_ThrowsArgumentException();
    public void GetWithRelated_ExistingEntityByPredicate_ReturnsEntity();
    public void GetWithRelated_NonExistingEntityByPredicate_ReturnsNull();
    public void GetAsNoTracking_ExistingEntity_ReturnsEntity();
    public void GetAsNoTracking_NonExistingEntity_ThrowsKeyNotFoundException();
    public void GetAsNoTracking_EmptyGuid_ThrowsArgumentException();
    public void GetAll_ReturnsAllEntities();
    public void GetAllAsNoTracking_ReturnsAllEntities();
    public void Exists_ExistingEntity_ReturnsTrue();
    public void Exists_NonExistingEntity_ReturnsFalse();
    public void Exists_EmptyGuid_ThrowsArgumentException();
    public void Add_ValidEntity_AddsEntity();
    public void Add_NullEntity_ThrowsArgumentNullException();
    public void Add_EntityWithExistingId_ThrowsArgumentException();
    public void Update_ValidEntity_UpdatesEntity();
    public void Update_NullEntity_ThrowsArgumentNullException();
    public void Update_EntityWithEmptyGuid_ThrowsArgumentException();
    public void Update_EntityWithNonExistingId_ThrowsKeyNotFoundException();
    public void Delete_ExistingEntity_DeletesEntity();
    public void Delete_NonExistingEntity_ThrowsKeyNotFoundException();
    public void Delete_EmptyGuid_ThrowsArgumentException();
}