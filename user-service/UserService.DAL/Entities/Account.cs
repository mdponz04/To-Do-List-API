using UserService.DAL.Entities.BaseModelEntity;

namespace UserService.DAL.Entities
{
    public class Account : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public virtual User? User { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
