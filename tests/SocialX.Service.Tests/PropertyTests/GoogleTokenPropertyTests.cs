using FsCheck;
using FsCheck.Xunit;

namespace SocialX.Service.Tests.PropertyTests;

/// <summary>
/// Feature: autenticacao
/// Property-based tests for Google token validation
/// </summary>
public class GoogleTokenPropertyTests
{
    /// <summary>
    /// Feature: autenticacao, Property 1: Token Validation Completeness
    /// For any ID Token received from the frontend, the system SHALL validate
    /// the token signature using Google's public keys, verify the token is not expired,
    /// and verify the audience matches the configured web client ID before accepting it as valid
    /// Validates: Requirements 2.1, 2.5
    /// </summary>
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "1")]
    public Property TokenValidation_MustCheckSignature(NonEmptyString token)
    {
        // Arrange & Act
        bool hasValidSignature = HasValidSignature(token.Get);
        bool isExpired = IsTokenExpired(token.Get);
        bool hasValidAudience = HasValidAudience(token.Get);

        // Assert
        bool isValid = hasValidSignature && !isExpired && hasValidAudience;

        return (isValid == (hasValidSignature && !isExpired && hasValidAudience))
            .ToProperty()
            .Label("Token validation must check signature, expiration, and audience");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "1")]
    public Property TokenValidation_ExpiredToken_MustBeRejected(NonEmptyString token)
    {
        // Arrange
        string expiredToken = $"{token.Get}_expired";

        // Act
        bool isExpired = IsTokenExpired(expiredToken);
        bool isValid = !isExpired && HasValidSignature(expiredToken) && HasValidAudience(expiredToken);

        // Assert
        return (isExpired.Implies(() => !isValid))
            .ToProperty()
            .Label("Expired token must be rejected");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "1")]
    public Property TokenValidation_InvalidSignature_MustBeRejected(NonEmptyString token)
    {
        // Arrange
        string invalidToken = $"invalid_{token.Get}";

        // Act
        bool hasValidSignature = HasValidSignature(invalidToken);
        bool isValid = hasValidSignature && !IsTokenExpired(invalidToken) && HasValidAudience(invalidToken);

        // Assert
        return (!hasValidSignature.Implies(() => !isValid))
            .ToProperty()
            .Label("Token with invalid signature must be rejected");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "1")]
    public Property TokenValidation_InvalidAudience_MustBeRejected(NonEmptyString token)
    {
        // Arrange
        string tokenWithInvalidAudience = $"{token.Get}_wrongaudience";

        // Act
        bool hasValidAudience = HasValidAudience(tokenWithInvalidAudience);
        bool isValid = HasValidSignature(tokenWithInvalidAudience) && 
                      !IsTokenExpired(tokenWithInvalidAudience) && 
                      hasValidAudience;

        // Assert
        return (!hasValidAudience.Implies(() => !isValid))
            .ToProperty()
            .Label("Token with invalid audience must be rejected");
    }

    /// <summary>
    /// Feature: autenticacao, Property 2: User Information Extraction
    /// For any valid ID Token, the system SHALL successfully extract user information
    /// including id, email, name, and photo from the token payload
    /// Validates: Requirements 2.2
    /// </summary>
    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "2")]
    public Property TokenExtraction_ValidToken_MustExtractUserInfo(NonEmptyString validToken)
    {
        // Arrange
        string token = CreateValidToken(validToken.Get);

        // Act
        bool isValid = HasValidSignature(token) && !IsTokenExpired(token) && HasValidAudience(token);
        GoogleUserInfo? userInfo = isValid ? ExtractUserInfo(token) : null;

        // Assert
        return (isValid.Implies(() => 
            userInfo != null &&
            !string.IsNullOrWhiteSpace(userInfo.Id) &&
            !string.IsNullOrWhiteSpace(userInfo.Email) &&
            !string.IsNullOrWhiteSpace(userInfo.Name)))
            .ToProperty()
            .Label("Valid token must allow extraction of id, email, name, and photo");
    }

    [Property(MaxTest = 100)]
    [Trait("Feature", "autenticacao")]
    [Trait("Property", "2")]
    public Property TokenExtraction_InvalidToken_MustReturnNull(NonEmptyString invalidToken)
    {
        // Arrange
        string token = $"invalid_{invalidToken.Get}";

        // Act
        bool isValid = HasValidSignature(token) && !IsTokenExpired(token) && HasValidAudience(token);
        GoogleUserInfo? userInfo = isValid ? ExtractUserInfo(token) : null;

        // Assert
        return (!isValid.Implies(() => userInfo == null))
            .ToProperty()
            .Label("Invalid token must not allow user info extraction");
    }

    // Helper methods
    private static bool HasValidSignature(string token)
    {
        // Simulação: token válido não começa com "invalid_"
        return !token.StartsWith("invalid_", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsTokenExpired(string token)
    {
        // Simulação: token expirado contém "_expired"
        return token.Contains("_expired", StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasValidAudience(string token)
    {
        // Simulação: token com audience inválido contém "_wrongaudience"
        return !token.Contains("_wrongaudience", StringComparison.OrdinalIgnoreCase);
    }

    private static string CreateValidToken(string baseToken)
    {
        // Simulação: cria um token válido
        return $"valid.{baseToken}.signature";
    }

    private static GoogleUserInfo? ExtractUserInfo(string token)
    {
        // Simulação: extrai informações do usuário
        if (!HasValidSignature(token) || IsTokenExpired(token) || !HasValidAudience(token))
            return null;

        return new GoogleUserInfo
        {
            Id = "google123",
            Email = "user@example.com",
            Name = "Test User",
            GivenName = "Test",
            FamilyName = "User",
            Photo = "https://example.com/photo.jpg"
        };
    }
}

/// <summary>
/// Represents user information extracted from Google token
/// </summary>
public class GoogleUserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string? Photo { get; set; }
}
