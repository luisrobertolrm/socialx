using SocialX.Core.Paginacao;
using SocialX.Service.DTOs;

namespace SocialX.Service.Interfaces;

public interface IEntidadeTesteService
{
    Task<EntidadeTesteDto> CriarAsync(EntidadeTesteCriarDto dto);
    Task<EntidadeTesteDto?> ObterPorIdAsync(long id);
    Task<ListaScrollInfinito<EntidadeTesteDto>> ScrollAsync(
        ParametrosScrollInfinito parametros,
        CancellationToken cancellationToken);
}
