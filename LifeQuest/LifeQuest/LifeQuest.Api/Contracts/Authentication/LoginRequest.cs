using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Api.Contracts.Authentication;

public sealed record LoginRequest(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Password);