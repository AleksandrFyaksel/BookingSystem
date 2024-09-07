using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class Office
    {
        [Key]
        public int OfficeID { get; set; }  // Уникальный идентификатор офиса

        [Required]
        [StringLength(100)]
        public string OfficeName { get; set; }  // Название офиса

        [Required]
        [StringLength(200)]
        public string Location { get; set; }  // Местоположение офиса

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }  // Максимальное количество рабочих мест

        [StringLength(15)]
        public string Phone { get; set; }  // Номер телефона офиса

        // Навигационные свойства
        public virtual ICollection<Floor> Floors { get; set; }  // Связь с этажами
        public virtual ICollection<Workspace> Workspaces { get; set; }  // Связь с рабочими местами
        public virtual ICollection<ParkingSpace> ParkingSpaces { get; set; }  // Связь с парковочными местами
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями

        // Конструктор для инициализации коллекций
        public Office()
        {
            Floors = new List<Floor>();  // Инициализация коллекции этажей
            Workspaces = new List<Workspace>();  // Инициализация коллекции рабочих мест
            ParkingSpaces = new List<ParkingSpace>();  // Инициализация коллекции парковочных мест
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
        }
    }
}