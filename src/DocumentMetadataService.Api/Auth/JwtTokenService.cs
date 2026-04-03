using DocumentMetadataService.Api.Domain;
using DocumentMetadataService.Api.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentMetadataService.Api.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public AuthResponse CreateToken(User user)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };

        var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey));

        var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new AuthResponse
        {
            Token = token,
            Email = user.Email,
            ExpiresAtUtc = expiresAtUtc
        };
    }
}
