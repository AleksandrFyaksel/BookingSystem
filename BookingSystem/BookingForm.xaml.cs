using System;
using System.Windows;
using System.Windows.Input;
using BookingSystem.Business.Managers;
using BookingSystem.Domain.Entities;

namespace BookingSystem
{
    public partial class BookingForm : Window
    {
        private readonly BookingManager _bookingManager;

        // Свойства для хранения идентификаторов
        public int UserID { get; set; } // Устанавливается при входе пользователя
        public int WorkspaceID { get; set; } // Устанавливается при рисовании рабочего места
        public int? ParkingSpaceID { get; set; } // Устанавливается при рисовании парковочного места
        public int FloorID { get; set; } // Устанавливается при выборе этажа
        public int BookingStatusID { get; set; } = 1; // Предположим, 1 - это активный статус

        public DateTime BookingDate { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        public string AdditionalRequirements { get; private set; }

        // Конструктор, принимающий BookingManager
        public BookingForm(BookingManager bookingManager)
        {
            InitializeComponent();
            _bookingManager = bookingManager;
            PopulateTimeComboBoxes();
        }

        private void PopulateTimeComboBoxes()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute++)
                {
                    string time = $"{hour:D2}:{minute:D2}";
                    StartTimeComboBox.Items.Add(time);
                    EndTimeComboBox.Items.Add(time);
                }
            }
        }

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для бронирования
            var selectedDate = DatePicker.SelectedDate;
            var startTime = StartTimeComboBox.SelectedItem?.ToString();
            var endTime = EndTimeComboBox.SelectedItem?.ToString();

            // Проверьте, что все необходимые поля заполнены
            if (selectedDate == null || string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Проверка на корректность времени
            if (!TimeSpan.TryParse(startTime, out TimeSpan startTimeSpan) ||
                !TimeSpan.TryParse(endTime, out TimeSpan endTimeSpan))
            {
                MessageBox.Show("Некорректный формат времени.");
                return;
            }

            // Проверка на то, что время начала меньше времени окончания
            if (endTimeSpan <= startTimeSpan)
            {
                MessageBox.Show("Время окончания должно быть больше времени начала.");
                return;
            }

            // Проверка на то, что дата бронирования не раньше текущего времени
            DateTime currentDateTime = DateTime.Now;
            DateTime selectedDateTime = selectedDate.Value.Add(startTimeSpan);

            if (selectedDateTime < currentDateTime)
            {
                MessageBox.Show("Вы не можете забронировать время раньше текущего дня и времени.");
                return;
            }

            AdditionalRequirements = AdditionalRequirementsTextBox.Text;

            // Создание объекта Booking
            var booking = new Booking
            {
                UserID = UserID,
                WorkspaceID = WorkspaceID,
                ParkingSpaceID = ParkingSpaceID,
             
                BookingStatusID = BookingStatusID,
                BookingDate = selectedDate.Value,
                StartDateTime = selectedDate.Value.Add(startTimeSpan),
                EndDateTime = selectedDate.Value.Add(endTimeSpan),
                AdditionalRequirements = AdditionalRequirements
            };

            // Сохранение бронирования в базе данных
            try
            {
                await _bookingManager.CreateBookingAsync(booking);
                this.DialogResult = true; // Закрываем окно с результатом "ОК"
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении бронирования: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Сброс значений полей, если это необходимо
            DatePicker.SelectedDate = null;
            StartTimeComboBox.SelectedItem = null;
            EndTimeComboBox.SelectedItem = null;
            AdditionalRequirementsTextBox.Clear();

            // Устанавливаем результат диалога в false
            this.DialogResult = false;

            // Закрываем окно
            this.Close();
        }
    }
}