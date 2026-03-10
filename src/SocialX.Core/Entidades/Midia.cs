using SocialX.Core.Enums;

namespace SocialX.Core.Entidades;

public class Midia
{
    public long Id { get; private set; }
    public TipoDonoMidiaEnum TipoDono { get; private set; }
    public long DonoId { get; private set; }
    public TipoArquivoMidiaEnum TipoArquivo { get; private set; }
    public string UrlArquivo { get; private set; }
    public string? UrlThumbnail { get; private set; }
    public int Ordem { get; private set; }
    public string? MimeType { get; private set; }
    public long? TamanhoBytes { get; private set; }
    public int? DuracaoSegundos { get; private set; }
    public DateTime DataUpload { get; private set; }

    private Midia()
    {
        // Construtor privado para o EF Core
    }

    public Midia(
        TipoDonoMidiaEnum tipoDono,
        long donoId,
        TipoArquivoMidiaEnum tipoArquivo,
        string urlArquivo,
        int ordem = 0)
    {
        TipoDono = tipoDono;
        DonoId = donoId;
        TipoArquivo = tipoArquivo;
        UrlArquivo = urlArquivo;
        Ordem = ordem;
        DataUpload = DateTime.UtcNow;
    }

    public void AtualizarMetadados(
        string? urlThumbnail,
        string? mimeType,
        long? tamanhoBytes,
        int? duracaoSegundos)
    {
        UrlThumbnail = urlThumbnail;
        MimeType = mimeType;
        TamanhoBytes = tamanhoBytes;
        DuracaoSegundos = duracaoSegundos;
    }

    public void AlterarOrdem(int novaOrdem)
    {
        Ordem = novaOrdem;
    }
}
