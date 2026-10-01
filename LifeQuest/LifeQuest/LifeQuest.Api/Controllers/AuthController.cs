using LifeQuest.Api.Contracts.Authentication;
using LifeQuest.Application.Common.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifeQuest.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private const string RefreshTokenCookie = "lifequest_refresh_token";

    private readonly IAuthenticationService _authenticationService;

    public AuthController(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.LoginAsync(request.Email, request.Password, cancellationToken);

        if (result is null)
        {
            return Unauthorized(
                new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Credenciais inválidas."
                });
        }

        Response.Cookies.Append(
            RefreshTokenCookie,
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires =
                    result.RefreshTokenExpiresAtUtc,
                Path = "/api/v1/auth"
            });

        return Ok(
            new LoginResponse(
                result.AccessToken,
                result.AccessTokenExpiresAtUtc));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var roles =
            User.FindAll(ClaimTypes.Role)
                .Select(x => x.Value);

        return Ok(new
        {
            userId,
            roles
        });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh(
    CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(
            RefreshTokenCookie,
            out var refreshToken))
        {
            return Unauthorized();
        }

        var result = await _authenticationService.RefreshAsync(refreshToken, cancellationToken);

        if (result is null)
        {
            return Unauthorized();
        }

        SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAtUtc);

        return Ok(
            new LoginResponse(result.AccessToken, result.AccessTokenExpiresAtUtc));
    }

    private void SetRefreshTokenCookie(string token, DateTimeOffset expiresAtUtc)
    {
        Response.Cookies.Append(
            RefreshTokenCookie,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiresAtUtc,
                Path = "/api/v1/auth"
            });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue(RefreshTokenCookie, out var refreshToken))
            await _authenticationService.LogoutAsync(refreshToken, cancellationToken);

        Response.Cookies.Delete(
            RefreshTokenCookie,
            new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/v1/auth"
            });

        return NoContent();
    }
}