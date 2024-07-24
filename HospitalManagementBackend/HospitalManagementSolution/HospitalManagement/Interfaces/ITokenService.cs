using HospitalManagement.Models;
using System.Security.Claims;

namespace HospitalManagement.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(UserDetails user);
        public RefreshToken GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
