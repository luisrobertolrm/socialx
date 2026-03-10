using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class ParticipacaoEvento
{
    public long Id { get; private set; }
    public long EventoId { get; private set; }
    public long PessoaId { get; private set; }
    public StatusParticipacaoEventoEnum StatusParticipacao { get; private set; }
    public string? Observacao { get; private set; }
    public DateTime DataResposta { get; private set; }

    public Evento Evento { get; private set; } = null!;
    public Pessoa Pessoa { get; private set; } = null!;

    private ParticipacaoEvento()
    {
        // Construtor privado para o EF Core
    }

    public ParticipacaoEvento(
        long eventoId,
        long pessoaId,
        StatusParticipacaoEventoEnum statusParticipacao,
        string? observacao = null)
    {
        EventoId = eventoId;
        PessoaId = pessoaId;
        StatusParticipacao = statusParticipacao;
        Observacao = observacao;
        DataResposta = DateTime.UtcNow;
    }

    public void AtualizarStatus(StatusParticipacaoEventoEnum novoStatus, string? observacao = null)
    {
        StatusParticipacao = novoStatus;
        Observacao = observacao;
        DataResposta = DateTime.UtcNow;
    }
}
