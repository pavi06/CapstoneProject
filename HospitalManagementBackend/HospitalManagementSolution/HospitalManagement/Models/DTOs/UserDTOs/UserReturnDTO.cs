namespace HospitalManagement.Models.DTOs.UserDTOs
{
    public class UserReturnDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UserReturnDTO(int userId, string name, string email, string role, string accessToken, string refreshToken)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Role = role;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
