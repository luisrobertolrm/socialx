namespace SocialX.Service.DTOs;

public class UpdateProfileDto
{
    public string Apelido { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? FotoPerfil { get; set; }
    public string? Bio { get; set; }
    public string? Cidade { get; set; }
}
