namespace SocialX.Service.DTOs;

public class CompletarCadastroDto
{
    public string GoogleId { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FotoPerfil { get; set; }
    public string Apelido { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}
