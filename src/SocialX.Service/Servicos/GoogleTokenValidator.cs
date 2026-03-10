using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;

namespace SocialX.Service.Servicos;

public class GoogleTokenValidator : IGoogleTokenValidator
{
    private readonly string _googleClientId;

    public GoogleTokenValidator(IConfiguration configuration)
    {
        string? clientId = configuration["Google:ClientId"];
        
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException("Google Client ID não configurado no appsettings.json");
        }
        
        _googleClientId = clientId;
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateTokenAsync(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            throw new ArgumentException("ID Token não pode ser vazio", nameof(idToken));
        }

        GoogleJsonWebSignature.ValidationSettings validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _googleClientId }
        };

        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(
            idToken,
            validationSettings
        );

        return payload;
    }

    public GoogleUserInfo ExtractUserInfo(GoogleJsonWebSignature.Payload payload)
    {
        if (payload == null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        GoogleUserInfo userInfo = new GoogleUserInfo
        {
            Id = payload.Subject,
            Email = payload.Email,
            Name = payload.Name ?? string.Empty,
            GivenName = payload.GivenName ?? string.Empty,
            FamilyName = payload.FamilyName ?? string.Empty,
            Picture = payload.Picture
        };

        return userInfo;
    }
}
