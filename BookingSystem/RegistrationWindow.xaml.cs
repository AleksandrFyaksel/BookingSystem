using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using BookingSystem.Business.Managers;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem
{
    public partial class RegistrationWindow : Window
    {
        private readonly BookingContext _context;
        private readonly BookingManager _bookingManager;
        private readonly IServiceProvider _serviceProvider;

        public RegistrationWindow(BookingContext context, BookingManager bookingManager, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            
            this.WindowState = WindowState.Maximized;
            
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(LoginTextBox.Text.Trim()))
            {
                MessageBox.Show("Логин не может быть пустым.");
                return;
            }

            string login = LoginTextBox.Text.Trim();

            if (string.IsNullOrEmpty(PasswordBox.Password.Trim()))
            {
                MessageBox.Show("Пароль не может быть пустым.");
                return;
            }

            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(ConfirmPasswordBox.Password.Trim()))
            {
                MessageBox.Show("Подтверждение пароля не может быть пустым.");
                return;
            }

            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(EmailTextBox.Text.Trim()))
            {
                MessageBox.Show("Email не может быть пустым.");
                return;
            }

            string email = EmailTextBox.Text.Trim();

            string phoneNumber = PhoneTextBox.Text.Trim(); // Здесь также можно добавить проверку на пустое значение

            // Валидация введенных данных
            if (!ValidateInput(login, password, confirmPassword, email))
                return;

            // Проверка на существование пользователя
            if (UserExists(login, email))
            {
                MessageBox.Show("Пользователь с таким логином или email уже существует.");
                return;
            }

            // Проверка на существование роли
            var defaultRole = _context.Roles.FirstOrDefault(r => r.RoleID == 1); // Замените 1 на ID вашей роли по умолчанию
            if (defaultRole == null)
            {
                MessageBox.Show("Роль по умолчанию не найдена. Пожалуйста, добавьте роль в систему.");
                return;
            }

            // Создание нового пользователя
            var newUser = new User
            {
                Name = login,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = HashPassword(password),
                RoleID = defaultRole.RoleID // Используем ID найденной роли
            };

            // Сохранение нового пользователя в базе данных
            SaveNewUser(newUser);
        }

        private bool ValidateInput(string login, string password, string confirmPassword, string email)
        {
            if (string.IsNullOrEmpty(login) || login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать не менее 3 символов.");
                return false;
            }

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.");
                return false;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return false;
            }

            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                MessageBox.Show("Введите действующий email.");
                return false;
            }

            return true;
        }

        private bool UserExists(string login, string email)
        {
            return _context.Users.Any(u => u.Email == email || u.Name == login);
        }

        private void SaveNewUser(User newUser)
        {
            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!");

                // Открываем окно входа
                var entranceWindow = new Entrance(_context, _bookingManager, _serviceProvider);
                entranceWindow.Show();
                this.Close();
            }
            catch (DbUpdateException dbEx)
            {
                // Получаем информацию о внутреннем исключении
                var innerException = dbEx.InnerException?.Message ?? "Неизвестная ошибка базы данных.";
                MessageBox.Show($"Ошибка при сохранении пользователя: {dbEx.Message}\n{innerException}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var entranceWindow = new Entrance(_context, _bookingManager, _serviceProvider);
            entranceWindow.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}