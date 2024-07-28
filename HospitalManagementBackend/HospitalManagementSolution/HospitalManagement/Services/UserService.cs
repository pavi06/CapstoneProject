using Hangfire;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Jobs;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.UserDTOs;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, UserLoginDetails> _userLoginRepository;
        private readonly IRepository<int, User> _userRepository;
        private readonly ITokenService _tokenService;
        protected static String OtpGenerated { get; set; }
        protected static User currentUser { get; set; }

        public UserService(IRepository<int, UserLoginDetails> userLoginRepository, IRepository<int, User> userRepository, ITokenService tokenService)
        {
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        #region Login
        public async Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            try
            {
                var user = _userRepository.Get().Result.SingleOrDefault(u => u.EmailId.ToLower() == loginDTO.Email.ToLower());
                if (user == null)
                {
                    throw new ObjectNotAvailableException("User");
                }
                var userLogin = await _userLoginRepository.Get(user.UserId);
                if (userLogin == null)
                {
                    throw new UnauthorizedUserException();
                }
                HMACSHA512 hMACSHA = new HMACSHA512(userLogin.PasswordHashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                bool isPasswordSame = ComparePassword(encrypterPass, userLogin.Password);
                if (isPasswordSame)
                {
                    if (userLogin.Status == "Active")
                    {
                        UserLoginReturnDTO loginReturnDTO = await MapUserToLoginReturn(user);
                        return loginReturnDTO;
                    }
                    throw new UserNotActiveException();
                }
                throw new UnauthorizedUserException();
            }
            catch (ObjectsNotAvailableException)
            {
                throw new ObjectNotAvailableException("User");
            }
        }

        private async Task<UserLoginReturnDTO> MapUserToLoginReturn(User user)
        {
            UserLoginReturnDTO returnDTO = new UserLoginReturnDTO();
            returnDTO.UserId = user.UserId;
            returnDTO.UserName = user.Name;
            returnDTO.Role = user.Role ?? "User";
            returnDTO.AccessToken = _tokenService.GenerateToken(user);
            var token = _tokenService.GenerateRefreshToken();
            returnDTO.RefreshToken = token.RfrshToken;
            var userLogin = await _userLoginRepository.Get(user.UserId);
            userLogin.RefreshToken = token.RfrshToken;
            userLogin.CreatedOn = token.Created;
            userLogin.ExpiresOn = token.ExpiresOn;
            await _userLoginRepository.Update(userLogin);
            return returnDTO;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        public static int CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        #region Register
        public async Task<UserReturnDTO> Register(UserRegistrationDTO userDTO)
        {
            User userAdded = null, user = null;
            UserLoginDetails userLoginAdded = null;
            try
            {
                user = new User(userDTO.Name, userDTO.DateOfBirth, CalculateAge(userDTO.DateOfBirth), userDTO.Gender, userDTO.EmailId, userDTO.ContactNo, userDTO.Address);
                var userLogin = MapUserDetailsToUser(userDTO);
                userAdded = await _userRepository.Add(user);
                userLogin.UserId = userAdded.UserId;
                var refToken = _tokenService.GenerateRefreshToken();
                userLogin.RefreshToken = refToken.RfrshToken;
                userLogin.CreatedOn = refToken.Created;
                userLogin.ExpiresOn = refToken.ExpiresOn;
                userLoginAdded = await _userLoginRepository.Add(userLogin);                
                UserReturnDTO addedUser = new UserReturnDTO(userAdded.UserId, userAdded.Name, userAdded.EmailId, userAdded.Role, _tokenService.GenerateToken(userAdded), refToken.RfrshToken);
                return addedUser;
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("User");
            }
            catch (Exception) { }
            if (userAdded != null)
                await RevertUserInsert(user);
            if (userLoginAdded != null && userAdded == null)
                await RevertUserLoginInsert(userLoginAdded);
            throw new UnableToRegisterException();
        }

        private async Task RevertUserLoginInsert(UserLoginDetails user)
        {
            await _userLoginRepository.Delete(user.UserId);
        }

        private async Task RevertUserInsert(User user)
        {

            await _userRepository.Delete(user.UserId);
        }
        private UserLoginDetails MapUserDetailsToUser(UserRegistrationDTO userDTO)
        {
            UserLoginDetails user = new UserLoginDetails();
            user.Status = "Disabled";
            HMACSHA512 hMACSHA = new HMACSHA512();
            user.PasswordHashKey = hMACSHA.Key;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
            return user;
        }

        #endregion


        #region LoginWithContactNo
        public async Task LoginWithContactNo(UserLoginWithContactDTO userDTO)
        {
            currentUser = _userRepository.Get().Result.Where(u=>u.ContactNo == userDTO.ContactNumber && u.Name == userDTO.UserName).FirstOrDefault();
            if(currentUser == null)
            {
                throw new ObjectNotAvailableException("User");
            }
            OtpGenerated = BackgroundJobs.GenerateOTPAndSend(currentUser.ContactNo, currentUser.Name);
        }

        public async Task<UserLoginReturnDTO> VerifyOTPAndGiveAccess(string otp)
        {
            if(OtpGenerated == otp)
            {
                return await MapUserToLoginReturn(currentUser);
            }
            throw new UnauthorizedAccessException();
        }
        #endregion


    }
}
