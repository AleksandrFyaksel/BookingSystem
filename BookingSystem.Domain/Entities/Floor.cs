using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    public class Floor
    {
        [Key]
        public int FloorID { get; set; }  // Уникальный идентификатор этажа

        [Required]
        [StringLength(100)]
        public string FloorName { get; set; }  // Название этажа

        [ForeignKey("Office")]
        public int OfficeID { get; set; }  // Идентификатор офиса, к которому принадлежит этаж

        public byte[] ImageData { get; set; }  // Данные изображения этажа
        public string MimeType { get; set; }  // MIME-тип изображения

        // Навигационные свойства
        public virtual Office Office { get; set; }  // Связь с офисом
        public virtual ICollection<Workspace> Workspaces { get; set; }  // Связь с рабочими местами
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями

        // Конструктор для инициализации коллекций
        public Floor()
        {
            Workspaces = new List<Workspace>();  // Инициализация коллекции рабочих мест
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
        }
    }
}