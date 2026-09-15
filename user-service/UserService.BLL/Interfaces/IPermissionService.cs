using UserService.BLL.Dtos.Permission;

namespace UserService.BLL.Interfaces
{
    public interface IPermissionService
    {
        public Task<bool> CheckPermissionAsync(Guid roleId, string permission);
        public Task<IEnumerable<PermissionGetDto>> GetByRoleId(Guid roleId);
        public Task<PermissionGetDto?> Create(PermissionPostDto permission);
        public Task<PermissionGetDto?> Update(Guid permissionId, PermissionPutDto permission);
        public Task<bool> Delete(Guid permissionId);
        public Task<PermissionGetDto?> Get(Guid permissionId);
        public Task<IEnumerable<PermissionGetDto>> GetAll();
    }
}
