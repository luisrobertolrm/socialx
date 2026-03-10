using SocialX.Core.Entidades;
using SocialX.Service.DTOs;

namespace SocialX.Service.Interfaces;

public interface IAuthService
{
    Task<LoginSocialVerificationResult> VerifyLoginSocialAsync(string googleId);
    Task<AccountStatusResult> CheckAccountStatusAsync(long pessoaId);
    Task UpdateLastAccessAsync(long pessoaId);
    Task<Pessoa> CreateUserAsync(CompletarCadastroDto dto);
    Task<Pessoa> UpdateProfileAsync(long pessoaId, UpdateProfileDto dto);
}

public class LoginSocialVerificationResult
{
    public bool Found { get; set; }
    public Pessoa? Pessoa { get; set; }
}

public class AccountStatusResult
{
    public bool IsActive { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsDeactivated { get; set; }
    public string? Message { get; set; }
}
