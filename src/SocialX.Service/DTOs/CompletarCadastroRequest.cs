using System.ComponentModel.DataAnnotations;

namespace SocialX.Service.DTOs;

public class CompletarCadastroRequest
{
    [Required(ErrorMessage = "IdToken é obrigatório")]
    public string IdToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "Apelido é obrigatório")]
    [MaxLength(50, ErrorMessage = "Apelido deve ter no máximo 50 caracteres")]
    public string Apelido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [Phone(ErrorMessage = "Formato de telefone inválido")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }
}
