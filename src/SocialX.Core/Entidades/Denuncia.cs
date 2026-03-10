using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Denuncia
{
    public long Id { get; private set; }
    public long DenunciantePessoaId { get; private set; }
    public TipoAlvoEnum TipoAlvo { get; private set; }
    public long AlvoId { get; private set; }
    public string Motivo { get; private set; }
    public string? Descricao { get; private set; }
    public StatusDenunciaEnum Status { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public long? ResolvidoPorAdminId { get; private set; }
    public DateTime? ResolvidoEm { get; private set; }

    public Pessoa Denunciante { get; private set; } = null!;
    public Pessoa? ResolvidoPor { get; private set; }

    private Denuncia()
    {
        // Construtor privado para o EF Core
    }

    public Denuncia(
        long denunciantePessoaId,
        TipoAlvoEnum tipoAlvo,
        long alvoId,
        string motivo,
        string? descricao = null)
    {
        DenunciantePessoaId = denunciantePessoaId;
        TipoAlvo = tipoAlvo;
        AlvoId = alvoId;
        Motivo = motivo;
        Descricao = descricao;
        Status = StatusDenunciaEnum.Aberta;
        CriadoEm = DateTime.UtcNow;
    }

    public void IniciarAnalise()
    {
        Status = StatusDenunciaEnum.EmAnalise;
    }

    public void Resolver(long resolvidoPorAdminId)
    {
        Status = StatusDenunciaEnum.Resolvida;
        ResolvidoPorAdminId = resolvidoPorAdminId;
        ResolvidoEm = DateTime.UtcNow;
    }

    public void Rejeitar(long resolvidoPorAdminId)
    {
        Status = StatusDenunciaEnum.Rejeitada;
        ResolvidoPorAdminId = resolvidoPorAdminId;
        ResolvidoEm = DateTime.UtcNow;
    }
}
