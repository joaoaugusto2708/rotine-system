using LifeQuest.Application.Common.Authentication;
using LifeQuest.Infrastructure.Identity;
using LifeQuest.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Infrastructure.Authentication;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly LifeQuestDbContext _dbContext;

    public AuthenticationService(UserManager<ApplicationUser> userManager, ITokenService tokenService, LifeQuestDbContext dbContext)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _dbContext = dbContext;
    }

    public async Task<LoginResult?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, roles);

        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken( user.Id, refreshToken.TokenHash, refreshToken.ExpiresAtUtc);

        _dbContext.RefreshTokens.Add(refreshTokenEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResult(accessToken.Token, accessToken.ExpiresAtUtc, refreshToken.Token, refreshToken.ExpiresAtUtc);
    }

    public async Task<LoginResult?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenHash = _tokenService.HashRefreshToken(refreshToken);

        var currentToken =
            await _dbContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync( x => x.TokenHash == tokenHash, cancellationToken);

        if (currentToken is null || !currentToken.IsActive)
            return null;

        var user = currentToken.User;

        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, roles);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        currentToken.Revoke( newRefreshToken.TokenHash);

        var newRefreshTokenEntity = new RefreshToken(user.Id, newRefreshToken.TokenHash, newRefreshToken.ExpiresAtUtc);

        _dbContext.RefreshTokens.Add(newRefreshTokenEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResult(accessToken.Token, accessToken.ExpiresAtUtc, newRefreshToken.Token, newRefreshToken.ExpiresAtUtc);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenHash = _tokenService.HashRefreshToken(refreshToken);

        var token =
            await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash,
                    cancellationToken);

        if (token is null || token.IsRevoked)
            return;

        token.Revoke();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}