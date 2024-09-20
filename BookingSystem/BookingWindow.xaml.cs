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
                var offices = await _context.Offices.ToListAsync();
                if (offices == null || !offices.Any())
                {
                    MessageBox.Show("Нет доступных офисов.");
                    return;
                }
                OfficesComboBox.ItemsSource = offices;
                OfficesComboBox.DisplayMemberPath = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке офисов: {ex.Message}");
            }
        }

        private void OfficesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OfficesComboBox.SelectedItem is Office selectedOffice)
            {
                MessageBox.Show($"Выбран офис: {selectedOffice.OfficeName}");
            }
        }

        private async void PopulateFloorsComboBox()
        {
            try
            {
                var floors = await _context.Floors.ToListAsync();
                FloorsComboBox.ItemsSource = floors;
                FloorsComboBox.DisplayMemberPath = "FloorName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке этажей: {ex.Message}");
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

        //private async void OfficesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (OfficesComboBox.SelectedItem is Office selectedOffice)
        //    {
        //        try
        //        {
        //            var workspaces = await _context.Workspaces
        //                .Where(ws => ws.FloorID == selectedOffice.OfficeID)
        //                .ToListAsync();
        //            // Обновите интерфейс для отображения рабочих мест
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка при загрузке рабочих мест: {ex.Message}");
        //        }
        //    }
        //}

        private async void FloorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FloorsComboBox.SelectedItem is Floor selectedFloor)
            {
                try
                {
                    var parkingSpaces = await _context.ParkingSpaces
                        .Where(ps => ps.FloorID == selectedFloor.FloorID)
                        .ToListAsync();
                    // Обновите интерфейс для отображения парковочных мест
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке парковочных мест: {ex.Message}");
                }
            }
        }

        private async void LoadImageForFloor(Floor selectedFloor)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Select Floor Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    byte[] imageData = File.ReadAllBytes(filePath);
                    string mimeType = GetMimeType(filePath);

                    selectedFloor.ImageData = imageData;
                    selectedFloor.ImageMimeType = mimeType;

                    _context.Floors.Update(entity: selectedFloor);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                }
            }
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

            if (cmdDelete.IsChecked == true)
            {
                if (shapeVisual != null)
                {
                    drawingSurface.DeleteVisual(shapeVisual.Visual);
                }
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