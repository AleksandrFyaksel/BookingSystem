using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Drawing
{
    public class DrawingCanvas : Canvas
    {
        private bool isDragging = false;
        private Point clickPosition;
        private UIElement selectedShape; // Хранит ссылку на выбранную фигуру

        // Метод для добавления круга
        public void AddCircle(Point position)
        {
            Ellipse circle = CreateEllipse(position);
            this.Children.Add(circle);
            SubscribeToMouseEvents(circle);
        }

        // Метод для добавления квадрата
        public void AddSquare(Point position)
        {
            Rectangle square = CreateRectangle(position);
            this.Children.Add(square);
            SubscribeToMouseEvents(square);
        }

        // Создание круга
        private Ellipse CreateEllipse(Point position)
        {
            Ellipse circle = new Ellipse
            {
                Width = 30,
                Height = 30,
                Stroke = Brushes.SteelBlue,
                StrokeThickness = 3,
                Fill = Brushes.AliceBlue
            };
            SetPosition(circle, position);
            return circle;
        }

        // Создание квадрата
        private Rectangle CreateRectangle(Point position)
        {
            Rectangle square = new Rectangle
            {
                Width = 30,
                Height = 30,
                Stroke = Brushes.SteelBlue,
                StrokeThickness = 3,
                Fill = Brushes.AliceBlue
            };
            SetPosition(square, position);
            return square;
        }

        // Установка позиции фигуры
        private void SetPosition(Shape shape, Point position)
        {
            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);
        }

        // Подписка на события мыши
        private void SubscribeToMouseEvents(UIElement element)
        {
            element.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            element.MouseRightButtonDown += Shape_MouseRightButtonDown; // Обработчик для правой кнопки
            element.MouseMove += Shape_MouseMove;
            element.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
        }

        // Обработчик нажатия левой кнопки мыши
        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            clickPosition = e.GetPosition(this);
            selectedShape = sender as UIElement; // Сохраняем ссылку на выбранную фигуру
            selectedShape.CaptureMouse();
        }

        // Обработчик нажатия правой кнопки мыши
        private void Shape_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var shape = sender as UIElement;
            if (shape != null)
            {
                // Удаляем фигуру при нажатии правой кнопки мыши
                this.Children.Remove(shape);
                e.Handled = true; // Устанавливаем флаг, чтобы предотвратить дальнейшую обработку события
            }
        }

        // Обработчик движения мыши
        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedShape != null)
            {
                var shape = selectedShape;
                Point currentPosition = e.GetPosition(this);
                double offsetX = currentPosition.X - clickPosition.X;
                double offsetY = currentPosition.Y - clickPosition.Y;

                double newLeft = Canvas.GetLeft(shape) + offsetX;
                double newTop = Canvas.GetTop(shape) + offsetY;

                Canvas.SetLeft(shape, newLeft);
                Canvas.SetTop(shape, newTop);

                clickPosition = currentPosition; // Обновляем позицию клика
            }
        }

        // Обработчик отпускания левой кнопки мыши
        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            if (selectedShape != null)
            {
                selectedShape.ReleaseMouseCapture();
                selectedShape = null; // Сбрасываем ссылку на выбранную фигуру
            }
        }

        // Метод для очистки холста
        public void Clear()
        {
            this.Children.Clear(); // Очищает все дочерние элементы
        }

        // Метод для добавления визуализации
        public void AddVisual(DrawingVisual visual, ShapeType type, string label, Point position)
        {
            if (visual == null)
            {
                throw new ArgumentNullException(nameof(visual));
            }

            // Создаем DrawingImage из DrawingVisual
            DrawingImage drawingImage = new DrawingImage(visual.Drawing); // Предполагается, что visual.Drawing доступен
            Image image = new Image { Source = drawingImage };

            // Устанавливаем позицию для Image
            Canvas.SetLeft(image, position.X);
            Canvas.SetTop(image, position.Y);

            // Добавляем Image в Children
            this.Children.Add(image);
        }

        // Метод для получения визуализации по позиции
        public ShapeVisual GetVisual(Point point)
        {
            foreach (var child in this.Children)
            {
                if (child is Shape shape)
                {
                    Rect bounds = new Rect(Canvas.GetLeft(shape), Canvas.GetTop(shape), shape.Width, shape.Height);
                    if (bounds.Contains(point))
                    {
                        // Создаем DrawingVisual на основе Shape
                        DrawingVisual drawingVisual = new DrawingVisual();
                        using (DrawingContext dc = drawingVisual.RenderOpen())
                        {
                            // Создаем Pen из Stroke
                            Pen pen = new Pen(shape.Stroke, shape.StrokeThickness);

                            if (shape is Rectangle rectangle)
                            {
                                dc.DrawRectangle(rectangle.Fill, pen, new Rect(0, 0, rectangle.Width, rectangle.Height));
                            }
                            else if (shape is Ellipse ellipse)
                            {
                                dc.DrawEllipse(ellipse.Fill, pen, new Point(ellipse.Width / 2, ellipse.Height / 2), ellipse.Width / 2, ellipse.Height / 2);
                            }
                        }

                        return new ShapeVisual(drawingVisual, shape is Rectangle ? ShapeType.Square : ShapeType.Circle, "Label", true);
                    }
                }
            }
            return null; // Если визуализация не найдена
        }

        // Метод для получения визуального элемента по DrawingVisual
        public UIElement GetVisualElement(DrawingVisual visual)
        {
            foreach (var child in this.Children)
            {
                if (child is Image image && image.Source is DrawingImage drawingImage)
                {
                    // Сравниваем DrawingVisual с DrawingImage
                    if (drawingImage.Drawing == visual.Drawing)
                    {
                        return image; // Возвращаем UIElement, соответствующий DrawingVisual
                    }
                }
            }
            return null; // Если не найдено
        }

        // Метод для удаления визуализации
        public void DeleteVisual(UIElement visual)
        {
            if (visual != null)
            {
                this.Children.Remove(visual);
            }
        }
    }
}