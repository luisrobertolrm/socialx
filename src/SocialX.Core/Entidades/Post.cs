using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Post
{
    public long Id { get; private set; }
    public long AutorPessoaId { get; private set; }
    public long? GrupoId { get; private set; }
    public long? EventoId { get; private set; }
    public string? Titulo { get; private set; }
    public string Texto { get; private set; }
    public VisibilidadePostEnum Visibilidade { get; private set; }
    public StatusPostEnum Status { get; private set; }
    public bool Fixado { get; private set; }
    public DateTime DataPublicacao { get; private set; }
    public DateTime? DataEdicao { get; private set; }

    public Pessoa Autor { get; private set; } = null!;
    public Grupo? Grupo { get; private set; }
    public Evento? Evento { get; private set; }

    private Post()
    {
        // Construtor privado para o EF Core
    }

    public Post(
        long autorPessoaId,
        string texto,
        VisibilidadePostEnum visibilidade,
        string? titulo = null,
        long? grupoId = null,
        long? eventoId = null)
    {
        AutorPessoaId = autorPessoaId;
        Texto = texto;
        Visibilidade = visibilidade;
        Titulo = titulo;
        GrupoId = grupoId;
        EventoId = eventoId;
        Status = StatusPostEnum.Ativo;
        Fixado = false;
        DataPublicacao = DateTime.UtcNow;
    }

    public void Editar(string texto, string? titulo = null)
    {
        Texto = texto;
        Titulo = titulo;
        DataEdicao = DateTime.UtcNow;
    }

    public void AlterarStatus(StatusPostEnum novoStatus)
    {
        Status = novoStatus;
    }

    public void Fixar()
    {
        Fixado = true;
    }

    public void Desafixar()
    {
        Fixado = false;
    }

    public void AlterarVisibilidade(VisibilidadePostEnum novaVisibilidade)
    {
        Visibilidade = novaVisibilidade;
    }
}
