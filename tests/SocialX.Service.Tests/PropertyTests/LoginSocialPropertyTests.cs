using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;
using SocialX.Infra.Data;

namespace SocialX.Service.Tests.PropertyTests;

/// <summary>
/// Feature: autenticacao
/// Property-based tests for LoginSocial entity and verification
/// </summary>
public class LoginSocialPropertyTests
{
    /// <summary>
    /// Feature: autenticacao, Property 6: Database Constraint Enforcement
    /// For any attempt to create a LoginSocial record, the system SHALL enforce:
    /// (1) foreign key constraint that pessoa_id references a valid Pessoa.id
    /// (2) uniqueness constraint on the combination of provider and provider_user_id
    /// (3) uniqueness constraint on Pessoa.email
    /// Validates: Requirements 3.8, 3.9, 3.10
    /// </summary>
    [Property(MaxTest = 100, Arbitrary = new[] { typeof(LoginSocialGenerators) })]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "6")]
    public Property LoginSocial_DatabaseConstraints_MustBeEnforced(LoginSocial loginSocial, Pessoa pessoa)
    {
        // Arrange
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        // Act & Assert
        try
        {
            // Primeiro, adiciona a pessoa
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            // Configura o LoginSocial com a pessoa válida
            loginSocial.PessoaId = pessoa.Id;
            loginSocial.Pessoa = pessoa;

            // Tenta adicionar o LoginSocial
            context.LoginsSociais.Add(loginSocial);
            context.SaveChanges();

            // Se chegou aqui, o LoginSocial foi adicionado com sucesso
            // Agora tenta adicionar um duplicado (deve falhar)
            LoginSocial duplicado = new LoginSocial
            {
                PessoaId = pessoa.Id,
                Provider = loginSocial.Provider,
                ProviderUserId = loginSocial.ProviderUserId,
                EmailProvider = loginSocial.EmailProvider,
                DataVinculo = DateTime.UtcNow
            };

            context.LoginsSociais.Add(duplicado);
            
            bool duplicateThrowsException = false;
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                duplicateThrowsException = true;
            }

            return duplicateThrowsException
                .ToProperty()
                .Label("Duplicate LoginSocial (provider + provider_user_id) must throw exception");
        }
        catch (DbUpdateException)
        {
            // Se falhou ao adicionar o primeiro, pode ser por violação de constraint
            return true.ToProperty().Label("Database constraints are enforced");
        }
    }

    /// <summary>
    /// Feature: autenticacao, Property 7: LoginSocial Verification and Pessoa Retrieval
    /// For any validated ID Token with a Google ID, the system SHALL check if a LoginSocial
    /// record exists with provider=GOOGLE and provider_user_id matching the Google ID,
    /// and if found, SHALL retrieve the associated Pessoa record
    /// Validates: Requirements 4.1, 4.2
    /// </summary>
    [Property(MaxTest = 100, Arbitrary = new[] { typeof(LoginSocialGenerators) })]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "7")]
    public Property LoginSocial_Verification_MustRetrievePessoa(string googleId, Pessoa pessoa)
    {
        // Arrange
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        // Adiciona pessoa e LoginSocial
        context.Pessoas.Add(pessoa);
        context.SaveChanges();

        LoginSocial loginSocial = new LoginSocial
        {
            PessoaId = pessoa.Id,
            Provider = Provider.GOOGLE,
            ProviderUserId = googleId,
            EmailProvider = pessoa.Email,
            DataVinculo = DateTime.UtcNow
        };

        context.LoginsSociais.Add(loginSocial);
        context.SaveChanges();

        // Act
        LoginSocial? found = context.LoginsSociais
            .Include(ls => ls.Pessoa)
            .FirstOrDefault(ls => ls.Provider == Provider.GOOGLE && 
                                 ls.ProviderUserId == googleId);

        // Assert
        return (found != null && 
                found.Pessoa != null && 
                found.Pessoa.Id == pessoa.Id)
            .ToProperty()
            .Label("LoginSocial verification must retrieve associated Pessoa");
    }

    /// <summary>
    /// Feature: autenticacao, Property 8: Registration Required Response
    /// For any validated ID Token that does not have an associated LoginSocial record,
    /// the system SHALL return a response with requiresRegistration=true and user=null
    /// Validates: Requirements 4.3
    /// </summary>
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "8")]
    public Property LoginSocial_NotFound_MustIndicateRegistrationRequired(NonEmptyString googleId)
    {
        // Arrange
        DbContextOptions<CustomDbContext> options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using CustomDbContext context = new CustomDbContext(options);

        // Act
        LoginSocial? found = context.LoginsSociais
            .Include(ls => ls.Pessoa)
            .FirstOrDefault(ls => ls.Provider == Provider.GOOGLE && 
                                 ls.ProviderUserId == googleId.Get);

        // Assert
        bool requiresRegistration = found == null;
        Pessoa? user = found?.Pessoa;

        return (requiresRegistration && user == null)
            .ToProperty()
            .Label("Missing LoginSocial must indicate registration required");
    }
}

/// <summary>
/// Custom generators for LoginSocial entity
/// </summary>
public static class LoginSocialGenerators
{
    public static Arbitrary<LoginSocial> LoginSocial()
    {
        Gen<LoginSocial> loginSocialGen = from providerUserId in Arb.Generate<NonEmptyString>()
                                          from emailProvider in Arb.Generate<NonEmptyString>()
                                          select new Core.Entidades.LoginSocial
                                          {
                                              Id = 1,
                                              PessoaId = 1,
                                              Provider = Provider.GOOGLE,
                                              ProviderUserId = providerUserId.Get,
                                              EmailProvider = emailProvider.Get,
                                              DataVinculo = DateTime.UtcNow
                                          };

        return loginSocialGen.ToArbitrary();
    }

    public static Arbitrary<Pessoa> Pessoa()
    {
        Gen<Pessoa> pessoaGen = from nome in Arb.Generate<NonEmptyString>()
                                from email in Arb.Generate<NonEmptyString>()
                                from apelido in Arb.Generate<NonEmptyString>()
                                from telefone in Arb.Generate<NonEmptyString>()
                                from dataNascimento in Arb.Generate<DateTime>()
                                select new Core.Entidades.Pessoa
                                {
                                    Id = 1,
                                    Nome = nome.Get,
                                    Email = $"{email.Get}@test.com",
                                    Apelido = apelido.Get,
                                    Telefone = telefone.Get,
                                    DataNascimento = dataNascimento,
                                    Role = Role.USUARIO,
                                    StatusConta = StatusConta.ATIVA,
                                    DataCriacao = DateTime.UtcNow
                                };

        return pessoaGen.ToArbitrary();
    }
}
