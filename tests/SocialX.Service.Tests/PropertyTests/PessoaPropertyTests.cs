using FsCheck;
using FsCheck.Xunit;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Service.Tests.PropertyTests;

public class PessoaPropertyTests
{
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "3")]
    public Property Pessoa_RequiredFields_MustBePresent(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento)
    {
        Pessoa pessoa = CreatePessoa(nome.Get, email.Get, apelido.Get, telefone.Get, dataNascimento);

        bool nomeIsValid = !string.IsNullOrWhiteSpace(pessoa.Nome);
        bool emailIsValid = !string.IsNullOrWhiteSpace(pessoa.Email);
        bool apelidoIsValid = !string.IsNullOrWhiteSpace(pessoa.Apelido);
        bool telefoneIsValid = !string.IsNullOrWhiteSpace(pessoa.Telefone);
        bool dataNascimentoIsValid = pessoa.DataNascimento != default;

        return (nomeIsValid && emailIsValid && apelidoIsValid && telefoneIsValid && dataNascimentoIsValid)
            .ToProperty()
            .Label("All required fields must be present and non-empty");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "5")]
    public Property Pessoa_DefaultValues_MustBeSetCorrectly(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento)
    {
        DateTime beforeCreation = DateTime.UtcNow;
        Pessoa pessoa = CreatePessoa(nome.Get, email.Get, apelido.Get, telefone.Get, dataNascimento);
        DateTime afterCreation = DateTime.UtcNow;

        bool roleIsUsuario = pessoa.Role == RoleUsuarioEnum.Usuario;
        bool statusIsAtiva = pessoa.StatusConta == StatusContaEnum.Ativa;
        bool dataCriacaoIsSet = pessoa.DataCriacao >= beforeCreation && pessoa.DataCriacao <= afterCreation;

        return (roleIsUsuario && statusIsAtiva && dataCriacaoIsSet)
            .ToProperty()
            .Label("Default values must be set on creation");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "26")]
    public Property Pessoa_ProfileUpdate_MustPreserveReadOnlyFields(
        NonEmptyString nome,
        NonEmptyString email,
        NonEmptyString apelido,
        NonEmptyString telefone,
        DateTime dataNascimento,
        NonEmptyString novoApelido,
        NonEmptyString novoTelefone,
        DateTime novaDataNascimento,
        string? novaFotoPerfil,
        string? novaBio,
        string? novaCidade)
    {
        Pessoa pessoa = CreatePessoa(nome.Get, email.Get, apelido.Get, telefone.Get, dataNascimento);

        string originalNome = pessoa.Nome;
        string originalEmail = pessoa.Email;

        pessoa.AtualizarPerfil(
            novoApelido.Get,
            novoTelefone.Get,
            novaDataNascimento,
            novaFotoPerfil,
            novaBio,
            novaCidade);

        return (pessoa.Nome == originalNome &&
                pessoa.Email == originalEmail &&
                pessoa.Apelido == novoApelido.Get &&
                pessoa.Telefone == novoTelefone.Get &&
                pessoa.DataNascimento == novaDataNascimento &&
                pessoa.FotoPerfil == novaFotoPerfil &&
                pessoa.Bio == novaBio &&
                pessoa.Cidade == novaCidade)
            .ToProperty()
            .Label("Profile update must preserve nome and email while updating editable fields");
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
