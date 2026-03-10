using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Evento
{
    public long Id { get; private set; }
    public long? GrupoId { get; private set; }
    public long CriadoPorPessoaId { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public TipoEventoEnum TipoEvento { get; private set; }
    public DateTime DataHoraInicio { get; private set; }
    public DateTime? DataHoraFim { get; private set; }
    public string? LocalNome { get; private set; }
    public string? Endereco { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string? LinkOnline { get; private set; }
    public int? CapacidadeMaxima { get; private set; }
    public PrivacidadeEventoEnum Privacidade { get; private set; }
    public StatusEventoEnum Status { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public Grupo? Grupo { get; private set; }
    public Pessoa CriadoPor { get; private set; } = null!;

    private Evento()
    {
        // Construtor privado para o EF Core
    }

    public Evento(
        string nome,
        string descricao,
        TipoEventoEnum tipoEvento,
        DateTime dataHoraInicio,
        PrivacidadeEventoEnum privacidade,
        long criadoPorPessoaId,
        long? grupoId = null)
    {
        Nome = nome;
        Descricao = descricao;
        TipoEvento = tipoEvento;
        DataHoraInicio = dataHoraInicio;
        Privacidade = privacidade;
        CriadoPorPessoaId = criadoPorPessoaId;
        GrupoId = grupoId;
        Status = StatusEventoEnum.Ativo;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(
        string nome,
        string descricao,
        DateTime dataHoraInicio,
        DateTime? dataHoraFim,
        string? localNome,
        string? endereco,
        decimal? latitude,
        decimal? longitude,
        string? linkOnline,
        int? capacidadeMaxima,
        PrivacidadeEventoEnum privacidade)
    {
        Nome = nome;
        Descricao = descricao;
        DataHoraInicio = dataHoraInicio;
        DataHoraFim = dataHoraFim;
        LocalNome = localNome;
        Endereco = endereco;
        Latitude = latitude;
        Longitude = longitude;
        LinkOnline = linkOnline;
        CapacidadeMaxima = capacidadeMaxima;
        Privacidade = privacidade;
    }

    public void AlterarStatus(StatusEventoEnum novoStatus)
    {
        Status = novoStatus;
    }
}
