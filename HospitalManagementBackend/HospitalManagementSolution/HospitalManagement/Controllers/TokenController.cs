using HospitalManagement.Contexts;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly HospitalManagementContext _context;
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService, HospitalManagementContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }


        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> Refresh(Token tokenModel)
        {
            if (tokenModel == null)
                return BadRequest("Invalid client request");
            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.FindFirstValue("UserId");
            var user = _context.Users.SingleOrDefault(u => u.PersonId.ToString() == username);
            if (user == null || user.RefreshToken != refreshToken || user.ExpiresOn <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GenerateToken(_context.UserDetails.SingleOrDefault(u => u.UserId == user.PersonId));
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.RfrshToken;
            user.CreatedOn = newRefreshToken.Created;
            user.ExpiresOn = newRefreshToken.ExpiresOn;
            await _context.SaveChangesAsync();
            return Ok(new AuthenticatedResponseToken()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.RfrshToken
            });
        }



        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.FindFirstValue("UserId");
            var user = _context.Users.SingleOrDefault(u => u.PersonId.ToString() == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            user.ExpiresOn = DateTime.MinValue;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
