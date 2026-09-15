using Microsoft.EntityFrameworkCore;
using UserService.DAL.DBContext;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class UserRepo : IUserRepo
    {
        private MyDbContext _context;

        public UserRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Create(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create user failed: " + ex.Message);
            }

            return user;
        }

        public async Task<bool> Delete(Guid userId)
        {
            try
            {
                await _context.Users.Where(a => a.Id == userId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete user fail: " + ex);
                return false;
            }

            return true;
        }

        public async Task<User?> Get(Guid userId)
        {
            return await _context.Users.Where(a => a.Id == userId).FirstOrDefaultAsync() ?? null;
        }

        public async Task<User?> Update(User user)
        {
            User? existedUser = await _context.Users
                .Where(a => a.Id == user.Id)
                .FirstOrDefaultAsync();
            if (existedUser == null) return null;

            existedUser.UserName = user.UserName;

            try
            {
                _context.Users.Update(existedUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Update user failed: " + ex.Message);
            }

            return existedUser;
        }
    }
}
