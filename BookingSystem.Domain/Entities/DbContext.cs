using Microsoft.EntityFrameworkCore;
using BookingSystem.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookingSystem.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        // Определение DbSet для каждой сущности
        public DbSet<Office> Offices { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка моделей, если необходимо
            // Например, можно настроить ключи, индексы и отношения между сущностями
            base.OnModelCreating(modelBuilder);
        }
    }
}