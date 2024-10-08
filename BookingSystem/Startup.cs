﻿using BookingSystem.Business.Managers;
using BookingSystem.DAL.Data;
using BookingSystem.DAL.Repositories; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem
{
    internal class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Регистрация служб
            services.AddDbContext<BookingContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("BookingDatabase"))); // Используйте строку подключения из конфигурации

            // Регистрация UnitOfWork
            services.AddScoped<IUnitOfWork>(provider => new EFUnitOfWork(_configuration.GetConnectionString("BookingDatabase"))); // Измените на EFUnitOfWork

            services.AddScoped<BookingManager>(); // Регистрация менеджера бронирования
            services.AddTransient<MainWindow>(); // Регистрация главного окна
            services.AddTransient<Entrance>(); // Регистрация окна входа
            services.AddTransient<RegistrationWindow>(); // Регистрация окна регистрации
        }
    }
}