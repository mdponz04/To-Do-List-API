using AutoMapper;
using UserService.BLL.Dtos.Permission;
using UserService.BLL.Dtos.Role;
using UserService.BLL.Interfaces;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _roleRepo;
        private readonly IRolePermissionRepo _rolePermissionRepo;
        private readonly IAccountRoleRepo _accountRoleRepo;
        private readonly IMapper Mapper;
        private readonly IPermissionRepo _permissionRepo;

        public RoleService(IRoleRepo roleRepo, IRolePermissionRepo rolePermissionRepo, IAccountRoleRepo accountRoleRepo, IMapper mapper, IPermissionRepo permissionRepo)
        {
            _roleRepo = roleRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _accountRoleRepo = accountRoleRepo;
            Mapper = mapper;
            _permissionRepo = permissionRepo;
        }

        public async Task<RoleGetDto?> Create(RolePostDto roleDto)
        {
            var role = Mapper.Map<Role>(roleDto);
            var createdRole = await _roleRepo.Create(role);
            return Mapper.Map<RoleGetDto>(createdRole);
        }

        public async Task<bool> Delete(Guid roleId)
        {
            return await _roleRepo.Delete(roleId);
        }

        public async Task<RoleGetDto?> Get(Guid roleId)
        {
            var role = await _roleRepo.Get(roleId);
            return Mapper.Map<RoleGetDto>(role);
        }

        public async Task<IEnumerable<RoleGetDto>> GetAll()
        {
            var roles = await _roleRepo.GetAll();
            return Mapper.Map<IEnumerable<RoleGetDto>>(roles);
        }

        public async Task<IEnumerable<RoleGetDto>> GetByAccountId(Guid accountId)
        {
            var roles = await _accountRoleRepo.GetRolesByAccountIdAsync(accountId);
            return Mapper.Map<IEnumerable<RoleGetDto>>(roles);
        }

        public async Task<RoleGetDto?> Update(Guid roleId, RolePutDto roleDto)
        {
            var role = Mapper.Map<Role>(roleDto);
            role.Id = roleId;
            var updatedRole = await _roleRepo.Update(role);
            return Mapper.Map<RoleGetDto>(updatedRole);
        }

        public async Task AddPermissionToRole(Guid roleId, Guid permissionId)
        {
            await _rolePermissionRepo.AddRolePermissionAsync(roleId, permissionId);
        }

        public async Task RemovePermissionFromRole(Guid roleId, Guid permissionId)
        {
            await _rolePermissionRepo.RemoveRolePermissionAsync(roleId, permissionId);
        }

        public async Task<IEnumerable<PermissionGetDto>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var permissions = await _rolePermissionRepo.GetPermissionsByRoleIdAsync(roleId);
            return Mapper.Map<IEnumerable<PermissionGetDto>>(permissions);
        }

        public async Task<bool> HasPermissionAsync(Guid accountId, string permissionName)
        {
            var roles = await _accountRoleRepo.GetRolesByAccountIdAsync(accountId);
            foreach(var role in roles)
            {
                var permissions = await _rolePermissionRepo.GetPermissionsByRoleIdAsync(role.Id);
                if (permissions.Any(p => p.Name == permissionName))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<PermissionGetDto>> GetMissingPermissionByRoleIdAsync(Guid roleId)
        {
            var allPermissions = await _permissionRepo.GetAll();
            var existingPermission = await _rolePermissionRepo.GetPermissionsByRoleIdAsync(roleId);
            var missingPermissions = allPermissions
                .Where(p => !existingPermission.Any(ep => ep.Id == p.Id))
                .ToList();

            return Mapper.Map<IEnumerable<PermissionGetDto>>(missingPermissions);
        }
    }
}