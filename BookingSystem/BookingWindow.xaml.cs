using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using BookingSystem.Business.Managers;
using BookingSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingSystem.DAL.Data;
using System.IO;
using Drawing;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem
{
    public partial class BookingWindow : Window
    {
        private bool isDragging = false;
        private Vector clickOffset;
        private ShapeVisual selectedVisual;
        private ImageBrush backgroundBrush;
        private readonly BookingContext _context;
        private readonly BookingManager _bookingManager;

        public BookingWindow(BookingContext context, BookingManager bookingManager)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
            PopulateOfficesComboBox();
            PopulateFloorsComboBox();
            this.WindowState = WindowState.Maximized;
        }

        private async void PopulateOfficesComboBox()
        {
            try
            {
                var offices = await _bookingManager.GetAllOfficesAsync();
                if (offices == null || !offices.Any())
                {
                    MessageBox.Show("Нет доступных офисов.");
                    return;
                }
                OfficesComboBox.ItemsSource = offices;
                OfficesComboBox.DisplayMemberPath = "OfficeName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке офисов: {ex.Message}");
            }
        }

        private async void PopulateFloorsComboBox()
        {
            try
            {
                var floors = await _bookingManager.GetAllFloorsAsync();
                FloorsComboBox.ItemsSource = floors;
                FloorsComboBox.DisplayMemberPath = "FloorName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке этажей: {ex.Message}");
            }
        }

        private void OfficesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OfficesComboBox.SelectedItem is Office selectedOffice)
            {
                MessageBox.Show($"Выбран офис: {selectedOffice.OfficeName}");
            }
        }

        private async void FloorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FloorsComboBox.SelectedItem is Floor selectedFloor)
            {
                LoadImageForFloor(selectedFloor); // Загружаем изображение для выбранного этажа

                try
                {
                    // Получаем парковочные места для выбранного этажа
                    var parkingSpaces = await _context.ParkingSpaces
                        .Where(ps => ps.FloorID == selectedFloor.FloorID)
                        .ToListAsync();

                    // Получаем рабочие места для выбранного этажа
                    var workspaces = await _context.Workspaces
                        .Where(ws => ws.FloorID == selectedFloor.FloorID)
                        .ToListAsync();

                    // Отображаем парковочные места и рабочие места на графической поверхности
                    DisplayParkingSpaces(parkingSpaces);
                    DisplayWorkspaces(workspaces);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
            }
        }


      

        public async Task AddFloorAsync(Floor floor, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                // Чтение данных изображения из файла
                floor.ImageData = await File.ReadAllBytesAsync(imagePath);
                floor.ImageMimeType = Path.GetExtension(imagePath).ToLower() switch
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".bmp" => "image/bmp",
                    _ => "application/octet-stream",
                };
            }
            else
            {
                // Обработка случая, когда файл не найден
                throw new FileNotFoundException($"Файл не найден: {imagePath}");
            }

            await _context.Floors.AddAsync(floor);
            await _context.SaveChangesAsync();
        }



        private void DisplayParkingSpaces(List<ParkingSpace> parkingSpaces)
        {
            drawingSurface.Clear(); // Очистка предыдущих визуализаций, если необходимо

            foreach (var space in parkingSpaces)
            {
                DrawingVisual visual = new DrawingVisual();
                ShapeType shapeType = ShapeType.Circle; // Указываем, что это парковочное место
                ShapeVisual shapeVisual = new ShapeVisual(visual, shapeType, space.Label, space.IsAvailable);

                // Обновляем цвет фигуры в зависимости от статуса доступности
                shapeVisual.UpdateVisualColor();

                // Добавляем визуализацию на поверхность
                drawingSurface.AddVisual(shapeVisual.Visual, shapeVisual.Type, shapeVisual.Label);
            }
        }

        private void DisplayWorkspaces(List<Workspace> workspaces)
        {
            foreach (var workspace in workspaces)
            {
                DrawingVisual visual = new DrawingVisual();
                ShapeType shapeType = ShapeType.Square; // Указываем, что это рабочее место
                ShapeVisual shapeVisual = new ShapeVisual(visual, shapeType, workspace.Label, workspace.IsAvailable);

                // Обновляем цвет фигуры в зависимости от статуса доступности
                shapeVisual.UpdateVisualColor();

                // Добавляем визуализацию на поверхность
                drawingSurface.AddVisual(shapeVisual.Visual, shapeVisual.Type, shapeVisual.Label);
            }
        }

        private void LoadImageForFloor(Floor selectedFloor)
        {
            // Проверка на null
            if (selectedFloor == null)
            {
                MessageBox.Show("Выберите этаж перед загрузкой изображения.");
                return;
            }

            // Проверка наличия данных изображения
            if (selectedFloor.ImageData != null && selectedFloor.ImageData.Length > 0)
            {
                MessageBox.Show($"Загружается изображение для этажа: {selectedFloor.FloorName}"); // Отладочное сообщение
                try
                {
                    using (var stream = new MemoryStream(selectedFloor.ImageData))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze(); // Замораживаем изображение для повышения производительности
                        FloorImageControl.Source = bitmap; // Установите изображение в элемент управления Image
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок при загрузке изображения
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                }
            }
            else
            {
                // Очистка изображения, если его нет
                MessageBox.Show("Изображение для этого этажа отсутствует."); // Отладочное сообщение
                FloorImageControl.Source = null;
            }
        }

        private void LoadBackground_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Select Background Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    backgroundBrush = new ImageBrush(new BitmapImage(new Uri(filePath)));
                    drawingSurface.Background = backgroundBrush;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                }
            }
        }

        private void RemoveBackground_Click(object sender, RoutedEventArgs e)
        {
            drawingSurface.Background = Brushes.White;
            backgroundBrush = null;
        }

        private string GetMimeType(string filePath)
        {
            var extension = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
        }

        private void drawingSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(drawingSurface);
            ShapeVisual shapeVisual = drawingSurface.GetVisual(pointClicked);

            if (cmdDelete.IsChecked == true && shapeVisual != null)
            {
                drawingSurface.DeleteVisual(shapeVisual.Visual);
                return;
            }

            if (shapeVisual != null)
            {
                BookingForm bookingForm = new BookingForm(_bookingManager);
                if (bookingForm.ShowDialog() == true)
                {
                    string bookingDate = bookingForm.BookingDate.ToString("d");
                    string startTime = bookingForm.StartTime;
                    string endTime = bookingForm.EndTime;
                    string additionalRequirements = bookingForm.AdditionalRequirements;

                    MessageBox.Show($"Бронирование на {bookingDate} с {startTime} до {endTime}.\nДополнительные требования: {additionalRequirements}");
                }
            }
            else
            {
                HandleShapeCreation(pointClicked);
            }
        }

        private void HandleShapeCreation(Point pointClicked)
        {
            if (cmdAdd.IsChecked == true)
            {
                string label = PromptForLabel();
                DrawingVisual visual = new DrawingVisual();
                DrawSquare(visual, pointClicked, false);
                drawingSurface.AddVisual(visual, ShapeType.Square, label);
            }
            else if (cmdAddCircle.IsChecked == true)
            {
                string label = PromptForLabel();
                DrawingVisual visual = new DrawingVisual();
                DrawCircle(visual, pointClicked, false);
                drawingSurface.AddVisual(visual, ShapeType.Circle, label);
            }
            else if (cmdSelectMove.IsChecked == true)
            {
                ShapeVisual shapeToMove = drawingSurface.GetVisual(pointClicked);
                if (shapeToMove != null)
                {
                    clickOffset = shapeToMove.Visual.ContentBounds.TopLeft - pointClicked;
                    isDragging = true;
                    selectedVisual = shapeToMove;
                }
            }
        }

        private void drawingSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            selectedVisual = null;
        }

        private void drawingSurface_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(drawingSurface);
            ShapeVisual shapeVisual = drawingSurface.GetVisual(pointClicked);

            if (shapeVisual != null)
            {
                clickOffset = shapeVisual.Visual.ContentBounds.TopLeft - pointClicked;
                isDragging = true;
                selectedVisual = shapeVisual;
                drawingSurface.CaptureMouse();
            }
        }

        private void drawingSurface_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            selectedVisual = null;
            drawingSurface.ReleaseMouseCapture();
        }

        private void drawingSurface_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedVisual != null)
            {
                Point pointDragged = e.GetPosition(drawingSurface) + clickOffset;
                if (selectedVisual.Type == ShapeType.Square)
                {
                    DrawSquare(selectedVisual.Visual, pointDragged, true);
                }
                else if (selectedVisual.Type == ShapeType.Circle)
                {
                    DrawCircle(selectedVisual.Visual, pointDragged, true);
                }
            }
        }

        private Brush drawingBrush = Brushes.AliceBlue;
        private Pen drawingPen = new Pen(Brushes.SteelBlue, 3);
        private Size squareSize = new Size(30, 30);

        private void DrawSquare(DrawingVisual visual, Point topLeftCorner, bool isSelected)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                Brush brush = isSelected ? Brushes.LightGoldenrodYellow : drawingBrush;
                dc.DrawRectangle(brush, drawingPen, new Rect(topLeftCorner, squareSize));

                ShapeVisual shapeVisual = drawingSurface.GetVisual(topLeftCorner);
                if (shapeVisual != null)
                {
                    FormattedText formattedText = new FormattedText(
                        shapeVisual.Label,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    dc.DrawText(formattedText, new Point(topLeftCorner.X + 5, topLeftCorner.Y + squareSize.Height + 5));
                }
            }
        }

        private void DrawCircle(DrawingVisual visual, Point center, bool isSelected)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                Brush brush = isSelected ? Brushes.LightGoldenrodYellow : drawingBrush;
                dc.DrawEllipse(brush, drawingPen, center, squareSize.Width / 2, squareSize.Height / 2);

                ShapeVisual shapeVisual = drawingSurface.GetVisual(center);
                if (shapeVisual != null)
                {
                    FormattedText formattedText = new FormattedText(
                        shapeVisual.Label,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    dc.DrawText(formattedText, new Point(center.X - formattedText.Width / 2, center.Y + (squareSize.Height / 2) + 5));
                }
            }
        }

        private string PromptForLabel()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Введите номер:", "Номер фигуры", "");
        }
    }
}