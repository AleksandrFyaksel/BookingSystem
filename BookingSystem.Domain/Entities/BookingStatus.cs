using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class BookingStatus
    {
        [Key]
        public int BookingStatusID { get; set; } // Уникальный идентификатор статуса бронирования

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }  // Название статуса (например, активное, отмененное)

        public string Description { get; set; }  // Описание статуса

        public bool IsActive { get; set; }  // Статус активности

        // Навигационное свойство для связи с бронированиями
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями

        // Конструктор для инициализации коллекции бронирований
        public BookingStatus()
        {
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
        }
    }
}