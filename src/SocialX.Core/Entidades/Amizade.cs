using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Amizade
{
    public long Id { get; private set; }
    public long PessoaOrigemId { get; private set; }
    public long PessoaDestinoId { get; private set; }
    public StatusAmizadeEnum Status { get; private set; }
    public DateTime DataSolicitacao { get; private set; }
    public DateTime? DataResposta { get; private set; }

    public Pessoa PessoaOrigem { get; private set; } = null!;
    public Pessoa PessoaDestino { get; private set; } = null!;

    private Amizade()
    {
        // Construtor privado para o EF Core
    }

    public Amizade(long pessoaOrigemId, long pessoaDestinoId)
    {
        PessoaOrigemId = pessoaOrigemId;
        PessoaDestinoId = pessoaDestinoId;
        Status = StatusAmizadeEnum.Pendente;
        DataSolicitacao = DateTime.UtcNow;
    }

    public void Aceitar()
    {
        Status = StatusAmizadeEnum.Aceita;
        DataResposta = DateTime.UtcNow;
    }

    public void Recusar()
    {
        Status = StatusAmizadeEnum.Recusada;
        DataResposta = DateTime.UtcNow;
    }

    public void Bloquear()
    {
        Status = StatusAmizadeEnum.Bloqueada;
        DataResposta = DateTime.UtcNow;
    }
}
