namespace SocialX.Core.Entidades;

public class Notificacao
{
    public long Id { get; private set; }
    public long PessoaId { get; private set; }
    public string Tipo { get; private set; }
    public string Titulo { get; private set; }
    public string Mensagem { get; private set; }
    public string? ReferenciaTipo { get; private set; }
    public long? ReferenciaId { get; private set; }
    public bool Lida { get; private set; }
    public DateTime DataEnvio { get; private set; }

    public Pessoa Pessoa { get; private set; } = null!;

    private Notificacao()
    {
        // Construtor privado para o EF Core
    }

    public Notificacao(
        long pessoaId,
        string tipo,
        string titulo,
        string mensagem,
        string? referenciaTipo = null,
        long? referenciaId = null)
    {
        PessoaId = pessoaId;
        Tipo = tipo;
        Titulo = titulo;
        Mensagem = mensagem;
        ReferenciaTipo = referenciaTipo;
        ReferenciaId = referenciaId;
        Lida = false;
        DataEnvio = DateTime.UtcNow;
    }

    public void MarcarComoLida()
    {
        Lida = true;
    }
}
