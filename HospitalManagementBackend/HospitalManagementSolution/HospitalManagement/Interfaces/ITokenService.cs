using HospitalManagement.Models;
using System.Security.Claims;

namespace HospitalManagement.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
        public RefreshToken GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
