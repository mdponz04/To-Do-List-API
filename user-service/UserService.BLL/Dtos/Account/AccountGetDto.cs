using UserService.BLL.Dtos.User;

namespace UserService.BLL.Dtos.Account
{
    public class AccountGetDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public UserGetDto? User { get; set; }
        public string? Roles { get; set; }
    }
}
