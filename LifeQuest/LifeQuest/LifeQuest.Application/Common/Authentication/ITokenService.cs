namespace LifeQuest.Application.Common.Authentication;

public interface ITokenService
{
    AccessTokenResult GenerateAccessToken(Guid userId, string? email, IEnumerable<string> roles);

    RefreshTokenResult GenerateRefreshToken();

    string HashRefreshToken(string token);
}