using AutoMapper;
using SocialX.Core.Entidades;
using SocialX.Service.DTOs;

namespace SocialX.Service.Mapeamentos;

public class EntidadeTesteProfile : Profile
{
    public EntidadeTesteProfile()
    {
        CreateMap<EntidadeTesteCriarDto, EntidadeTeste>()
            .ConstructUsing(dto => new EntidadeTeste(dto.Nome, dto.Valor));

        CreateMap<EntidadeTeste, EntidadeTesteDto>();
    }
}
