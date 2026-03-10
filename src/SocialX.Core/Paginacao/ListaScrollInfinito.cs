namespace SocialX.Core.Paginacao;

[Serializable]
public class ListaScrollInfinito<T>
{
    public List<T> Lista { get; set; } = new();
    public string? ProximoCursor { get; set; }
    public bool TemMais { get; set; }
}
