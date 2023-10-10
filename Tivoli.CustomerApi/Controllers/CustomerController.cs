using Microsoft.AspNetCore.Mvc;

namespace Tivoli.CustomerApi.Controllers;

public class CustomerController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomer()
    {
        return Ok();
    }
}