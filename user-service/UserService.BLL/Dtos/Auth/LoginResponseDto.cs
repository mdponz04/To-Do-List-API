using UserService.BLL.Dtos.Account;

namespace UserService.BLL.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public AccountGetDto Account { get; set; } = null!;
    }
}
