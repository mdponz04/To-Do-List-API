using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly MyDbContext _context;

        public RefreshTokenRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> Create(RefreshToken refreshToken)
        {
            try
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create refresh token failed: " + ex.Message);
            }

            return refreshToken;
        }

        public async Task<IEnumerable<RefreshToken>> GetRefreshTokenByAccountIdAsync(Guid accountId)
        {
            var tokens =  await _context.RefreshTokens
                .Where(rt => Guid.Equals(rt.AccountId, accountId) 
                && rt.IsActive)
                .ToListAsync();

            foreach (var token in tokens)
            {
                if (IsTokenExpired(token))
                {
                    token.IsActive = false;
                    _context.RefreshTokens.Update(token);
                }
            }
            await _context.SaveChangesAsync();

            return tokens;
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.Account)
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash 
                && rt.IsActive
                && rt.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<RefreshToken?> Revoke(Guid tokenId)
        {
            var token = await _context.RefreshTokens.FindAsync(tokenId);
            if (token == null) return null;

            if(IsTokenExpired(token))
            {
                throw new Exception("Token has already expired.");
            }

            token.IsActive = false;
            token.ExpiresAt = DateTime.UtcNow;

            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
            return token;
        }

        private bool IsTokenExpired(RefreshToken token)
        {
            if (token.ExpiresAt < DateTime.UtcNow) return true;
            return false;
        }
    }
}
