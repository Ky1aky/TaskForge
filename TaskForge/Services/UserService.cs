using Microsoft.EntityFrameworkCore;
using TaskForge.Data;
using TaskForge.Models;

namespace TaskForge.Services
{
    public class UserService
    {
        private readonly TaskForgeDbContext _context;
        private readonly PasswordService _passwordService;

        public UserService(TaskForgeDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // Check if email already exists
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        // Find user by email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Register new user
        public async Task RegisterUserAsync(User user, string password)
        {
            user.PasswordHash = _passwordService.HashPassword(user, password);
            user.DateCreated = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Validate login
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
                return null;

            bool valid = _passwordService.VerifyPassword(
                user,
                user.PasswordHash,
                password);

            return valid ? user : null;
        }
    }
}