using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IRefreshTokenRepo
    {
        public Task<IEnumerable<RefreshToken>> GetRefreshTokenByAccountIdAsync(Guid accountId);
        public Task<RefreshToken?> GetRefreshTokenByTokenHashAsync(string tokenHash);
        public Task<RefreshToken> Create(RefreshToken refreshToken);
        public Task<RefreshToken?> Revoke(Guid tokenId);
    }
}
