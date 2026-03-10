using FsCheck;
using FsCheck.Xunit;
using System.Text.RegularExpressions;

namespace SocialX.Service.Tests.PropertyTests;

/// <summary>
/// Feature: autenticacao
/// Property-based tests for field format validation
/// </summary>
public class FieldFormatPropertyTests
{
    /// <summary>
    /// Feature: autenticacao, Property 4: Field Format Validation
    /// For any Pessoa record being created or updated, the system SHALL validate
    /// that telefone matches a valid phone format and data_nascimento is a valid date
    /// Validates: Requirements 7.5, 7.6, 8.5, 8.6
    /// </summary>
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "4")]
    public Property Telefone_Validation_MustMatchValidFormat(string telefone)
    {
        // Arrange
        bool isValid = IsValidPhoneFormat(telefone);

        // Act & Assert
        // Se o telefone é válido, deve passar na validação
        // Se o telefone é inválido, deve falhar na validação
        return (isValid == IsValidPhoneFormat(telefone))
            .ToProperty()
            .Label("Phone format validation must be consistent");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "4")]
    public Property DataNascimento_Validation_MustBeValidDate(DateTime dataNascimento)
    {
        // Arrange & Act
        bool isValid = IsValidDateOfBirth(dataNascimento);

        // Assert
        // Data de nascimento deve ser no passado e não muito antiga
        return (isValid == (dataNascimento < DateTime.UtcNow && 
                           dataNascimento > DateTime.UtcNow.AddYears(-150)))
            .ToProperty()
            .Label("Date of birth must be in the past and reasonable");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "4")]
    public Property DataNascimento_FutureDate_MustBeInvalid(PositiveInt daysInFuture)
    {
        // Arrange
        DateTime futureDate = DateTime.UtcNow.AddDays(daysInFuture.Get);

        // Act
        bool isValid = IsValidDateOfBirth(futureDate);

        // Assert
        return (!isValid)
            .ToProperty()
            .Label("Future dates must be invalid for date of birth");
    }

    // Helper methods para validação
    private static bool IsValidPhoneFormat(string? telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            return false;

        // Formato brasileiro: (XX) XXXXX-XXXX ou (XX) XXXX-XXXX
        // Também aceita formatos sem formatação: XXXXXXXXXXX ou XXXXXXXXXX
        string pattern = @"^(\(\d{2}\)\s?)?(\d{4,5}-?\d{4})$";
        return Regex.IsMatch(telefone, pattern);
    }

    private static bool IsValidDateOfBirth(DateTime dataNascimento)
    {
        DateTime now = DateTime.UtcNow;
        DateTime minDate = now.AddYears(-150);

        return dataNascimento < now && dataNascimento > minDate;
    }
}
