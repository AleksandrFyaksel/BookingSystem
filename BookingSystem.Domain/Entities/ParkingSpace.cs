using BookingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class ParkingSpace
    {
        [Key]
        public int ParkingSpaceID { get; set; }  // Уникальный идентификатор парковочного места

        [ForeignKey("Floor")]
        public int FloorID { get; set; }  // Идентификатор этажа, на котором находится парковочное место

        public string Label { get; set; }  // Метка парковочного места
        public int PositionX { get; set; }  // Позиция парковочного места по оси X
        public int PositionY { get; set; }  // Позиция парковочного места по оси Y
        public bool IsAvailable { get; set; }  // Статус доступности (свободно/занято)

        // Навигационные свойства
        public virtual Floor Floor { get; set; }  // Связь с этажом
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями

        // Конструктор для инициализации коллекции бронирований
        public ParkingSpace()
        {
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
            IsAvailable = true; // По умолчанию парковочное место доступно
        }

        // Метод для установки статуса парковочного места
        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;  // Устанавливает статус доступности
        }

        // Метод для бронирования парковочного места
        public void Book()
        {
            if (IsAvailable)
            {
                IsAvailable = false; // Устанавливаем статус как занятое
                // Логика для добавления бронирования в коллекцию Bookings
            }
            else
            {
                throw new InvalidOperationException("Парковочное место уже забронировано.");
            }
        }
    }
}