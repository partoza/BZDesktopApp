using BZDesktopApp.Shared.Dtos;
using System.Threading.Tasks;

namespace BZDesktopApp.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
