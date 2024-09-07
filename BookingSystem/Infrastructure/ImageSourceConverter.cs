using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookingSystem.Infrastructure
{
    /// <summary>
    /// Конвертер для преобразования строкового пути к изображению в объект BitmapImage.
    /// </summary>
    public class ImageSourceConverter : IValueConverter
    {
        private readonly string root = Directory.GetCurrentDirectory();
        private string ImageDirectory => Path.Combine(root, "Images");

        /// <summary>
        /// Преобразует строку пути к изображению в объект BitmapImage.
        /// </summary>
        /// <param name="value">Путь к изображению.</param>
        /// <param name="targetType">Тип целевого значения.</param>
        /// <param name="parameter">Дополнительный параметр.</param>
        /// <param name="culture">Культура.</param>
        /// <returns>BitmapImage, если путь не null; иначе null.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var imagePath = Path.Combine(ImageDirectory, (string)value);
            var image = new BitmapImage();

            try
            {
                using (var stream = File.OpenRead(imagePath))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
            }
            catch (FileNotFoundException)
            {
                // Обработка случая, когда файл не найден
                Console.WriteLine($"Image not found: {imagePath}");
                return null; // Или можно вернуть изображение по умолчанию
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null; // Или можно вернуть изображение по умолчанию
            }

            return image;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="value">Значение для преобразования.</param>
        /// <param name="targetType">Тип целевого значения.</param>
        /// <param name="parameter">Дополнительный параметр.</param>
        /// <param name="culture">Культура.</param>
        /// <returns>Не реализовано.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}