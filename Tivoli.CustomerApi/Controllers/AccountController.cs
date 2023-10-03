using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tivoli.CustomerApi.Models;
using Tivoli.Models.Entity;

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
    private readonly SignInManager<Customer> _signInManager;

    /// <summary>
    ///    Constructor for the AccountController class.
    /// </summary>
    /// <param name="userManager">Service for managing users.</param>
    public AccountController(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     Register a new user.
    /// </summary>
    /// <param name="model">Dto to convert to <c>Customer</c>.</param>
    /// <returns>Ok if successful; otherwise if model is invalid; BadRequest.</returns>
    /// <exception cref="DbUpdateException">Creation of user in db was unsuccessful.</exception>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

        Customer user = new()
        {
            UserName = model.Username,
            Email = model.Username
        };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) throw new DbUpdateException(result.Errors.First().Description);

        // await _signInManager.SignInAsync(user, false);
        return Ok(model.Username);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        if (!result.Succeeded) return BadRequest(result);

        return Ok(result);
    }
}