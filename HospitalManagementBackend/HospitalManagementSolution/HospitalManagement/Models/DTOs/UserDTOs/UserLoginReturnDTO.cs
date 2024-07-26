namespace HospitalManagement.Models.DTOs.UserDTOs
{
    public class UserLoginReturnDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
