using SocialX.Core.Interface;
using SocialX.Infra.Data;

namespace SocialX.Infra.Repositorio;

public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
{
    protected readonly CustomDbContext contexto;

    public RepositorioGenerico(CustomDbContext customDbContext)
    {
        contexto = customDbContext;
    }

    public virtual T Adicionar(T entidade)
    {
        contexto.Set<T>().Add(entidade);
        return entidade;
    }

    public virtual T Editar(T entidade)
    {
        contexto.Set<T>().Update(entidade);
        return entidade;
    }

    public virtual void Deletar(T entidade)
    {
        contexto.Set<T>().Remove(entidade);
    }

    public virtual IQueryable<T> IQueryable()
    {
        return contexto.Set<T>();
    }
}
