using System;

namespace AuthService.Models.Auth;

public class AuthResponse
{

    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string Username { get; set; } = string.Empty;

}
