namespace SocialX.Service.DTOs;

public class UpdateProfileResponse
{
    public bool Success { get; set; }
    public PessoaDto User { get; set; } = null!;
    public string? Message { get; set; }
}
