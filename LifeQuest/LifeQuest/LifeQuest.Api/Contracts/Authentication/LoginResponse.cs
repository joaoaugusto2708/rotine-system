namespace LifeQuest.Api.Contracts.Authentication;

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc);