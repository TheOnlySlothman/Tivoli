namespace Tivoli.AdminApi.Models;

/// <summary>
///    This is a DTO (Data Transfer Object) for the Login method in the AccountController.
/// </summary>
public class LoginDto
{
    /// <summary>
    ///     Constructor for the LoginDto class.
    /// </summary>
    public LoginDto()
    {
        
    }
    
    /// <summary>
    ///     Constructor for the LoginDto class.
    /// </summary>
    /// <param name="username">Username of User.</param>
    /// <param name="password">Password of User.</param>
    public LoginDto(string username, string password)
    {
        Username = username;
        Password = password;
    }

    /// <summary>
    ///    Username of User.
    /// </summary>
    public required string Username { get; init; }
    /// <summary>
    ///   Password of User.
    /// </summary>
    public required string Password { get; init; }
}