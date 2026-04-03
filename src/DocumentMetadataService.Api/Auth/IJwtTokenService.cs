using DocumentMetadataService.Api.Domain;
using DocumentMetadataService.Api.Contracts;

namespace DocumentMetadataService.Api.Auth;

public interface IJwtTokenService
{
    AuthResponse CreateToken(User user);
}
