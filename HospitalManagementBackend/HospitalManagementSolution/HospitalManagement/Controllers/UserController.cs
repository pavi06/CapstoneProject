using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.UserDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
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
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginReturnDTO>> LoginForExternalUsers([FromBody] string contactNo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await _userService.LoginWithContactNo(contactNo);
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
    }
}
