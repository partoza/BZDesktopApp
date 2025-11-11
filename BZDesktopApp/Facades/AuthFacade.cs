using BZDesktopApp.Shared.Dtos;
using BZDesktopApp.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace BZDesktopApp.Facades
{
    public class AuthFacade
    {
        private readonly IAuthService _authService;

        public AuthFacade(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Currently logged-in user (in-memory)
        /// </summary>
        public LoginResponse? CurrentUser { get; set; }

        /// <summary>
        /// Event triggered on login/logout
        /// </summary>
        public event Action? OnAuthenticationChanged;

        /// <summary>
        /// Login user and set CurrentUser
        /// </summary>
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _authService.LoginAsync(request);

            if (user != null)
            {
                CurrentUser = user;
                OnAuthenticationChanged?.Invoke();
            }

            return user;
        }

        /// <summary>
        /// Logout user and clear CurrentUser
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
            OnAuthenticationChanged?.Invoke();
        }

        public bool IsLoggedIn => CurrentUser != null;
    }
}
