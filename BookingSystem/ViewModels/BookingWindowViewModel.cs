using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingSystem.Domain.Entities;
using BookingSystem.Business;
using BookingSystem.Commands;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using System.Windows;
using BookingSystem.Business.Managers;

namespace BookingSystem.ViewModels
{
    public class BookingWindowViewModel : ViewModelBase
    {
        private readonly BookingManager bookingManager;
        private readonly BookingDbContext context;

        public ObservableCollection<Workspace> Workspaces { get; set; }
        public ObservableCollection<ParkingSpace> ParkingSpaces { get; set; }

        private string selectedOffice;
        public string SelectedOffice
        {
            get => selectedOffice;
            set => Set(ref selectedOffice, value);
        }

        private string selectedFloor;
        public string SelectedFloor
        {
            get => selectedFloor;
            set => Set(ref selectedFloor, value);
        }

        public ICommand RegisterWorkspaceCommand { get; }
        public ICommand RegisterParkingSpaceCommand { get; }
        public ICommand DeleteWorkspaceCommand { get; }
        public ICommand DeleteParkingSpaceCommand { get; }

        public BookingWindowViewModel(BookingManager bookingManager, BookingDbContext context)
        {
            this.bookingManager = bookingManager;
            this.context = context;
            Workspaces = new ObservableCollection<Workspace>();
            ParkingSpaces = new ObservableCollection<ParkingSpace>();

            RegisterWorkspaceCommand = new AsyncRelayCommand(async _ => await OnRegisterWorkspaceExecuted());
            RegisterParkingSpaceCommand = new AsyncRelayCommand(async _ => await OnRegisterParkingSpaceExecuted());
            DeleteWorkspaceCommand = new RelayCommand<int>(async id => await OnDeleteWorkspaceExecuted(id));
            DeleteParkingSpaceCommand = new RelayCommand<int>(async id => await OnDeleteParkingSpaceExecuted(id));
        }

        private async Task OnRegisterWorkspaceExecuted()
        {
            if (string.IsNullOrWhiteSpace(SelectedOffice) || string.IsNullOrWhiteSpace(SelectedFloor))
            {
                MessageBox.Show("Пожалуйста, выберите офис и этаж.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var workspace = new Workspace
            {
                Label = PromptForLabel(),
                FloorID = await GetFloorIdAsync(SelectedFloor)
            };

            if (workspace.FloorID == 0)
            {
                MessageBox.Show("Не удалось получить ID офиса или этажа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await bookingManager.AddWorkspaceAsync(workspace);
            Workspaces.Add(workspace);
            MessageBox.Show("Рабочее место успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task OnRegisterParkingSpaceExecuted()
        {
            if (string.IsNullOrWhiteSpace(SelectedOffice))
            {
                MessageBox.Show("Пожалуйста, выберите офис.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var parkingSpace = new ParkingSpace
            {
                Label = PromptForLabel(),
                OfficeID = await GetOfficeIdAsync(SelectedOffice),
                IsAvailable = true
            };

            if (parkingSpace.OfficeID == 0)
            {
                MessageBox.Show("Не удалось получить ID офиса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await bookingManager.AddParkingSpaceAsync(parkingSpace);
            ParkingSpaces.Add(parkingSpace);
            MessageBox.Show("Парковочное место успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task OnDeleteWorkspaceExecuted(int workspaceId)
        {
            var result = await bookingManager.DeleteWorkspaceAsync(workspaceId);
            if (result)
            {
                var workspaceToRemove = Workspaces.FirstOrDefault(w => w.WorkspaceID == workspaceId);
                if (workspaceToRemove != null)
                {
                    Workspaces.Remove(workspaceToRemove);
                }
                MessageBox.Show("Рабочее место успешно удалено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось удалить рабочее место.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task OnDeleteParkingSpaceExecuted(int parkingSpaceId)
        {
            var result = await bookingManager.DeleteParkingSpaceAsync(parkingSpaceId);
            if (result)
            {
                var parkingSpaceToRemove = ParkingSpaces.FirstOrDefault(p => p.ParkingSpaceID == parkingSpaceId);
                if (parkingSpaceToRemove != null)
                {
                    ParkingSpaces.Remove(parkingSpaceToRemove);
                }
                MessageBox.Show("Парковочное место успешно удалено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось удалить парковочное место.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string PromptForLabel()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Введите номер:", "Номер места", "");
        }

        private async Task<int> GetOfficeIdAsync(string officeName)
        {
            var office = await context.Offices.FirstOrDefaultAsync(o => o.OfficeName == officeName);
            if (office != null)
            {
                return office.OfficeID;
            }
            else
            {
                MessageBox.Show("Офис не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0; // Возвращаем 0, если офис не найден
            }
        }

        private async Task<int> GetFloorIdAsync(string floorName)
        {
            var floor = await context.Floors.FirstOrDefaultAsync(f => f.FloorName == floorName);
            if (floor != null)
            {
                return floor.FloorID;
            }
            else
            {
                MessageBox.Show("Этаж не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0; // Возвращаем 0, если этаж не найден
            }
        }
    }
}