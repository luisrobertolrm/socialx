namespace SocialX.Core.Interface;

public interface IRepositorioGenerico<T> where T : class
{
    T Adicionar(T entidade);
    T Editar(T entidade);
    void Deletar(T entidade);
    IQueryable<T> IQueryable();
}
