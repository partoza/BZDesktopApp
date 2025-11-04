namespace BZDesktopApp.Api.Models
{
    public class Employee
    {
        public long EmployeeId { get; set; }
        public long BranchId { get; set; }

        // Initialize non-nullable reference types to a safe default
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;    
        public string Role { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public bool ActiveStatus { get; set; } = true;

        // Navigation property is nullable because EF will populate it; avoids CS8618.
        public Branch? Branch { get; set; }
    }
}
