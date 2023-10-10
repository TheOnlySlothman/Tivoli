using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tivoli.Models.DTO;

namespace Tivoli.CustomerApi.Controllers;

[Controller]
[Route("[controller]")]
[Authorize]
public class CardController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CardDto request)
    {
        return Ok();
    }
}