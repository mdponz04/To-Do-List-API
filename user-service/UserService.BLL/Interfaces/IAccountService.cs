using UserService.BLL.Dtos.Account;
using UserService.BLL.Dtos.Auth;
using UserService.BLL.Dtos.Role;

namespace UserService.BLL.Interfaces
{
    public interface IAccountService
    {
        public Task<AccountGetDto> Create(AccountPostDto accountDto);
        public Task<LoginResponseDto?> Login(string email, string password);
        public Task Logout(string refreshToken);
        public Task<string> Refresh(string refreshToken);
        public Task<AccountGetDto> Update(Guid accountId, AccountPutDto accountDto);
        public Task<bool> HardDelete(Guid accountId);
        public Task<bool> SoftDelete(Guid accountId);
        public Task<AccountGetDto?> Get(Guid accountId);
        public Task<IEnumerable<AccountGetDto>> GetAll();
        public Task AddRoleToAccount(Guid accountId, Guid roleId);
        public Task RemoveRoleFromAccount(Guid accountId, Guid roleId);
        public Task<IEnumerable<RoleGetDto>> GetRolesByAccountIdAsync(Guid accountId);
    }
}
