using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class GrupoMembro
{
    public long Id { get; private set; }
    public long GrupoId { get; private set; }
    public long PessoaId { get; private set; }
    public StatusGrupoMembroEnum Status { get; private set; }
    public DateTime DataEntrada { get; private set; }
    public long? AprovadoPorPessoaId { get; private set; }
    public DateTime? DataAprovacao { get; private set; }

    public Grupo Grupo { get; private set; } = null!;
    public Pessoa Pessoa { get; private set; } = null!;
    public Pessoa? AprovadoPor { get; private set; }

    private GrupoMembro()
    {
        // Construtor privado para o EF Core
    }

    public GrupoMembro(long grupoId, long pessoaId, StatusGrupoMembroEnum status)
    {
        GrupoId = grupoId;
        PessoaId = pessoaId;
        Status = status;
        DataEntrada = DateTime.UtcNow;
    }

    public static GrupoMembro Criar(long grupoId, long pessoaId, bool requerAprovacao)
    {
        StatusGrupoMembroEnum status = requerAprovacao 
            ? StatusGrupoMembroEnum.PendenteAprovacao 
            : StatusGrupoMembroEnum.Ativo;
        
        return new GrupoMembro(grupoId, pessoaId, status);
    }

    public void Aprovar(long aprovadoPorPessoaId)
    {
        Status = StatusGrupoMembroEnum.Ativo;
        AprovadoPorPessoaId = aprovadoPorPessoaId;
        DataAprovacao = DateTime.UtcNow;
    }

    public void Remover()
    {
        Status = StatusGrupoMembroEnum.Removido;
    }

    public void Sair()
    {
        Status = StatusGrupoMembroEnum.Saiu;
    }

    public void Banir()
    {
        Status = StatusGrupoMembroEnum.Banido;
    }
}
