namespace LifeQuest.Application.Common.Authentication;

public sealed record RefreshTokenResult(
    string Token,
    string TokenHash,
    DateTimeOffset ExpiresAtUtc);