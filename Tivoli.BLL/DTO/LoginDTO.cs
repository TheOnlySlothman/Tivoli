﻿namespace Tivoli.BLL.DTO;

/// <summary>
///    This is a DTO (Data Transfer Object) for the Login method in the AccountController.
/// </summary>
public class LoginDto
{
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
    ///   Gets or sets the username of the user.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    ///   Gets or sets the password of the user.
    /// </summary>
    public string Password { get; init; }
}