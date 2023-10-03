using Microsoft.AspNetCore.Mvc;
using Tivoli.Dal.Repo;
using Tivoli.Models.DTO;
using Tivoli.Models.Entity;

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