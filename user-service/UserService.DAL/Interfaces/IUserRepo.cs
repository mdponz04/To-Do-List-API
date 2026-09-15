using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IUserRepo
    {
        public Task<User?> Create(User user);
        public Task<User?> Update(User user);
        public Task<bool> Delete(Guid userId);
        public Task<User?> Get(Guid userId);
    }
}
