using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;
using SocialX.Infra.Data;

namespace SocialX.Service.Tests.PropertyTests;

public class LoginSocialPropertyTests
{
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "6")]
    public Property LoginSocial_DatabaseConstraints_MustBeRepresentable(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento,
        NonEmptyString providerUserId)
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
            providerUserId.Get,
            pessoa.Email);

        context.LoginsSociais.Add(loginSocial);
        context.SaveChanges();

        LoginSocial? saved = context.LoginsSociais.SingleOrDefault(ls => ls.ProviderUserId == providerUserId.Get);

        return (saved != null &&
                saved.PessoaId == pessoa.Id &&
                saved.Provider == ProviderEnum.Google &&
                saved.EmailProvider == pessoa.Email)
            .ToProperty()
            .Label("LoginSocial must persist with the linked Pessoa and provider data");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "7")]
    public Property LoginSocial_Verification_MustRetrievePessoa(
        NonEmptyString googleId,
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

        LoginSocial loginSocial = new LoginSocial(
            pessoa.Id,
            ProviderEnum.Google,
            googleId.Get,
            pessoa.Email);

        context.LoginsSociais.Add(loginSocial);
        context.SaveChanges();

        LoginSocial? found = context.LoginsSociais
            .Include(ls => ls.Pessoa)
            .FirstOrDefault(ls => ls.Provider == ProviderEnum.Google && ls.ProviderUserId == googleId.Get);

        return (found != null && found.Pessoa.Id == pessoa.Id)
            .ToProperty()
            .Label("LoginSocial verification must retrieve associated Pessoa");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "8")]
    public Property LoginSocial_NotFound_MustIndicateRegistrationRequired(NonEmptyString googleId)
    {
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        LoginSocial? found = context.LoginsSociais
            .Include(ls => ls.Pessoa)
            .FirstOrDefault(ls => ls.Provider == ProviderEnum.Google && ls.ProviderUserId == googleId.Get);

        return (found == null)
            .ToProperty()
            .Label("Missing LoginSocial must indicate registration required");
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
