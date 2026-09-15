using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Attributes;
using UserService.API.Models;
using UserService.BLL.Dtos.Account;
using UserService.BLL.Dtos.Auth;
using UserService.BLL.Dtos.Role;
using UserService.BLL.Interfaces;
using UserService.DAL.Authorization;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("")]
        [HasPermission(Permissions.AccountReadAll)]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAll();
            var response = new ResponseModel<IEnumerable<AccountGetDto>>(
                StatusCodes.Status200OK,
                "Accounts retrieved successfully.",
                accounts
            );
            return Ok(response);
        }
        [HttpGet("{accountId:guid}")]
        [HasPermission(Permissions.AccountRead)]
        public async Task<IActionResult> Get(Guid accountId)
        {
            var account = await _accountService.Get(accountId);
            if (account == null)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Account not found."
                ));
            }

            var response = new ResponseModel<AccountGetDto>(
                StatusCodes.Status200OK,
                "Account retrieved successfully.",
                account
            );
            return Ok(response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AccountPostDto accountDto)
        {
            try
            {
                var createdAccount = await _accountService.Create(accountDto);
                var response = new ResponseModel<AccountGetDto>(
                    StatusCodes.Status201Created,
                    "Account successfully registered.",
                    createdAccount
                );
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseModel<string>(
                    StatusCodes.Status400BadRequest,
                    ex.Message
                ));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _accountService.Login(loginDto.Email, loginDto.Password);
            if (result == null)
            {
                return Unauthorized(new ResponseModel<string>(
                    StatusCodes.Status401Unauthorized,
                    "Invalid email or password."
                ));
            }

            var response = new ResponseModel<LoginResponseDto>(
                StatusCodes.Status200OK,
                "Login successful.",
                result);

            return Ok(response);
        }

        [HttpPut("{accountId:guid}")]
        [HasPermission(Permissions.AccountUpdate)]
        public async Task<IActionResult> Update(Guid accountId, [FromBody] AccountPutDto accountDto)
        {
            try
            {
                var updatedAccount = await _accountService.Update(accountId, accountDto);
                var response = new ResponseModel<AccountGetDto>(
                    StatusCodes.Status200OK,
                    "Account successfully updated.",
                    updatedAccount
                );
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ));
            }
        }

        [HttpDelete("hard/{accountId:guid}")]
        [HasPermission(Permissions.AccountDelete)]
        public async Task<IActionResult> HardDelete(Guid accountId)
        {
            var result = await _accountService.HardDelete(accountId);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "Account not found."
                ));
            }

            var response = new ResponseModel<string>(
                StatusCodes.Status204NoContent,
                "Account successfully deleted."
            );
            return Ok(response);
        }

        [HttpDelete("soft/{accountId:guid}")]
        [HasPermission(Permissions.AccountDelete)]
        public async Task<IActionResult> SoftDelete(Guid accountId)
        {
            try
            {
                var result = await _accountService.SoftDelete(accountId);
                var response = new ResponseModel<string>(
                    StatusCodes.Status204NoContent,
                    "Account successfully soft deleted."
                );
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ));
            }
        }

        [HttpPost("{accountId:guid}/roles/{roleId:guid}")]
        [HasPermission(Permissions.AccountRoleAssign)]
        public async Task<IActionResult> AddRoleToAccount(Guid accountId, Guid roleId)
        {
            try
            {
                await _accountService.AddRoleToAccount(accountId, roleId);
                var response = new ResponseModel<string>(
                    StatusCodes.Status200OK,
                    "Role successfully added to account."
                );
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ));
            }
        }

        [HttpDelete("{accountId:guid}/roles/{roleId:guid}")]
        [HasPermission(Permissions.AccountRoleRemove)]
        public async Task<IActionResult> RemoveRoleFromAccount(Guid accountId, Guid roleId)
        {
            try
            {
                await _accountService.RemoveRoleFromAccount(accountId, roleId);
                var response = new ResponseModel<string>(
                    StatusCodes.Status200OK,
                    "Role successfully removed from account."
                );
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ));
            }
        }

        [HttpGet("{accountId:guid}/roles")]
        [HasPermission(Permissions.AccountRoleRead)]
        public async Task<IActionResult> GetRolesByAccountId(Guid accountId)
        {
            try
            {
                var roles = await _accountService.GetRolesByAccountIdAsync(accountId);
                var response = new ResponseModel<IEnumerable<RoleGetDto>>(
                    StatusCodes.Status200OK,
                    "Roles retrieved successfully.",
                    roles
                );
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _accountService.Logout(refreshToken);
            var response = new ResponseModel<string>(
                StatusCodes.Status200OK,
                "Logout successful."
            );
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var result = await _accountService.Refresh(refreshToken);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized(new ResponseModel<string>(
                    StatusCodes.Status401Unauthorized,
                    "Invalid refresh token."
                ));
            }

            var response = new ResponseModel<string>(
                StatusCodes.Status200OK,
                "Token refreshed successfully.",
                result
            );
            return Ok(response);
        }
    }
}
