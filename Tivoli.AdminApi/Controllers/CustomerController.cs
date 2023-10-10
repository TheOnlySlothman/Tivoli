using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;


namespace Tivoli.AdminApi.Controllers;

/// <summary>
///   Controller for <see cref="Customer"/> entity.
/// </summary>
[Controller]
[Route("[controller]")]
public class CustomerController : BaseCrudController<Customer, CustomerDto>
{
    /// <inheritdoc />
    public CustomerController(UnitOfWork unitOfWork) : base(unitOfWork, uow => uow.Customers)
    {
    }
}