using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IRoleRepo
    {
        public Task<Role?> Create(Role role);
        public Task<Role?> Update(Role role);
        public Task<bool> Delete(Guid roleId);
        public Task<Role?> Get(Guid roleId);
        public Task<Role?> GetByName(string name);
        public Task<IEnumerable<Role>> GetAll();
    }
}
