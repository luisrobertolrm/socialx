using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class GrupoModerador
{
    public long Id { get; private set; }
    public long GrupoId { get; private set; }
    public long PessoaId { get; private set; }
    public PapelGrupoEnum Papel { get; private set; }
    public DateTime DataInicio { get; private set; }

    public Grupo Grupo { get; private set; } = null!;
    public Pessoa Pessoa { get; private set; } = null!;

    private GrupoModerador()
    {
        // Construtor privado para o EF Core
    }

    public GrupoModerador(long grupoId, long pessoaId, PapelGrupoEnum papel)
    {
        GrupoId = grupoId;
        PessoaId = pessoaId;
        Papel = papel;
        DataInicio = DateTime.UtcNow;
    }

    public void AlterarPapel(PapelGrupoEnum novoPapel)
    {
        Papel = novoPapel;
    }
}
