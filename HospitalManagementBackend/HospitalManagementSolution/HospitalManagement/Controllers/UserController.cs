using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.UserDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [AllowAnonymous]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        #region UserLogin
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginReturnDTO>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await _userService.Login(userLoginDTO);
                    _logger.LogInformation("Login successfull");
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("User not authenticated!");
                    return Unauthorized(new ErrorModel(401, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check");
        }
        #endregion

        #region UserRegistration
        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReturnDTO>> Register([FromBody] UserRegistrationDTO userDTO)
        {
            try
            {
                var user = await _userService.Register(userDTO);
                _logger.LogInformation("Registration successfull");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot register at this moment");
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }
        #endregion

        #region ExternalUserLogin
        [HttpPost("ExternalLogin")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> LoginForExternalUsers([FromBody] UserLoginWithContactDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.LoginWithContactNo(userDTO);
                    _logger.LogInformation("Otp sent successfull");
                    return Ok("Otp sent successfully!");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("User not authenticated!");
                    return Unauthorized(new ErrorModel(401, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check");
        }
        #endregion

        #region ExternalUserLogin
        [HttpPost("VerifyOTPForExternalLogin")]
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginReturnDTO>> ValidateExternalUsers([FromBody] string otp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await _userService.VerifyOTPAndGiveAccess(otp);
                    _logger.LogInformation("Otp is verified successfull");
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("User not authenticated!");
                    return Unauthorized(new ErrorModel(401, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check");
        }
        #endregion
    }
}
