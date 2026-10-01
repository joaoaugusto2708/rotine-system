using Microsoft.AspNetCore.Identity;

namespace LifeQuest.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        Id = Guid.NewGuid();
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public DateTimeOffset CreatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

}
