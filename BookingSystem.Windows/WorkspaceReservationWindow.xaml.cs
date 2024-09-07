using System.Windows.Controls;
using System.Windows;

namespace BookingSystem.Windows
{

    public partial class WorkspaceReservationWindow : Window
    {
        public WorkspaceReservationWindow()
        {
            InitializeComponent(); // Убедитесь, что этот метод вызывается
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для отмены
            this.Close(); // Закрыть окно
        }

        private void EmployeeNotesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Логика для изменения текста заметок сотрудника
        }

        private void EndHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика для изменения часа окончания
        }

        private void EndMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика для изменения минуты окончания
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для резервирования
        }

        private void StartHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика для изменения часа начала
        }

        private void StartMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика для изменения минуты начала
        }

        private void WorkSpacesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика для изменения выбора рабочего пространства
        }
    }
}