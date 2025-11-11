using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BZDesktopApp
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use the same connection string as in MauiProgram
            string localConn = "Server=localhost;Database=bz_desktop;Trusted_Connection=True;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(localConn);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
