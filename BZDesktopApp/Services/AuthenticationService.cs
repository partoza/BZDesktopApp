using BZDesktopApp.Facades;
using BZDesktopApp.Shared.Dtos;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BZDesktopApp.Services
{
    public class AuthenticationService
    {
        private readonly IJSRuntime _js;
        private readonly AuthFacade _authFacade;

        public bool IsLoggedIn => _authFacade.CurrentUser != null;
        public event Action? OnAuthenticationChanged;

        public AuthenticationService(IJSRuntime js, AuthFacade authFacade)
        {
            _js = js;
            _authFacade = authFacade;
        }

        /// <summary>
        /// Initialize auth state from localStorage
        /// </summary>
        public async Task InitializeAsync()
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                _authFacade.CurrentUser = user;
            }
            OnAuthenticationChanged?.Invoke();
        }

        /// <summary>
        /// Read current user from localStorage
        /// </summary>
        public async Task<LoginResponse?> GetUserAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "bz_user");
                if (string.IsNullOrEmpty(json))
                    return null;

                return JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Login and store user
        /// </summary>
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            var user = await _authFacade.LoginAsync(new LoginRequest
            {
                Username = username,
                Password = password
            });

            if (user != null)
            {
                await _js.InvokeVoidAsync("localStorage.setItem", "bz_user", JsonSerializer.Serialize(user));
                OnAuthenticationChanged?.Invoke();
            }

            return user;
        }

        /// <summary>
        /// Logout
        /// </summary>
        public async Task LogoutAsync()
        {
            _authFacade.CurrentUser = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", "bz_user");
            OnAuthenticationChanged?.Invoke();
        }
    }
}
