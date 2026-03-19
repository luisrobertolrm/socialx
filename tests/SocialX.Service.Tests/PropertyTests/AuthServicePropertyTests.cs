using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;
using SocialX.Infra.Data;

namespace SocialX.Service.Tests.PropertyTests;

public class AuthServicePropertyTests
{
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "9")]
    public Property AccountStatus_AccessControl_MustBeEnforced(NonEmptyString nome)
    {
        Pessoa ativa = CreatePessoa(nome.Get, "ativa@test.com", "apelido1", "111", new DateTime(2000, 1, 1));
        Pessoa bloqueada = CreatePessoa(nome.Get, "bloq@test.com", "apelido2", "222", new DateTime(2000, 1, 1));
        Pessoa desativada = CreatePessoa(nome.Get, "des@test.com", "apelido3", "333", new DateTime(2000, 1, 1));

        bloqueada.AlterarStatusConta(StatusContaEnum.Bloqueada);
        desativada.AlterarStatusConta(StatusContaEnum.Desativada);

        bool activeAllows = ativa.StatusConta == StatusContaEnum.Ativa;
        bool blockedDenied = bloqueada.StatusConta != StatusContaEnum.Ativa;
        bool disabledDenied = desativada.StatusConta != StatusContaEnum.Ativa;

        return (activeAllows && blockedDenied && disabledDenied)
            .ToProperty()
            .Label("Account status must control access for active, blocked, and disabled users");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "10")]
    public Property BlockedAccount_MustBeLogged(NonEmptyString nome)
    {
        Pessoa pessoa = CreatePessoa(nome.Get, "blocked@test.com", "apelido", "111", new DateTime(2000, 1, 1));
        pessoa.AlterarStatusConta(StatusContaEnum.Bloqueada);

        bool isBlocked = pessoa.StatusConta == StatusContaEnum.Bloqueada ||
                         pessoa.StatusConta == StatusContaEnum.Desativada;

        return isBlocked
            .ToProperty()
            .Label("Blocked accounts must remain detectable for logging workflows");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "11")]
    public Property LastAccess_MustBeUpdated_OnSuccessfulAuth(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento)
    {
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        Pessoa pessoa = CreatePessoa(nome.Get, email.Get, apelido.Get, telefone.Get, dataNascimento);
        context.Pessoas.Add(pessoa);
        context.SaveChanges();

        DateTime beforeUpdate = DateTime.UtcNow;
        pessoa.AtualizarUltimoAcesso();
        context.SaveChanges();
        DateTime afterUpdate = DateTime.UtcNow;

        return (pessoa.UltimoAcesso != null &&
                pessoa.UltimoAcesso >= beforeUpdate &&
                pessoa.UltimoAcesso <= afterUpdate)
            .ToProperty()
            .Label("Last access must be updated to current timestamp on successful authentication");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "12")]
    public Property CompleteRegistration_MustCreateBothRecords(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento,
        NonEmptyString googleId)
    {
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        Pessoa pessoa = CreatePessoa(nome.Get, email.Get, apelido.Get, telefone.Get, dataNascimento);
        context.Pessoas.Add(pessoa);
        context.SaveChanges();

        LoginSocial loginSocial = new LoginSocial(
            pessoa.Id,
            ProviderEnum.Google,
            googleId.Get,
            pessoa.Email);

        context.LoginsSociais.Add(loginSocial);
        context.SaveChanges();

        Pessoa? pessoaSalva = context.Pessoas.Find(pessoa.Id);
        LoginSocial? loginSocialSalvo = context.LoginsSociais.FirstOrDefault(ls => ls.PessoaId == pessoa.Id);

        return (pessoaSalva != null && loginSocialSalvo != null && loginSocialSalvo.PessoaId == pessoaSalva.Id)
            .ToProperty()
            .Label("Complete registration must create both Pessoa and LoginSocial");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "14")]
    public Property ProfileUpdate_MustPersistChanges_PreservingReadOnlyFields(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento,
        NonEmptyString newApelido,
        NonEmptyString newTelefone,
        DateTime newDataNascimento,
        string? newFotoPerfil,
        string? newBio,
        string? newCidade)
    {
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        Pessoa originalPessoa = CreatePessoa(nome.Get, email.Get, apelido.Get, telefone.Get, dataNascimento);
        context.Pessoas.Add(originalPessoa);
        context.SaveChanges();

        string originalNome = originalPessoa.Nome;
        string originalEmail = originalPessoa.Email;

        originalPessoa.AtualizarPerfil(
            newApelido.Get,
            newTelefone.Get,
            NormalizeDate(newDataNascimento),
            newFotoPerfil,
            newBio,
            newCidade);

        context.SaveChanges();

        Pessoa? updated = context.Pessoas.Find(originalPessoa.Id);

        return (updated != null &&
                updated.Nome == originalNome &&
                updated.Email == originalEmail &&
                updated.Apelido == newApelido.Get &&
                updated.Telefone == newTelefone.Get &&
                updated.DataNascimento == NormalizeDate(newDataNascimento) &&
                updated.FotoPerfil == newFotoPerfil &&
                updated.Bio == newBio &&
                updated.Cidade == newCidade)
            .ToProperty()
            .Label("Profile update must persist editable fields while preserving nome and email");
    }

    private static Pessoa CreatePessoa(
        string nome,
        string email,
        string apelido,
        string telefone,
        DateTime dataNascimento)
    {
        return new Pessoa(
            nome,
            apelido,
            telefone,
            NormalizeDate(dataNascimento),
            NormalizeEmail(email));
    }

    private static DateTime NormalizeDate(DateTime value)
    {
        DateTime normalized = value == default ? new DateTime(2000, 1, 1) : value;
        return normalized.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(normalized, DateTimeKind.Utc)
            : normalized.ToUniversalTime();
    }

    private static string NormalizeEmail(string value)
    {
        return value.Contains('@') ? value : $"{value}@test.com";
    }
}
