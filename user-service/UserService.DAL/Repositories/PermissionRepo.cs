using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class PermissionRepo : IPermissionRepo
    {
        private readonly MyDbContext _context;

        public PermissionRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Permission?> Create(Permission permission)
        {
            try
            {
                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create permission failed: " + ex.Message);
            }

            return permission;
        }

        public async Task<bool> Delete(Guid permissionId)
        {
            var permission = await _context.Permissions
                .Where(p => p.Id == permissionId)
                .FirstOrDefaultAsync();

            if(permission == null) return false;

            try
            {
                await _context.Permissions
                    .Where(p => p.Id == permissionId)
                    .ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete permission failed: " + ex);
                return false;
            }

            return true;
        }

        public async Task<Permission?> Get(Guid permissionId)
        {
            return await _context.Permissions.Where(p => p.Id == permissionId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Permission>> GetAll()
        {
            return await _context.Permissions
                .OrderByDescending(p => p.Name)
                .ToListAsync();
        }

        public async Task<Permission?> Update(Permission permission)
        {
            Permission? existingPermission = await _context.Permissions
                .Where(p => p.Id == permission.Id)
                .FirstOrDefaultAsync();
            if (existingPermission == null) return null;

            existingPermission.Name = permission.Name;
            existingPermission.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Permissions.Update(existingPermission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Update permission failed: " + ex.Message);
            }

            return existingPermission;
        }
    }
}