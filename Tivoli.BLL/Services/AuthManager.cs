using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tivoli.BLL.Models;
using Tivoli.Dal.Entities;

namespace Tivoli.BLL.Services;

/// <summary>
///     This is a class for managing authentication.
/// </summary>
public class AuthManager : IAuthManager
{
    private readonly UserManager<Customer> _userManager;
    private readonly IConfiguration _configuration;
    private Customer? _user;

    public AuthManager(UserManager<Customer> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> ValidateUser(LoginDto model)
    {
        Customer? user = await _userManager.FindByNameAsync(model.Username);
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password)) return false;
        _user = user;
        return true;
    }

    public async Task<string> CreateToken()
    {
        SigningCredentials signingCredentials = GetSigningCredentials();
        List<Claim> claims = await GetClaims();
        SecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }


    private SigningCredentials GetSigningCredentials()
    {
        string? key = Environment.GetEnvironmentVariable("TivoliApiKey");
        SymmetricSecurityKey secret = new(Encoding.UTF8.GetBytes(key ?? throw new InvalidOperationException()));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        if (_user?.UserName is null) return new List<Claim>();

        IList<string> roles = await _userManager.GetRolesAsync(_user);
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private SecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");
        JwtSecurityToken token = new(
            issuer: jwtSettings.GetSection("Issuer").Value,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
        );
        return token;
    }

    public async Task<string> CreateRefreshToken()
    {
        if (_user is null) return string.Empty;
        await _userManager.RemoveAuthenticationTokenAsync(_user, "TivoliApi", "RefreshToken");
        string newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, "TivoliApi", "RefreshToken");
        await _userManager.SetAuthenticationTokenAsync(_user, "TivoliApi", "RefreshToken", newRefreshToken);
        return newRefreshToken;
    }
}