using UserService.DAL.Entities.BaseModelEntity;

namespace UserService.DAL.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
