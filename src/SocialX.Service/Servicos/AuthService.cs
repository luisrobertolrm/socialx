using Microsoft.EntityFrameworkCore;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;
using SocialX.Core.Interface;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;

namespace SocialX.Service.Servicos;

public class AuthService : IAuthService
{
    private readonly IRepositorioGenerico<Pessoa> _pessoaRepositorio;
    private readonly IRepositorioGenerico<LoginSocial> _loginSocialRepositorio;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IRepositorioGenerico<Pessoa> pessoaRepositorio,
        IRepositorioGenerico<LoginSocial> loginSocialRepositorio,
        IUnitOfWork unitOfWork)
    {
        _pessoaRepositorio = pessoaRepositorio;
        _loginSocialRepositorio = loginSocialRepositorio;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginSocialVerificationResult> VerifyLoginSocialAsync(string googleId)
    {
        if (string.IsNullOrWhiteSpace(googleId))
        {
            throw new ArgumentException("Google ID não pode ser vazio", nameof(googleId));
        }

        LoginSocial? loginSocial = await _loginSocialRepositorio
            .IQueryable()
            .Include(ls => ls.Pessoa)
            .FirstOrDefaultAsync(ls => 
                ls.Provider == ProviderEnum.Google && 
                ls.ProviderUserId == googleId);

        if (loginSocial == null)
        {
            return new LoginSocialVerificationResult
            {
                Found = false,
                Pessoa = null
            };
        }

        return new LoginSocialVerificationResult
        {
            Found = true,
            Pessoa = loginSocial.Pessoa
        };
    }

    public async Task<AccountStatusResult> CheckAccountStatusAsync(long pessoaId)
    {
        Pessoa? pessoa = await _pessoaRepositorio
            .IQueryable()
            .FirstOrDefaultAsync(p => p.Id == pessoaId);

        if (pessoa == null)
        {
            throw new InvalidOperationException($"Pessoa com ID {pessoaId} não encontrada");
        }

        AccountStatusResult result = new AccountStatusResult();

        switch (pessoa.StatusConta)
        {
            case StatusContaEnum.Ativa:
                result.IsActive = true;
                result.IsBlocked = false;
                result.IsDeactivated = false;
                break;

            case StatusContaEnum.Bloqueada:
                result.IsActive = false;
                result.IsBlocked = true;
                result.IsDeactivated = false;
                result.Message = "Sua conta está bloqueada. Entre em contato com o suporte.";
                break;

            case StatusContaEnum.Desativada:
                result.IsActive = false;
                result.IsBlocked = false;
                result.IsDeactivated = true;
                result.Message = "Sua conta está desativada.";
                break;

            default:
                throw new InvalidOperationException($"Status de conta desconhecido: {pessoa.StatusConta}");
        }

        return result;
    }

    public async Task UpdateLastAccessAsync(long pessoaId)
    {
        Pessoa? pessoa = await _pessoaRepositorio
            .IQueryable()
            .FirstOrDefaultAsync(p => p.Id == pessoaId);

        if (pessoa == null)
        {
            throw new InvalidOperationException($"Pessoa com ID {pessoaId} não encontrada");
        }

        pessoa.AtualizarUltimoAcesso();
        _pessoaRepositorio.Editar(pessoa);
        await _unitOfWork.SalvarAsync();
    }

    public async Task<Pessoa> CreateUserAsync(CompletarCadastroDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            throw new ArgumentException("Nome é obrigatório", nameof(dto.Nome));
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Email é obrigatório", nameof(dto.Email));
        }

        if (string.IsNullOrWhiteSpace(dto.Apelido))
        {
            throw new ArgumentException("Apelido é obrigatório", nameof(dto.Apelido));
        }

        if (string.IsNullOrWhiteSpace(dto.Telefone))
        {
            throw new ArgumentException("Telefone é obrigatório", nameof(dto.Telefone));
        }

        if (dto.DataNascimento == default)
        {
            throw new ArgumentException("Data de nascimento é obrigatória", nameof(dto.DataNascimento));
        }

        if (string.IsNullOrWhiteSpace(dto.GoogleId))
        {
            throw new ArgumentException("Google ID é obrigatório", nameof(dto.GoogleId));
        }

        // Verificar se já existe LoginSocial com este Google ID
        bool loginSocialExists = await _loginSocialRepositorio
            .IQueryable()
            .AnyAsync(ls => 
                ls.Provider == ProviderEnum.Google && 
                ls.ProviderUserId == dto.GoogleId);

        if (loginSocialExists)
        {
            throw new InvalidOperationException("Conta Google já vinculada a outro usuário");
        }

        // Verificar se já existe Pessoa com este email
        bool emailExists = await _pessoaRepositorio
            .IQueryable()
            .AnyAsync(p => p.Email == dto.Email);

        if (emailExists)
        {
            throw new InvalidOperationException("Email já cadastrado");
        }

        // Criar Pessoa
        Pessoa pessoa = new Pessoa(
            nome: dto.Nome,
            apelido: dto.Apelido,
            telefone: dto.Telefone,
            dataNascimento: dto.DataNascimento,
            email: dto.Email
        );

        // Atualizar campos opcionais
        if (!string.IsNullOrWhiteSpace(dto.FotoPerfil))
        {
            pessoa.Atualizar(
                nome: pessoa.Nome,
                telefone: pessoa.Telefone,
                email: pessoa.Email,
                fotoPerfil: dto.FotoPerfil,
                bio: pessoa.Bio,
                cidade: pessoa.Cidade
            );
        }

        // Atualizar último acesso
        pessoa.AtualizarUltimoAcesso();

        // Adicionar Pessoa
        Pessoa pessoaAdicionada = _pessoaRepositorio.Adicionar(pessoa);
        await _unitOfWork.SalvarAsync();

        // Criar LoginSocial
        LoginSocial loginSocial = new LoginSocial(
            pessoaId: pessoaAdicionada.Id,
            provider: ProviderEnum.Google,
            providerUserId: dto.GoogleId,
            emailProvider: dto.Email
        );

        _loginSocialRepositorio.Adicionar(loginSocial);
        await _unitOfWork.SalvarAsync();

        return pessoaAdicionada;
    }

    public async Task<Pessoa> UpdateProfileAsync(long pessoaId, UpdateProfileDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrWhiteSpace(dto.Apelido))
        {
            throw new ArgumentException("Apelido é obrigatório", nameof(dto.Apelido));
        }

        if (string.IsNullOrWhiteSpace(dto.Telefone))
        {
            throw new ArgumentException("Telefone é obrigatório", nameof(dto.Telefone));
        }

        if (dto.DataNascimento == default)
        {
            throw new ArgumentException("Data de nascimento é obrigatória", nameof(dto.DataNascimento));
        }

        Pessoa? pessoa = await _pessoaRepositorio
            .IQueryable()
            .FirstOrDefaultAsync(p => p.Id == pessoaId);

        if (pessoa == null)
        {
            throw new InvalidOperationException($"Pessoa com ID {pessoaId} não encontrada");
        }

        // Atualizar dados (mantendo nome e email inalterados)
        pessoa.AtualizarPerfil(
            apelido: dto.Apelido,
            telefone: dto.Telefone,
            dataNascimento: dto.DataNascimento,
            fotoPerfil: dto.FotoPerfil,
            bio: dto.Bio,
            cidade: dto.Cidade
        );

        _pessoaRepositorio.Editar(pessoa);
        await _unitOfWork.SalvarAsync();

        return pessoa;
    }
}
