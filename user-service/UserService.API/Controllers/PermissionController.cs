using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Attributes;
using UserService.API.Models;
using UserService.BLL.Dtos.Permission;
using UserService.BLL.Interfaces;
using UserService.DAL.Authorization;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [HasPermission(Permissions.PermissionReadAll)]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _permissionService.GetAll();
            var response = new ResponseModel<IEnumerable<PermissionGetDto>>(
                StatusCodes.Status200OK,
                "Permissions retrieved successfully.",
                permissions
            );
            return Ok(response);
        }

        [HttpGet("{permissionId:guid}")]
        [HasPermission(Permissions.PermissionRead)]
        public async Task<IActionResult> Get(Guid permissionId)
        {
            var permission = await _permissionService.Get(permissionId);
            if (permission == null)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Permission not found."
                ));
            }

            var response = new ResponseModel<PermissionGetDto>(
                StatusCodes.Status200OK,
                "Permission retrieved successfully.",
                permission
            );
            return Ok(response);
        }

        [HttpPost]
        [HasPermission(Permissions.PermissionCreate)]
        public async Task<IActionResult> Create([FromBody] PermissionPostDto permissionDto)
        {
            var createdPermission = await _permissionService.Create(permissionDto);
            var response = new ResponseModel<PermissionGetDto>(
                StatusCodes.Status201Created,
                "Permission successfully created.",
                createdPermission
            );
            return Ok(response);
        }

        [HttpPut("{permissionId:guid}")]
        [HasPermission(Permissions.PermissionUpdate)]
        public async Task<IActionResult> Update(Guid permissionId, [FromBody] PermissionPutDto permissionDto)
        {
            var updatedPermission = await _permissionService.Update(permissionId, permissionDto);
            if (updatedPermission == null)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Permission not found."
                ));
            }

            var response = new ResponseModel<PermissionGetDto>(
                StatusCodes.Status200OK,
                "Permission successfully updated.",
                updatedPermission
            );
            return Ok(response);
        }

        [HttpDelete("{permissionId:guid}")]
        [HasPermission(Permissions.PermissionDelete)]
        public async Task<IActionResult> Delete(Guid permissionId)
        {
            var result = await _permissionService.Delete(permissionId);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Permission not found."
                ));
            }

            var response = new ResponseModel<string>(
                StatusCodes.Status204NoContent,
                "Permission successfully deleted."
            );
            return Ok(response);
        }

        [HttpGet("role/{roleId:guid}")]
        [HasPermission(Permissions.RolePermissionRead)]
        public async Task<IActionResult> GetByRoleId(Guid roleId)
        {
            var permissions = await _permissionService.GetByRoleId(roleId);
            var response = new ResponseModel<IEnumerable<PermissionGetDto>>(
                StatusCodes.Status200OK,
                "Permissions retrieved successfully for the role.",
                permissions
            );
            return Ok(response);
        }

        //[HttpGet("check/{roleId:guid}")]
        //public async Task<IActionResult> CheckPermission(Guid roleId, [FromQuery] string permissionName)
        //{
        //    var hasPermission = await _permissionService.CheckPermissionAsync(roleId, permissionName);
        //    var response = new ResponseModel<bool>(
        //        StatusCodes.Status200OK,
        //        "Permission check completed.",
        //        hasPermission
        //    );
        //    return Ok(response);
        //}
    }
}