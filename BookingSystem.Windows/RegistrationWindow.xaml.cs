using System;
using System.Data.SqlClient; // Не забудьте добавить ссылку на System.Data
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media;

namespace BookingSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            // Логика регистрации
            string login = texBoxLogin.Text.Trim();
            string password = passBox.Password.Trim();
            string confirmPassword = passBox_2.Password.Trim();
            string email = texBoxEmail.Text.ToLower();
            string phone = texBoxTelephone.Text.Trim();

            // Сброс фона полей
            ResetFieldStyles();

            bool isValid = true;

            // Валидация
            if (string.IsNullOrWhiteSpace(login) || login.Length < 5)
            {
                texBoxLogin.ToolTip = "Логин должен содержать не менее 5 символов.";
                texBoxLogin.Background = Brushes.DarkRed;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
            {
                passBox.ToolTip = "Пароль должен содержать не менее 5 символов.";
                passBox.Background = Brushes.DarkRed;
                isValid = false;
            }
            if (password != confirmPassword)
            {
                passBox_2.ToolTip = "Пароли не совпадают.";
                passBox_2.Background = Brushes.DarkRed;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
            {
                texBoxEmail.ToolTip = "Введите корректный Email.";
                texBoxEmail.Background = Brushes.DarkRed;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                texBoxTelephone.ToolTip = "Введите номер телефона.";
                texBoxTelephone.Background = Brushes.DarkRed;
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме.");
                return;
            }

            // Хеширование пароля
            var (passwordHash, passwordSalt) = HashPassword(password);

            // Сохранение в базу данных
            SavePasswordToDatabase(login, email, phone, passwordHash, passwordSalt);

            // Если все проверки пройдены, можно продолжить регистрацию
            MessageBox.Show("Регистрация успешна!");

            // Переход обратно в главное окно
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private (string hash, string salt) HashPassword(string password)
        {
            // Генерация соли
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                string salt = Convert.ToBase64String(saltBytes);

                // Хеширование пароля
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
                {
                    string hash = Convert.ToBase64String(pbkdf2.GetBytes(32)); // 256 бит
                    return (hash, salt);
                }
            }
        }

        private void SavePasswordToDatabase(string login, string email, string phone, string passwordHash, string passwordSalt)
        {
            // Здесь должна быть логика для сохранения данных в базу данных
            // Пример с использованием ADO.NET
            using (var connection = new SqlConnection("your_connection_string")) // Замените на вашу строку подключения
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (Login, Email, Phone, PasswordHash, PasswordSalt) VALUES (@login, @Email, @Phone, @hash, @salt)", connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@hash", passwordHash);
                command.Parameters.AddWithValue("@salt", passwordSalt);
                command.ExecuteNonQuery();
            }
        }

        private void ResetFieldStyles()
        {
            // Сброс фона и подсказок для всех полей
            texBoxLogin.ToolTip = "";
            texBoxLogin.Background = Brushes.Transparent;
            passBox.ToolTip = "";
            passBox.Background = Brushes.Transparent;
            passBox_2.ToolTip = "";
            passBox_2.Background = Brushes.Transparent;
            texBoxEmail.ToolTip = "";
            texBoxEmail.Background = Brushes.Transparent;
            texBoxTelephone.ToolTip = "";
            texBoxTelephone.Background = Brushes.Transparent;
        }
    }
}