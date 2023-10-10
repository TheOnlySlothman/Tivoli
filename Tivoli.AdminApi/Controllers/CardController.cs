using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.AdminApi.Controllers;

/// <summary>
///    Controller for <see cref="Card"/> entity.
/// </summary>
[Controller]
[Route("[controller]")]
public class CardController : BaseCrudController<Card, CardDto>
{
    /// <inheritdoc />
    public CardController(UnitOfWork unitOfWork) : base(unitOfWork, uow => uow.Cards)
    {
    }
}