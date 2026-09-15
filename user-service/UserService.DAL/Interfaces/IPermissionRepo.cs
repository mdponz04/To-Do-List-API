using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IPermissionRepo
    {
        public Task<Permission?> Create(Permission permission);
        public Task<Permission?> Update(Permission permission);
        public Task<bool> Delete(Guid permissionId);
        public Task<Permission?> Get(Guid permissionId);
        public Task<IEnumerable<Permission>> GetAll();
    }
}
