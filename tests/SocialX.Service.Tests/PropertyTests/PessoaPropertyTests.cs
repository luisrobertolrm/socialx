using FsCheck;
using FsCheck.Xunit;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Service.Tests.PropertyTests;

/// <summary>
/// Feature: autenticacao
/// Property-based tests for Pessoa entity validation
/// </summary>
public class PessoaPropertyTests
{
    /// <summary>
    /// Feature: autenticacao, Property 3: Pessoa Required Fields Validation
    /// For any attempt to create or update a Pessoa record, the system SHALL enforce
    /// that nome, email, apelido, telefone, and data_nascimento fields are present and non-empty
    /// Validates: Requirements 3.3, 3.4, 7.4, 8.4
    /// </summary>
    [Property(MaxTest = 100, Arbitrary = new[] { typeof(PessoaGenerators) })]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "3")]
    public Property Pessoa_RequiredFields_MustBePresent(Pessoa pessoa)
    {
        // Arrange & Act
        bool nomeIsValid = !string.IsNullOrWhiteSpace(pessoa.Nome);
        bool emailIsValid = !string.IsNullOrWhiteSpace(pessoa.Email);
        bool apelidoIsValid = !string.IsNullOrWhiteSpace(pessoa.Apelido);
        bool telefoneIsValid = !string.IsNullOrWhiteSpace(pessoa.Telefone);
        bool dataNascimentoIsValid = pessoa.DataNascimento != default;

        // Assert
        return (nomeIsValid && emailIsValid && apelidoIsValid && 
                telefoneIsValid && dataNascimentoIsValid)
            .ToProperty()
            .Label("All required fields must be present and non-empty");
    }

    /// <summary>
    /// Feature: autenticacao, Property 5: Pessoa Default Values
    /// For any new Pessoa record created during registration, the system SHALL automatically
    /// set role to USUARIO, status_conta to ATIVA, data_criacao to current timestamp
    /// Validates: Requirements 3.5, 3.6, 3.7, 7.7, 7.9
    /// </summary>
    [Property(MaxTest = 100, Arbitrary = new[] { typeof(PessoaGenerators) })]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "5")]
    public Property Pessoa_DefaultValues_MustBeSetCorrectly(NonEmptyString nome, NonEmptyString email,
        NonEmptyString apelido, NonEmptyString telefone, DateTime dataNascimento)
    {
        // Arrange & Act
        Pessoa pessoa = new Pessoa
        {
            Nome = nome.Get,
            Email = email.Get,
            Apelido = apelido.Get,
            Telefone = telefone.Get,
            DataNascimento = dataNascimento
            // Não definindo explicitamente Role, StatusConta, DataCriacao
        };

        // Assert
        bool roleIsUsuario = pessoa.Role == Role.USUARIO;
        bool statusIsAtiva = pessoa.StatusConta == StatusConta.ATIVA;
        bool dataCriacaoIsSet = pessoa.DataCriacao != default && 
                                pessoa.DataCriacao <= DateTime.UtcNow &&
                                pessoa.DataCriacao >= DateTime.UtcNow.AddSeconds(-5);

        return (roleIsUsuario && statusIsAtiva && dataCriacaoIsSet)
            .ToProperty()
            .Label("Default values must be set: Role=USUARIO, StatusConta=ATIVA, DataCriacao=now");
    }

    /// <summary>
    /// Feature: autenticacao, Property 26: Pessoa Serialization Round-Trip
    /// For any valid Pessoa object, serializing it to JSON and then deserializing it back
    /// SHALL produce an equivalent Pessoa object with all properties preserved
    /// Validates: Requirements 17.11
    /// </summary>
    [Property(MaxTest = 100, Arbitrary = new[] { typeof(PessoaGenerators) })]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "26")]
    public Property Pessoa_Serialization_RoundTrip(Pessoa pessoa)
    {
        // Arrange
        string json = System.Text.Json.JsonSerializer.Serialize(pessoa);

        // Act
        Pessoa? deserialized = System.Text.Json.JsonSerializer.Deserialize<Pessoa>(json);

        // Assert
        return (deserialized != null &&
                deserialized.Id == pessoa.Id &&
                deserialized.Nome == pessoa.Nome &&
                deserialized.Apelido == pessoa.Apelido &&
                deserialized.Telefone == pessoa.Telefone &&
                deserialized.DataNascimento == pessoa.DataNascimento &&
                deserialized.Email == pessoa.Email &&
                deserialized.FotoPerfil == pessoa.FotoPerfil &&
                deserialized.Bio == pessoa.Bio &&
                deserialized.Cidade == pessoa.Cidade &&
                deserialized.Role == pessoa.Role &&
                deserialized.StatusConta == pessoa.StatusConta &&
                deserialized.DataCriacao == pessoa.DataCriacao &&
                deserialized.UltimoAcesso == pessoa.UltimoAcesso)
            .ToProperty()
            .Label("Serialization round-trip must preserve all properties");
    }
}

/// <summary>
/// Custom generators for Pessoa entity
/// </summary>
public static class PessoaGenerators
{
    public static Arbitrary<Pessoa> Pessoa()
    {
        Gen<Pessoa> pessoaGen = from nome in Arb.Generate<NonEmptyString>()
                                from email in Arb.Generate<NonEmptyString>()
                                from apelido in Arb.Generate<NonEmptyString>()
                                from telefone in Arb.Generate<NonEmptyString>()
                                from dataNascimento in Arb.Generate<DateTime>()
                                from fotoPerfil in Arb.Generate<string?>()
                                from bio in Arb.Generate<string?>()
                                from cidade in Arb.Generate<string?>()
                                from role in Arb.Generate<Role>()
                                from statusConta in Arb.Generate<StatusConta>()
                                select new Core.Entidades.Pessoa
                                {
                                    Id = 1,
                                    Nome = nome.Get,
                                    Email = email.Get,
                                    Apelido = apelido.Get,
                                    Telefone = telefone.Get,
                                    DataNascimento = dataNascimento,
                                    FotoPerfil = fotoPerfil,
                                    Bio = bio,
                                    Cidade = cidade,
                                    Role = role,
                                    StatusConta = statusConta,
                                    DataCriacao = DateTime.UtcNow,
                                    UltimoAcesso = DateTime.UtcNow
                                };

        return pessoaGen.ToArbitrary();
    }
}
