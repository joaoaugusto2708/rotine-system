using LifeQuest.Application.Common.Authentication;

public interface IAuthenticationService
{
    Task<LoginResult?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<LoginResult?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
}