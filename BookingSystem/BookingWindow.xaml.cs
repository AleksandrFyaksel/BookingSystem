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
        private Floor _currentFloor;

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
                _currentFloor = selectedFloor; // Сохраняем текущий этаж
                Console.WriteLine($"Selected Floor ID: {selectedFloor.FloorID}"); // Логирование ID этажа

                // Попробуем загрузить изображение для выбранного этажа
                try
                {
                    await DrawImageOnCanvas(selectedFloor); // Загружаем изображение для выбранного этажа
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                    return;
                }

                try
                {
                    // Получаем парковочные места для выбранного этажа
                    var parkingSpaces = await _context.ParkingSpaces
                        .Where(ps => ps.FloorID == selectedFloor.FloorID)
                        .ToListAsync();

                    // Логирование количества парковочных мест
                    Console.WriteLine($"Parking Spaces Count: {parkingSpaces.Count}");

                    // Получаем рабочие места для выбранного этажа
                    var workspaces = await _context.Workspaces
                        .Where(ws => ws.FloorID == selectedFloor.FloorID)
                        .ToListAsync();

                    // Логирование количества рабочих мест
                    Console.WriteLine($"Workspaces Count: {workspaces.Count}");

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

        private void DisplayWorkspaces(List<Workspace> workspaces)
        {
            foreach (var workspace in workspaces)
            {
                DrawingVisual visual = new DrawingVisual();
                ShapeType shapeType = ShapeType.Square; // Указываем, что это рабочее место
                ShapeVisual shapeVisual = new ShapeVisual(visual, shapeType, workspace.Label, workspace.IsAvailable);

                // Обновляем цвет фигуры в зависимости от статуса доступности
                shapeVisual.UpdateVisualColor();

                // Устанавливаем позицию на основе PositionX и PositionY из базы данных
                Point position = new Point(workspace.PositionX, workspace.PositionY);

                // Добавляем визуализацию на поверхность с учетом позиции
                drawingSurface.AddVisual(shapeVisual.Visual, shapeVisual.Type, shapeVisual.Label, position);
                shapeVisual.VisualElement = drawingSurface.GetVisualElement(shapeVisual.Visual); // Сохраняем ссылку на UIElement
            }
        }

        private async Task DrawImageOnCanvas(Floor selectedFloor)
        {
            if (selectedFloor == null)
            {
                MessageBox.Show("Выберите этаж.");
                return;
            }

            string imagePath = await _bookingManager.GetFloorImagePathAsync(selectedFloor.FloorID);

            // Проверка существования файла
            if (File.Exists(imagePath))
            {
                backgroundBrush = new ImageBrush(new BitmapImage(new Uri(imagePath)));
                drawingSurface.Background = backgroundBrush;
            }
            else
            {
                MessageBox.Show("Изображение не найдено.");
            }
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

                // Устанавливаем позицию на основе PositionX и PositionY из базы данных
                Point position = new Point(space.PositionX, space.PositionY);

                // Добавляем визуализацию на поверхность с учетом позиции
                drawingSurface.AddVisual(shapeVisual.Visual, shapeVisual.Type, shapeVisual.Label, position);
                shapeVisual.VisualElement = drawingSurface.GetVisualElement(shapeVisual.Visual); // Сохраняем ссылку на UIElement
            }
        }

        private async void drawingSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(drawingSurface);
            ShapeVisual shapeVisual = drawingSurface.GetVisual(pointClicked);

            if (cmdDelete.IsChecked == true && shapeVisual != null)
            {
                // Удаляем фигуру с холста
                drawingSurface.DeleteVisual(shapeVisual.VisualElement); // Используем VisualElement

                // Удаляем соответствующую запись из базы данных
                if (shapeVisual.Type == ShapeType.Circle)
                {
                    var parkingSpace = await _context.ParkingSpaces
                        .FirstOrDefaultAsync(ps => ps.Label == shapeVisual.Label);
                    if (parkingSpace != null)
                    {
                        _context.ParkingSpaces.Remove(parkingSpace);
                        await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                        MessageBox.Show($"Парковочное место '{shapeVisual.Label}' удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Парковочное место не найдено в базе данных.");
                    }
                }
                else if (shapeVisual.Type == ShapeType.Square)
                {
                    var workspace = await _context.Workspaces
                        .FirstOrDefaultAsync(ws => ws.Label == shapeVisual.Label);
                    if (workspace != null)
                    {
                        _context.Workspaces.Remove(workspace);
                        await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                        MessageBox.Show($"Рабочее место '{shapeVisual.Label}' удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Рабочее место не найдено в базе данных.");
                    }
                }

                return; // Выход из метода после удаления
            }

            if (shapeVisual != null)
            {
                // Пример: Отображаем информацию о фигуре
                MessageBox.Show($"Вы выбрали фигуру: {shapeVisual.Label}, Тип: {shapeVisual.Type}");

                // Переключение режима перемещения
                if (cmdSelectMove.IsChecked == true)
                {
                    // Устанавливаем фигуру для перемещения
                    clickOffset = shapeVisual.VisualElement.TranslatePoint(new Point(0, 0), drawingSurface) - pointClicked;
                    isDragging = true;
                    selectedVisual = shapeVisual;
                    drawingSurface.CaptureMouse(); // Захватываем мышь для перетаскивания
                }
            }
            else
            {
                HandleShapeCreation(pointClicked);
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
                    // Проверка, является ли файл изображением
                    if (File.Exists(filePath))
                    {
                        backgroundBrush = new ImageBrush(new BitmapImage(new Uri(filePath)));
                        drawingSurface.Background = backgroundBrush;
                    }
                    else
                    {
                        MessageBox.Show("Выбранный файл не существует.");
                    }
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

        private void HandleShapeCreation(Point pointClicked)
        {
            if (cmdAdd.IsChecked == true)
            {
                string label = PromptForLabel();
                DrawingVisual visual = new DrawingVisual();
                DrawSquare(visual, pointClicked, false);
                ShapeVisual shapeVisual = new ShapeVisual(visual, ShapeType.Square, label);
                drawingSurface.AddVisual(shapeVisual.Visual, shapeVisual.Type, shapeVisual.Label, pointClicked);
                shapeVisual.VisualElement = drawingSurface.GetVisualElement(shapeVisual.Visual); // Сохраняем ссылку на UIElement
                shapeVisual.UpdateVisualColor(); // Обновляем цвет фигуры
            }
            else if (cmdAddCircle.IsChecked == true)
            {
                string label = PromptForLabel();
                DrawingVisual visual = new DrawingVisual();
                DrawCircle(visual, pointClicked, false);
                ShapeVisual shapeVisual = new ShapeVisual(visual, ShapeType.Circle, label);
                drawingSurface.AddVisual(shapeVisual.Visual, shapeVisual.Type, shapeVisual.Label, pointClicked);
                shapeVisual.VisualElement = drawingSurface.GetVisualElement(shapeVisual.Visual); // Сохраняем ссылку на UIElement
                shapeVisual.UpdateVisualColor(); // Обновляем цвет фигуры
            }
        }

        private void drawingSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            selectedVisual = null;
            drawingSurface.ReleaseMouseCapture(); // Освобождаем захват мыши
        }

        private void drawingSurface_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(drawingSurface);
            ShapeVisual shapeVisual = drawingSurface.GetVisual(pointClicked);

            if (shapeVisual != null)
            {
                clickOffset = shapeVisual.VisualElement.TranslatePoint(new Point(0, 0), drawingSurface) - pointClicked;
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

                // Обновляем позицию UIElement
                Canvas.SetLeft(selectedVisual.VisualElement, pointDragged.X);
                Canvas.SetTop(selectedVisual.VisualElement, pointDragged.Y);
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