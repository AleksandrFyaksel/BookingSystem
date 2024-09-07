using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BookingSystem
{
    public partial class RegistrationWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public RegistrationWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += RegistrationWindow_Loaded;
        }

        private void RegistrationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            this.Topmost = true;
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Включаем кнопку регистрации, если длина логина >= 3
            RegisterButton.IsEnabled = LoginTextBox.Text.Length >= 3;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            // Сбор данных из полей ввода
            string username = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string email = EmailTextBox.Text.Trim();
            string phoneNumber = PhoneTextBox.Text.Trim();

            ResetErrorTags();

            // Проверка на пустые поля и установка ошибок
            if (string.IsNullOrWhiteSpace(username))
            {
                SetErrorTag(LoginTextBox, "Пожалуйста, введите логин.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                SetErrorTag(PasswordBox, "Пожалуйста, введите пароль.");
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                SetErrorTag(ConfirmPasswordBox, "Пожалуйста, повторите пароль.");
                return;
            }

            if (password != confirmPassword)
            {
                SetErrorTag(PasswordBox, "Пароли не совпадают.");
                SetErrorTag(ConfirmPasswordBox, "Пароли не совпадают.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                SetErrorTag(EmailTextBox, "Пожалуйста, введите email.");
                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                SetErrorTag(PhoneTextBox, "Пожалуйста, введите номер телефона.");
                return;
            }

            // Настройка контекста базы данных
            var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
            optionsBuilder.UseSqlServer(((App)Application.Current).Configuration.GetConnectionString("BookingDatabase"));

            using (var context = new BookingContext(optionsBuilder.Options))
            {
                // Проверка на существование пользователя с таким логином
                if (context.Users.Any(u => u.Name == username || u.Email == email))
                {
                    SetErrorTag(LoginTextBox, "Пользователь с таким именем или email уже существует.");
                    return;
                }

                // Создание нового пользователя
                var newUser = new User
                {
                    Name = username,
                    PasswordHash = HashPassword(password),
                    Email = email,
                    PhoneNumber = phoneNumber
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }

            // Переход к окну входа после успешной регистрации
            var entranceWindow = _serviceProvider.GetRequiredService<Entrance>();
            entranceWindow.Show();
            this.Close();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void ResetErrorTags()
        {
            LoginTextBox.Tag = null;
            PasswordBox.Tag = null;
            ConfirmPasswordBox.Tag = null;
            EmailTextBox.Tag = null;
            PhoneTextBox.Tag = null;
        }

        private void SetErrorTag(Control control, string message)
        {
            control.Tag = "Error";
            MessageBox.Show(message);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var entranceWindow = _serviceProvider.GetRequiredService<Entrance>();
            entranceWindow.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}