using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class RolePermissionRepo : IRolePermissionRepo
    {
        private readonly MyDbContext _context;

        public RolePermissionRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddRolePermissionAsync(Guid roleId, Guid permissionId)
        {
            var rolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId };
            try
            {
                await _context.RolePermissions.AddAsync(rolePermission);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to add role permission: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task RemoveRolePermissionAsync(Guid roleId, Guid permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.PermissionId == permissionId)
                .FirstOrDefaultAsync();

            if (rolePermission != null)
            {
                try
                {
                    _context.RolePermissions.Remove(rolePermission);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to remove role permission: {ex.Message}", ex);
                }
            }
        }
    }
}