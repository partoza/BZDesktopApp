namespace BZDesktopApp.Services
{
    public class AuthenticationService
    {
private bool _isLoggedIn = false;

public bool IsLoggedIn => _isLoggedIn;

        public event Action? OnAuthenticationChanged;

  public void Login()
        {
         _isLoggedIn = true;
    OnAuthenticationChanged?.Invoke();
        }

        public void Logout()
        {
         _isLoggedIn = false;
            OnAuthenticationChanged?.Invoke();
        }
    }
}
