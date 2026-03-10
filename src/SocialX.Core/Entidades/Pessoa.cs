using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Pessoa
{
    public long Id { get; private set; }
    public string Nome { get; private set; }
    public string Apelido { get; private set; }
    public string Telefone { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Email { get; private set; }
    public string? FotoPerfil { get; private set; }
    public string? Bio { get; private set; }
    public string? Cidade { get; private set; }
    public RoleUsuarioEnum Role { get; private set; }
    public StatusContaEnum StatusConta { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? UltimoAcesso { get; private set; }

    // Navigation property
    public ICollection<LoginSocial> LoginsSociais { get; private set; } = new List<LoginSocial>();

    private Pessoa()
    {
        // Construtor privado para o EF Core
    }

    public Pessoa(
        string nome,
        string apelido,
        string telefone,
        DateTime dataNascimento,
        string email)
    {
        Nome = nome;
        Apelido = apelido;
        Telefone = telefone;
        DataNascimento = dataNascimento;
        Email = email;
        Role = RoleUsuarioEnum.Usuario;
        StatusConta = StatusContaEnum.Ativa;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(
        string nome,
        string telefone,
        string email,
        string? fotoPerfil,
        string? bio,
        string? cidade)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;
        FotoPerfil = fotoPerfil;
        Bio = bio;
        Cidade = cidade;
    }

    public void AtualizarPerfil(
        string apelido,
        string telefone,
        DateTime dataNascimento,
        string? fotoPerfil,
        string? bio,
        string? cidade)
    {
        Apelido = apelido;
        Telefone = telefone;
        DataNascimento = dataNascimento;
        FotoPerfil = fotoPerfil;
        Bio = bio;
        Cidade = cidade;
    }

    public void AtualizarUltimoAcesso()
    {
        UltimoAcesso = DateTime.UtcNow;
    }

    public void AlterarRole(RoleUsuarioEnum novoRole)
    {
        Role = novoRole;
    }

    public void AlterarStatusConta(StatusContaEnum novoStatus)
    {
        StatusConta = novoStatus;
    }
}
