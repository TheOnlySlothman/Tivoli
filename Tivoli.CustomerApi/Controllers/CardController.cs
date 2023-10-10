using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.CustomerApi.Controllers;

[Controller]
[Route("[controller]")]
[Authorize]
public class CardController : ControllerBase
{
    private UnitOfWork _unitOfWork;
    private CardRepo _repo;

    private UserManager<Customer> _userManager;

    public CardController(UnitOfWork unitOfWork, UserManager<Customer> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _repo = unitOfWork.Cards;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CardDto request)
    {
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCards()
    {
        List<Customer> users = _unitOfWork.Customers.GetAll().ToList();
        string? t = _userManager.GetUserId(User);
        Customer? user = _userManager.GetUserAsync(User).Result;

        if (t is null && user is null)
        {
            // _userManager.AddClaimAsync(user, new Claim("CustomerId", ));
        }


        return null;
        // if (t is null) return Forbid();
        //
        // Guid customerId = Guid.Parse(t);
        // IEnumerable<Card> cards = _repo.GetCardsByCustomerId(customerId);
        // return Ok(cards);
    }
}