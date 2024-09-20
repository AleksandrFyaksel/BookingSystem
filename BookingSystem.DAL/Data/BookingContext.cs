using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.DAL.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }

        // Определение DbSet для каждой сущности
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<BookingStatus> BookingStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений между сущностями

            modelBuilder.Entity<Office>()
                .HasMany(o => o.Floors)
                .WithOne(f => f.Office)
                .HasForeignKey(f => f.OfficeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.BookingStatus)
                .WithMany(bs => bs.Bookings)
                .HasForeignKey(b => b.BookingStatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Floor>()
                .HasOne(f => f.Office)
                .WithMany(o => o.Floors)
                .HasForeignKey(f => f.OfficeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParkingSpace>()
                .HasOne(ps => ps.Floor)
                .WithMany(f => f.ParkingSpaces)
                .HasForeignKey(ps => ps.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workspace>()
                .HasOne(ws => ws.Floor)
                .WithMany(f => f.Workspaces)
                .HasForeignKey(ws => ws.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}