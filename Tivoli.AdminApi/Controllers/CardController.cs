using Microsoft.AspNetCore.Mvc;
using Tivoli.Dal.Repo;
using Tivoli.Models.DTO;
using Tivoli.Models.Entity;

namespace Tivoli.AdminApi.Controllers;

[Controller]
[Route("[controller]")]
public class CardController : BaseCrudController<Card, CardDto>
{
    public CardController(UnitOfWork unitOfWork) : base(unitOfWork, uow => uow.Cards)
    {
    }
}