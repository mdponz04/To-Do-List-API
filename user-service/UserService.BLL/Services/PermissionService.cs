using AutoMapper;
using UserService.BLL.Dtos.Permission;
using UserService.BLL.Interfaces;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepo _permissionRepo;
        private readonly IRolePermissionRepo _rolePermissionRepo;
        private readonly IMapper Mapper;

        public PermissionService(IPermissionRepo permissionRepo, IRolePermissionRepo rolePermissionRepo, IMapper mapper)
        {
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
            Mapper = mapper;
        }

        public async Task<PermissionGetDto?> Create(PermissionPostDto permissionDto)
        {
            var permission = Mapper.Map<Permission>(permissionDto);
            var createdPermission = await _permissionRepo.Create(permission);
            return Mapper.Map<PermissionGetDto>(createdPermission);
        }

        public async Task<bool> Delete(Guid permissionId)
        {
            return await _permissionRepo.Delete(permissionId);
        }

        public async Task<PermissionGetDto?> Get(Guid permissionId)
        {
            var permission = await _permissionRepo.Get(permissionId);
            return Mapper.Map<PermissionGetDto>(permission);
        }

        public async Task<IEnumerable<PermissionGetDto>> GetAll()
        {
            var permissions = await _permissionRepo.GetAll();
            return Mapper.Map<IEnumerable<PermissionGetDto>>(permissions);
        }

        public async Task<IEnumerable<PermissionGetDto>> GetByRoleId(Guid roleId)
        {
            var permissions = await _rolePermissionRepo.GetPermissionsByRoleIdAsync(roleId);
            return Mapper.Map<IEnumerable<PermissionGetDto>>(permissions);
        }

        public async Task<PermissionGetDto?> Update(Guid permissionId, PermissionPutDto permissionDto)
        {
            var permission = Mapper.Map<Permission>(permissionDto);
            permission.Id = permissionId;
            var updatedPermission = await _permissionRepo.Update(permission);
            return Mapper.Map<PermissionGetDto>(updatedPermission);
        }

        public async Task<bool> CheckPermissionAsync(Guid roleId, string permissionName)
        {
            var permissions = await _rolePermissionRepo.GetPermissionsByRoleIdAsync(roleId);
            return permissions.Any(p => p.Name == permissionName);
        }
    }
}