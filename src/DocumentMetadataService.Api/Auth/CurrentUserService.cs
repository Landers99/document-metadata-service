using System.Security.Claims;

namespace DocumentMetadataService.Api.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException("No HTTP context user is available.");

        var rawUserId =
            user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            user.FindFirstValue("sub");

        if (rawUserId is null || !Guid.TryParse(rawUserId, out var userId))
        {
            throw new InvalidOperationException("Authenticated user id claim is missing or invalid.");
        }

        return userId;
    }

    public string? GetEmail()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirstValue(ClaimTypes.Email)
            ?? user?.FindFirstValue("email");
    }
}
