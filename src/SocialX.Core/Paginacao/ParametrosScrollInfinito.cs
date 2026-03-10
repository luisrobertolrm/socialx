namespace SocialX.Core.Paginacao;

public class ParametrosScrollInfinito
{
    public int Tamanho { get; set; } = 20;
    public string? OrderBy { get; set; }
    public TipoOrdem Ordem { get; set; } = TipoOrdem.Desc;
    public string? Cursor { get; set; }
}

public class ParametrosScrollInfinito<T> : ParametrosScrollInfinito
{
    public T? Parametros { get; set; }
}
