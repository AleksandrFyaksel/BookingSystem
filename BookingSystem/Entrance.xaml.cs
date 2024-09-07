using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BookingSystem
{
    public partial class Entrance : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public Entrance(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += Entrance_Loaded;
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

            if (IsLoginValid(login, password))
            {
                MessageBox.Show("Вход выполнен успешно!");
                var bookingWindow = _serviceProvider.GetRequiredService<BookingWindow>();
                bookingWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private bool IsLoginValid(string login, string password)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
            optionsBuilder.UseSqlServer(((App)Application.Current).Configuration.GetConnectionString("BookingDatabase"));

            using (var context = new BookingContext(optionsBuilder.Options))
            {
                var user = context.Users.SingleOrDefault(u => u.Email == login || u.Name == login);
                if (user != null && user.PasswordHash == HashPassword(password))
                {
                    return true;
                }
            }
            return false;
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
            // Запрос email у пользователя
            string email = Microsoft.VisualBasic.Interaction.InputBox("Введите ваш email:", "Сброс пароля", "");

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Пожалуйста, введите email.");
                return;
            }

            // Настройка контекста базы данных
            var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
            optionsBuilder.UseSqlServer(((App)Application.Current).Configuration.GetConnectionString("BookingDatabase"));

            using (var context = new BookingContext(optionsBuilder.Options))
            {
                // Проверка существования пользователя с таким email
                var user = context.Users.SingleOrDefault(u => u.Email == email);
                if (user == null)
                {
                    MessageBox.Show("Пользователь с таким email не найден.");
                    return;
                }

                // Генерация временного пароля
                string tempPassword = GenerateTemporaryPassword();
                user.PasswordHash = HashPassword(tempPassword); // Хешируем временный пароль
                context.SaveChanges();

                // Отправка email с временным паролем
                SendPasswordResetEmail(email, tempPassword);
                MessageBox.Show("На ваш email отправлен временный пароль для сброса.");
            }
        }

        private string GenerateTemporaryPassword()
        {
            // Генерация временного пароля (например, случайная строка)
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendPasswordResetEmail(string email, string tempPassword)
        {
            // Настройка SMTP-клиента для отправки email
            var smtpClient = new SmtpClient("smtp.example.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your_email@example.com", "your_email_password"),
                EnableSsl = true,
            };

            // Создание сообщения
            var mailMessage = new MailMessage
            {
                From = new MailAddress("your_email@example.com"),
                Subject = "Сброс пароля",
                Body = $"Ваш временный пароль: {tempPassword}\nПожалуйста, измените его после входа в систему.",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);

            // Отправка email
            smtpClient.Send(mailMessage);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = _serviceProvider.GetRequiredService<RegistrationWindow>();
            registrationWindow.Show();
            this.Close();
        }
    }
}