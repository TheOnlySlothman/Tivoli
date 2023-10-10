using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tivoli.CustomerApi.Controllers;

[Controller]
[Authorize]
public class CustomerController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomer()
    {
        return Ok();
    }
}