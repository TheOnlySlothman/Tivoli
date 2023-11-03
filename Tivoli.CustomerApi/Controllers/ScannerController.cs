using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.DTO;
using Tivoli.BLL.Services;
using Tivoli.Dal.Entities;

namespace Tivoli.CustomerApi.Controllers;

[Controller]
[Route("[controller]")]
public class ScannerController : ControllerBase
{
    CardManager _cardManager;

    public ScannerController(CardManager cardManager)
    {
        _cardManager = cardManager;
    }

    [HttpPost("pay-with-card/{token}")]
    public ActionResult PayWithCard([FromBody] string CardData, string token)
    {
        decimal price = _entertainmentPrices.FirstOrDefault(ep => ep.Key == token).Value;
        ActionResult result = _cardManager.ValidateAndPayWithCard(CardData, price);
        return result;
    }

    [HttpGet("get-price/{token}")]
    public ActionResult GetPrice(string token)
    {
        decimal price = _entertainmentPrices.FirstOrDefault(ep => ep.Key == token).Value;
        return Ok(price);
    }

    [HttpGet("ValidateAndPayWithCard/{token}")]
    public ActionResult ValidateAndPayWithCard(string cardData, string token)
    {
        decimal price = _entertainmentPrices.FirstOrDefault(ep => ep.Key == token).Value;
        ActionResult result = _cardManager.ValidateAndPayWithCard(cardData, price);
        return result;
    }

    private readonly List<KeyValuePair<string, decimal>> _entertainmentPrices = new()
    {
        new KeyValuePair<string, decimal>("f2f48034-5827-4e53-9d45-ac320501b3a6", 11)
    };

    [HttpGet("ping")]
    public ActionResult Ping()
    {
        return Ok("pong");
    }
}