using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Attributes;
using UserService.API.Models;
using UserService.BLL.Dtos.Permission;
using UserService.BLL.Dtos.Role;
using UserService.BLL.Interfaces;
using UserService.DAL.Authorization;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [HasPermission(Permissions.RoleReadAll)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            var response = new ResponseModel<IEnumerable<RoleGetDto>>(
                StatusCodes.Status200OK,
                "Roles retrieved successfully.",
                roles
            );
            return Ok(response);
        }

        [HttpGet("{roleId:guid}")]
        [HasPermission(Permissions.RoleRead)]
        public async Task<IActionResult> Get(Guid roleId)
        {
            var role = await _roleService.Get(roleId);
            if (role == null)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Role not found."
                ));
            }

            var response = new ResponseModel<RoleGetDto>(
                StatusCodes.Status200OK,
                "Role retrieved successfully.",
                role
            );
            return Ok(response);
        }

        [HttpPost]
        [HasPermission(Permissions.RoleCreate)]
        public async Task<IActionResult> Create([FromBody] RolePostDto roleDto)
        {
            var createdRole = await _roleService.Create(roleDto);
            var response = new ResponseModel<RoleGetDto>(
                StatusCodes.Status201Created,
                "Role successfully created.",
                createdRole
            );
            return Ok(response);
        }

        [HttpPut("{roleId:guid}")]
        [HasPermission(Permissions.RoleUpdate)]
        public async Task<IActionResult> Update(Guid roleId, [FromBody] RolePutDto roleDto)
        {
            var updatedRole = await _roleService.Update(roleId, roleDto);
            if (updatedRole == null)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Role not found."
                ));
            }

            var response = new ResponseModel<RoleGetDto>(
                StatusCodes.Status200OK,
                "Role successfully updated.",
                updatedRole
            );
            return Ok(response);
        }

        [HttpDelete("{roleId:guid}")]
        [HasPermission(Permissions.RoleDelete)]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var result = await _roleService.Delete(roleId);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Role not found."
                ));
            }

            var response = new ResponseModel<string>(
                StatusCodes.Status204NoContent,
                "Role successfully deleted."
            );
            return Ok(response);
        }

        [HttpPost("{roleId:guid}/permissions/{permissionId:guid}")]
        [HasPermission(Permissions.RolePermissionAssign)]
        public async Task<IActionResult> AddPermissionToRole(Guid roleId, Guid permissionId)
        {
            await _roleService.AddPermissionToRole(roleId, permissionId);
            var response = new ResponseModel<string>(
                StatusCodes.Status200OK,
                "Permission successfully added to role."
            );
            return Ok(response);
        }

        [HttpDelete("{roleId:guid}/permissions/{permissionId:guid}")]
        [HasPermission(Permissions.RolePermissionRemove)]
        public async Task<IActionResult> RemovePermissionFromRole(Guid roleId, Guid permissionId)
        {
            await _roleService.RemovePermissionFromRole(roleId, permissionId);
            var response = new ResponseModel<string>(
                StatusCodes.Status200OK,
                "Permission successfully removed from role."
            );
            return Ok(response);
        }

        [HttpGet("{roleId:guid}/permissions")]
        //[HasPermission(Permissions.RolePermissionRead)]
        public async Task<IActionResult> GetPermissionsByRoleId(Guid roleId)
        {
            var permissions = await _roleService.GetPermissionsByRoleIdAsync(roleId);
            var response = new ResponseModel<IEnumerable<PermissionGetDto>>(
                StatusCodes.Status200OK,
                "Permissions retrieved successfully.",
                permissions
            );
            return Ok(response);
        }

        [HttpGet("{roleId:guid}/permissions/missing")]
        [HasPermission(Permissions.RolePermissionRead)]
        public async Task<IActionResult> GetMissingPermissionByRoleId(Guid roleId)
        {
            var permissions = await _roleService.GetMissingPermissionByRoleIdAsync(roleId);
            var response = new ResponseModel<IEnumerable<PermissionGetDto>>(
                StatusCodes.Status200OK,
                "Missing permissions retrieved successfully.",
                permissions
            );
            return Ok(response);
        }
    }
}