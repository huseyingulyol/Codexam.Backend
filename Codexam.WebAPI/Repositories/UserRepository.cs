using Azure.Core;
using Codexam.WebAPI.Entities;
using Codexam.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Codexam.WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByCredentialsAsync(string email, string password)
        {

            var user = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            bool isPasswordMatch = HashingHelper.VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash);
             
            if (!isPasswordMatch) return null;
          
            return user;
        }

           public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
