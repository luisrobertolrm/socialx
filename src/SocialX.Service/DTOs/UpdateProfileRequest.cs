using System.ComponentModel.DataAnnotations;

namespace SocialX.Service.DTOs;

public class UpdateProfileRequest
{
    [Required(ErrorMessage = "Apelido é obrigatório")]
    [MaxLength(50, ErrorMessage = "Apelido deve ter no máximo 50 caracteres")]
    public string Apelido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [Phone(ErrorMessage = "Formato de telefone inválido")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    [MaxLength(500, ErrorMessage = "URL da foto de perfil deve ter no máximo 500 caracteres")]
    public string? FotoPerfil { get; set; }

    [MaxLength(500, ErrorMessage = "Bio deve ter no máximo 500 caracteres")]
    public string? Bio { get; set; }

    [MaxLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
    public string? Cidade { get; set; }
}
