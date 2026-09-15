using UserService.DAL.Entities.BaseModelEntity;

namespace UserService.DAL.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
