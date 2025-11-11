using BCrypt.Net;
using BZDesktopApp;
using BZDesktopApp.Shared.Dtos;
using BZDesktopApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BZDesktopApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Authenticate user locally using AppDbContext
        /// </summary>
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return null;

            return new LoginResponse
            {
                Username = user.Username,
                Role = user.Role,
                FullName = $"{user.FirstName} {user.LastName}",
                AvatarUrl = user.Avatar,
                Email = user.EmailAddress
            };
        }
    }
}
