using System.ComponentModel.DataAnnotations;

namespace SocialX.Service.DTOs;

public class ValidateGoogleTokenRequest
{
    [Required(ErrorMessage = "IdToken é obrigatório")]
    public string IdToken { get; set; } = string.Empty;
}
