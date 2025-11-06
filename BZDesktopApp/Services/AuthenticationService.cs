using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using BZDesktopApp.Shared.Dtos;

namespace BZDesktopApp.Services;

public class AuthenticationService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    public bool IsLoggedIn { get; private set; }
    public event Action? OnAuthenticationChanged;

    public AuthenticationService(HttpClient http, IJSRuntime js)
    {
        _http = http;

        // ⚠️ Set the API base URL here
        if (_http.BaseAddress == null)
            _http.BaseAddress = new Uri("https://localhost:7028/");

        _js = js;
    }

    public async Task<LoginResponse?> LoginAsync(string username, string password)
    {
        try
        {
            Console.WriteLine($"[AuthService] Attempting login for username: {username}");

            var payload = new { username, password };
            var res = await _http.PostAsJsonAsync("api/auth/login", payload);

            Console.WriteLine($"[AuthService] HTTP Status Code: {res.StatusCode}");

            if (!res.IsSuccessStatusCode)
            {
                var errorContent = await res.Content.ReadAsStringAsync();
                Console.WriteLine($"[AuthService] Login failed. Response: {errorContent}");
                return null;
            }

            var loginResponse = await res.Content.ReadFromJsonAsync<LoginResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loginResponse != null)
            {
                Console.WriteLine($"[AuthService] Login succeeded. Token length: {loginResponse.Token?.Length ?? 0}");
            }
            else
            {
                Console.WriteLine("[AuthService] Login succeeded but response deserialization failed.");
            }

            return loginResponse;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[AuthService] HTTP request failed: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"[AuthService] Inner exception: {ex.InnerException.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] Unexpected error: {ex.GetType().Name}: {ex.Message}");
            return null;
        }
    }

    public async Task<LoginResponse?> GetUserAsync()
    {
        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", "bz_user");
            if (string.IsNullOrEmpty(json)) return null;
            return JsonSerializer.Deserialize<LoginResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] GetUserAsync failed: {ex.Message}");
            return null;
        }
    }

    public async Task StoreAuthDataAsync(LoginResponse resp)
    {
        if (resp == null) return;

        await _js.InvokeVoidAsync("localStorage.setItem", "bz_token", resp.Token);
        await _js.InvokeVoidAsync("localStorage.setItem", "bz_user", JsonSerializer.Serialize(resp));

        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", resp.Token);

        IsLoggedIn = true;
        OnAuthenticationChanged?.Invoke();

        Console.WriteLine("[AuthService] Auth data stored successfully.");
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "bz_token");
            await _js.InvokeVoidAsync("localStorage.removeItem", "bz_user");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] Error removing localStorage items: {ex.Message}");
        }

        _http.DefaultRequestHeaders.Authorization = null;
        IsLoggedIn = false;
        OnAuthenticationChanged?.Invoke();

        Console.WriteLine("[AuthService] Logged out successfully.");
    }

    public async Task InitializeAsync()
    {
        try
        {
            var token = await _js.InvokeAsync<string?>("localStorage.getItem", "bz_token");
            IsLoggedIn = !string.IsNullOrEmpty(token);

            if (IsLoggedIn)
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("[AuthService] Existing token loaded from localStorage.");
            }

            OnAuthenticationChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] InitializeAsync failed: {ex.GetType().Name}: {ex.Message}");
            IsLoggedIn = false;
            OnAuthenticationChanged?.Invoke();
        }
    }

    public async Task<string?> GetTokenAsync()
        => await _js.InvokeAsync<string?>("localStorage.getItem", "bz_token");
}
