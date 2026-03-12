using AutoMapper;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;
using SocialX.Service.DTOs;
using SocialX.Service.Mapeamentos;
using Xunit;

namespace SocialX.Service.Tests.Mapeamentos;

public class PessoaProfileTests
{
    private readonly IMapper _mapper;

    public PessoaProfileTests()
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PessoaProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void DeveMappearPessoaParaPessoaDto()
    {
        // Arrange
        Pessoa pessoa = new Pessoa(
            nome: "João Silva",
            apelido: "joao123",
            telefone: "+5511999999999",
            dataNascimento: new DateTime(1990, 5, 15),
            email: "joao@example.com"
        );

        // Act
        PessoaDto dto = _mapper.Map<PessoaDto>(pessoa);

        // Assert
        Assert.Equal(pessoa.Id, dto.Id);
        Assert.Equal(pessoa.Nome, dto.Nome);
        Assert.Equal(pessoa.Apelido, dto.Apelido);
        Assert.Equal(pessoa.Telefone, dto.Telefone);
        Assert.Equal(pessoa.DataNascimento, dto.DataNascimento);
        Assert.Equal(pessoa.Email, dto.Email);
        Assert.Equal(pessoa.FotoPerfil, dto.FotoPerfil);
        Assert.Equal(pessoa.Bio, dto.Bio);
        Assert.Equal(pessoa.Cidade, dto.Cidade);
        Assert.Equal(pessoa.Role, dto.Role);
        Assert.Equal(pessoa.StatusConta, dto.StatusConta);
        Assert.Equal(pessoa.DataCriacao, dto.DataCriacao);
        Assert.Equal(pessoa.UltimoAcesso, dto.UltimoAcesso);
    }

    [Fact]
    public void DeveMappearPessoaComCamposNulosParaPessoaDto()
    {
        // Arrange
        Pessoa pessoa = new Pessoa(
            nome: "Pedro Costa",
            apelido: "pedro789",
            telefone: "+5511977777777",
            dataNascimento: new DateTime(1988, 3, 10),
            email: "pedro@example.com"
        );

        // Act
        PessoaDto dto = _mapper.Map<PessoaDto>(pessoa);

        // Assert
        Assert.Null(dto.FotoPerfil);
        Assert.Null(dto.Bio);
        Assert.Null(dto.Cidade);
        Assert.Null(dto.UltimoAcesso);
    }

    [Fact]
    public void DeveMappearEnumsCorretamente()
    {
        // Arrange
        Pessoa pessoa = new Pessoa(
            nome: "Ana Lima",
            apelido: "ana321",
            telefone: "+5511966666666",
            dataNascimento: new DateTime(1992, 11, 25),
            email: "ana@example.com"
        );

        pessoa.AlterarRole(RoleUsuarioEnum.ModeradorSistema);
        pessoa.AlterarStatusConta(StatusContaEnum.Bloqueada);

        // Act
        PessoaDto dto = _mapper.Map<PessoaDto>(pessoa);

        // Assert
        Assert.Equal(RoleUsuarioEnum.ModeradorSistema, dto.Role);
        Assert.Equal(StatusContaEnum.Bloqueada, dto.StatusConta);
    }

    [Fact]
    public void DeveMappearPessoaComTodosCamposPreenchidos()
    {
        // Arrange
        Pessoa pessoa = new Pessoa(
            nome: "Maria Santos",
            apelido: "maria456",
            telefone: "+5511988888888",
            dataNascimento: new DateTime(1995, 8, 20),
            email: "maria@example.com"
        );

        pessoa.AtualizarPerfil(
            apelido: "maria_updated",
            telefone: "+5511977777777",
            dataNascimento: new DateTime(1995, 8, 20),
            fotoPerfil: "https://example.com/photo.jpg",
            bio: "Desenvolvedora de software",
            cidade: "São Paulo"
        );

        pessoa.AtualizarUltimoAcesso();

        // Act
        PessoaDto dto = _mapper.Map<PessoaDto>(pessoa);

        // Assert
        Assert.Equal(pessoa.Id, dto.Id);
        Assert.Equal(pessoa.Nome, dto.Nome);
        Assert.Equal(pessoa.Apelido, dto.Apelido);
        Assert.Equal(pessoa.Telefone, dto.Telefone);
        Assert.Equal(pessoa.DataNascimento, dto.DataNascimento);
        Assert.Equal(pessoa.Email, dto.Email);
        Assert.Equal("https://example.com/photo.jpg", dto.FotoPerfil);
        Assert.Equal("Desenvolvedora de software", dto.Bio);
        Assert.Equal("São Paulo", dto.Cidade);
        Assert.Equal(RoleUsuarioEnum.Usuario, dto.Role);
        Assert.Equal(StatusContaEnum.Ativa, dto.StatusConta);
        Assert.NotNull(dto.UltimoAcesso);
    }

    [Fact]
    public void DeveValidarConfiguracaoDoMapper()
    {
        // Arrange
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PessoaProfile>();
        });

        // Act & Assert
        config.AssertConfigurationIsValid();
    }
}
