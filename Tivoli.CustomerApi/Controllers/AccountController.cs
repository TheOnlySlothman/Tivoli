using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.Models;
using Tivoli.BLL.Services;
using Tivoli.Dal.Entities;

namespace Tivoli.CustomerApi.Controllers;

/// <summary>
///   Controller for the Account management.
/// </summary>
[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<Customer> _userManager;
    private readonly AuthManager _authManager;

    public AccountController(UserManager<Customer> userManager, AuthManager authManager)
    {
        _userManager = userManager;
        _authManager = authManager;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

        Customer user = new()
        {
            UserName = model.Username,
            Email = model.Username
        };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
        }

        result = await _userManager.AddToRoleAsync(user, "Customer");
        if (result.Succeeded) return Accepted();
        {
            foreach (IdentityError error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            if (!await _authManager.ValidateUser(model)) return Unauthorized();

            return Accepted(new TokenRequest
                { Token = await _authManager.CreateToken() });

            // return Accepted(new TokenRequest
            // { Token = await _authManager.CreateToken(), RefreshToken = await _authManager.CreateRefreshToken() });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}