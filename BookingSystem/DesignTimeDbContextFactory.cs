using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BookingSystem.DAL.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookingContext>
    {
        public BookingContext CreateDbContext(string[] args = null)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("BookingDatabase"));

                return new BookingContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании контекста базы данных: {ex.Message}");
                throw;
            }
        }
    }
}