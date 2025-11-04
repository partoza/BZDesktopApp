using Microsoft.EntityFrameworkCore;
using BZDesktopApp.Api.Models;   // so the DbContext can see your model classes

namespace BZDesktopApp.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Branch> Branches { get; set; }
    }
}
