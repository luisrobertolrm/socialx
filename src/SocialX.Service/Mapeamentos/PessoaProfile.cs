using AutoMapper;
using SocialX.Core.Entidades;
using SocialX.Service.DTOs;

namespace SocialX.Service.Mapeamentos;

public class PessoaProfile : Profile
{
    public PessoaProfile()
    {
        CreateMap<Pessoa, PessoaDto>();
    }
}
