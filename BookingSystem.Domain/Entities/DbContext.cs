using Microsoft.EntityFrameworkCore;
using BookingSystem.Domain.Entities;

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
        public DbSet<BookingStatus> BookingStatuses { get; set; } // Добавлено для управления статусами бронирования

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений между сущностями

            // Отношение между Booking и User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между Booking и Workspace
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Workspace)
                .WithMany(ws => ws.Bookings)
                .HasForeignKey(b => b.WorkspaceID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между Booking и ParkingSpace
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ParkingSpace)
                .WithMany(ps => ps.Bookings)
                .HasForeignKey(b => b.ParkingSpaceID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между Floor и Office
            modelBuilder.Entity<Floor>()
                .HasOne(f => f.Office)
                .WithMany(o => o.Floors)
                .HasForeignKey(f => f.OfficeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между Workspace и Floor
            modelBuilder.Entity<Workspace>()
                .HasOne(ws => ws.Floor)
                .WithMany(f => f.Workspaces)
                .HasForeignKey(ws => ws.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между ParkingSpace и Floor
            modelBuilder.Entity<ParkingSpace>()
                .HasOne(ps => ps.Floor)
                .WithMany(f => f.ParkingSpaces)
                .HasForeignKey(ps => ps.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между User и UserPassword
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserPasswords)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Отношение между Role и User
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между Department и User
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Users)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Отношение между Booking и BookingStatus
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.BookingStatus) // Предполагается, что у Booking есть свойство BookingStatus
                .WithMany(bs => bs.Bookings)
                .HasForeignKey(b => b.BookingStatusID) // Предполагается, что у Booking есть свойство BookingStatusID
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}