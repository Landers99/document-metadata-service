namespace DocumentMetadataService.Api.Contracts;

public sealed class AuthResponse
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
}
