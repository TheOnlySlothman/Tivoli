using Tivoli.CustomerApi.Models;

namespace Tivoli.CustomerApi.Services;

public interface IAuthManager
{
    Task<bool> ValidateUser(LoginDto model);
    Task<string> CreateToken();
}