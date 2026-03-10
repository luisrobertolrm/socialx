using SocialX.Core.Enums;

namespace SocialX.Service.DTOs;

public class PessoaDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Apelido { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FotoPerfil { get; set; }
    public string? Bio { get; set; }
    public string? Cidade { get; set; }
    public RoleUsuarioEnum Role { get; set; }
    public StatusContaEnum StatusConta { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? UltimoAcesso { get; set; }
}
