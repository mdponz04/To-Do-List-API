using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IAccountRepo
    {
        public Task<Account?> Create(Account account);
        public Task<Account?> Update(Account account);
        public Task<bool> Delete(Guid accountId);
        public Task<Account?> GetById(Guid accountId);
        public Task<Account?> GetByEmail(string email);
        public Task<IEnumerable<Account>> GetAll();
    }
}
