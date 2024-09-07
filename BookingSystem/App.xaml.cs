using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using BookingSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                // Загрузка конфигурации
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                Configuration = builder.Build();

                // Настройка DI
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();

                // Запуск главного окна
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске приложения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Регистрация контекста базы данных
            services.AddDbContext<BookingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BookingDatabase"))); // Используйте строку подключения из конфигурации

            // Регистрация других сервисов
            services.AddTransient<MainWindow>(); // Регистрация главного окна
            services.AddTransient<BookingWindow>(); // Регистрация BookingWindow
            services.AddTransient<Entrance>(); // Регистрация окна входа
            services.AddTransient<RegistrationWindow>(); // Регистрация окна регистрации
        }
    }
}