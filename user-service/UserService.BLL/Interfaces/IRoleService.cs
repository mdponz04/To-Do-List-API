using UserService.BLL.Dtos.Permission;
using UserService.BLL.Dtos.Role;

namespace UserService.BLL.Interfaces
{
    public interface IRoleService
    {
        public Task<IEnumerable<RoleGetDto>> GetByAccountId(Guid accountId);
        public Task<RoleGetDto?> Create(RolePostDto role);
        public Task<RoleGetDto?> Update(Guid roleId, RolePutDto role);
        public Task<bool> Delete(Guid roleId);
        public Task<RoleGetDto?> Get(Guid roleId);
        public Task<IEnumerable<RoleGetDto>> GetAll();
        public Task<bool> HasPermissionAsync(Guid accountId, string permissionName);
        public Task AddPermissionToRole(Guid roleId, Guid permissionId);
        public Task RemovePermissionFromRole(Guid roleId, Guid permissionId);
        public Task<IEnumerable<PermissionGetDto>> GetPermissionsByRoleIdAsync(Guid roleId);
        public Task<IEnumerable<PermissionGetDto>> GetMissingPermissionByRoleIdAsync(Guid roleId);
    }
}
