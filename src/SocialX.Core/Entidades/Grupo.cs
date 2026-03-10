using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Grupo
{
    public long Id { get; private set; }
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public string? FotoCapa { get; private set; }
    public string? FotoPerfil { get; private set; }
    public TipoPrivacidadeGrupoEnum TipoPrivacidade { get; private set; }
    public string? Regras { get; private set; }
    public StatusGrupoEnum Status { get; private set; }
    public long CriadoPorPessoaId { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public Pessoa CriadoPor { get; private set; } = null!;

    private Grupo()
    {
        // Construtor privado para o EF Core
    }

    public Grupo(
        string nome,
        TipoPrivacidadeGrupoEnum tipoPrivacidade,
        long criadoPorPessoaId,
        string? descricao = null)
    {
        Nome = nome;
        TipoPrivacidade = tipoPrivacidade;
        CriadoPorPessoaId = criadoPorPessoaId;
        Descricao = descricao;
        Status = StatusGrupoEnum.Ativo;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(
        string nome,
        string? descricao,
        string? fotoCapa,
        string? fotoPerfil,
        string? regras,
        TipoPrivacidadeGrupoEnum tipoPrivacidade)
    {
        Nome = nome;
        Descricao = descricao;
        FotoCapa = fotoCapa;
        FotoPerfil = fotoPerfil;
        Regras = regras;
        TipoPrivacidade = tipoPrivacidade;
    }

    public void AlterarStatus(StatusGrupoEnum novoStatus)
    {
        Status = novoStatus;
    }
}
