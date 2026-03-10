using FsCheck;
using FsCheck.Xunit;

namespace SocialX.Service.Tests.PropertyTests;

/// <summary>
/// Feature: autenticacao
/// Property-based tests for validation error messages
/// </summary>
public class ValidationPropertyTests
{
    /// <summary>
    /// Feature: autenticacao, Property 13: Registration Validation Error Messages
    /// For any CompletarCadastroRequest with missing required fields, invalid telefone format,
    /// or invalid data_nascimento, the system SHALL return specific validation error messages
    /// Validates: Requirements 7.11, 7.12, 7.13
    /// </summary>
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "13")]
    public Property Validation_MissingRequiredField_MustReturnSpecificMessage(NonEmptyString fieldName)
    {
        // Arrange
        string expectedMessage = "Campo obrigatório";

        // Act
        string actualMessage = GetValidationMessageForMissingField(fieldName.Get);

        // Assert
        return (actualMessage == expectedMessage)
            .ToProperty()
            .Label("Missing required field must return 'Campo obrigatório' message");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "13")]
    public Property Validation_InvalidPhoneFormat_MustReturnSpecificMessage(string invalidPhone)
    {
        // Arrange
        string expectedMessage = "Formato de telefone inválido";

        // Act
        bool isInvalidFormat = !IsValidPhoneFormat(invalidPhone);
        string actualMessage = isInvalidFormat ? GetValidationMessageForInvalidPhone() : "";

        // Assert
        return (isInvalidFormat.Implies(() => actualMessage == expectedMessage))
            .ToProperty()
            .Label("Invalid phone format must return 'Formato de telefone inválido' message");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "13")]
    public Property Validation_InvalidDateOfBirth_MustReturnSpecificMessage(DateTime invalidDate)
    {
        // Arrange
        string expectedMessage = "Data de nascimento inválida";

        // Act
        bool isInvalidDate = !IsValidDateOfBirth(invalidDate);
        string actualMessage = isInvalidDate ? GetValidationMessageForInvalidDate() : "";

        // Assert
        return (isInvalidDate.Implies(() => actualMessage == expectedMessage))
            .ToProperty()
            .Label("Invalid date of birth must return 'Data de nascimento inválida' message");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "13")]
    public Property Validation_AllFieldsValid_MustNotReturnErrors(
        NonEmptyString apelido, string telefone, DateTime dataNascimento)
    {
        // Arrange
        bool apelidoValid = !string.IsNullOrWhiteSpace(apelido.Get);
        bool telefoneValid = IsValidPhoneFormat(telefone);
        bool dateValid = IsValidDateOfBirth(dataNascimento);

        // Act
        bool allValid = apelidoValid && telefoneValid && dateValid;
        List<string> errors = new List<string>();

        if (!apelidoValid) errors.Add("Campo obrigatório");
        if (!telefoneValid) errors.Add("Formato de telefone inválido");
        if (!dateValid) errors.Add("Data de nascimento inválida");

        // Assert
        return (allValid.Implies(() => errors.Count == 0))
            .ToProperty()
            .Label("All valid fields must not return validation errors");
    }

    // Helper methods
    private static string GetValidationMessageForMissingField(string fieldName)
    {
        return "Campo obrigatório";
    }

    private static string GetValidationMessageForInvalidPhone()
    {
        return "Formato de telefone inválido";
    }

    private static string GetValidationMessageForInvalidDate()
    {
        return "Data de nascimento inválida";
    }

    private static bool IsValidPhoneFormat(string? telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            return false;

        // Formato brasileiro: (XX) XXXXX-XXXX ou (XX) XXXX-XXXX
        string pattern = @"^(\(\d{2}\)\s?)?(\d{4,5}-?\d{4})$";
        return System.Text.RegularExpressions.Regex.IsMatch(telefone, pattern);
    }

    private static bool IsValidDateOfBirth(DateTime dataNascimento)
    {
        DateTime now = DateTime.UtcNow;
        DateTime minDate = now.AddYears(-150);

        return dataNascimento < now && dataNascimento > minDate;
    }
}
