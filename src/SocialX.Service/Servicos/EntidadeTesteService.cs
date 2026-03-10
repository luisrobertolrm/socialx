using AutoMapper;
using SocialX.Core.Entidades;
using SocialX.Core.Interface;
using SocialX.Core.Paginacao;
using SocialX.Infra.Paginacao;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace SocialX.Service.Servicos;

public class EntidadeTesteService : IEntidadeTesteService
{
    private readonly IRepositorioGenerico<EntidadeTeste> _repositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<EntidadeTesteCriarDto> _validator;

    public EntidadeTesteService(
        IRepositorioGenerico<EntidadeTeste> repositorio,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<EntidadeTesteCriarDto> validator)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<EntidadeTesteDto> CriarAsync(EntidadeTesteCriarDto dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        EntidadeTeste entidade = _mapper.Map<EntidadeTeste>(dto);

        _repositorio.Adicionar(entidade);
        await _unitOfWork.SalvarAsync();

        EntidadeTesteDto retorno = _mapper.Map<EntidadeTesteDto>(entidade);
        return retorno;
    }

    public async Task<EntidadeTesteDto?> ObterPorIdAsync(long id)
    {
        EntidadeTeste? entidade = await _repositorio
            .IQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entidade == null)
        {
            return null;
        }

        EntidadeTesteDto retorno = _mapper.Map<EntidadeTesteDto>(entidade);
        return retorno;
    }

    public async Task<ListaScrollInfinito<EntidadeTesteDto>> ScrollAsync(
        ParametrosScrollInfinito parametros,
        CancellationToken cancellationToken)
    {
        ListaScrollInfinito<EntidadeTeste> paginaEntidade = await _repositorio
            .IQueryable()
            .AsNoTracking()
            .ScrollAsync(parametros, x => x.Id, cancellationToken);

        List<EntidadeTesteDto> listaDto = _mapper.Map<List<EntidadeTesteDto>>(paginaEntidade.Lista);

        ListaScrollInfinito<EntidadeTesteDto> retorno = new ListaScrollInfinito<EntidadeTesteDto>
        {
            Lista = listaDto,
            TemMais = paginaEntidade.TemMais,
            ProximoCursor = paginaEntidade.ProximoCursor
        };

        return retorno;
    }
}
