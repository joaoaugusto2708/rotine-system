using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LifeQuest.Application.Common.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LifeQuest.Infrastructure.Authentication;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public AccessTokenResult GenerateAccessToken(Guid userId, string? email, IEnumerable<string> roles)
    {
        var now = DateTimeOffset.UtcNow;

        var expiresAt = now.AddMinutes(_settings.AccessTokenExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),

            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        if (!string.IsNullOrWhiteSpace(email))
        {
            claims.Add( new Claim(JwtRegisteredClaimNames.Email, email));
        }

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(_settings.Secret));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessTokenResult(tokenValue, expiresAt);
    }

    public RefreshTokenResult GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        var token = WebEncoders.Base64UrlEncode(bytes);

        var tokenHash = HashRefreshToken(token);

        var expiresAt = DateTimeOffset.UtcNow.AddDays( _settings.RefreshTokenExpirationDays);

        return new RefreshTokenResult(token, tokenHash, expiresAt);
    }
    public string HashRefreshToken(string token)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(hashBytes);
    }
}