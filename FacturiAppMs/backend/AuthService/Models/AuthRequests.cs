namespace AuthService.Models;

public class RegisterRequest
{
    public string Username { get; set; } = "";
    public string Nume { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class ResetPasswordRequest
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string NouaParola { get; set; } = "";
}
