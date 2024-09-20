using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookingSystem.DAL.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookingContext>
    {
        public BookingContext CreateDbContext(string[] args = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BookingDatabase"));

            return new BookingContext(optionsBuilder.Options);
        }
    }
}