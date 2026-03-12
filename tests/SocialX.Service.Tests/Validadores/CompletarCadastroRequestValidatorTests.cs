using Xunit;
using FluentValidation.TestHelper;
using SocialX.Service.DTOs;
using SocialX.Service.Validadores;

namespace SocialX.Service.Tests.Validadores;

public class CompletarCadastroRequestValidatorTests
{
    private readonly CompletarCadastroRequestValidator validator;

    public CompletarCadastroRequestValidatorTests()
    {
        validator = new CompletarCadastroRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_IdToken_Is_Empty()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "",
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.IdToken)
            .WithErrorMessage("Campo obrigatório");
    }

    [Fact]
    public void Should_Have_Error_When_Apelido_Is_Empty()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Apelido)
            .WithErrorMessage("Campo obrigatório");
    }

    [Fact]
    public void Should_Have_Error_When_Apelido_Exceeds_MaxLength()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = new string('a', 51),
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Apelido)
            .WithErrorMessage("Apelido deve ter no máximo 50 caracteres");
    }

    [Fact]
    public void Should_Have_Error_When_Telefone_Is_Empty()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "TestUser",
            Telefone = "",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Telefone)
            .WithErrorMessage("Campo obrigatório");
    }

    [Theory]
    [InlineData("11987654321")]
    [InlineData("1198765432")]
    [InlineData("(11) 98765-4321")]
    [InlineData("(11) 9876-5432")]
    [InlineData("5511987654321")]
    [InlineData("551198765432")]
    public void Should_Not_Have_Error_When_Telefone_Is_Valid_Brazilian_Format(string telefone)
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "TestUser",
            Telefone = telefone,
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Telefone);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("abc")]
    [InlineData("12345")]
    [InlineData("123456789")]
    public void Should_Have_Error_When_Telefone_Is_Invalid_Format(string telefone)
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "TestUser",
            Telefone = telefone,
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Telefone)
            .WithErrorMessage("Formato de telefone inválido");
    }

    [Fact]
    public void Should_Have_Error_When_DataNascimento_Is_Future_Date()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddDays(1)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.DataNascimento)
            .WithErrorMessage("Data de nascimento não pode ser no futuro");
    }

    [Fact]
    public void Should_Have_Error_When_DataNascimento_Is_Invalid()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = default
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.DataNascimento);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        CompletarCadastroRequest request = new CompletarCadastroRequest
        {
            IdToken = "valid-token",
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<CompletarCadastroRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
