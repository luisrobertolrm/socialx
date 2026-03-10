using SocialX.Core.Paginacao;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SocialX.Infra.Paginacao;

public static class ScrollInfinitoExtensions
{
    public static async Task<ListaScrollInfinito<TEntity>> ScrollAsync<TEntity>(
        this IQueryable<TEntity> query,
        ParametrosScrollInfinito parametros,
        Expression<Func<TEntity, long>> getId,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        CursorPadrao? cursor = CursorHelper.Desserializar<CursorPadrao>(parametros.Cursor);

        if (cursor is not null)
        {
            if (parametros.Ordem == TipoOrdem.Asc)
            {
                query = query.Where(
                    Expression.Lambda<Func<TEntity, bool>>(
                        Expression.GreaterThan(
                            getId.Body,
                            Expression.Constant(cursor.Id)
                        ),
                        getId.Parameters
                    )
                );
            }
            else
            {
                query = query.Where(
                    Expression.Lambda<Func<TEntity, bool>>(
                        Expression.LessThan(
                            getId.Body,
                            Expression.Constant(cursor.Id)
                        ),
                        getId.Parameters
                    )
                );
            }
        }

        int tamanho = parametros.Tamanho <= 0 ? 20 : parametros.Tamanho;

        query = parametros.Ordem == TipoOrdem.Asc
            ? query.OrderBy(getId)
            : query.OrderByDescending(getId);

        List<TEntity> itens = await query
            .Take(tamanho + 1)
            .ToListAsync(cancellationToken);

        bool temMais = itens.Count > tamanho;

        if (temMais)
        {
            itens = itens.Take(tamanho).ToList();
        }

        string? proximoCursor = null;

        if (temMais && itens.Count > 0)
        {
            long ultimoId = getId.Compile().Invoke(itens[^1]);

            CursorPadrao cursorProximo = new CursorPadrao
            {
                Id = ultimoId
            };

            proximoCursor = CursorHelper.Serializar(cursorProximo);
        }

        return new ListaScrollInfinito<TEntity>
        {
            Lista = itens,
            TemMais = temMais,
            ProximoCursor = proximoCursor
        };
    }
}
