namespace LifeQuest.Infrastructure.Identity;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset ExpiresAtUtc { get; private set; }

    public DateTimeOffset? RevokedAtUtc { get; private set; }

    public string? ReplacedByTokenHash { get; private set; }

    public ApplicationUser User { get; private set; } = null!;

    private RefreshToken()
    {
    }

    public RefreshToken(
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAtUtc)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        ExpiresAtUtc = expiresAtUtc;
    }

    public bool IsExpired =>
        DateTimeOffset.UtcNow >= ExpiresAtUtc;

    public bool IsRevoked =>
        RevokedAtUtc.HasValue;

    public bool IsActive =>
        !IsExpired && !IsRevoked;

    public void Revoke(string? replacedByTokenHash = null)
    {
        if (IsRevoked)
            return;

        RevokedAtUtc = DateTimeOffset.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}