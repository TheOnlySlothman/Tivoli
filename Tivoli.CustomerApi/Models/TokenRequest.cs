﻿namespace Tivoli.CustomerApi.Models;

public class TokenRequest
{
    public string Token { get; set; }
    public object RefreshToken { get; set; }
}