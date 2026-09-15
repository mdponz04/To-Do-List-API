using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class AccountRoleRepo : IAccountRoleRepo
    {
        private readonly MyDbContext _context;

        public AccountRoleRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAccountRoleAsync(Guid accountId, Guid roleId)
        {
            var accountRole = new AccountRole { AccountId = accountId, RoleId = roleId };
            await _context.AccountRoles.AddAsync(accountRole);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesByAccountIdAsync(Guid accountId)
        {
            return await _context.AccountRoles
                .Where(ar => ar.AccountId == accountId)
                .Select(ar => ar.Role)
                .ToListAsync();
        }

        public async Task RemoveAccountRoleAsync(Guid accountId, Guid roleId)
        {
            var accountRole = await _context.AccountRoles
                .Where(ar => ar.AccountId == accountId && ar.RoleId == roleId)
                .FirstOrDefaultAsync();

            if (accountRole != null)
            {
                _context.AccountRoles.Remove(accountRole);
                await _context.SaveChangesAsync();
            }
        }
    }
}