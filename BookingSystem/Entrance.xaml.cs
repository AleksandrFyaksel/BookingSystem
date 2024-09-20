using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using BookingSystem.Business.Managers;

namespace BookingSystem
{
    
        public partial class Entrance : Window
        {
            private readonly BookingContext _context;
            private readonly BookingManager _bookingManager;
            private readonly IServiceProvider _serviceProvider;

            public Entrance(BookingContext context, BookingManager bookingManager, IServiceProvider serviceProvider)
            {
                InitializeComponent();
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            }

            private void Entrance_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.Topmost = true;
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            string login = texBoxLogin.Text.Trim();
            string password = passBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            try
            {
                if (IsLoginValid(login, password))
                {
                    MessageBox.Show("Вход выполнен успешно!");
                    var bookingWindow = new BookingWindow(_context, _bookingManager);
                    bookingWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private bool IsLoginValid(string login, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == login || u.Name == login);
            return user != null && user.PasswordHash == HashPassword(password);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void Button_ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция сброса пароля еще не реализована.");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow(_context, _bookingManager, _serviceProvider);
            registrationWindow.Show();
            this.Close();
        }
    }
}