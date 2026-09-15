using Microsoft.AspNetCore.Authorization;
using UserService.BLL.Interfaces;

namespace UserService.BLL.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IRoleService _roleService;

        public PermissionHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var accountIdClaim = context.User.FindFirst("accountId");

            if (accountIdClaim == null)
                return;

            if (!Guid.TryParse(accountIdClaim.Value, out var accountId))
                return;

            var hasPermission = await _roleService.HasPermissionAsync(accountId, requirement.Permission);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
