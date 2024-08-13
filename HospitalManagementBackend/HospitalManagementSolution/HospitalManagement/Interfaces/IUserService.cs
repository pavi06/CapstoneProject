using HospitalManagement.Models.DTOs.UserDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IUserService
    {
        public Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO);
        public Task<UserReturnDTO> Register(UserRegistrationDTO userDTO);
        public Task LoginWithContactNo(UserLoginWithContactDTO userDTO);
        public Task<UserLoginReturnDTO> VerifyOTPAndGiveAccess(string otp);
    }
}
