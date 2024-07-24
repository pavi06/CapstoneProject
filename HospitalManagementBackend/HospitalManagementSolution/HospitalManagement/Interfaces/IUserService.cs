namespace HospitalManagement.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Method for login process. check if a person already an user or not
        /// </summary>
        /// <param name="loginDTO">Dto with email and password value</param>
        /// <returns>return dto which holds token along with user role</returns>
        //public Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO);
        //public Task<UserReturnDTO> Register(UserRegisterDTO guestDTO);
    }
}
