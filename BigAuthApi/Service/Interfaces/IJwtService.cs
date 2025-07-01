using BigAuthApi.Model;
using System.Security.Claims;

namespace BigAuthApi.Service.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}