using LifeQuest.Application.Common.Security;
using Microsoft.AspNetCore.Identity;

namespace LifeQuest.Infrastructure.Identity;

public sealed class IdentitySeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentitySeeder(
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        foreach (var roleName in RoleNames.All)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                continue;

            var result = await _roleManager.CreateAsync(
                new IdentityRole<Guid>(roleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Não foi possível criar a role '{roleName}': {errors}");
            }
        }
    }
}