using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.BLL.Dtos.Account;
using UserService.BLL.Dtos.Auth;
using UserService.BLL.Dtos.Role;
using UserService.BLL.Dtos.User;
using UserService.BLL.Interfaces;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepo AccountRepo { get; set; }
        private IRefreshTokenRepo RefreshTokenRepo { get; set; }
        private IMapper Mapper { get; set; }
        private readonly string _JWTSECRETKEY;
        private readonly string _ISSUER;
        private readonly string _AUDIENCE;
        private IUserService UserService { get; set; }
        private IAccountRoleRepo AccountRoleRepo { get; set; }
        private IRoleRepo RoleRepo { get; set; }

        public AccountService(IAccountRepo accountRepo, IMapper mapper, IConfiguration configuration, IUserService userRepo, IAccountRoleRepo accountRoleRepo, IRefreshTokenRepo refreshTokenRepo, IRoleRepo roleRepo)
        {
            AccountRepo = accountRepo;
            Mapper = mapper;
            _JWTSECRETKEY = configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is not configured.");
            _ISSUER = configuration["JwtSettings:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            _AUDIENCE = configuration["JwtSettings:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
            UserService = userRepo;
            AccountRoleRepo = accountRoleRepo;
            RefreshTokenRepo = refreshTokenRepo;
            RoleRepo = roleRepo;
        }

        public async Task<AccountGetDto?> Get(Guid accountId)
        {
            return Mapper.Map<AccountGetDto>(await AccountRepo.GetById(accountId));
        }

        public async Task<bool> HardDelete(Guid accountId)
        {
            return await AccountRepo.Delete(accountId);
        }

        public async Task<LoginResponseDto?> Login(string email, string password)
        {
            var account = await AccountRepo.GetByEmail(email);
            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.PasswordHash)) return null;

            var accessToken = await CreateAccessToken(Mapper.Map<AccountGetDto>(account));

            //create refresh token
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshTokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

            await RefreshTokenRepo.Create(new RefreshToken
            {
                AccountId = account.Id,
                TokenHash = refreshTokenHash,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            return new LoginResponseDto { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken, 
                Account = Mapper.Map<AccountGetDto>(account)};
        }

        public async Task<AccountGetDto> Create(AccountPostDto accountDto)
        {
            var existingAccount = await AccountRepo.GetByEmail(accountDto.Email);
            if (existingAccount != null)
            {
                throw new InvalidOperationException("An account with this email already exists.");
            }
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(accountDto.Password);

            var account = Mapper.Map<Account>(accountDto);
            account.PasswordHash = hashedPassword;
            account.IsActive = true;

            var createdAccount = await AccountRepo.Create(account);

            if(createdAccount == null) throw new InvalidOperationException("Failed to create account.");

            await UserService.Create(new UserPostDto { UserName = createdAccount.Email, AccountId = createdAccount.Id });
            await AccountRoleRepo.AddAccountRoleAsync(createdAccount.Id, (await RoleRepo.GetByName("Auditor"))?.Id ?? throw new InvalidOperationException("Default role 'User' not found."));

            return Mapper.Map<AccountGetDto>(createdAccount);
        }

        public async Task<bool> SoftDelete(Guid accountId)
        {
            var account = await AccountRepo.GetById(accountId) ?? throw new KeyNotFoundException("Account not found.");
            account.IsDeleted = true;
            await AccountRepo.Update(account);
            return true;
        }

        public async Task<AccountGetDto> Update(Guid accountId, AccountPutDto accountDto)
        {
            var account = await AccountRepo.GetById(accountId) ?? throw new KeyNotFoundException("Account not found.");
            return Mapper.Map<AccountGetDto>(await AccountRepo.Update(Mapper.Map<Account>(accountDto)));
        }

        public async Task AddRoleToAccount(Guid accountId, Guid roleId)
        {
            await AccountRoleRepo.AddAccountRoleAsync(accountId, roleId);
        }

        public async Task RemoveRoleFromAccount(Guid accountId, Guid roleId)
        {
            await AccountRoleRepo.RemoveAccountRoleAsync(accountId, roleId);
        }

        public async Task<IEnumerable<RoleGetDto>> GetRolesByAccountIdAsync(Guid accountId)
        {
            return Mapper.Map<IEnumerable<RoleGetDto>>(await AccountRoleRepo.GetRolesByAccountIdAsync(accountId));
        }

        public async Task Logout(string refreshToken)
        {
            var refreshTokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
            var token = await RefreshTokenRepo.GetRefreshTokenByTokenHashAsync(refreshTokenHash);

            if(token == null)
            {
                throw new KeyNotFoundException("Refresh token not found.");
            }

            await RefreshTokenRepo.Revoke(token.Id);
        }

        public async Task<string> Refresh(string refreshToken)
        {
            var refreshTokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
            var token = await RefreshTokenRepo.GetRefreshTokenByTokenHashAsync(refreshTokenHash);
            
            if (token == null) return string.Empty;

            return await CreateAccessToken(Mapper.Map<AccountGetDto>(token.Account));
        }

        private async Task<string> CreateAccessToken(AccountGetDto account)
        {
            var roles = await AccountRoleRepo.GetRolesByAccountIdAsync(account.Id);
            string rolesString = string.Join(",", roles.Select(r => r.Name));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_JWTSECRETKEY);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("accountId", account.Id.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, rolesString),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer = _ISSUER,
                Audience = _AUDIENCE,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<AccountGetDto>> GetAll()
        {
            var accounts = Mapper.Map<IEnumerable<AccountGetDto>>(await AccountRepo.GetAll());
            foreach(var account in accounts)
            {
                var roles = Mapper.Map<IEnumerable<RoleGetDto>>(await AccountRoleRepo.GetRolesByAccountIdAsync(account.Id));
                string roleNames = string.Join(", ", roles.Select(r => r.Name));
                account.Roles = roleNames;
            }

            return accounts;
        }
    }
}
