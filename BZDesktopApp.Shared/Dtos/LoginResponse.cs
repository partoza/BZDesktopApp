namespace BZDesktopApp.Shared.Dtos
{
    public class LoginResponse
    {
        public long EmployeeId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        // new fields
        public string? AvatarUrl { get; set; }
        public string? Email { get; set; }
    }
}
