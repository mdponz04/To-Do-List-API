using UserService.DAL.Entities.BaseModelEntity;

namespace UserService.DAL.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public virtual Account? Account { get; set; }
    }
}
