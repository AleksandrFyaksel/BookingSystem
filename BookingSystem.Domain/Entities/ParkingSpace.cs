using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    public class ParkingSpace
    {
        [Key]
        public int ParkingSpaceID { get; set; }  // Уникальный идентификатор парковочного места

        [ForeignKey("Office")]
        public int OfficeID { get; set; }  // Идентификатор офиса, к которому принадлежит парковочное место

        [ForeignKey("Floor")]
        public int FloorID { get; set; }  // Идентификатор этажа, на котором находится парковочное место

        public string Label { get; set; }  // Метка парковочного места
        public string Position { get; set; }  // Позиция парковочного места
        public bool IsAvailable { get; set; }  // Статус доступности (свободно/занято)

        // Навигационные свойства
        public virtual Office Office { get; set; }  // Связь с офисом
        public virtual Floor Floor { get; set; }  // Связь с этажом
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями

        // Конструктор для инициализации коллекции бронирований
        public ParkingSpace()
        {
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
        }
    }
}