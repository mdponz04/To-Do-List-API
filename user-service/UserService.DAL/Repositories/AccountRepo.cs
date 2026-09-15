using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private MyDbContext _context;

        public AccountRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> Create(Account account)
        {
            if(IsEmailExist(account.Email)) throw new Exception("Email already exists.");
            
            try
            {
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create account failed: " + ex.Message);
            }
            
            return account;
        }

        public async Task<bool> Delete(Guid accountId)
        {
            try
            {
                await _context.Accounts.Where(a => a.Id == accountId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete account fail: " + ex);
                return false;
            }

            return true;
        }

        public async Task<Account?> GetById(Guid accountId)
        {
            return await _context.Accounts.Where(a => a.Id == accountId).FirstOrDefaultAsync() ?? null;
        }

        public async Task<Account?> GetByEmail(string email)
        {
            return await _context.Accounts.Where(a => a.Email == email).FirstOrDefaultAsync() ?? null;
        }

        public async Task<Account?> Update(Account account)
        {
            Account? existedAccount = await _context.Accounts
                .Where(a => a.Id == account.Id)
                .FirstOrDefaultAsync();
            if (existedAccount == null) return null;

            existedAccount.Email = account.Email;
            existedAccount.PasswordHash = account.PasswordHash;
            existedAccount.IsActive = account.IsActive;
            existedAccount.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Accounts.Update(existedAccount);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Update account failed: " + ex.Message);
            }
            
            return existedAccount;
        }
        private bool IsEmailExist(string email)
        {
            if(_context.Accounts.Any(a => a.Email == email))
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _context.Accounts
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
