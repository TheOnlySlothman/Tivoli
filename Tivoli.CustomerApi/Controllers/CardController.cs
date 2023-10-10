using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.DTO;
using Tivoli.BLL.Services;
using Tivoli.Dal.Entities;

namespace Tivoli.CustomerApi.Controllers;

[Controller]
[Route("[controller]")]
[Authorize]
public class CardController : ControllerBase
{
    private UserManager<Customer> _userManager;
    private readonly CardManager _cardManager;

    public CardController(UserManager<Customer> userManager, CardManager cardManager)
    {
        _userManager = userManager;
        _cardManager = cardManager;
    }

    // [HttpGet("CreateCard")]
    // public IActionResult CreateCard()
    // {
    //     Customer? user = _userManager.GetUserAsync(User).Result;
    //     if (user is null) return Forbid();
    //     _cardManager.CreateCard(user);
    //     return Ok();
    // }

    [HttpGet("Cards")]
    public IActionResult GetCards()
    {
        string? userId = _userManager.GetUserId(User);

        if (userId is null) return Forbid();

        IEnumerable<CardDto> cards = _cardManager.GetUserCards(Guid.Parse(userId));
        return Ok(cards);
    }
}