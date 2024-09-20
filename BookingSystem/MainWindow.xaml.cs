using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BookingSystem.DAL.Data;
using BookingSystem.Business.Managers;

namespace BookingSystem
{
    public partial class MainWindow : Window
    {
        private readonly BookingContext _context;
        private readonly BookingManager _bookingManager;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(BookingContext context, BookingManager bookingManager, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized; // Устанавливаем окно в полноэкранный режим
            LoginButton.Focus(); // Устанавливаем фокус на кнопку "Вход"
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var entranceWindow = new Entrance(_context, _bookingManager, _serviceProvider);
            entranceWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow(_context, _bookingManager, _serviceProvider);
            registrationWindow.Show();
            this.Close();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программа 'Booking System'\nВерсия 1.0\nРазработано Фякселем A.M.",
                            "О программе",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}