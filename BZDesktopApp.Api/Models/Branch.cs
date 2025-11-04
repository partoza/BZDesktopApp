namespace BZDesktopApp.Api.Models
{
    public class Branch
    {
        public long BranchId { get; set; }
        public required string Name { get; set; }
        public string? Location { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
