using UserService.DAL.Entities.BaseModelEntity;

namespace UserService.DAL.Entities
{
    public class Account : BaseEntity
    {
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual User? User { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
