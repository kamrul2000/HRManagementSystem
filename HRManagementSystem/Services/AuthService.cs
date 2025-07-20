using HRManagementSystem.Data;
using HRManagementSystem.Interface;
using HRManagementSystem.Models;
using HRManagementSystem.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(RegisterViewModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                throw new Exception("Email already exists");

            var subscription = new Subscription { IsActive = true };
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password, 
                Mobile = model.ContactNumber,
                RoleId = model.RoleId ?? 1,
                Status = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SubscriptionId = subscription.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<User?> LoginAsync(LoginViewModel model)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
        }
    }
}
