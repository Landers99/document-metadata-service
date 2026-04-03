namespace DocumentMetadataService.Api.Auth;

public interface ICurrentUserService
{
    Guid GetUserId();
    string? GetEmail();
    bool IsAuthenticated { get; }
}
