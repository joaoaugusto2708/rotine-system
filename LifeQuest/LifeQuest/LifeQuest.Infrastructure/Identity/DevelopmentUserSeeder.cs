using LifeQuest.Application.Common.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LifeQuest.Infrastructure.Identity;

public sealed class DevelopmentUserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public DevelopmentUserSeeder(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        _userManager = userManager;
        _configuration = configuration;
        _environment = environment;
    }

    public async Task SeedAsync()
    {
        if (!_environment.IsDevelopment())
            return;

        var email =
            _configuration["DevelopmentUser:Email"];

        var password =
            _configuration["DevelopmentUser:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var existingUser =
            await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
            return;

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var createResult =
            await _userManager.CreateAsync(
                user,
                password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                "; ",
                createResult.Errors.Select(x => x.Description));

            throw new InvalidOperationException(
                $"Não foi possível criar o usuário de desenvolvimento: {errors}");
        }

        var roleResult =
            await _userManager.AddToRoleAsync(
                user,
                RoleNames.User);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(
                "; ",
                roleResult.Errors.Select(x => x.Description));

            throw new InvalidOperationException(
                $"Não foi possível atribuir a role User: {errors}");
        }
    }
}