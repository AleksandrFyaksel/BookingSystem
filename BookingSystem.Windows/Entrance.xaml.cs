using System;
using System.Windows;

namespace BookingSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для Entrance.xaml
    /// </summary>
    public partial class Entrance : Window
    {
        public Entrance()
        {
            InitializeComponent();
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            string login = texBoxLogin.Text.Trim(); // Получаем логин из текстового поля
            string password = passBox.Password.Trim(); // Получаем пароль из поля пароля

            // Проверка логина и пароля
            if (IsLoginValid(login, password))
            {
                MessageBox.Show("Вход выполнен успешно!");
                // Здесь можно открыть главное окно приложения
                // MainWindow mainAppWindow = new MainWindow();
                // mainAppWindow.Show();
                // this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            // Переход на окно регистрации
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private void Button_ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Здесь вы должны получить email и телефон пользователя
            string email = "user@example.com"; // Замените на реальный email
            string phone = "+1234567890"; // Замените на реальный номер телефона
            string password = "userPassword"; // Замените на реальный пароль пользователя

            // Отправка email
            SendEmail(email, password);
            // Отправка SMS
            SendSms(phone, password);

            MessageBox.Show("Письмо с паролем отправлено на вашу почту и SMS на номер телефона.");
        }

        private bool IsLoginValid(string login, string password)
        {
            // Здесь должна быть логика проверки логина и пароля
            // Например, проверка с данными из базы данных
            return login == "admin" && password == "password"; // Пример проверки
        }

        private void SendEmail(string toEmail, string password)
        {
            try
            {
                // Логика отправки email
                // Используйте SmtpClient для отправки email
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке email: {ex.Message}");
            }
        }

        private void SendSms(string phone, string password)
        {
            // Здесь должна быть логика отправки SMS
            // Например, использование API Twilio или другого сервиса
        }
    }
}