using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для ParkingSpaceReservationWindow.xaml
    /// </summary>
    
        public partial class ParkingSpaceReservationWindow : Window
        {
            public ParkingSpaceReservationWindow()
            {
                InitializeComponent();
            }

            private void OfficesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Логика для изменения выбора офиса
            }

            private void StartHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Логика для изменения часа начала
            }

            private void StartMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Логика для изменения минуты начала
            }

            private void EndHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Логика для изменения часа окончания
            }

            private void EndMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Логика для изменения минуты окончания
            }

            private void EmployeeNotesTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                // Логика для изменения текста заметок сотрудника
            }

            private void ReserveButton_Click(object sender, RoutedEventArgs e)
            {
                // Логика для резервирования
            }

            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                // Логика для отмены
                this.Close(); // Закрыть окно
            }
        }
    
}
