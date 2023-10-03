﻿using Microsoft.AspNetCore.Mvc;
using Tivoli.Dal.Repo;
using Tivoli.Models.DTO;
using Tivoli.Models.Entity;

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