using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.DAL.Authorization;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;

namespace UserService.DAL.Initializers
{
    public static class PermissionInitializer
    {
        public static async Task InitializeAsync(MyDbContext context)
        {
            var existingPermissions = await context.Permissions
                .Select(p => p.Name)
                .ToArrayAsync();
            
            var permissionFields = typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.IsLiteral
                    &&!field.IsInitOnly 
                    && field.FieldType == typeof(string));

            foreach (var field in permissionFields)
            {
                var name = (string) field.GetRawConstantValue()!;

                if (!existingPermissions.Contains(name))
                {
                    context.Permissions.Add(new Permission
                    {
                        Name = name
                    });
                }
            }


            await context.SaveChangesAsync();
        }
    }
}
