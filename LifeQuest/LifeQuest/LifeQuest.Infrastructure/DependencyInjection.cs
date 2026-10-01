using LifeQuest.Application.Common.Authentication;
using LifeQuest.Infrastructure.Authentication;
using LifeQuest.Infrastructure.Identity;
using LifeQuest.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LifeQuest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("LifeQuestDatabase") ?? throw new InvalidOperationException(
                "Connection string 'LifeQuestDatabase' not found.");

        services.AddDataProtection();

        services.AddDbContext<LifeQuestDbContext>(options => options.UseMySQL(connectionString));

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<LifeQuestDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddScoped<IdentitySeeder>();
        services.AddScoped<DevelopmentUserSeeder>();


        services
            .AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer),
                "JWT Issuer is required.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Audience),
                "JWT Audience is required.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Secret),
                "JWT Secret is required.")
            .ValidateOnStart();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT configuration not found.");

        services
            .AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier,

                        ClockSkew = TimeSpan.Zero
                    };
            });

        services.AddAuthorization();

        return services;
    }
}