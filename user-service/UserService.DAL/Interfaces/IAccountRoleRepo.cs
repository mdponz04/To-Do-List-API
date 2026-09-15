using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IAccountRoleRepo
    {
        public Task AddAccountRoleAsync(Guid accountId, Guid roleId);
        public Task RemoveAccountRoleAsync(Guid accountId, Guid roleId);
        public Task<IEnumerable<Role>> GetRolesByAccountIdAsync(Guid accountId);
    }
}
