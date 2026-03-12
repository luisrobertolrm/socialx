using Xunit;
using FluentValidation.TestHelper;
using SocialX.Service.DTOs;
using SocialX.Service.Validadores;

namespace SocialX.Service.Tests.Validadores;

public class UpdateProfileRequestValidatorTests
{
    private readonly UpdateProfileRequestValidator validator;

    public UpdateProfileRequestValidatorTests()
    {
        validator = new UpdateProfileRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Apelido_Is_Empty()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Apelido)
            .WithErrorMessage("Campo obrigatório");
    }

    [Fact]
    public void Should_Have_Error_When_Apelido_Exceeds_MaxLength()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = new string('a', 51),
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Apelido)
            .WithErrorMessage("Apelido deve ter no máximo 50 caracteres");
    }

    [Fact]
    public void Should_Have_Error_When_Telefone_Is_Empty()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

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
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = telefone,
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Telefone);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("abc")]
    [InlineData("12345")]
    [InlineData("123456789")]
    public void Should_Have_Error_When_Telefone_Is_Invalid_Format(string telefone)
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = telefone,
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Telefone)
            .WithErrorMessage("Formato de telefone inválido");
    }

    [Fact]
    public void Should_Have_Error_When_DataNascimento_Is_Future_Date()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddDays(1)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.DataNascimento)
            .WithErrorMessage("Data de nascimento não pode ser no futuro");
    }

    [Fact]
    public void Should_Have_Error_When_DataNascimento_Is_Invalid()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = default
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.DataNascimento);
    }

    [Fact]
    public void Should_Have_Error_When_FotoPerfil_Exceeds_MaxLength()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            FotoPerfil = new string('a', 501)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FotoPerfil)
            .WithErrorMessage("URL da foto de perfil deve ter no máximo 500 caracteres");
    }

    [Fact]
    public void Should_Not_Have_Error_When_FotoPerfil_Is_Null()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            FotoPerfil = null
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.FotoPerfil);
    }

    [Fact]
    public void Should_Have_Error_When_Bio_Exceeds_MaxLength()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            Bio = new string('a', 501)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Bio)
            .WithErrorMessage("Bio deve ter no máximo 500 caracteres");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Bio_Is_Null()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            Bio = null
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Bio);
    }

    [Fact]
    public void Should_Have_Error_When_Cidade_Exceeds_MaxLength()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            Cidade = new string('a', 101)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Cidade)
            .WithErrorMessage("Cidade deve ter no máximo 100 caracteres");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Cidade_Is_Null()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            Cidade = null
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Cidade);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Required_Fields_Are_Valid()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20)
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid_Including_Optional()
    {
        UpdateProfileRequest request = new UpdateProfileRequest
        {
            Apelido = "TestUser",
            Telefone = "11987654321",
            DataNascimento = DateTime.UtcNow.AddYears(-20),
            FotoPerfil = "https://example.com/photo.jpg",
            Bio = "This is my bio",
            Cidade = "São Paulo"
        };

        TestValidationResult<UpdateProfileRequest> result = validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
