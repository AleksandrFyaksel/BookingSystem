﻿// <auto-generated />
using System;
using BookingSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookingSystem.DAL.Migrations
{
    [DbContext(typeof(BookingContext))]
    [Migration("20240923064407_Migration5")]
    partial class Migration5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookingSystem.Domain.Entities.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"));

                    b.Property<string>("AdditionalRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BookingStatusID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParkingSpaceID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("WorkspaceID")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("BookingStatusID");

                    b.HasIndex("ParkingSpaceID");

                    b.HasIndex("UserID");

                    b.HasIndex("WorkspaceID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.BookingStatus", b =>
                {
                    b.Property<int>("BookingStatusID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingStatusID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BookingStatusID");

                    b.ToTable("BookingStatuses");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Floor", b =>
                {
                    b.Property<int>("FloorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FloorID"));

                    b.Property<string>("FloorName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ImageMimeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OfficeID")
                        .HasColumnType("int");

                    b.HasKey("FloorID");

                    b.HasIndex("OfficeID");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Office", b =>
                {
                    b.Property<int>("OfficeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OfficeID"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("OfficeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("OfficeID");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.ParkingSpace", b =>
                {
                    b.Property<int>("ParkingSpaceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParkingSpaceID"));

                    b.Property<int>("FloorID")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParkingSpaceID");

                    b.HasIndex("FloorID");

                    b.ToTable("ParkingSpaces");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleID"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<int?>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.UserPassword", b =>
                {
                    b.Property<int>("UserPasswordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPasswordID"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserPasswordID");

                    b.HasIndex("UserID");

                    b.ToTable("UserPasswords");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Workspace", b =>
                {
                    b.Property<int>("WorkspaceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkspaceID"));

                    b.Property<int>("FloorID")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WorkspaceID");

                    b.HasIndex("FloorID");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Booking", b =>
                {
                    b.HasOne("BookingSystem.Domain.Entities.BookingStatus", "BookingStatus")
                        .WithMany("Bookings")
                        .HasForeignKey("BookingStatusID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BookingSystem.Domain.Entities.ParkingSpace", "ParkingSpace")
                        .WithMany("Bookings")
                        .HasForeignKey("ParkingSpaceID");

                    b.HasOne("BookingSystem.Domain.Entities.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSystem.Domain.Entities.Workspace", "Workspace")
                        .WithMany("Bookings")
                        .HasForeignKey("WorkspaceID");

                    b.Navigation("BookingStatus");

                    b.Navigation("ParkingSpace");

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Floor", b =>
                {
                    b.HasOne("BookingSystem.Domain.Entities.Office", "Office")
                        .WithMany("Floors")
                        .HasForeignKey("OfficeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Office");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.ParkingSpace", b =>
                {
                    b.HasOne("BookingSystem.Domain.Entities.Floor", "Floor")
                        .WithMany("ParkingSpaces")
                        .HasForeignKey("FloorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Floor");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.User", b =>
                {
                    b.HasOne("BookingSystem.Domain.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BookingSystem.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.UserPassword", b =>
                {
                    b.HasOne("BookingSystem.Domain.Entities.User", "User")
                        .WithMany("UserPasswords")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Workspace", b =>
                {
                    b.HasOne("BookingSystem.Domain.Entities.Floor", "Floor")
                        .WithMany("Workspaces")
                        .HasForeignKey("FloorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Floor");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.BookingStatus", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Department", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Floor", b =>
                {
                    b.Navigation("ParkingSpaces");

                    b.Navigation("Workspaces");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Office", b =>
                {
                    b.Navigation("Floors");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.ParkingSpace", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("UserPasswords");
                });

            modelBuilder.Entity("BookingSystem.Domain.Entities.Workspace", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
