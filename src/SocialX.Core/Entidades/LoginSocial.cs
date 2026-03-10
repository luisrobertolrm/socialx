using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class LoginSocial
{
    public long Id { get; private set; }
    public long PessoaId { get; private set; }
    public ProviderEnum Provider { get; private set; }
    public string ProviderUserId { get; private set; }
    public string EmailProvider { get; private set; }
    public DateTime DataVinculo { get; private set; }

    // Navigation property
    public Pessoa Pessoa { get; private set; } = null!;

    private LoginSocial()
    {
        // Construtor privado para o EF Core
    }

    public LoginSocial(
        long pessoaId,
        ProviderEnum provider,
        string providerUserId,
        string emailProvider)
    {
        PessoaId = pessoaId;
        Provider = provider;
        ProviderUserId = providerUserId;
        EmailProvider = emailProvider;
        DataVinculo = DateTime.UtcNow;
    }
}
