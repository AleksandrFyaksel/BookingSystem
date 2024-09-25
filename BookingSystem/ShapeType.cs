using System.Windows.Media;
using System.Windows;
using System.Threading.Tasks;
using BookingSystem.Business.Managers;
using BookingSystem.Domain.Entities;

namespace Drawing
{
    /// <summary>
    /// Типы фигур, которые могут быть нарисованы.
    /// </summary>
    public enum ShapeType
    {
        Square, // Рабочее место
        Circle  // Парковочное место
    }

    /// <summary>
    /// Представляет визуальное представление фигуры.
    /// </summary>
    public class ShapeVisual
    {
        /// <summary>
        /// Получает или устанавливает визуальное представление фигуры.
        /// </summary>
        public DrawingVisual Visual { get; set; }
        public ShapeType Type { get; set; }
        public string Label { get; set; }
        public bool IsBooked { get; set; } // Статус бронирования
        public UIElement VisualElement { get; set; } // Добавлено для хранения ссылки на UIElement

        public ShapeVisual(DrawingVisual visual, ShapeType type, string label = "", bool isBooked = false)
        {
            Visual = visual;
            Type = type;
            Label = label;
            IsBooked = isBooked; // Инициализация статуса
        }

        /// <summary>
        /// Сохраняет фигуру в базе данных.
        /// </summary>
        public async Task SaveToDatabase(BookingManager bookingManager)
        {
            if (Type == ShapeType.Square)
            {
                var workspace = new Workspace { Label = Label };
                await bookingManager.AddWorkspaceAsync(workspace);
            }
            else if (Type == ShapeType.Circle)
            {
                var parkingSpace = new ParkingSpace { Label = Label };
                await bookingManager.AddParkingSpaceAsync(parkingSpace);
            }
        }

        /// <summary>
        /// Обновляет цвет фигуры в зависимости от статуса бронирования.
        /// </summary>
        public void UpdateVisualColor()
        {
            using (DrawingContext dc = Visual.RenderOpen())
            {
                Brush brush = IsBooked ? Brushes.Red : Brushes.Blue; // Красный, если забронировано, синий по умолчанию
                Rect rect = new Rect(0, 0, 30, 30); // Размер фигуры (например, квадрат)

                // Рисуем фигуру
                if (Type == ShapeType.Square)
                {
                    dc.DrawRectangle(brush, null, rect); // Рисуем квадрат
                }
                else if (Type == ShapeType.Circle)
                {
                    dc.DrawEllipse(brush, null, new Point(15, 15), 15, 15); // Рисуем круг
                }

                // Добавляем текст или метку, если необходимо
                FormattedText formattedText = new FormattedText(
                    Label,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    12,
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(Visual).PixelsPerDip);

                // Рисуем текст
                dc.DrawText(formattedText, new Point(5, 35)); // Позиция текста под фигурой
            }
        }
    }
}