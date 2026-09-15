using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class RoleRepo : IRoleRepo
    {
        private readonly MyDbContext _context;

        public RoleRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> Create(Role role)
        {
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create role failed: " + ex.Message);
            }

            return role;
        }

        public async Task<bool> Delete(Guid roleId)
        {
            try
            {
                await _context.Roles.Where(r => r.Id == roleId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete role failed: " + ex);
                return false;
            }

            return true;
        }

        public async Task<Role?> Get(Guid roleId)
        {
            return await _context.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetByName(string name)
        {
            return await _context.Roles
                .Where(r => r.Name.ToLower().Trim() == name.ToLower().Trim())
                .FirstOrDefaultAsync();
        }

        public async Task<Role?> Update(Role role)
        {
            Role? existingRole = await _context.Roles
                .Where(r => r.Id == role.Id)
                .FirstOrDefaultAsync();
            if (existingRole == null) return null;

            existingRole.Name = role.Name;
            existingRole.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Roles.Update(existingRole);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Update role failed: " + ex.Message);
            }

            return existingRole;
        }
    }
}