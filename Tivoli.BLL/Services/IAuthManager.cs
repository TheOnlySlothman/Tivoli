using Tivoli.BLL.DTO;

namespace Tivoli.BLL.Services;

public interface IAuthManager
{
    Task<bool> ValidateUser(LoginDto model);
    Task<string> CreateToken();
}