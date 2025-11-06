using System;
using System.Linq;
using BZDesktopApp.Api.Data;
using BZDesktopApp.Api.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace BZDesktopApp.Api.Seed
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Apply migrations if any
            context.Database.Migrate();

            // Create or get the main branch
            var branch = context.Branches.FirstOrDefault(b => b.Name == "Main Branch");
            if (branch == null)
            {
                branch = new Branch
                {
                    Name = "Main Branch",
                    Location = "Davao"
                };
                context.Branches.Add(branch);
                context.SaveChanges();
            }

            var random = new Random();

            // Helper function to generate unique 5-digit Employee ID
            long GenerateUniqueEmployeeId()
            {
                long id;
                do
                {
                    id = random.Next(10000, 99999);
                } while (context.Employees.Any(e => e.EmployeeId == id));
                return id;
            }

            // Seed Superadmin
            if (!context.Employees.Any(e => e.Username == "superadmin"))
            {
                var superadmin = new Employee
                {
                    EmployeeId = GenerateUniqueEmployeeId(),
                    BranchId = branch.BranchId,
                    FirstName = "Super",
                    LastName = "Admin",
                    Role = "superadmin",
                    PhoneNumber = "0923123112",
                    EmailAddress = "superadmin@account.com",
                    Username = "superadmin",
                    Password = BCrypt.Net.BCrypt.HashPassword("bzsuperadmin"),
                    ActiveStatus = true
                };

                context.Employees.Add(superadmin);
                context.SaveChanges();

                Console.WriteLine("Superadmin created (Username: superadmin, Password: bzsuperadmin)");
            }
            else
            {
                Console.WriteLine("Superadmin already exists.");
            }

            // Seed Admin
            if (!context.Employees.Any(e => e.Username == "admin"))
            {
                var admin = new Employee
                {
                    EmployeeId = GenerateUniqueEmployeeId(),
                    BranchId = branch.BranchId,
                    FirstName = "System",
                    LastName = "Admin",
                    Role = "admin",
                    PhoneNumber = "09123456789",
                    EmailAddress = "admin@account.com",
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("bzadmin"),
                    ActiveStatus = true
                };

                context.Employees.Add(admin);
                context.SaveChanges();

                Console.WriteLine("Admin created (Username: admin, Password: bzadmin)");
            }
            else
            {
                Console.WriteLine("Admin already exists.");
            }
        }
    }
}
