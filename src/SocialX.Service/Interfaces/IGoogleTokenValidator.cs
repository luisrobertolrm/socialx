using Google.Apis.Auth;
using SocialX.Service.DTOs;

namespace SocialX.Service.Interfaces;

public interface IGoogleTokenValidator
{
    Task<GoogleJsonWebSignature.Payload> ValidateTokenAsync(string idToken);
    GoogleUserInfo ExtractUserInfo(GoogleJsonWebSignature.Payload payload);
}
