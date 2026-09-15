using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IRolePermissionRepo
    {
        public Task AddRolePermissionAsync(Guid roleId, Guid permissionId);
        public Task RemoveRolePermissionAsync(Guid roleId, Guid permissionId);
        public Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
    }
}
