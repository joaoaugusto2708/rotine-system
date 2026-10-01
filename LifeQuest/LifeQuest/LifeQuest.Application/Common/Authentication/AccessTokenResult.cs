namespace LifeQuest.Application.Common.Authentication;

public sealed record AccessTokenResult(
    string Token,
    DateTimeOffset ExpiresAtUtc);