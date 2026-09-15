using UserService.DAL.Entities.BaseModelEntity;

namespace UserService.DAL.Entities
{
    public class User : BaseEntity
    {
        public Guid AccountId { get; set; }
        public required string UserName { get; set; }
        public virtual Account? Account { get; set; }
    }
}
