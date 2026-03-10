namespace SocialX.Service.DTOs;

public class AuthResponse
{
    public bool Success { get; set; }
    public bool RequiresRegistration { get; set; }
    public PessoaDto? User { get; set; }
    public string? Message { get; set; }
}
